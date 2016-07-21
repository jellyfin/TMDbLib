using System;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Collection> GetCollectionAsync(int collectionId, CollectionMethods extraMethods = CollectionMethods.Undefined)
        {
            return await GetCollectionAsync(collectionId, DefaultLanguage, extraMethods).ConfigureAwait(false);
        }

        public async Task<Collection> GetCollectionAsync(int collectionId, string language, CollectionMethods extraMethods = CollectionMethods.Undefined)
        {
            RestRequest req = _client.Create("collection/{collectionId}");
            req.AddUrlSegment("collectionId", collectionId.ToString());

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
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

            RestResponse<Collection> resp = await req.ExecuteGet<Collection>().ConfigureAwait(false);

            return resp;
        }

        public async Task<ImagesWithId> GetCollectionImagesAsync(int collectionId, string language = null)
        {
            return await GetCollectionMethod<ImagesWithId>(collectionId, CollectionMethods.Images, language).ConfigureAwait(false);
        }

        private async Task<T> GetCollectionMethod<T>(int collectionId, CollectionMethods collectionMethod, string language = null) where T : new()
        {
            RestRequest req = _client.Create("collection/{collectionId}/{method}");
            req.AddUrlSegment("collectionId", collectionId.ToString());
            req.AddUrlSegment("method", collectionMethod.GetDescription());

            if (language != null)
                req.AddParameter("language", language);

            RestResponse<T> resp = await req.ExecuteGet<T>().ConfigureAwait(false);

            return resp;
        }
    }
}
