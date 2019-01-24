using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMDbLibTests.TestFramework.HttpMocking
{
    internal class RecordingHandler : MockingHandlerBase
    {
        private readonly string _storeFile;
        private readonly HttpMessageHandler _innerHandler;
        private readonly MethodInfo _method;
        private readonly ResponseContainer _responses;

        public RecordingHandler(string storeFile, HttpMessageHandler innerHandler)
        {
            _storeFile = storeFile;
            _innerHandler = innerHandler;
            _method = _innerHandler.GetType().GetMethod(nameof(SendAsync), BindingFlags.Instance | BindingFlags.NonPublic);

            _responses = new ResponseContainer();
            _responses.Load(storeFile);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
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

        protected override void Dispose(bool disposing)
        {
            _responses.Save(_storeFile);
        }
    }
}