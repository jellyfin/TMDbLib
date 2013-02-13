using RestSharp;
using TMDbLib.Objects.Search;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        private T SearchMethod<T>(string method, string query, int page, string language = null, bool? includeAdult = null, int year = -1, string dateFormat = null) where T : new()
        {
            RestRequest req = new RestRequest("search/{method}");
            req.AddUrlSegment("method", method);
            req.AddParameter("query", query);

            if (language != null)
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);
            if (year >= 1)
                req.AddParameter("year", year);
            if (includeAdult.HasValue)
                req.AddParameter("include_adult", includeAdult.Value ? "true" : "false");

            if (dateFormat != null)
                req.DateFormat = dateFormat;
            
            IRestResponse<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        public SearchContainer<SearchResultMovie> SearchMovie(string query, int page = -1, bool includeAdult = false, int year = -1)
        {
            return SearchMovie(query, DefaultLanguage, page, includeAdult, year);
        }

        public SearchContainer<SearchResultMovie> SearchMovie(string query, string language, int page = -1, bool includeAdult = false, int year = -1)
        {
            return SearchMethod<SearchContainer<SearchResultMovie>>("movie", query, page, language, includeAdult, year, "yyyy-MM-dd");
        }

        public SearchContainer<SearchResultCollection> SearchCollection(string query, int page = -1)
        {
            return SearchCollection(query, DefaultLanguage, page);
        }

        public SearchContainer<SearchResultCollection> SearchCollection(string query, string language, int page = -1)
        {
            return SearchMethod<SearchContainer<SearchResultCollection>>("collection", query, page, language);
        }

        public SearchContainer<SearhcResultPerson> SearchPerson(string query, int page = -1, bool includeAdult = false)
        {
            return SearchMethod<SearchContainer<SearhcResultPerson>>("person", query, page, includeAdult: includeAdult);
        }

        public SearchContainer<SearhcResultList> SearchList(string query, int page = -1, bool includeAdult = false)
        {
            return SearchMethod<SearchContainer<SearhcResultList>>("list", query, page, includeAdult: includeAdult);
        }

        public SearchContainer<SearhcResultCompany> SearchCompany(string query, int page = -1)
        {
            return SearchMethod<SearchContainer<SearhcResultCompany>>("company", query, page);
        }

        public SearchContainer<SearhcResultKeyword> SearchKeyword(string query, int page = -1)
        {
            return SearchMethod<SearchContainer<SearhcResultKeyword>>("keyword", query, page);
        }
    }
}