using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TMDbLib.Rest
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

        public async Task<string> GetContent()
        {
            return await _response.Content.ReadAsStringAsync();
        }
    }

    internal class TmdbRestResponse<T> : TmdbRestResponse
    {
        public TmdbRestResponse(HttpResponseMessage response)
            : base(response)
        {
        }

        public async Task<T> GetDataObject()
        {
            string content = await _response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(content);
        }

        public static implicit operator T(TmdbRestResponse<T> response)
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