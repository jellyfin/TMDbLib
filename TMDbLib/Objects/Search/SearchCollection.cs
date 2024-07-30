using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TMDbLib.Objects.Search
{
    public class SearchCollection : SearchBase
    {
        // Property to hold additional data from the JSON
        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData;
        
        [JsonProperty("adult")]
        public bool Adult { get; set; }
        
        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }
        
        private string _name;
        private string _originalName;

        [JsonProperty("name")]
        public string Name
        {
            get
            {
                // If _name is not set, attempt to retrieve the "title" property from additional data
                if (_name == null && _additionalData != null && _additionalData.TryGetValue("title", out var nameToken))
                {
                    return nameToken.ToString();
                }

                return _name;
            }
            set => _name = value;
        }
        
        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_name")]
        public string OriginalName
        {
            get
            {
                // If _originalName is not set, attempt to retrieve the "original_title" property from additional data
                if (_originalName == null && _additionalData != null &&
                    _additionalData.TryGetValue("original_title", out var originalNameToken))
                {
                    return originalNameToken.ToString();
                }

                return _originalName;
            }
            set => _originalName = value;
        }
        
        [JsonProperty("overview")]
        public string Overview { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }
    }
}