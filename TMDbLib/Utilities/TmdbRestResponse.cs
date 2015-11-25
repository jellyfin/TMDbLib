using System.Linq;
using System.Net;
using System.Net.Http;

namespace TMDbLib.Utilities
{
    internal class TmdbRestResponse
    {
        protected readonly HttpResponseMessage _response;

        public TmdbRestResponse(HttpResponseMessage response)
        {
            _response = response;
        }

        public HttpStatusCode StatusCode => _response.StatusCode;

        public string GetHeader(string name, string @default = null)
        {
            return _response.Headers.GetValues(name).FirstOrDefault() ?? @default;
        }
    }
}