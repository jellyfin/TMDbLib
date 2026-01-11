using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using TMDbLib.Utilities.Serializer;

namespace TMDbLib.Rest;

internal sealed class RestClient : IDisposable
{
    private int _maxRetryCount;

    public RestClient(Uri baseUrl, ITMDbSerializer serializer, IWebProxy? proxy = null)
        : this(baseUrl, serializer, proxy, null)
    {
    }

    internal RestClient(Uri baseUrl, ITMDbSerializer serializer, IWebProxy? proxy, HttpMessageHandler? httpMessageHandler)
    {
        BaseUrl = baseUrl;
        Serializer = serializer;
        DefaultQueryString = [];

        MaxRetryCount = 0;
        Proxy = proxy;

        if (httpMessageHandler is null)
        {
            var handler = new HttpClientHandler();
            if (proxy is not null)
            {
                // Blazor apparently throws on the Proxy setter.
                // https://github.com/jellyfin/TMDbLib/issues/354
                handler.Proxy = proxy;
            }

            httpMessageHandler = handler;
        }

        HttpClient = new HttpClient(httpMessageHandler);
    }

    internal Uri BaseUrl { get; }

    internal List<KeyValuePair<string, string>> DefaultQueryString { get; }

    internal Encoding Encoding { get; } = new UTF8Encoding(false);

    internal IWebProxy? Proxy { get; private set; }

    internal HttpClient HttpClient { get; private set; }

    public int MaxRetryCount
    {
        get => _maxRetryCount;

        set
        {
            ArgumentOutOfRangeException.ThrowIfNegative(value);

            _maxRetryCount = value;
        }
    }

    public TimeSpan Timeout
    {
        get => HttpClient.Timeout;
        set => HttpClient.Timeout = value;
    }

    public bool ThrowApiExceptions { get; set; }

    internal ITMDbSerializer Serializer { get; }

    public void AddDefaultQueryString(string key, string value)
    {
        DefaultQueryString.Add(new KeyValuePair<string, string>(key, value));
    }

    public RestRequest Create(string endpoint)
    {
        return new RestRequest(this, endpoint);
    }

    public void Dispose()
    {
        HttpClient?.Dispose();
    }
}
