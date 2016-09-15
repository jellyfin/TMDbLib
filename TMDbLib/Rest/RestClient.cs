using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using System.Net;

namespace TMDbLib.Rest
{
    internal class RestClient
    {
        private int _maxRetryCount;
        private IWebProxy _proxy;

        public RestClient(Uri baseUrl, JsonSerializer serializer, IWebProxy proxy = null)
        {
            BaseUrl = baseUrl;
            Serializer = serializer;
            DefaultQueryString = new List<KeyValuePair<string, string>>();

            MaxRetryCount = 0;
            _proxy = proxy;
        }

        internal Uri BaseUrl { get; }
        internal List<KeyValuePair<string, string>> DefaultQueryString { get; }
        internal Encoding Encoding { get; } = new UTF8Encoding(false);
        internal IWebProxy Proxy { get { return _proxy; } }

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

        internal JsonSerializer Serializer { get; }

        public void AddDefaultQueryString(string key, string value)
        {
            DefaultQueryString.Add(new KeyValuePair<string, string>(key, value));
        }

        public RestRequest Create(string endpoint)
        {
            return new RestRequest(this, endpoint);
        }
    }
}