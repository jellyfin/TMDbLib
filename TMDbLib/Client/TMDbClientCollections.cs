using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<T> GetCollectionMethodInternal<T>(int collectionId, CollectionMethods collectionMethod, string language = null, CancellationToken cancellationToken = default)
        where T : new()
    {
        RestRequest req = _client.Create("collection/{collectionId}/{method}");
        req.AddUrlSegment("collectionId", collectionId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", collectionMethod.GetDescription());

        if (language is not null)
        {
            req.AddParameter("language", language);
        }

        T resp = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves a collection by its TMDb ID.
    /// </summary>
    /// <param name="collectionId">The TMDb ID of the collection.</param>
    /// <param name="extraMethods">Additional data to include in the response using the append_to_response pattern.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The collection object, or null if not found.</returns>
    public async Task<Collection> GetCollectionAsync(int collectionId, CollectionMethods extraMethods = CollectionMethods.Undefined, CancellationToken cancellationToken = default)
    {
        return await GetCollectionAsync(collectionId, DefaultLanguage, null, extraMethods, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a collection by its TMDb ID with language and image language options.
    /// </summary>
    /// <param name="collectionId">The TMDb ID of the collection.</param>
    /// <param name="language">The ISO 639-1 language code for the collection text. Defaults to the client's DefaultLanguage.</param>
    /// <param name="includeImageLanguages">A comma-separated list of ISO 639-1 language codes for including images. Defaults to the client's DefaultImageLanguage.</param>
    /// <param name="extraMethods">Additional data to include in the response using the append_to_response pattern.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The collection object, or null if not found.</returns>
    public async Task<Collection> GetCollectionAsync(int collectionId, string language, string includeImageLanguages, CollectionMethods extraMethods = CollectionMethods.Undefined, CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("collection/{collectionId}");
        req.AddUrlSegment("collectionId", collectionId.ToString(CultureInfo.InvariantCulture));

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        includeImageLanguages ??= DefaultImageLanguage;
        if (!string.IsNullOrWhiteSpace(includeImageLanguages))
        {
            req.AddParameter("include_image_language", includeImageLanguages);
        }

        string appends = string.Join(
            ",",
            Enum.GetValues(typeof(CollectionMethods))
                                         .OfType<CollectionMethods>()
                                         .Except([CollectionMethods.Undefined])
                                         .Where(s => extraMethods.HasFlag(s))
                                         .Select(s => s.GetDescription()));

        if (appends != string.Empty)
        {
            req.AddParameter("append_to_response", appends);
        }

        // req.DateFormat = "yyyy-MM-dd";

        using RestResponse<Collection> response = await req.Get<Collection>(cancellationToken).ConfigureAwait(false);

        if (!response.IsValid)
        {
            return null;
        }

        Collection item = await response.GetDataObject().ConfigureAwait(false);

        if (item is not null)
        {
            item.Overview = WebUtility.HtmlDecode(item.Overview);
        }

        return item;
    }

    /// <summary>
    /// Retrieves images for a specific collection.
    /// </summary>
    /// <param name="collectionId">The TMDb ID of the collection.</param>
    /// <param name="language">The ISO 639-1 language code for filtering images. If null, uses the client's DefaultLanguage.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>An object containing the collection's images (posters and backdrops).</returns>
    public async Task<ImagesWithId> GetCollectionImagesAsync(int collectionId, string language = null, CancellationToken cancellationToken = default)
    {
        return await GetCollectionMethodInternal<ImagesWithId>(collectionId, CollectionMethods.Images, language, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all translations for a specific collection.
    /// </summary>
    /// <param name="collectionId">The TMDb ID of the collection.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A container with all available translations for the collection.</returns>
    public async Task<TranslationsContainer> GetCollectionTranslationsAsync(int collectionId, CancellationToken cancellationToken = default)
    {
        return await GetCollectionMethodInternal<TranslationsContainer>(collectionId, CollectionMethods.Translations, null, cancellationToken).ConfigureAwait(false);
    }
}
