using System;
using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Utilities;
using Credits = TMDbLib.Objects.People.Credits;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Person GetPerson(int personId, PersonMethods extraMethods = PersonMethods.Undefined)
        {
            RestQueryBuilder req = new RestQueryBuilder("person/{personId}");
            req.AddUrlSegment("personId", personId.ToString());

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(PersonMethods))
                                             .OfType<PersonMethods>()
                                             .Except(new[] { PersonMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            // TODO: Dateformat
            //req.DateFormat = "yyyy-MM-dd";

            ResponseContainer<Person> resp = _client.Get<Person>(req);

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

        private T GetPersonMethod<T>(int personId, PersonMethods personMethod, string dateFormat = null, string country = null, string language = null,
                                        int page = 0, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            RestQueryBuilder req = new RestQueryBuilder("person/{personId}/{method}");
            req.AddUrlSegment("personId", personId.ToString());
            req.AddUrlSegment("method", personMethod.GetDescription());

            // TODO: Dateformat
            //if (dateFormat != null)
            //    req.DateFormat = dateFormat;

            if (country != null)
                req.AddParameter("country", country);
            if (language != null)
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (startDate.HasValue)
                req.AddParameter("startDate", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                req.AddParameter("endDate", endDate.Value.ToString("yyyy-MM-dd"));

            ResponseContainer<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        public Credits GetPersonCredits(int personId)
        {
            return GetPersonCredits(personId, DefaultLanguage);
        }

        public Credits GetPersonCredits(int personId, string language)
        {
            return GetPersonMethod<Credits>(personId, PersonMethods.Credits, language: language);
        }

        public ProfileImages GetPersonImages(int personId)
        {
            return GetPersonMethod<ProfileImages>(personId, PersonMethods.Images);
        }

        public List<Change> GetPersonChanges(int personId, DateTime? startDate = null, DateTime? endDate = null)
        {
            ChangesContainer changesContainer = GetPersonMethod<ChangesContainer>(personId, PersonMethods.Changes, startDate: startDate, endDate: endDate, dateFormat: "yyyy-MM-dd HH:mm:ss UTC");
            return changesContainer.Changes;
        }
    }
}