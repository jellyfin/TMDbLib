using System.Collections.Generic;
using RestSharp;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Can be used to discover new tv shows matching certain criteria
        /// </summary>
        public SearchContainer<TvShowBase> DiscoverTvShows(DiscoverTv discover = null, string language = null, int page = 0)
        {
            RestRequest request = new RestRequest("discover/tv");

            if (page != 1 && page > 1)
                request.AddParameter("page", page);

            if (!string.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            if (discover != null)
                foreach (KeyValuePair<string, string> parameter in discover.GetAllParameters())
                    request.AddParameter(parameter.Key, parameter.Value);

            IRestResponse<SearchContainer<TvShowBase>> response = _client.Get<SearchContainer<TvShowBase>>(request);
            return response.Data;
        }

        /// <summary>
        /// Can be used to discover movies matching certain criteria
        /// </summary>
        public SearchContainer<SearchMovie> DiscoverMovies(DiscoverMovie discover = null, string language = null, int page = 0)
        {
            RestRequest request = new RestRequest("discover/movie");

            if (page != 1 && page > 1)
                request.AddParameter("page", page);

            if (!string.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            if (discover != null)
                foreach (KeyValuePair<string, string> parameter in discover.GetAllParameters())
                    request.AddParameter(parameter.Key, parameter.Value);

            IRestResponse<SearchContainer<SearchMovie>> response = _client.Get<SearchContainer<SearchMovie>>(request);
            return response.Data;
        }
    }
}
