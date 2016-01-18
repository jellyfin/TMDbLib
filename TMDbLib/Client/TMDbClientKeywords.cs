using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Keyword> GetKeyword(int keywordId)
        {
            RestRequest req = _client2.Create("keyword/{keywordId}");
            req.AddUrlSegment("keywordId", keywordId.ToString());

            RestResponse<Keyword> resp = await req.ExecuteGet<Keyword>().ConfigureAwait(false);

            return resp;
        }

        public async Task<SearchContainer<MovieResult>> GetKeywordMovies(int keywordId, int page = 0)
        {
            return await GetKeywordMovies(keywordId, DefaultLanguage, page);
        }

        public async Task<SearchContainer<MovieResult>> GetKeywordMovies(int keywordId, string language, int page = 0)
        {
            RestRequest req = _client2.Create("keyword/{keywordId}/movies");
            req.AddUrlSegment("keywordId", keywordId.ToString());

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());

            RestResponse<SearchContainer<MovieResult>> resp = await req.ExecuteGet<SearchContainer<MovieResult>>().ConfigureAwait(false);

            return resp;
        }
    }
}