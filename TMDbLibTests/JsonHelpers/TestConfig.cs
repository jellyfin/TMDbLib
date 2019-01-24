using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TMDbLib.Client;
using TMDbLibTests.Exceptions;
using Xunit;
using ErrorEventArgs = Newtonsoft.Json.Serialization.ErrorEventArgs;

namespace TMDbLibTests.JsonHelpers
{
    [CollectionDefinition(nameof(ClientFixture))]
    public class ClientFixture : ICollectionFixture<TestConfig>
    {
    }

    public class TestConfig : IDisposable
    {
        const Handler.OperationKind OperationKind = Handler.OperationKind.Replay;

        // This API key should only be used to test the library, for further use please request a dedicated key from the TMDb
        public const string APIKey = "c6b31d1cdad6a56a23f0c913e2482a31";

        // Required to be able to successfully run all authenticated tests
        public readonly string UserSessionId = "c413282cdadad9af972c06d9b13096a8b13ab1c1";
        public readonly string GuestTestSessionId = "d425468da2781d6799ba14c05f7327e7";

        public TMDbClient Client { get; set; }

        public string Username = "TMDbTestAccount";

        public string Password = "TJX6vP7bPC%!ZrJwAqtCU5FshHEKAwzr6YvR3%CU9s7BrjqUWmjC8AMuXju*eTEu524zsxDQK5ySY6EmjAC3e54B%WvkS9FNPE3K";

        public event Action<ErrorEventArgs> OnJsonError;

        public TestConfig()
        {
            if (APIKey.Length == 0)
                throw new ConfigurationErrorsException("You need to configure the API Key before running any tests. Look at the TestConfig class.");

            Handler handler = new Handler("data.json", OperationKind, new SocketsHttpHandler());
            HttpClient httpClient = new HttpClient(handler);

            JsonSerializerSettings sett = new JsonSerializerSettings();

            sett.MissingMemberHandling = MissingMemberHandling.Error;
            sett.ContractResolver = new FailingContractResolver();
            sett.Error = Error;

            JsonSerializer serializer = JsonSerializer.Create(sett);

            // TODO: Disable SSL
            Client = new TMDbClient(httpClient, APIKey, serializer: serializer);
        }

        private void Error(object sender, ErrorEventArgs errorEventArgs)
        {
            OnJsonError?.Invoke(errorEventArgs);
            errorEventArgs.ErrorContext.Handled = true;
        }

        public void Dispose()
        {
            Client?.Dispose();
        }
    }

    internal class Handler : HttpMessageHandler
    {
        private readonly string _storeFile;
        private readonly OperationKind _kind;
        private readonly HttpMessageHandler _innerHandler;
        private readonly MethodInfo _method;
        private readonly ResponseContainer _responses;

        public Handler(string storeFile, OperationKind kind, HttpMessageHandler innerHandler)
        {
            _storeFile = storeFile;
            _kind = kind;
            _innerHandler = innerHandler;
            _method = _innerHandler.GetType().GetMethod(nameof(SendAsync), BindingFlags.Instance | BindingFlags.NonPublic);

            _responses = new ResponseContainer();
            _responses.Load(storeFile);
        }

        private string GetReducedUri(Uri uri)
        {
            UriBuilder uriBuilder = new UriBuilder(uri);
            NameValueCollection uriQueryString = HttpUtility.ParseQueryString(uriBuilder.Query);
            uriQueryString.Remove("api_key");

            uriBuilder.Query = uriQueryString.ToString();

            return uriBuilder.Uri.PathAndQuery;
        }

        private async Task<HttpResponseMessage> DoStoringOperation(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Load request content
            JObject reqObj = null;
            if (request.Content != null)
            {
                await request.Content.LoadIntoBufferAsync();

                string reqJson = await request.Content.ReadAsStringAsync();
                reqObj = JsonConvert.DeserializeObject<JObject>(reqJson);
            }

            // Do the live request
            HttpResponseMessage result = await (Task<HttpResponseMessage>)_method.Invoke(_innerHandler, new object[] { request, cancellationToken });

            // Load the response
            await result.Content.LoadIntoBufferAsync();

            Dictionary<string, string> headers = new Dictionary<string, string>();
            headers.Add("Content-Type", result.Content.Headers.ContentType.ToString());

            string respJson = await result.Content.ReadAsStringAsync();
            JObject respObj = JsonConvert.DeserializeObject<JObject>(respJson);

            _responses.AddResponse(new ResponseObject
            {
                ReducedUri = GetReducedUri(request.RequestUri),
                ReqMethod = request.Method.Method,
                ReqData = reqObj,
                RespStatusCode = result.StatusCode,
                RespData = respObj,
                RespHeaders = headers
            });

            return result;
        }

