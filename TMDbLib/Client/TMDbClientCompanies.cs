using System;
using System.Linq;
using RestSharp;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Company GetCompany(int companyId, CompanyMethods extraMethods = CompanyMethods.Undefined)
        {
            RestRequest req = new RestRequest("company/{companyId}");
            req.AddUrlSegment("companyId", companyId.ToString());

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

        private T GetCompanyMethod<T>(int companyId, CompanyMethods companyMethod, int page = 0, string language = null) where T : new()
        {
            RestRequest req = new RestRequest("company/{companyId}/{method}");
            req.AddUrlSegment("companyId", companyId.ToString());
            req.AddUrlSegment("method", companyMethod.GetDescription());

            if (page >= 1)
                req.AddParameter("page", page);
            if (language != null)
                req.AddParameter("language", language);

            IRestResponse<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        public SearchContainerWithId<MovieResult> GetCompanyMovies(int companyId, int page = 0)
        {
            return GetCompanyMovies(companyId, DefaultLanguage, page);
        }

        public SearchContainerWithId<MovieResult> GetCompanyMovies(int companyId, string language, int page = 0)
        {
            return GetCompanyMethod<SearchContainerWithId<MovieResult>>(companyId, CompanyMethods.Movies, page, language);
        }
    }
}