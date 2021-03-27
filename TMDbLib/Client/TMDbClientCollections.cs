using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Collection> GetCollectionAsync(int collectionId, CollectionMethods extraMethods = CollectionMethods.Undefined, CancellationToken cancellationToken = default)
        {
            return await GetCollectionAsync(collectionId, DefaultLanguage, null, extraMethods, cancellationToken).ConfigureAwait(false);
        }

        public async Task<Collection> GetCollectionAsync(int collectionId, string language, string includeImageLanguages, CollectionMethods extraMethods = CollectionMethods.Undefined, CancellationToken cancellationToken = default)
        {
            RestRequest req = _client.Create("collection/{collectionId}");
            req.AddUrlSegment("collectionId", collectionId.ToString());

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            includeImageLanguages = includeImageLanguages ?? DefaultImageLanguage;
            if (!string.IsNullOrWhiteSpace(includeImageLanguages))
                req.AddParameter("include_image_language", includeImageLanguages);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(CollectionMethods))
                                             .OfType<CollectionMethods>()
                                             .Except(new[] { CollectionMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            //req.DateFormat = "yyyy-MM-dd";

            RestResponse<Collection> response = await req.ExecuteGet<Collection>(cancellationToken).ConfigureAwait(false);

            if (!response.IsValid)
                return null;

            Collection item = await response.GetDataObject().ConfigureAwait(false);

            if (item != null)
                item.Overview = WebUtility.HtmlDecode(item.Overview);

            return item;
        }

        public async Task<ImagesWithId> GetCollectionImagesAsync(int collectionId, string language = null, CancellationToken cancellationToken = default)
        {
            return await GetCollectionMethod<ImagesWithId>(collectionId, CollectionMethods.Images, language, cancellationToken).ConfigureAwait(false);
        }

        private async Task<T> GetCollectionMethod<T>(int collectionId, CollectionMethods collectionMethod, string language = null, CancellationToken cancellationToken = default) where T : new()
        {
            RestRequest req = _client.Create("collection/{collectionId}/{method}");
            req.AddUrlSegment("collectionId", collectionId.ToString());
            req.AddUrlSegment("method", collectionMethod.GetDescription());

            if (language != null)
                req.AddParameter("language", language);

            RestResponse<T> resp = await req.ExecuteGet<T>(cancellationToken).ConfigureAwait(false);

            return resp;
        }
    }
}