using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TMDbLib.Utilities
{
    // TODO: Rename to RestRequest
    internal class TmdbRestRequest
    {
        private readonly RestClient _client;
        private readonly string _endpoint;

        private List<KeyValuePair<string, string>> _queryString;
        private List<KeyValuePair<string, string>> _urlSegment;

        private object _bodyObj;

        public TmdbRestRequest(RestClient client, string endpoint)
        {
            _client = client;
            _endpoint = endpoint;
        }

        private void AppendQueryString(StringBuilder sb, string key, string value)
        {
            if (sb.Length > 0)
                sb.Append("&");

            sb.Append(key);
            sb.Append("=");
            sb.Append(WebUtility.UrlEncode(value));
        }

        private void AppendQueryString(StringBuilder sb, KeyValuePair<string, string> value)
        {
            AppendQueryString(sb, value.Key, value.Value);
        }

        public TmdbRestRequest AddParameter(KeyValuePair<string, string> pair, TmdbParameterType type = TmdbParameterType.QueryString)
        {
            AddParameter(pair.Key, pair.Value, type);

            return this;
        }

        public TmdbRestRequest AddParameter(string key, string value, TmdbParameterType type = TmdbParameterType.QueryString)
        {
            switch (type)
            {
                case TmdbParameterType.QueryString:
                    AddQueryString(key, value);
                    break;
                case TmdbParameterType.UrlSegment:
                    AddUrlSegment(key, value);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            return this;
        }

        public TmdbRestRequest AddUrlSegment(string key, string value)
        {
            if (_urlSegment == null)
                _urlSegment = new List<KeyValuePair<string, string>>();

            _urlSegment.Add(new KeyValuePair<string, string>(key, value));

            return this;
        }

        public TmdbRestRequest AddQueryString(string key, string value)
        {
            if (_queryString == null)
                _queryString = new List<KeyValuePair<string, string>>();

            _queryString.Add(new KeyValuePair<string, string>(key, value));

            return this;
        }

        public TmdbRestRequest SetBody(object obj)
        {
            _bodyObj = obj;

            return this;
        }

        private HttpRequestMessage PrepRequest(HttpMethod method)
        {
            StringBuilder queryStringSb = new StringBuilder();

            // Query String
            if (_queryString != null)
            {
                foreach (KeyValuePair<string, string> pair in _queryString)
                    AppendQueryString(queryStringSb, pair);
            }

            foreach (KeyValuePair<string, string> pair in _client.DefaultQueryString)
                AppendQueryString(queryStringSb, pair);

            // Url
            string endpoint = _endpoint;
            if (_urlSegment != null)
            {
                foreach (KeyValuePair<string, string> pair in _urlSegment)
                    endpoint = endpoint.Replace("{" + pair.Key + "}", pair.Value);
            }

            // Build
            UriBuilder builder = new UriBuilder(new Uri(_client.BaseUrl, endpoint));
            builder.Query = queryStringSb.ToString();

            HttpRequestMessage req = new HttpRequestMessage(method, builder.Uri);

            // Body
            if (method == HttpMethod.Post && _bodyObj != null)
            {
                string json = JsonConvert.SerializeObject(_bodyObj);

                req.Content = new StringContent(json);
                req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return req;
        }

        private void CheckResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");
        }

        public async Task<TmdbRestResponse> ExecuteGet()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Get);
            HttpResponseMessage resp = await new HttpClient().SendAsync(req);

            CheckResponse(resp);

            return new TmdbRestResponse(resp);
        }

        public async Task<TmdbRestResponse<T>> ExecuteGet<T>()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Get);
            HttpResponseMessage resp = await new HttpClient().SendAsync(req);

            CheckResponse(resp);

            return new TmdbRestResponse<T>(resp);
        }

        public async Task<TmdbRestResponse<T>> ExecuteGetTaskAsync<T>()
        {
            // TODO: Inline this
            return await ExecuteGet<T>();
        }

        public async Task<TmdbRestResponse> ExecutePost()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Post);
            HttpResponseMessage resp = await new HttpClient().SendAsync(req);

            CheckResponse(resp);

            return new TmdbRestResponse(resp);
        }

        public async Task<TmdbRestResponse<T>> ExecutePost<T>()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Post);
            HttpResponseMessage resp = await new HttpClient().SendAsync(req);

            CheckResponse(resp);

            return new TmdbRestResponse<T>(resp);
        }

        public async Task<TmdbRestResponse> ExecuteDelete()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Delete);
            HttpResponseMessage resp = await new HttpClient().SendAsync(req);

            CheckResponse(resp);

            return new TmdbRestResponse(resp);
        }

        public async Task<TmdbRestResponse<T>> ExecuteDelete<T>()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Delete);
            HttpResponseMessage resp = await new HttpClient().SendAsync(req);

            CheckResponse(resp);

            return new TmdbRestResponse<T>(resp);
        }
    }
}