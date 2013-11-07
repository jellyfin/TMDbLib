using RestSharp;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        private T SearchMethod<T>(string method, string query, int page, string language = null, bool? includeAdult = null, int year = 0, string dateFormat = null) where T : new()
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

        public SearchContainer<SearchMovie> SearchMovie(string query, int page = 0, bool includeAdult = false, int year = 0)
        {
            return SearchMovie(query, DefaultLanguage, page, includeAdult, year);
        }

        public SearchContainer<SearchMovie> SearchMovie(string query, string language, int page = 0, bool includeAdult = false, int year = 0)
        {
            return SearchMethod<SearchContainer<SearchMovie>>("movie", query, page, language, includeAdult, year, "yyyy-MM-dd");
        }

        public SearchContainer<SearchResultCollection> SearchCollection(string query, int page = 0)
        {
            return SearchCollection(query, DefaultLanguage, page);
        }

        public SearchContainer<SearchResultCollection> SearchCollection(string query, string language, int page = 0)
        {
            return SearchMethod<SearchContainer<SearchResultCollection>>("collection", query, page, language);
        }

        public SearchContainer<SearchPerson> SearchPerson(string query, int page = 0, bool includeAdult = false)
        {
            return SearchMethod<SearchContainer<SearchPerson>>("person", query, page, includeAdult: includeAdult);
        }

        public SearchContainer<SearchList> SearchList(string query, int page = 0, bool includeAdult = false)
        {
            return SearchMethod<SearchContainer<SearchList>>("list", query, page, includeAdult: includeAdult);
        }

        public SearchContainer<SearchCompany> SearchCompany(string query, int page = 0)
        {
            return SearchMethod<SearchContainer<SearchCompany>>("company", query, page);
        }

        public SearchContainer<SearchKeyword> SearchKeyword(string query, int page = 0)
        {
            return SearchMethod<SearchContainer<SearchKeyword>>("keyword", query, page);
        }
    }
}