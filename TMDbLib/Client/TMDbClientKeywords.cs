using RestSharp;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Keyword GetKeyword(int id, int page = -1)
        {
            return GetKeyword(id, DefaultLanguage, page);
        }

        public Keyword GetKeyword(int id, string language, int page = -1)
        {
            RestRequest req = new RestRequest("keyword/{id}");
            req.AddUrlSegment("id", id.ToString());

            if (language != null)
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);

            IRestResponse<Keyword> resp = _client.Get<Keyword>(req);

            return resp.Data;
        }

        public SearchContainer<MovieResult> GetKeywordMovies(int id, int page = -1)
        {
            return GetKeywordMovies(id, DefaultLanguage, page);
        }

        public SearchContainer<MovieResult> GetKeywordMovies(int id, string language, int page = -1)
        {
            RestRequest req = new RestRequest("keyword/{id}/movies");
            req.AddUrlSegment("id", id.ToString());

            if (language != null)
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);

            IRestResponse<SearchContainer<MovieResult>> resp = _client.Get<SearchContainer<MovieResult>>(req);

            return resp.Data;
        }
    }
}