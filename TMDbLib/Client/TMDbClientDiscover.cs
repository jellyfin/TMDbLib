using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Can be used to discover new tv shows matching certain criteria
        /// </summary>
        public DiscoverTv DiscoverTvShows()
        {
            return new DiscoverTv(this);
        }

        /// <summary>
        /// Can be used to discover movies matching certain criteria
        /// </summary>
        public DiscoverMovie DiscoverMovies()
        {
            return new DiscoverMovie(this);
        }

        internal SearchContainer<T> DiscoverPerform<T>(string endpoint, string language, int page, NameValueCollection parameters)
        {
            RestRequest request = new RestRequest(endpoint);

            if (page != 1 && page > 1)
                request.AddParameter("page", page);

            if (!string.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            foreach (string key in parameters.Keys)
                request.AddParameter(key, parameters[key]);

            IRestResponse<SearchContainer<T>> response = _client.Get<SearchContainer<T>>(request);
            return response.Data;
        }
    }
}
