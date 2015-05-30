using System;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Collection> GetCollection(int collectionId, CollectionMethods extraMethods = CollectionMethods.Undefined)
        {
            return await GetCollection(collectionId, DefaultLanguage, extraMethods);
        }

        public async Task<Collection> GetCollection(int collectionId, string language, CollectionMethods extraMethods = CollectionMethods.Undefined)
        {
            RestRequest req = new RestRequest("collection/{collectionId}");
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

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<Collection> resp = await _client.ExecuteGetTaskAsync<Collection>(req).ConfigureAwait(false);

            return resp.Data;
        }

        private async Task<T> GetCollectionMethod<T>(int collectionId, CollectionMethods collectionMethod, string language = null) where T : new()
        {
            RestRequest req = new RestRequest("collection/{collectionId}/{method}");
            req.AddUrlSegment("collectionId", collectionId.ToString());
            req.AddUrlSegment("method", collectionMethod.GetDescription());

            if (language != null)
                req.AddParameter("language", language);

            IRestResponse<T> resp = await _client.ExecuteGetTaskAsync<T>(req).ConfigureAwait(false);

            return resp.Data;
        }

        public async Task<ImagesWithId> GetCollectionImages(int collectionId, string language = null)
        {
            return await GetCollectionMethod<ImagesWithId>(collectionId, CollectionMethods.Images, language);
        }
    }
}
