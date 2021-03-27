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

        public bool IsValid => Response != null;

        public HttpStatusCode StatusCode => Response.StatusCode;

        public async Task<Stream> GetContent()
        {
            return await Response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        }

        public string GetHeader(string name, string @default = null)
        {
            return Response.Headers.GetValues(name).FirstOrDefault() ?? @default;
        }
    }

    internal class RestResponse<T> : RestResponse
    {
        private readonly RestClient _client;

        public RestResponse(HttpResponseMessage response, RestClient client)
            : base(response)
        {
            _client = client;
        }

        public async Task<T> GetDataObject()
        {
            Stream content = await GetContent().ConfigureAwait(false);

            using (StreamReader sr = new StreamReader(content, _client.Encoding))
            using (JsonTextReader tr = new JsonTextReader(sr))
                return _client.Serializer.Deserialize<T>(tr);
        }

        [Obsolete("Remove this, uses .Result")]
        public static implicit operator T(RestResponse<T> response)
        {
            try
            {
                if (response.IsValid)
                    return response.GetDataObject().Result;

                return default;
            }
            catch (AggregateException ex)
            {
                throw ex.InnerException;
            }
        }
    }
}