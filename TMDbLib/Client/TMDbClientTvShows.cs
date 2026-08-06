using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLib.Rest;
using TMDbLib.Utilities;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<T?> GetTvShowMethodInternal<T>(int id, TvShowMethods tvShowMethod, string? dateFormat = null, string? language = null, string? includeMediaLanguage = null, int page = 0, CancellationToken cancellationToken = default)
        where T : new()
    {
        var req = _client.Create("tv/{id}/{method}");
        req.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", tvShowMethod.GetDescription());

        // TODO: Dateformat?
        // if (dateFormat is not null)
        //    req.DateFormat = dateFormat;

        if (page > 0)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        includeMediaLanguage ??= DefaultImageLanguage;
        if (!string.IsNullOrWhiteSpace(includeMediaLanguage))
        {
            req.AddParameter(
                tvShowMethod == TvShowMethods.Videos ? "include_video_language" : "include_image_language",
                includeMediaLanguage);
        }

        var resp = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    private async Task<SearchContainer<SearchTv>?> GetTvShowListInternal(int page, string? language, string tvShowListType, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("tv/" + tvShowListType);

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        var response = await req.GetOfT<SearchContainer<SearchTv>>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the most recently created TV show on TMDb.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The latest TV show.</returns>
    public async Task<TvShow?> GetLatestTvShowAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("tv/latest");
        var resp = await req.GetOfT<TvShow>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the current user's account state for a TV show.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's account state.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<AccountState?> GetTvShowAccountStateAsync(int tvShowId, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var req = _client.Create("tv/{tvShowId}/{method}");
        req.AddUrlSegment("tvShowId", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", TvShowMethods.AccountStates.GetDescription());
        AddSessionId(req, SessionType.UserSession);

        return await req.GetOfT<AccountState>(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the alternative titles for a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's alternative titles.</returns>
    public async Task<ResultContainer<AlternativeTitle>?> GetTvShowAlternativeTitlesAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<ResultContainer<AlternativeTitle>>(id, TvShowMethods.AlternativeTitles, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a TV show by id.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="extraMethods">Additional methods to append to the response.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="includeImageLanguage">Comma-separated ISO 639-1 codes for image languages.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show.</returns>
    public async Task<TvShow?> GetTvShowAsync(int id, TvShowMethods extraMethods = TvShowMethods.Undefined, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
    {
        if (extraMethods.HasFlag(TvShowMethods.AccountStates))
        {
            RequireSessionId(SessionType.UserSession);
        }

        var req = _client.Create("tv/{id}");
        req.AddUrlSegment("id", id.ToString(CultureInfo.InvariantCulture));

        if (extraMethods.HasFlag(TvShowMethods.AccountStates))
        {
            AddSessionId(req, SessionType.UserSession);
        }

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        if (!string.IsNullOrWhiteSpace(includeImageLanguage))
        {
            req.AddParameter("include_image_language", includeImageLanguage);
        }

        var appends = string.Join(
            ",",
            Enum.GetValues<TvShowMethods>()
                                         .Except([TvShowMethods.Undefined])
                                         .Where(s => extraMethods.HasFlag(s))
                                         .Select(s => s.GetDescription()));

        if (appends != string.Empty)
        {
            req.AddParameter("append_to_response", appends);
        }

        using var response = await req.Get<TvShow>(cancellationToken).ConfigureAwait(false);

        if (!response.IsValid)
        {
            return null;
        }

        var item = await response.GetDataObject().ConfigureAwait(false);

        // No data to patch up so return
        if (item is null)
        {
            return null;
        }

        // Patch up data, so that the end user won't notice that we share objects between request-types.
        if (item.Translations is not null)
        {
            item.Translations.Id = id;
        }

        if (item.AccountStates is not null)
        {
            item.AccountStates.Id = id;
        }

        if (item.ExternalIds is not null)
        {
            item.ExternalIds.Id = id;
        }

        return item;
    }

    /// <summary>
    /// Gets the content ratings for a TV show by country.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The content ratings by country.</returns>
    public async Task<ResultContainer<ContentRating>?> GetTvShowContentRatingsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<ResultContainer<ContentRating>>(id, TvShowMethods.ContentRatings, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the credits for a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's cast and crew.</returns>
    public async Task<Credits?> GetTvShowCreditsAsync(int id, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<Credits>(id, TvShowMethods.Credits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the aggregate credits for a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's aggregate credits.</returns>
    public async Task<CreditsAggregate?> GetAggregateCredits(int id, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<CreditsAggregate>(id, TvShowMethods.CreditsAggregate, language: language, page: 0, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the external ids for a TV show (IMDb, TVDB, Facebook, Twitter, etc.).
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's external ids.</returns>
    public async Task<ExternalIdsTvShow?> GetTvShowExternalIdsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<ExternalIdsTvShow>(id, TvShowMethods.ExternalIds, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the images for a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="language">The ISO 639-1 language code. Images may contain language-specific text.</param>
    /// <param name="includeImageLanguage">Comma-separated ISO 639-1 codes for image fallback (e.g. "en,null").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's posters, backdrops, and logos.</returns>
    public async Task<ImagesWithId?> GetTvShowImagesAsync(int id, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<ImagesWithId>(id, TvShowMethods.Images, language: language, includeMediaLanguage: includeImageLanguage, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the user reviews for a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's reviews.</returns>
    public async Task<SearchContainerWithId<ReviewBase>?> GetTvShowReviewsAsync(int id, string? language = null, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<SearchContainerWithId<ReviewBase>>(id, TvShowMethods.Reviews, language: language, page: page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the keywords for a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's keywords.</returns>
    public async Task<ResultContainer<Keyword>?> GetTvShowKeywordsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<ResultContainer<Keyword>>(id, TvShowMethods.Keywords, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the public lists that contain a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="page">The page number.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The lists containing the TV show.</returns>
    public async Task<SearchContainerWithId<ListResult>?> GetTvShowListsAsync(int id, int page = 0, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<SearchContainerWithId<ListResult>>(id, TvShowMethods.Lists, language: language, page: page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a dynamic list of TV shows.
    /// </summary>
    /// <param name="list">The list type.</param>
    /// <param name="page">The page number.</param>
    /// <param name="timezone">Only relevant for the AiringToday list.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paged list of TV shows.</returns>
    public async Task<SearchContainer<SearchTv>?> GetTvShowListAsync(TvShowListType list, int page = 0, string? timezone = null, CancellationToken cancellationToken = default)
    {
        return await GetTvShowListAsync(list, DefaultLanguage, page, timezone, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a dynamic list of TV shows in a specific language.
    /// </summary>
    /// <param name="list">The list type.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="timezone">Only relevant for the AiringToday list.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A paged list of TV shows.</returns>
    public async Task<SearchContainer<SearchTv>?> GetTvShowListAsync(TvShowListType list, string? language, int page = 0, string? timezone = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("tv/{method}");
        req.AddUrlSegment("method", list.GetDescription());

        if (page > 0)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrEmpty(timezone))
        {
            req.AddParameter("timezone", timezone);
        }

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        var resp = await req.GetOfT<SearchContainer<SearchTv>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the list of popular TV shows. Refreshes daily.
    /// </summary>
    /// <param name="page">The page number. Use -1 for the default.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Basic info for popular TV shows; use <see cref="GetTvShowAsync(int, TvShowMethods, string, string, CancellationToken)"/> for details.</returns>
    public async Task<SearchContainer<SearchTv>?> GetTvShowPopularAsync(int page = -1, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvShowListInternal(page, language, "popular", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets TV shows similar to a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The similar TV shows.</returns>
    public async Task<SearchContainer<SearchTv>?> GetTvShowSimilarAsync(int id, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetTvShowSimilarAsync(id, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets TV shows similar to a TV show in a specific language.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The similar TV shows.</returns>
    public async Task<SearchContainer<SearchTv>?> GetTvShowSimilarAsync(int id, string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<SearchContainer<SearchTv>>(id, TvShowMethods.Similar, language: language, page: page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets recommended TV shows based on a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The recommended TV shows.</returns>
    public async Task<SearchContainer<SearchTv>?> GetTvShowRecommendationsAsync(int id, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetTvShowRecommendationsAsync(id, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets recommended TV shows based on a TV show in a specific language.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The recommended TV shows.</returns>
    public async Task<SearchContainer<SearchTv>?> GetTvShowRecommendationsAsync(int id, string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<SearchContainer<SearchTv>>(id, TvShowMethods.Recommendations, language: language, page: page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of top-rated TV shows. Only includes shows with 2 or more votes; refreshes daily.
    /// </summary>
    /// <param name="page">The page number. Use -1 for the default.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Basic info for top-rated TV shows; use <see cref="GetTvShowAsync(int, TvShowMethods, string, string, CancellationToken)"/> for details.</returns>
    public async Task<SearchContainer<SearchTv>?> GetTvShowTopRatedAsync(int page = -1, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvShowListInternal(page, language, "top_rated", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the available translations for a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's translations.</returns>
    public async Task<TranslationsContainerTv?> GetTvShowTranslationsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<TranslationsContainerTv>(id, TvShowMethods.Translations, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the videos (trailers, teasers, clips, etc.) for a TV show.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="includeMediaLanguage">Comma-separated ISO 639-1 codes for video languages.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's videos.</returns>
    public async Task<ResultContainer<Video>?> GetTvShowVideosAsync(int id, string? includeMediaLanguage = null, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<ResultContainer<Video>>(id, TvShowMethods.Videos, includeMediaLanguage: includeMediaLanguage, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the watch providers for a TV show by region.
    /// </summary>
    /// <param name="id">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's watch providers by region.</returns>
    public async Task<SingleResultContainer<Dictionary<string, WatchProviders>>?> GetTvShowWatchProvidersAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetTvShowMethodInternal<SingleResultContainer<Dictionary<string, WatchProviders>>>(id, TvShowMethods.WatchProviders, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the user's rating for a TV show.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the rating was removed.</returns>
    /// <remarks>Requires a valid guest or user session.</remarks>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest or user session is assigned.</exception>
    public async Task<bool> TvShowRemoveRatingAsync(int tvShowId, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.GuestSession);

        var req = _client.Create("tv/{tvShowId}/rating");
        req.AddUrlSegment("tvShowId", tvShowId.ToString(CultureInfo.InvariantCulture));
        AddSessionId(req);

        using var response = await req.Delete<PostReply>(cancellationToken).ConfigureAwait(false);

        // status code 13 = "The item/record was deleted successfully."
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item is not null && item.StatusCode == 13;
    }

    /// <summary>
    /// Sets the user's rating for a TV show.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="rating">The rating, between 0.5 and 10 in increments of 0.5. Other values are rejected.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the rating was set.</returns>
    /// <remarks>Requires a valid guest or user session.</remarks>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest or user session is assigned.</exception>
    public async Task<bool> TvShowSetRatingAsync(int tvShowId, double rating, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.GuestSession);

        var req = _client.Create("tv/{tvShowId}/rating");
        req.AddUrlSegment("tvShowId", tvShowId.ToString(CultureInfo.InvariantCulture));
        AddSessionId(req);

        // Force at least one fractional digit so STJ emits `5.0` rather than `5`, matching the TMDb wire format.
        req.SetBody(new { value = (decimal)rating + 0.0m });

        using var response = await req.Post<PostReply>(cancellationToken).ConfigureAwait(false);

        // status code 1 = "Success"
        // status code 12 = "The item/record was updated successfully" - Used when an item was previously rated by the user
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item is not null && (item.StatusCode == 1 || item.StatusCode == 12);
    }
}
