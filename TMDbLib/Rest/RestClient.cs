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

    public RestClient(Uri baseUrl, ITMDbSerializer serializer, IWebProxy proxy = null)
        : this(baseUrl, serializer, proxy, null)
    {
    }

    internal RestClient(Uri baseUrl, ITMDbSerializer serializer, IWebProxy proxy, HttpMessageHandler httpMessageHandler)
    {
        BaseUrl = baseUrl;
        Serializer = serializer;
        DefaultQueryString = new List<KeyValuePair<string, string>>();

        MaxRetryCount = 0;
        Proxy = proxy;

        if (httpMessageHandler is not null)
        {
            HttpClient = new HttpClient(httpMessageHandler);
        }
        else
        {
            HttpClientHandler handler = new HttpClientHandler();
            if (proxy is not null)
            {
                // Blazor apparently throws on the Proxy setter.
                // https://github.com/LordMike/TMDbLib/issues/354
                handler.Proxy = proxy;
            }

            HttpClient = new HttpClient(handler);
        }
    }

    internal Uri BaseUrl { get; }

    internal List<KeyValuePair<string, string>> DefaultQueryString { get; }

    internal Encoding Encoding { get; } = new UTF8Encoding(false);

    internal IWebProxy Proxy { get; private set; }

    internal HttpClient HttpClient { get; private set; }

    public int MaxRetryCount
    {
        get => _maxRetryCount;

        set
        {
#if NETSTANDARD2_0
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
#else
            ArgumentOutOfRangeException.ThrowIfNegative(value);
#endif
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
