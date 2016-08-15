using System;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Company> GetCompanyAsync(int companyId, CompanyMethods extraMethods = CompanyMethods.Undefined)
        {
            RestRequest req = _client.Create("company/{companyId}");
            req.AddUrlSegment("companyId", companyId.ToString());

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(CompanyMethods))
                                             .OfType<CompanyMethods>()
                                             .Except(new[] { CompanyMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            //req.DateFormat = "yyyy-MM-dd";

            RestResponse<Company> resp = await req.ExecuteGet<Company>().ConfigureAwait(false);

            return resp;
        }

        private async Task<T> GetCompanyMethod<T>(int companyId, CompanyMethods companyMethod, int page = 0, string language = null) where T : new()
        {
            RestRequest req = _client.Create("company/{companyId}/{method}");
            req.AddUrlSegment("companyId", companyId.ToString());
            req.AddUrlSegment("method", companyMethod.GetDescription());

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            RestResponse<T> resp = await req.ExecuteGet<T>().ConfigureAwait(false);

            return resp;
        }

        public async Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, int page = 0)
        {
            return await GetCompanyMoviesAsync(companyId, DefaultLanguage, page).ConfigureAwait(false);
        }

        public async Task<SearchContainerWithId<SearchMovie>> GetCompanyMoviesAsync(int companyId, string language, int page = 0)
        {
            return await GetCompanyMethod<SearchContainerWithId<SearchMovie>>(companyId, CompanyMethods.Movies, page, language).ConfigureAwait(false);
        }
    }
}