using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TMDbLib.Rest
{
    internal class RestResponse
    {
        protected readonly HttpResponseMessage Response;

        public RestResponse(HttpResponseMessage response)
        {
            Response = response;
        }

        public HttpStatusCode StatusCode => Response.StatusCode;

        public async Task<string> GetContent()
        {
            return await Response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public string GetHeader(string name, string @default = null)
        {
            return Response.Headers.GetValues(name).FirstOrDefault() ?? @default;
        }
    }

    internal class RestResponse<T> : RestResponse
    {
        private readonly JsonSerializer _serializer;

        public RestResponse(HttpResponseMessage response, JsonSerializer serializer)
            : base(response)
        {
            _serializer = serializer;
        }

        public async Task<T> GetDataObject()
        {
            Stream content = await Response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            using (StreamReader sr = new StreamReader(content))
            using (JsonTextReader tr = new JsonTextReader(sr))
                return _serializer.Deserialize<T>(tr);
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