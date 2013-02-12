using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;

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

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (resp.Data.Movies != null)
                resp.Data.Movies.Id = resp.Data.Id;

            return resp.Data;
        }

        private T GetCollectionMethod<T>(int id, CompanyMethods companyMethod, int page = -1, string dateFormat = null, string country = null, string language = null) where T : new()
        {
            RestRequest req = new RestRequest("company/{id}/{method}");
            req.AddUrlSegment("id", id.ToString());
            req.AddUrlSegment("method", companyMethod.GetDescription());

            if (dateFormat != null)
                req.DateFormat = dateFormat;

            if (page >= 1)
                req.AddParameter("page", page);
            if (country != null)
                req.AddParameter("country", country);
            if (language != null)
                req.AddParameter("language", language);

            IRestResponse<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        public MovieResultContainer GetCompanyMovies(int id, int page = -1, string language = null)
        {
            return GetCollectionMethod<MovieResultContainer>(id, CompanyMethods.Movies, page: page, language: language);
        }
    }

    public class CompanyMovies
    {
        public int id { get; set; }
        public int page { get; set; }
        public List<MovieResult> results { get; set; }
        public int total_pages { get; set; }
        public int total_results { get; set; }
    }

    public class ParentCompany
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string LogoPath { get; set; }
    }

    public class Company
    {
        public string Description { get; set; }
        public string Headquarters { get; set; }
        public string Homepage { get; set; }
        public int Id { get; set; }
        public string LogoPath { get; set; }
        public string Name { get; set; }
        public ParentCompany ParentCompany { get; set; }
        public MovieResultContainer Movies { get; set; }
    }
}