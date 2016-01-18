using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMDbLib.Objects.Exceptions;

namespace TMDbLib.Rest
{
    // TODO: Rename to RestRequest
    internal class RestRequest
    {
        private readonly RestClient _client;
        private readonly string _endpoint;

        private List<KeyValuePair<string, string>> _queryString;
        private List<KeyValuePair<string, string>> _urlSegment;

        private object _bodyObj;

        public RestRequest(RestClient client, string endpoint)
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

        public RestRequest AddParameter(KeyValuePair<string, string> pair, ParameterType type = ParameterType.QueryString)
        {
            AddParameter(pair.Key, pair.Value, type);

            return this;
        }

        public RestRequest AddParameter(string key, string value, ParameterType type = ParameterType.QueryString)
        {
            switch (type)
            {
                case ParameterType.QueryString:
                    return AddQueryString(key, value);
                case ParameterType.UrlSegment:
                    return AddUrlSegment(key, value);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public RestRequest AddUrlSegment(string key, string value)
        {
            if (_urlSegment == null)
                _urlSegment = new List<KeyValuePair<string, string>>();

            _urlSegment.Add(new KeyValuePair<string, string>(key, value));

            return this;
        }

        public RestRequest AddQueryString(string key, string value)
        {
            if (_queryString == null)
                _queryString = new List<KeyValuePair<string, string>>();

            _queryString.Add(new KeyValuePair<string, string>(key, value));

            return this;
        }

        public RestRequest SetBody(object obj)
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

        public async Task<RestResponse> ExecuteGet()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Get);
            HttpResponseMessage resp = await SendInternal(req);

            CheckResponse(resp);

            return new RestResponse(resp);
        }

        public async Task<RestResponse<T>> ExecuteGet<T>()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Get);
            HttpResponseMessage resp = await SendInternal(req);

            CheckResponse(resp);

            return new RestResponse<T>(resp);
        }

        public async Task<RestResponse> ExecutePost()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Post);
            HttpResponseMessage resp = await SendInternal(req);

            CheckResponse(resp);

            return new RestResponse(resp);
        }

        public async Task<RestResponse<T>> ExecutePost<T>()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Post);
            HttpResponseMessage resp = await SendInternal(req);

            CheckResponse(resp);

            return new RestResponse<T>(resp);
        }

        public async Task<RestResponse> ExecuteDelete()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Delete);
            HttpResponseMessage resp = await SendInternal(req);

            CheckResponse(resp);

            return new RestResponse(resp);
        }

        public async Task<RestResponse<T>> ExecuteDelete<T>()
        {
            HttpRequestMessage req = PrepRequest(HttpMethod.Delete);
            HttpResponseMessage resp = await SendInternal(req);

            CheckResponse(resp);

            return new RestResponse<T>(resp);
        }

        private async Task<HttpResponseMessage> SendInternal(HttpRequestMessage req)
        {
            HttpClient client = new HttpClient();

            // Account for the following settings:
            // - MaxRetryCount                          Max times to retry
            // DEPRECATED RetryWaitTimeInSeconds        Time to wait between retries
            // DEPRECATED ThrowErrorOnExeedingMaxCalls  Throw an exception if we hit a ratelimit

            HttpResponseMessage resp = null;
            for (int i = 0; i <= _client.MaxRetryCount; i++)
            {
                if (resp != null && resp.StatusCode == (HttpStatusCode)429)
                {
                    // The previous result was a ratelimit, read the Retry-After header and wait the allotted time
                    RetryConditionHeaderValue retryAfter = resp.Headers.RetryAfter;
                    TimeSpan timeToWait = (retryAfter?.Delta.Value ?? (TimeSpan?)TimeSpan.FromSeconds(5)).Value;

                    await Task.Delay(timeToWait);
                }

                resp = await client.SendAsync(req);

                if (resp.IsSuccessStatusCode)
                    // We have a success
                    break;
            }

            if (resp == null || !resp.IsSuccessStatusCode)
            {
                // We never reached a success
                throw new RequestLimitExceededException();
            }

            return resp;
        }
    }
}