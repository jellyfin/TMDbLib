using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Person> GetLatestPersonAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("person/latest");

            // TODO: Dateformat?
            //req.DateFormat = "yyyy-MM-dd";

            RestResponse<Person> resp = await req.ExecuteGet<Person>(cancellationToken).ConfigureAwait(false);

            return resp;
        }

        public async Task<Person> GetPersonAsync(int personId, PersonMethods extraMethods = PersonMethods.Undefined, CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("person/{personId}");
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

            RestResponse<Person> resp = await req.ExecuteGet<Person>(cancellationToken).ConfigureAwait(false);

            if (!resp.IsValid)
            {
                return null;
            }

            Person item = await resp.GetDataObject().ConfigureAwait(false);

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (item != null)
            {
                if (item.Images != null)
                    item.Images.Id = item.Id;

                if (item.TvCredits != null)
                    item.TvCredits.Id = item.Id;

                if (item.MovieCredits != null)
                    item.MovieCredits.Id = item.Id;
            }

            return item;
        }

        public async Task<List<Change>> GetPersonChangesAsync(int personId, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            ChangesContainer changesContainer = await GetPersonMethod<ChangesContainer>(personId, PersonMethods.Changes, startDate: startDate, endDate: endDate, dateFormat: "yyyy-MM-dd HH:mm:ss UTC", cancellationToken: cancellationToken).ConfigureAwait(false);
            return changesContainer.Changes;
        }

        public async Task<ExternalIdsPerson> GetPersonExternalIdsAsync(int personId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetPersonMethod<ExternalIdsPerson>(personId, PersonMethods.ExternalIds, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<ProfileImages> GetPersonImagesAsync(int personId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetPersonMethod<ProfileImages>(personId, PersonMethods.Images, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<SearchContainer<PersonResult>> GetPersonListAsync(PersonListType type, int page = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req;
            switch (type)
            {
                case PersonListType.Popular:
                    req = _client.Create("person/popular");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type));
            }

            if (page >= 1)
                req.AddParameter("page", page.ToString());

            // TODO: Dateformat?
            //req.DateFormat = "yyyy-MM-dd";

            RestResponse<SearchContainer<PersonResult>> resp = await req.ExecuteGet<SearchContainer<PersonResult>>(cancellationToken).ConfigureAwait(false);

            return resp;
        }

        private async Task<T> GetPersonMethod<T>(int personId, PersonMethods personMethod, string dateFormat = null, string country = null, string language = null,
                                        int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default(CancellationToken)) where T : new()
        {
            RestRequest req = _client.Create("person/{personId}/{method}");
            req.AddUrlSegment("personId", personId.ToString());
            req.AddUrlSegment("method", personMethod.GetDescription());

            // TODO: Dateformat?
            //if (dateFormat != null)
            //    req.DateFormat = dateFormat;

            if (country != null)
                req.AddParameter("country", country);
            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (startDate.HasValue)
                req.AddParameter("startDate", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                req.AddParameter("endDate", endDate.Value.ToString("yyyy-MM-dd"));

            RestResponse<T> resp = await req.ExecuteGet<T>(cancellationToken).ConfigureAwait(false);

            return resp;
        }

        public async Task<MovieCredits> GetPersonMovieCreditsAsync(int personId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetPersonMovieCreditsAsync(personId, DefaultLanguage, cancellationToken).ConfigureAwait(false);
        }

        public async Task<MovieCredits> GetPersonMovieCreditsAsync(int personId, string language, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetPersonMethod<MovieCredits>(personId, PersonMethods.MovieCredits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, int page, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetPersonTaggedImagesAsync(personId, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
        }

        public async Task<SearchContainerWithId<TaggedImage>> GetPersonTaggedImagesAsync(int personId, string language, int page, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetPersonMethod<SearchContainerWithId<TaggedImage>>(personId, PersonMethods.TaggedImages, language: language, page: page, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<TvCredits> GetPersonTvCreditsAsync(int personId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetPersonTvCreditsAsync(personId, DefaultLanguage, cancellationToken).ConfigureAwait(false);
        }

        public async Task<TvCredits> GetPersonTvCreditsAsync(int personId, string language, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await GetPersonMethod<TvCredits>(personId, PersonMethods.TvCredits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}