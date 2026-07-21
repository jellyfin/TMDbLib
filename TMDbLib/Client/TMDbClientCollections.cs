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
    private async Task<T?> GetCollectionMethodInternal<T>(int collectionId, CollectionMethods collectionMethod, string? language = null, CancellationToken cancellationToken = default)
        where T : new()
    {
        var req = _client.Create("collection/{collectionId}/{method}");
        req.AddUrlSegment("collectionId", collectionId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", collectionMethod.GetDescription());

        if (language is not null)
        {
            req.AddParameter("language", language);
        }

        var resp = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets a collection by id.
    /// </summary>
    /// <param name="collectionId">The TMDb id of the collection.</param>
    /// <param name="extraMethods">Additional data to append to the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The collection, or null if not found.</returns>
    public async Task<Collection?> GetCollectionAsync(int collectionId, CollectionMethods extraMethods = CollectionMethods.Undefined, CancellationToken cancellationToken = default)
    {
        return await GetCollectionAsync(collectionId, DefaultLanguage, null, extraMethods, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a collection by id with language options.
    /// </summary>
    /// <param name="collectionId">The TMDb id of the collection.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="includeImageLanguages">Comma-separated ISO 639-1 codes for image languages.</param>
    /// <param name="extraMethods">Additional data to append to the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The collection, or null if not found.</returns>
    public async Task<Collection?> GetCollectionAsync(int collectionId, string? language, string? includeImageLanguages, CollectionMethods extraMethods = CollectionMethods.Undefined, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("collection/{collectionId}");
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

        var appends = string.Join(
            ",",
            Enum.GetValues<CollectionMethods>()
                                         .Except([CollectionMethods.Undefined])
                                         .Where(s => extraMethods.HasFlag(s))
                                         .Select(s => s.GetDescription()));

        if (appends != string.Empty)
        {
            req.AddParameter("append_to_response", appends);
        }

        using var response = await req.Get<Collection>(cancellationToken).ConfigureAwait(false);

        if (!response.IsValid)
        {
            return null;
        }

        var item = await response.GetDataObject().ConfigureAwait(false);

        if (item is not null)
        {
            item.Overview = WebUtility.HtmlDecode(item.Overview);
        }

        return item;
    }

    /// <summary>
    /// Gets the images for a collection.
    /// </summary>
    /// <param name="collectionId">The TMDb id of the collection.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The collection's posters and backdrops.</returns>
    public async Task<ImagesWithId?> GetCollectionImagesAsync(int collectionId, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetCollectionMethodInternal<ImagesWithId>(collectionId, CollectionMethods.Images, language, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the translations for a collection.
    /// </summary>
    /// <param name="collectionId">The TMDb id of the collection.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The collection's translations.</returns>
    public async Task<TranslationsContainer?> GetCollectionTranslationsAsync(int collectionId, CancellationToken cancellationToken = default)
    {
        return await GetCollectionMethodInternal<TranslationsContainer>(collectionId, CollectionMethods.Translations, null, cancellationToken).ConfigureAwait(false);
    }
}
