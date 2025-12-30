using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Utilities.Serializer;

namespace TMDbLib.Rest;

internal class RestRequest
{
    private readonly RestClient _client;
    private readonly string _endpoint;

    private object? _bodyObj;

    private List<KeyValuePair<string, string>>? _queryString;
    private List<KeyValuePair<string, string>>? _urlSegment;

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
        if (_queryString is null)
        {
            _queryString = [];
        }

        _queryString.Add(new KeyValuePair<string, string>(key, value));

        return this;
    }

    public RestRequest AddUrlSegment(string key, string value)
    {
        if (_urlSegment is null)
        {
            _urlSegment = [];
        }

        _urlSegment.Add(new KeyValuePair<string, string>(key, value));

        return this;
    }

    private void AppendQueryString(StringBuilder sb, string key, string value)
    {
        if (sb.Length > 0)
        {
            sb.Append('&');
        }

        sb.Append(key);
        sb.Append('=');
        sb.Append(WebUtility.UrlEncode(value));
    }

    private void AppendQueryString(StringBuilder sb, KeyValuePair<string, string> value)
    {
        AppendQueryString(sb, value.Key, value.Value);
    }

    public async Task<RestResponse> Delete(CancellationToken cancellationToken)
    {
        var resp = await SendInternal(HttpMethod.Delete, cancellationToken).ConfigureAwait(false);

        return new RestResponse(resp);
    }

    public async Task<RestResponse<T>> Delete<T>(CancellationToken cancellationToken)
    {
        var resp = await SendInternal(HttpMethod.Delete, cancellationToken).ConfigureAwait(false);

        return new RestResponse<T>(resp, _client);
    }

    public async Task<RestResponse> Get(CancellationToken cancellationToken)
    {
        var resp = await SendInternal(HttpMethod.Get, cancellationToken).ConfigureAwait(false);

        return new RestResponse(resp);
    }

    public async Task<RestResponse<T>> Get<T>(CancellationToken cancellationToken)
    {
        var resp = await SendInternal(HttpMethod.Get, cancellationToken).ConfigureAwait(false);

        return new RestResponse<T>(resp, _client);
    }

    public async Task<RestResponse> Post(CancellationToken cancellationToken)
    {
        var resp = await SendInternal(HttpMethod.Post, cancellationToken).ConfigureAwait(false);

        return new RestResponse(resp);
    }

    public async Task<RestResponse<T>> Post<T>(CancellationToken cancellationToken)
    {
        var resp = await SendInternal(HttpMethod.Post, cancellationToken).ConfigureAwait(false);

        return new RestResponse<T>(resp, _client);
    }

    private HttpRequestMessage PrepRequest(HttpMethod method)
    {
        var queryStringSb = new StringBuilder();

        // Query String
        if (_queryString is not null)
        {
            foreach (var pair in _queryString)
            {
                AppendQueryString(queryStringSb, pair);
            }
        }

        foreach (var pair in _client.DefaultQueryString)
        {
            AppendQueryString(queryStringSb, pair);
        }

        // Url
        string endpoint = _endpoint;
        if (_urlSegment is not null)
        {
            foreach (var pair in _urlSegment)
            {
                endpoint = endpoint.Replace("{" + pair.Key + "}", pair.Value, StringComparison.OrdinalIgnoreCase);
            }
        }

        // Build
        var builder = new UriBuilder(new Uri(_client.BaseUrl, endpoint))
        {
            Query = queryStringSb.ToString()
        };

        var req = new HttpRequestMessage(method, builder.Uri);

        // Body
        if (method == HttpMethod.Post && _bodyObj is not null)
        {
            var bodyBytes = _client.Serializer.SerializeToBytes(_bodyObj);

            req.Content = new ByteArrayContent(bodyBytes);
            req.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        return req;
    }

    private async Task<HttpResponseMessage?> SendInternal(HttpMethod method, CancellationToken cancellationToken)
    {
        // Account for the following settings:
        // - MaxRetryCount                          Max times to retry

        var timesToTry = _client.MaxRetryCount + 1;

        RetryConditionHeaderValue? retryHeader = null;
        TMDbStatusMessage? statusMessage = null;

        Debug.Assert(timesToTry >= 1, "Times to try must be at least 1");

        do
        {
            using var req = PrepRequest(method);
            var resp = await _client.HttpClient.SendAsync(req, cancellationToken).ConfigureAwait(false);

            var isJson = resp.Content.Headers.ContentType?.MediaType?.Equals("application/json", StringComparison.OrdinalIgnoreCase) ?? false;

            if (resp.IsSuccessStatusCode && isJson)
            {
#pragma warning disable IDISP011 // Don't return disposed instance - False positive, resp is not disposed in this path
                return resp;
#pragma warning restore IDISP011
            }

            if (isJson)
            {
                statusMessage = JsonConvert.DeserializeObject<TMDbStatusMessage>(await resp.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false));
            }
            else
            {
                statusMessage = null;
            }

            switch (resp.StatusCode)
            {
                case (HttpStatusCode)429:
                    // The previous result was a ratelimit, read the Retry-After header and wait the allotted time
                    retryHeader = resp.Headers.RetryAfter;
                    var retryAfter = retryHeader?.Delta;

                    if (retryAfter.HasValue && retryAfter.Value.TotalSeconds > 0)
                    {
                        await Task.Delay(retryAfter.Value, cancellationToken).ConfigureAwait(false);
                    }
                    else
                    {
                        // TMDb sometimes gives us 0-second waits, which can lead to rapid succession of requests
                        await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken).ConfigureAwait(false);
                    }

                    resp.Dispose();
                    continue;

                case HttpStatusCode.Unauthorized:
                    resp.Dispose();
                    throw new UnauthorizedAccessException(
                        "Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");

                case HttpStatusCode.NotFound:
                    if (_client.ThrowApiExceptions)
                    {
                        resp.Dispose();
                        throw new NotFoundException(statusMessage);
                    }
                    else
                    {
                        resp.Dispose();
                        return null;
                    }
            }

#pragma warning disable IDISP016, IDISP017 // Explicit disposal is correct here for error paths
            resp.Dispose();
#pragma warning restore IDISP016, IDISP017
            throw new GeneralHttpException(resp.StatusCode);
        }
        while (timesToTry-- > 0);

        // We never reached a success
        throw new RequestLimitExceededException(statusMessage, retryHeader?.Date, retryHeader?.Delta);
    }

    public RestRequest SetBody(object obj)
    {
        _bodyObj = obj;

        return this;
    }
}
