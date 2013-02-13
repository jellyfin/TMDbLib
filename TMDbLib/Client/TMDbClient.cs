using System;
using System.IO;
using RestSharp;
using RestSharp.Deserializers;
using TMDbLib.Objects.General;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public string ApiKey { get; private set; }

        /// <summary>
        ///     ISO 639-1 code.
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        ///     ISO 3166-1 code.
        /// </summary>
        public string DefaultCountry { get; set; }

        public TMDbConfig Config
        {
            get
            {
                if (!HasConfig)
                    throw new InvalidOperationException("Call GetConfig() or SetConfig() first");
                return _config;
            }
            private set { _config = value; }
        }

        public bool HasConfig { get; private set; }

        private const string ApiVersion = "3";
        private const string ProductionUrl = "api.themoviedb.org";
        private RestClient _client;
        private TMDbConfig _config;

        public TMDbClient(string apiKey, bool useSsl = false, string baseUrl = ProductionUrl)
        {
            DefaultLanguage = null;
            DefaultCountry = null;

            Initialize(baseUrl, useSsl, apiKey);
        }

        private void Initialize(string baseUrl, bool useSsl, string apiKey)
        {
            ApiKey = apiKey;

            string httpScheme = useSsl ? "https" : "http";
            _client = new RestClient(httpScheme + "://" + baseUrl + "/" + ApiVersion + "/");
            _client.AddDefaultParameter("api_key", apiKey);

            _client.ClearHandlers();
            _client.AddHandler("application/json", new JsonDeserializer());
        }

        public void GetConfig()
        {
            RestRequest req = new RestRequest("configuration");
            IRestResponse<TMDbConfig> resp = _client.Get<TMDbConfig>(req);

            if (resp.ResponseStatus != ResponseStatus.Completed)
                throw new Exception("Error");

            // Store config
            Config = resp.Data;
            HasConfig = true;
        }

        public void SetConfig(TMDbConfig config)
        {
            // Store config
            Config = config;
            HasConfig = true;
        }

        public Uri GetImageUrl(string size, string filePath, bool useSsl = false)
        {
            string baseUrl = useSsl ? Config.Images.SecureBaseUrl : Config.Images.BaseUrl;
            return new Uri(baseUrl + size + filePath);
        }
    }
}