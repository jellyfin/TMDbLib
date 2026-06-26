using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<T?> GetTvEpisodeMethodInternal<T>(int tvShowId, int seasonNumber, int episodeNumber, TvEpisodeMethods tvShowMethod, string? dateFormat = null, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        where T : new()
    {
        var req = _client.Create("tv/{id}/season/{season_number}/episode/{episode_number}/{method}");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

        req.AddUrlSegment("method", tvShowMethod.GetDescription());

        // TODO: Dateformat?
        // if (dateFormat is not null)
        //    req.DateFormat = dateFormat;

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        if (!string.IsNullOrWhiteSpace(includeImageLanguage))
        {
            // Videos endpoint expects `include_video_language`; only images endpoint uses `include_image_language`.
            var key = tvShowMethod == TvEpisodeMethods.Videos
                ? "include_video_language"
                : "include_image_language";
            req.AddParameter(key, includeImageLanguage);
        }

        var resp = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the current user's account state for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The episode's account state.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<TvEpisodeAccountState?> GetTvEpisodeAccountStateAsync(int tvShowId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var req = _client.Create("tv/{id}/season/{season_number}/episode/{episode_number}/account_states");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));
        AddSessionId(req, SessionType.UserSession);

        using var response = await req.Get<TvEpisodeAccountState>(cancellationToken).ConfigureAwait(false);

        return await response.GetDataObject().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="extraMethods">Additional methods to append to the response.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="includeImageLanguage">Comma-separated ISO 639-1 codes for image languages.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The episode.</returns>
    public async Task<TvEpisode?> GetTvEpisodeAsync(int tvShowId, int seasonNumber, int episodeNumber, TvEpisodeMethods extraMethods = TvEpisodeMethods.Undefined, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
    {
        if (extraMethods.HasFlag(TvEpisodeMethods.AccountStates))
        {
            RequireSessionId(SessionType.UserSession);
        }

        var req = _client.Create("tv/{id}/season/{season_number}/episode/{episode_number}");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

        if (extraMethods.HasFlag(TvEpisodeMethods.AccountStates))
        {
            AddSessionId(req, SessionType.UserSession);
        }

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        includeImageLanguage ??= DefaultImageLanguage;
        if (!string.IsNullOrWhiteSpace(includeImageLanguage))
        {
            req.AddParameter("include_image_language", includeImageLanguage);
        }

        var appends = string.Join(
            ",",
            Enum.GetValues(typeof(TvEpisodeMethods))
                                         .OfType<TvEpisodeMethods>()
                                         .Except([TvEpisodeMethods.Undefined])
                                         .Where(s => extraMethods.HasFlag(s))
                                         .Select(s => s.GetDescription()));

        if (appends != string.Empty)
        {
            req.AddParameter("append_to_response", appends);
        }

        using var response = await req.Get<TvEpisode>(cancellationToken).ConfigureAwait(false);

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
        if (item.Videos is not null)
        {
            item.Videos.Id = item.Id ?? 0;
        }

        if (item.Credits is not null)
        {
            item.Credits.Id = item.Id ?? 0;
        }

        if (item.Images is not null)
        {
            item.Images.Id = item.Id ?? 0;
        }

        if (item.ExternalIds is not null)
        {
            item.ExternalIds.Id = item.Id ?? 0;
        }

        return item;
    }

    /// <summary>
    /// Gets the TV episodes screened theatrically.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Episodes screened theatrically.</returns>
    public async Task<ResultContainer<TvEpisodeInfo>?> GetTvEpisodesScreenedTheatricallyAsync(int tvShowId, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("tv/{tv_id}/screened_theatrically");
        req.AddUrlSegment("tv_id", tvShowId.ToString(CultureInfo.InvariantCulture));

        return await req.GetOfT<ResultContainer<TvEpisodeInfo>>(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the credits for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The episode's cast, crew, and guest stars.</returns>
    public async Task<CreditsWithGuestStars?> GetTvEpisodeCreditsAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvEpisodeMethodInternal<CreditsWithGuestStars>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Credits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the external ids for a TV episode (IMDb, TVDB, etc.).
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The episode's external ids.</returns>
    public async Task<ExternalIdsTvEpisode?> GetTvEpisodeExternalIdsAsync(int tvShowId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
    {
        return await GetTvEpisodeMethodInternal<ExternalIdsTvEpisode>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.ExternalIds, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the still images for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="language">The ISO 639-1 language code. Images may contain language-specific text.</param>
    /// <param name="includeImageLanguage">Comma-separated ISO 639-1 codes for image fallback (e.g. "en,null").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The episode's stills.</returns>
    public async Task<StillImages?> GetTvEpisodeImagesAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
    {
        return await GetTvEpisodeMethodInternal<StillImages>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Images, language: language, includeImageLanguage: includeImageLanguage, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the videos for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The episode's videos.</returns>
    public async Task<ResultContainer<Video>?> GetTvEpisodeVideosAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvEpisodeMethodInternal<ResultContainer<Video>>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Videos, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the available translations for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The episode's translations.</returns>
    public async Task<TranslationsContainer?> GetTvEpisodeTranslationsAsync(int tvShowId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
    {
        return await GetTvEpisodeMethodInternal<TranslationsContainer>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Translations, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the user's rating for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the rating was removed.</returns>
    /// <remarks>Requires a valid guest or user session.</remarks>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest or user session is assigned.</exception>
    public async Task<bool> TvEpisodeRemoveRatingAsync(int tvShowId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.GuestSession);

        var req = _client.Create("tv/{id}/season/{season_number}/episode/{episode_number}/rating");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

        AddSessionId(req);

        using var response = await req.Delete<PostReply>(cancellationToken).ConfigureAwait(false);

        // status code 13 = "The item/record was deleted successfully."
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item is not null && item.StatusCode == 13;
    }

    /// <summary>
    /// Sets the user's rating for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="rating">The rating, between 0.5 and 10 in increments of 0.5.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the rating was set.</returns>
    /// <remarks>Requires a valid guest or user session.</remarks>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest or user session is assigned.</exception>
    public async Task<bool> TvEpisodeSetRatingAsync(int tvShowId, int seasonNumber, int episodeNumber, double rating, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.GuestSession);

        var req = _client.Create("tv/{id}/season/{season_number}/episode/{episode_number}/rating");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

        AddSessionId(req);

        req.SetBody(new { value = rating });

        using var response = await req.Post<PostReply>(cancellationToken).ConfigureAwait(false);

        // status code 1 = "Success"
        // status code 12 = "The item/record was updated successfully" - Used when an item was previously rated by the user
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item is not null && (item.StatusCode == 1 || item.StatusCode == 12);
    }
}
