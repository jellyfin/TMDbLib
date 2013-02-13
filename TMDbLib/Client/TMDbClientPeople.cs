using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Person;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Person GetPerson(int id, PersonMethods extraMethods = PersonMethods.Undefined)
        {
            return GetPerson(id, DefaultLanguage, extraMethods);
        }

        public Person GetPerson(int id, string language, PersonMethods extraMethods = PersonMethods.Undefined)
        {
            RestRequest req = new RestRequest("person/{id}");
            req.AddUrlSegment("id", id.ToString());

            if (language != null)
                req.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(PersonMethods))
                                             .OfType<PersonMethods>()
                                             .Except(new[] { PersonMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<Person> resp = _client.Get<Person>(req);

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (resp.Data.Images != null)
                resp.Data.Images.Id = resp.Data.Id;

            if (resp.Data.Credits != null)
                resp.Data.Credits.Id = resp.Data.Id;

            return resp.Data;
        }

        private T GetPersonMethod<T>(int id, PersonMethods personMethod, string dateFormat = null, string country = null, string language = null,
                                        int page = -1, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            RestRequest req = new RestRequest("person/{id}/{method}");
            req.AddUrlSegment("id", id.ToString());
            req.AddUrlSegment("method", personMethod.GetDescription());

            if (dateFormat != null)
                req.DateFormat = dateFormat;

            if (country != null)
                req.AddParameter("country", country);
            if (language != null)
                req.AddParameter("language", language);

            if (page >= 1)
                req.AddParameter("page", page);
            if (startDate.HasValue)
                req.AddParameter("startDate", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                req.AddParameter("endDate", endDate.Value.ToString("yyyy-MM-dd"));

            IRestResponse<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        public Credits GetPersonCredits(int id)
        {
            return GetPersonCredits(id, DefaultLanguage);
        }

        public Credits GetPersonCredits(int id, string language)
        {
            return GetPersonMethod<Credits>(id, PersonMethods.Credits, language: language);
        }

        public ProfileImages GetPersonImages(int id)
        {
            return GetPersonMethod<ProfileImages>(id, PersonMethods.Images);
        }

        public List<Change> GetPersonChanges(int id, DateTime? startDate = null, DateTime? endDate = null)
        {
            ChangesContainer changesContainer = GetPersonMethod<ChangesContainer>(id, PersonMethods.Changes, startDate: startDate, endDate: endDate);
            return changesContainer.Changes;
        }
    }
}