        private async Task<HttpResponseMessage> DoMockedOperation(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Load request content
            JObject reqObj = null;
            if (request.Content != null)
            {
                await request.Content.LoadIntoBufferAsync();

                string reqJson = await request.Content.ReadAsStringAsync();
                reqObj = JsonConvert.DeserializeObject<JObject>(reqJson);
            }

            string reducedUri = GetReducedUri(request.RequestUri);
            HttpMethod method = request.Method;

            // Identify the response to replay
            ResponseObject response = _responses.FindResponse(reducedUri, method.Method, reqObj);

            // Build a response object
            string json = JsonConvert.SerializeObject(response.RespData);
            StringContent responseContent = new StringContent(json);

            if (response.RespHeaders.TryGetValue("Content-Type", out var val))
                responseContent.Headers.ContentType = MediaTypeHeaderValue.Parse(val);

            return new HttpResponseMessage(response.RespStatusCode)
            {
                RequestMessage = request,
                Content = responseContent
            };
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            switch (_kind)
            {
                case OperationKind.StoreLiveResponses:
                    return DoStoringOperation(request, cancellationToken);
                case OperationKind.Replay:
                    return DoMockedOperation(request, cancellationToken);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override void Dispose(bool disposing)
        {
            _responses.Save(_storeFile);
        }

        class ResponseContainer
        {
            private readonly JsonSerializer _serializer;
            private readonly List<ResponseObject> _responses;

            public ResponseContainer()
            {
                _responses = new List<ResponseObject>();
                _serializer = JsonSerializer.Create(new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });
            }

            public void Save(string file)
            {
                IOrderedEnumerable<ResponseObject> data = _responses
                    .OrderBy(s => s.ReducedUri)
                    .ThenBy(s => s.ReqMethod)
                    .ThenBy(s => s.RespStatusCode);

                using (StreamWriter sw = new StreamWriter(file))
                using (JsonTextWriter tw = new JsonTextWriter(sw))
                    _serializer.Serialize(tw, data);
            }

            public void Load(string file)
            {
                if (!File.Exists(file))
                    return;

                using (StreamReader sr = new StreamReader(file))
                using (JsonTextReader tr = new JsonTextReader(sr))
                    _responses.AddRange(_serializer.Deserialize<IEnumerable<ResponseObject>>(tr));
            }

            public void AddResponse(ResponseObject responseObject)
            {
                _responses.RemoveAll(s => s.ReducedUri == responseObject.ReducedUri && s.ReqMethod == responseObject.ReqMethod);
                _responses.Add(responseObject);
            }

            public ResponseObject FindResponse(string reducedUri, string method, JObject requestObject)
            {
                IEnumerable<ResponseObject> applicables = _responses.Where(s =>
                    s.ReducedUri.Equals(reducedUri, StringComparison.OrdinalIgnoreCase) &&
                    s.ReqMethod.Equals(method, StringComparison.OrdinalIgnoreCase));

                if (requestObject != null)
                    applicables = applicables.Where(s => JToken.DeepEquals(s.ReqData, requestObject));

                if (applicables.Count() != 1)
                    throw new Exception("There wasn't exactly one matching response to replay");

                return applicables.First();
            }
        }

        class ResponseObject
        {
            public string ReducedUri { get; set; }
            public JObject RespData { get; set; }
            public JObject ReqData { get; set; }
            public HttpStatusCode RespStatusCode { get; set; }
            public string ReqMethod { get; set; }
            public Dictionary<string, string> RespHeaders { get; set; }
        }

        public enum OperationKind
        {
            StoreLiveResponses,
            Replay
        }
    }
}