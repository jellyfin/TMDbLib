using System;
using System.Linq;
using RestSharp;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Person;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Collection GetCollection(int id, string language = null, CollectionMethods extraMethods = CollectionMethods.Undefined)
        {
            RestRequest req = new RestRequest("collection/{id}");
            req.AddUrlSegment("id", id.ToString());

            if (language != null)
                req.AddUrlSegment("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(CollectionMethods))
                                             .OfType<CollectionMethods>()
                                             .Except(new[] { CollectionMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            req.DateFormat = "yyyy-MM-dd";
            
            IRestResponse<Collection> resp = _client.Get<Collection>(req);

            return resp.Data;
        }

        private T GetCollectionMethod<T>(int id, CollectionMethods collectionMethod, string dateFormat = null, string country = null, string language = null) where T : new()
        {
            RestRequest req = new RestRequest("collection/{id}/{method}");
            req.AddUrlSegment("id", id.ToString());
            req.AddUrlSegment("method", collectionMethod.GetDescription());

            if (dateFormat != null)
                req.DateFormat = dateFormat;

            if (country != null)
                req.AddParameter("country", country);
            if (language != null)
                req.AddParameter("language", language);

            IRestResponse<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        public ImagesWithId GetCollectionImages(int id, string language = null)
        {
            return GetCollectionMethod<ImagesWithId>(id, CollectionMethods.Images, language: language);
        }
    }
}