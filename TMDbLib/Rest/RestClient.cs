using System;
using System.Collections.Generic;

namespace TMDbLib.Rest
{
    internal class RestClient
    {
        internal Uri BaseUrl { get; set; }
        internal List<KeyValuePair<string, string>> DefaultQueryString { get; }

        public RestClient(Uri baseUrl)
        {
            BaseUrl = baseUrl;
            DefaultQueryString = new List<KeyValuePair<string, string>>();
        }

        public void AddDefaultQueryString(string key, string value)
        {
            DefaultQueryString.Add(new KeyValuePair<string, string>(key, value));
        }

        public TmdbRestRequest Create(string endpoint)
        {
            return new TmdbRestRequest(this, endpoint);
        }
    }
}