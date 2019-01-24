using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;

namespace TMDbLib.Rest
{
    internal class RestClient : IDisposable
    {
        private int _maxRetryCount;

        public RestClient(HttpClient httpClient, Uri baseUrl, JsonSerializer serializer)
        {
            BaseUrl = baseUrl;
            Serializer = serializer;
            DefaultQueryString = new List<KeyValuePair<string, string>>();

            MaxRetryCount = 0;

            HttpClient = httpClient;
        }

        internal Uri BaseUrl { get; }
        internal List<KeyValuePair<string, string>> DefaultQueryString { get; }
        internal Encoding Encoding { get; } = new UTF8Encoding(false);

        internal HttpClient HttpClient { get; private set; }

        public int MaxRetryCount
        {
            get { return _maxRetryCount; }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                _maxRetryCount = value;
            }
        }

        public bool ThrowApiExceptions { get; set; }

        internal JsonSerializer Serializer { get; }

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
}