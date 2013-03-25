using System;
using System.Linq;
using RestSharp;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Utilitiess;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Company GetCompany(int id, CompanyMethods extraMethods = CompanyMethods.Undefined)
        {
            RestRequest req = new RestRequest("company/{id}");
            req.AddUrlSegment("id", id.ToString());

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(CompanyMethods))
                                             .OfType<CompanyMethods>()
                                             .Except(new[] { CompanyMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<Company> resp = _client.Get<Company>(req);

            return resp.Data;
        }

        private T GetCompanyMethod<T>(int id, CompanyMethods companyMethod, int page = -1, string language = null) where T : new()
        {
            RestRequest req = new RestRequest("company/{id}/{method}");
            req.AddUrlSegment("id", id.ToString());
            req.AddUrlSegment("method", companyMethod.GetDescription());

            if (page >= 1)
                req.AddParameter("page", page);
            if (language != null)
                req.AddParameter("language", language);

            IRestResponse<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        public SearchContainerWithId<MovieResult> GetCompanyMovies(int id, int page = -1)
        {
            return GetCompanyMovies(id, DefaultLanguage, page);
        }

        public SearchContainerWithId<MovieResult> GetCompanyMovies(int id, string language, int page = -1)
        {
            return GetCompanyMethod<SearchContainerWithId<MovieResult>>(id, CompanyMethods.Movies, page, language);
        }
    }
}