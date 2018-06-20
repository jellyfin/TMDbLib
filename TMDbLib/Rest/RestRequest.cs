using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
    internal class RestRequest
    {
        private readonly RestClient _client;
        private readonly string _endpoint;

        private object _bodyObj;

        private List<KeyValuePair<string, string>> _queryString;
        private List<KeyValuePair<string, string>> _urlSegment;

        public RestRequest(RestClient client, string endpoint)
        {
            _client = client;
            _endpoint = endpoint;
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

        public RestRequest AddQueryString(string key, string value)
        {
            if (_queryString == null)
                _queryString = new List<KeyValuePair<string, string>>();

            _queryString.Add(new KeyValuePair<string, string>(key, value));

            return this;
        }

        public RestRequest AddUrlSegment(string key, string value)
        {
            if (_urlSegment == null)
                _urlSegment = new List<KeyValuePair<string, string>>();

            _urlSegment.Add(new KeyValuePair<string, string>(key, value));

            return this;
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

        /**private void CheckResponse(HttpResponseMessage response)
        {
            if (response.StatusCode == HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");
        }**/

        public async Task<RestResponse> ExecuteDelete(CancellationToken cancellationToken)
        {
            HttpResponseMessage resp = await SendInternal(HttpMethod.Delete, cancellationToken).ConfigureAwait(false);

            //CheckResponse(resp);

            return new RestResponse(resp);
        }

        public async Task<RestResponse<T>> ExecuteDelete<T>(CancellationToken cancellationToken)
        {
            HttpResponseMessage resp = await SendInternal(HttpMethod.Delete, cancellationToken).ConfigureAwait(false);

            //CheckResponse(resp);

            return new RestResponse<T>(resp, _client);
        }

        public async Task<RestResponse> ExecuteGet(CancellationToken cancellationToken)
        {
            HttpResponseMessage resp = await SendInternal(HttpMethod.Get, cancellationToken).ConfigureAwait(false);

            //CheckResponse(resp);

            return new RestResponse(resp);
        }

        public async Task<RestResponse<T>> ExecuteGet<T>(CancellationToken cancellationToken)
        {
            HttpResponseMessage resp = await SendInternal(HttpMethod.Get, cancellationToken).ConfigureAwait(false);

            //CheckResponse(resp);

            return new RestResponse<T>(resp, _client);
        }

        public async Task<RestResponse> ExecutePost(CancellationToken cancellationToken)
        {
            HttpResponseMessage resp = await SendInternal(HttpMethod.Post, cancellationToken).ConfigureAwait(false);

            //CheckResponse(resp);

            return new RestResponse(resp);
        }

        public async Task<RestResponse<T>> ExecutePost<T>(CancellationToken cancellationToken)
        {
            HttpResponseMessage resp = await SendInternal(HttpMethod.Post, cancellationToken).ConfigureAwait(false);

            //CheckResponse(resp);

            return new RestResponse<T>(resp, _client);
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
                MemoryStream ms = new MemoryStream();
                using (StreamWriter sw = new StreamWriter(ms, _client.Encoding, 4096, true))
                using (JsonTextWriter tw = new JsonTextWriter(sw))
                {
                    _client.Serializer.Serialize(tw, _bodyObj);
                }

                ms.Seek(0, SeekOrigin.Begin);

                req.Content = new StreamContent(ms);
                req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return req;
        }

        private async Task<HttpResponseMessage> SendInternal(HttpMethod method, CancellationToken cancellationToken)
        {
            // Account for the following settings:
            // - MaxRetryCount                          Max times to retry

            int timesToTry = _client.MaxRetryCount + 1;
            RetryConditionHeaderValue retryHeader = null;

            Debug.Assert(timesToTry >= 1);

            TMDbStatusMessage statusMessage = null;

            do
            {
                using (HttpRequestMessage req = PrepRequest(method))
                {
                    HttpResponseMessage resp =
                        await _client.HttpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);

                    if (!resp.IsSuccessStatusCode)
                    {
                        statusMessage =
                            JsonConvert.DeserializeObject<TMDbStatusMessage>(await resp.Content.ReadAsStringAsync());
                    }

                    if (resp.StatusCode == (HttpStatusCode)429)
                    {
                        // The previous result was a ratelimit, read the Retry-After header and wait the allotted time
                        retryHeader = resp.Headers.RetryAfter;
                        TimeSpan? retryAfter = retryHeader?.Delta.Value;

                        if (retryAfter.HasValue && retryAfter.Value.TotalSeconds > 0)
                            await Task.Delay(retryAfter.Value, cancellationToken).ConfigureAwait(false);
                        else
                            // TMDb sometimes gives us 0-second waits, which can lead to rapid succession of requests
                            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken).ConfigureAwait(false);

                        continue;
                    }

                    if (resp.IsSuccessStatusCode)
                        return resp;

                    if (!resp.IsSuccessStatusCode)
                    {
                        switch (resp.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");

                            case HttpStatusCode.NotFound:
                                if (_client.ThrowExceptionsOnNotFound)
                                {
                                    throw new NotFoundException(resp.StatusCode, statusMessage);
                                }
                                else
                                {
                                    return null;
                                }
                        }
                        throw new UnhandledException(resp.StatusCode, statusMessage);
                        //return resp;
                    }
                }
            } while (timesToTry-- > 0);

            // We never reached a success
            throw new RequestLimitExceededException((HttpStatusCode)429, statusMessage, retryHeader?.Date, retryHeader?.Delta);
        }

        public RestRequest SetBody(object obj)
        {
            _bodyObj = obj;

            return this;
        }
    }
}