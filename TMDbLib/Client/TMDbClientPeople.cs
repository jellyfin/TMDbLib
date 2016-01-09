using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Person> GetPerson(int personId, PersonMethods extraMethods = PersonMethods.Undefined)
        {
            TmdbRestRequest req = _client2.Create("person/{personId}");
            req.AddUrlSegment("personId", personId.ToString());

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(PersonMethods))
                                             .OfType<PersonMethods>()
                                             .Except(new[] { PersonMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            // TODO: Dateformat?
            //req.DateFormat = "yyyy-MM-dd";

            TmdbRestResponse<Person> resp = await req.ExecuteGetTaskAsync<Person>().ConfigureAwait(false);

            Person item = await resp.GetDataObject();

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (item != null)
            {
                if (item.Images != null)
                    item.Images.Id = item.Id;

                if (item.Credits != null)
                    item.Credits.Id = item.Id;
            }

            return item;
        }

        private async Task<T> GetPersonMethod<T>(int personId, PersonMethods personMethod, string dateFormat = null, string country = null, string language = null,
                                        int page = 0, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            TmdbRestRequest req =  _client2.Create("person/{personId}/{method}");
            req.AddUrlSegment("personId", personId.ToString());
            req.AddUrlSegment("method", personMethod.GetDescription());

            // TODO: Dateformat?
            //if (dateFormat != null)
            //    req.DateFormat = dateFormat;

            if (country != null)
                req.AddParameter("country", country);
            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (startDate.HasValue)
                req.AddParameter("startDate", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                req.AddParameter("endDate", endDate.Value.ToString("yyyy-MM-dd"));

            TmdbRestResponse<T> resp = await req.ExecuteGetTaskAsync<T>().ConfigureAwait(false);

            return resp;
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
            TmdbRestRequest req;
            switch (type)
            {
                case PersonListType.Popular:
                    req =  _client2.Create("person/popular");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }

            if (page >= 1)
                req.AddParameter("page", page.ToString());

            // TODO: Dateformat?
            //req.DateFormat = "yyyy-MM-dd";

            TmdbRestResponse<SearchContainer<PersonResult>> resp = await req.ExecuteGet<SearchContainer<PersonResult>>().ConfigureAwait(false);

            return resp;
        }

        public async Task<Person> GetLatestPerson()
        {
            TmdbRestRequest req =  _client2.Create("person/latest");

            // TODO: Dateformat?
            //req.DateFormat = "yyyy-MM-dd";

            TmdbRestResponse<Person> resp = await req.ExecuteGet<Person>().ConfigureAwait(false);

            return resp;
        }
    }
}