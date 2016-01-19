using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TMDbLib.Rest
{
    internal class RestResponse
    {
        protected readonly HttpResponseMessage _response;

        public RestResponse(HttpResponseMessage response)
        {
            _response = response;
        }

        public HttpStatusCode StatusCode => _response.StatusCode;

        public string GetHeader(string name, string @default = null)
        {
            return _response.Headers.GetValues(name).FirstOrDefault() ?? @default;
        }

        public async Task<string> GetContent()
        {
            return await _response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }
    }
    
    internal class RestResponse<T> : RestResponse
    {
        public RestResponse(HttpResponseMessage response)
            : base(response)
        {
        }

        public async Task<T> GetDataObject()
        {
            string content = await _response.Content.ReadAsStringAsync().ConfigureAwait(false);

            return JsonConvert.DeserializeObject<T>(content);
        }

        public static implicit operator T(RestResponse<T> response)
        {
            try
            {

                return response.GetDataObject().Result;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}