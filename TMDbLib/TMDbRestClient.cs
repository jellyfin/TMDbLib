using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLib.Utilities;

namespace TMDbLib
{
    internal class TMDbRestClient
    {
        private string _apiKey;
        private HttpClient _client;
        public int MaxRetryCount { get; set; }
        public int RetryWaitTimeInSeconds { get; set; }

        public TMDbRestClient(string baseUrl, string apiKey)
        {
            _apiKey = apiKey;
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUrl);

            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            MaxRetryCount = 3;
            RetryWaitTimeInSeconds = 10;
        }

        /// <summary>
        /// Executes the specified request and deserializes the response content using the appropriate content handler
        /// </summary>
        /// <typeparam name="T">Target deserialization type</typeparam>
        /// <param name="request">Request to execute</param>
        /// <returns>RestResponse[[T]] with deserialized data in Data property</returns>
        /// <exception cref="UnauthorizedAccessException">Can be thrown if either to provided API key is invalid or when relavant the provided session id does not grant to required access</exception>
        public ResponseContainer<T> Get<T>(RestQueryBuilder uriBuilder)
        {
            uriBuilder.AddParameter("api_key", _apiKey);
            string uri = uriBuilder.GetUri();

            for (int i = 0; i < MaxRetryCount; i++)
            {
                try
                {
                    HttpResponseMessage xy = _client.GetAsync(uri).Result;

                    if (xy.StatusCode == HttpStatusCode.Unauthorized)
                        throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");

                    string content = xy.Content.ReadAsStringAsync().Result;
                    T data = JsonConvert.DeserializeObject<T>(content);

                    return new ResponseContainer<T>
                    {
                        Data = data,
                        Headers = xy.Headers,
                        Content = content
                    };
                }
                catch (WebException)
                {
                    // Retry the call after waiting the configured ammount of time, it gets progressively longer every retry
                    Thread.Sleep((i + 1) * RetryWaitTimeInSeconds * 1000);

                    // Retry
                }
            }

            throw new Exception("Unable to fetch resource");
        }

        public ResponseContainer<T> Delete<T>(RestQueryBuilder uriBuilder)
        {
            uriBuilder.AddParameter("api_key", _apiKey);
            string uri = uriBuilder.GetUri();

            for (int i = 0; i < MaxRetryCount; i++)
            {
                try
                {
                    HttpResponseMessage xy = _client.DeleteAsync(uri).Result;

                    if (xy.StatusCode == HttpStatusCode.Unauthorized)
                        throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");

                    string content = xy.Content.ReadAsStringAsync().Result;
                    T data = JsonConvert.DeserializeObject<T>(content);

                    return new ResponseContainer<T>
                    {
                        Data = data,
                        Headers = xy.Headers,
                        Content = content
                    };
                }
                catch (WebException)
                {
                    // Retry the call after waiting the configured ammount of time, it gets progressively longer every retry
                    Thread.Sleep((i + 1) * RetryWaitTimeInSeconds * 1000);

                    // Retry
                }
            }

            throw new Exception("Unable to fetch resource");
        }

        public ResponseContainer<T> Post<T>(RestQueryBuilder uriBuilder, object bodyObject)
        {
            uriBuilder.AddParameter("api_key", _apiKey);
            string uri = uriBuilder.GetUri();

            string body = JsonConvert.SerializeObject(bodyObject);

            for (int i = 0; i < MaxRetryCount; i++)
            {
                try
                {
                    HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, uri);
                    req.Content = new StringContent(body, Encoding.UTF8, "application/json");
                    req.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage xy = _client.SendAsync(req).Result;

                    if (xy.StatusCode == HttpStatusCode.Unauthorized)
                        throw new UnauthorizedAccessException("Call to TMDb returned unauthorized. Most likely the provided API key is invalid.");

                    string content = xy.Content.ReadAsStringAsync().Result;
                    T data = JsonConvert.DeserializeObject<T>(content);

                    return new ResponseContainer<T>
                    {
                        Data = data,
                        Headers = xy.Headers,
                        Content = content
                    };
                }
                catch (WebException)
                {
                    // Retry the call after waiting the configured ammount of time, it gets progressively longer every retry
                    Thread.Sleep((i + 1) * RetryWaitTimeInSeconds * 1000);

                    // Retry
                }
            }

            throw new Exception("Unable to fetch resource");
        }
    }
}
