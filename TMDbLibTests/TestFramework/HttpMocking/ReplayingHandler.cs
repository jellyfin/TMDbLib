using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMDbLibTests.TestFramework.HttpMocking
{
    internal class ReplayingHandler : MockingHandlerBase
    {
        private readonly ResponseContainer _responses;

        public ReplayingHandler(string storeFile)
        {
            _responses = new ResponseContainer();
            _responses.Load(storeFile);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Load request content
            JToken reqObj = null;
            if (request.Content != null)
            {
                await request.Content.LoadIntoBufferAsync();

                string reqJson = await request.Content.ReadAsStringAsync();
                reqObj = JsonConvert.DeserializeObject<JToken>(reqJson);
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
    }
}