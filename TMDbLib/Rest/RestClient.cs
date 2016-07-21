using System;
using System.Collections.Generic;

namespace TMDbLib.Rest
{
    internal class RestClient
    {
        private int _maxRetryCount;

        public RestClient(Uri baseUrl)
        {
            BaseUrl = baseUrl;
            DefaultQueryString = new List<KeyValuePair<string, string>>();

            MaxRetryCount = 0;
        }

        internal Uri BaseUrl { get; }
        internal List<KeyValuePair<string, string>> DefaultQueryString { get; }

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