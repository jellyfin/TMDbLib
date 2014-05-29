using System;
using System.Linq;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Collection GetCollection(int collectionId, CollectionMethods extraMethods = CollectionMethods.Undefined)
        {
            return GetCollection(collectionId, null, extraMethods);
        }

        public Collection GetCollection(int collectionId, string language, CollectionMethods extraMethods = CollectionMethods.Undefined)
        {
            RestQueryBuilder req = new RestQueryBuilder("collection/{collectionId}");
            req.AddUrlSegment("collectionId", collectionId.ToString());

            if (language != null)
                req.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(CollectionMethods))
                                             .OfType<CollectionMethods>()
                                             .Except(new[] { CollectionMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            //req.DateFormat = "yyyy-MM-dd";

            ResponseContainer<Collection> resp = _client.Get<Collection>(req);

            return resp.Data;
        }

        private T GetCollectionMethod<T>(int collectionId, CollectionMethods collectionMethod, string language = null) where T : new()
        {
            RestQueryBuilder req = new RestQueryBuilder("collection/{collectionId}/{method}");
            req.AddUrlSegment("collectionId", collectionId.ToString());
            req.AddUrlSegment("method", collectionMethod.GetDescription());

            if (language != null)
                req.AddParameter("language", language);

            ResponseContainer<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        public ImagesWithId GetCollectionImages(int collectionId, string language = null)
        {
            return GetCollectionMethod<ImagesWithId>(collectionId, CollectionMethods.Images, language);
        }
    }
}