using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Person> GetPerson(int personId, PersonMethods extraMethods = PersonMethods.Undefined)
        {
            RestRequest req = new RestRequest("person/{personId}");
            req.AddUrlSegment("personId", personId.ToString());

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(PersonMethods))
                                             .OfType<PersonMethods>()
                                             .Except(new[] { PersonMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<Person> resp = await _client.ExecuteGetTaskAsync<Person>(req).ConfigureAwait(false);

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (resp.Data != null)
            {
                if (resp.Data.Images != null)
                    resp.Data.Images.Id = resp.Data.Id;

                if (resp.Data.Credits != null)
                    resp.Data.Credits.Id = resp.Data.Id;
            }

            return resp.Data;
        }

        private async Task<T> GetPersonMethod<T>(int personId, PersonMethods personMethod, string dateFormat = null, string country = null, string language = null,
                                        int page = 0, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            RestRequest req = new RestRequest("person/{personId}/{method}");
            req.AddUrlSegment("personId", personId.ToString());
            req.AddUrlSegment("method", personMethod.GetDescription());

            if (dateFormat != null)
                req.DateFormat = dateFormat;

            if (country != null)
                req.AddParameter("country", country);
            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);
            if (startDate.HasValue)
                req.AddParameter("startDate", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                req.AddParameter("endDate", endDate.Value.ToString("yyyy-MM-dd"));

            IRestResponse<T> resp = await _client.ExecuteGetTaskAsync<T>(req).ConfigureAwait(false);

            return resp.Data;
        }

        public async Task<MovieCredits> GetPersonMovieCredits(int personId)
        {
            return await GetPersonMovieCredits(personId, DefaultLanguage);
        }

        public async Task<MovieCredits> GetPersonMovieCredits(int personId, string language)
        {
            return await GetPersonMethod<MovieCredits>(personId, PersonMethods.MovieCredits, language: language);
        }

        public async Task<TvCredits> GetPersonTvCredits(int personId)
        {
            return await GetPersonTvCredits(personId, DefaultLanguage);
        }

        public async Task<TvCredits> GetPersonTvCredits(int personId, string language)
        {
            return await GetPersonMethod<TvCredits>(personId, PersonMethods.TvCredits, language: language);
        }

        public async Task<ProfileImages> GetPersonImages(int personId)
        {
            return await GetPersonMethod<ProfileImages>(personId, PersonMethods.Images);
        }

        public async Task<SearchContainer<TaggedImage>> GetPersonTaggedImages(int personId, int page)
        {
            return await  GetPersonTaggedImages(personId, DefaultLanguage, page);
        }

        public async Task<SearchContainer<TaggedImage>> GetPersonTaggedImages(int personId, string language, int page)
        {
            return await GetPersonMethod<SearchContainer<TaggedImage>>(personId, PersonMethods.TaggedImages, language: language, page: page);
        }

        public async Task<ExternalIds> GetPersonExternalIds(int personId)
        {
            return await GetPersonMethod<ExternalIds>(personId, PersonMethods.ExternalIds);
        }
        
        public async Task<List<Change>> GetPersonChanges(int personId, DateTime? startDate = null, DateTime? endDate = null)
        {
            ChangesContainer changesContainer = await GetPersonMethod<ChangesContainer>(personId, PersonMethods.Changes, startDate: startDate, endDate: endDate, dateFormat: "yyyy-MM-dd HH:mm:ss UTC");
            return changesContainer.Changes;
        }

        public async Task<SearchContainer<PersonResult>> GetPersonList(PersonListType type, int page = 0)
        {
            RestRequest req;
            switch (type)
            {
                case PersonListType.Popular:
                    req = new RestRequest("person/popular");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            if (page >= 1)
                req.AddParameter("page", page.ToString());

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<SearchContainer<PersonResult>> resp = await _client.ExecuteGetTaskAsync<SearchContainer<PersonResult>>(req).ConfigureAwait(false);

            return resp.Data;
        }

        public async Task<Person> GetLatestPerson()
        {
            RestRequest req = new RestRequest("person/latest");

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<Person> resp = await _client.ExecuteGetTaskAsync<Person>(req).ConfigureAwait(false);

            return resp.Data;
        }
    }
}