using System.Collections.Specialized;
using System.Threading.Tasks;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Rest;

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

        internal async Task<SearchContainer<T>> DiscoverPerform<T>(string endpoint, string language, int page, NameValueCollection parameters)
        {
            TmdbRestRequest request = _client2.Create(endpoint);

            if (page != 1 && page > 1)
                request.AddParameter("page", page.ToString());

            if (!string.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            foreach (string key in parameters.Keys)
                request.AddParameter(key, parameters[key]);

            TmdbRestResponse<SearchContainer<T>> response = await request.ExecuteGet<SearchContainer<T>>();
            return response;
        }
    }
}
