using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Keyword GetKeyword(int keywordId)
        {
            RestQueryBuilder req = new RestQueryBuilder("keyword/{keywordId}");
            req.AddUrlSegment("keywordId", keywordId.ToString());

            ResponseContainer<Keyword> resp = _client.Get<Keyword>(req);

            return resp.Data;
        }

        public SearchContainer<MovieResult> GetKeywordMovies(int keywordId, int page = 0)
        {
            return GetKeywordMovies(keywordId, DefaultLanguage, page);
        }

        public SearchContainer<MovieResult> GetKeywordMovies(int keywordId, string language, int page = 0)
        {
            RestQueryBuilder req = new RestQueryBuilder("keyword/{keywordId}/movies");
            req.AddUrlSegment("keywordId", keywordId.ToString());

            if (language != null)
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());

            ResponseContainer<SearchContainer<MovieResult>> resp = _client.Get<SearchContainer<MovieResult>>(req);

            return resp.Data;
        }
    }
}