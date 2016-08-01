using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, int page = 0)
        {
            return await SearchCollectionAsync(query, DefaultLanguage, page).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchCollection>> SearchCollectionAsync(string query, string language, int page = 0)
        {
            return await SearchMethod<SearchContainer<SearchCollection>>("collection", query, page, language).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchCompany>> SearchCompanyAsync(string query, int page = 0)
        {
            return await SearchMethod<SearchContainer<SearchCompany>>("company", query, page).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchKeyword>> SearchKeywordAsync(string query, int page = 0)
        {
            return await SearchMethod<SearchContainer<SearchKeyword>>("keyword", query, page).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchList>> SearchListAsync(string query, int page = 0, bool includeAdult = false)
        {
            return await SearchMethod<SearchContainer<SearchList>>("list", query, page, includeAdult: includeAdult).ConfigureAwait(false);
        }

        private async Task<T> SearchMethod<T>(string method, string query, int page, string language = null, bool? includeAdult = null, int year = 0, string dateFormat = null) where T : new()
        {
            RestRequest req = _client.Create("search/{method}");
            req.AddUrlSegment("method", method);
            req.AddParameter("query", query);

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (year >= 1)
                req.AddParameter("year", year.ToString());
            if (includeAdult.HasValue)
                req.AddParameter("include_adult", includeAdult.Value ? "true" : "false");

            // TODO: Dateformat?
            //if (dateFormat != null)
            //    req.DateFormat = dateFormat;

            RestResponse<T> resp = await req.ExecuteGet<T>().ConfigureAwait(false);

            return resp;
        }

        public async Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, int page = 0, bool includeAdult = false, int year = 0)
        {
            return await SearchMovieAsync(query, DefaultLanguage, page, includeAdult, year).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchMovie>> SearchMovieAsync(string query, string language, int page = 0, bool includeAdult = false, int year = 0)
        {
            return await SearchMethod<SearchContainer<SearchMovie>>("movie", query, page, language, includeAdult, year, "yyyy-MM-dd").ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, int page = 0, bool includeAdult = false, int year = 0)
        {
            return await SearchMultiAsync(query, DefaultLanguage, page, includeAdult, year).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchBase>> SearchMultiAsync(string query, string language, int page = 0, bool includeAdult = false, int year = 0)
        {
            return await SearchMethod<SearchContainer<SearchBase>>("multi", query, page, language, includeAdult, year, "yyyy-MM-dd").ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchPerson>> SearchPersonAsync(string query, int page = 0, bool includeAdult = false)
        {
            return await SearchMethod<SearchContainer<SearchPerson>>("person", query, page, includeAdult: includeAdult).ConfigureAwait(false);
        }

        public async Task<SearchContainer<SearchTv>> SearchTvShowAsync(string query, int page = 0)
        {
            return await SearchMethod<SearchContainer<SearchTv>>("tv", query, page).ConfigureAwait(false);
        }
    }
}