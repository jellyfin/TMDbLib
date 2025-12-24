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
            req.AddParameter("include_image_language", includeImageLanguage);
        }

        var resp = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves the account states for a TV episode (rating, watchlist status, favorite status).
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The account states for the episode.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when the current client object doesn't have a user session assigned.</exception>
    public async Task<TvEpisodeAccountState?> GetTvEpisodeAccountStateAsync(int tvShowId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var req = _client.Create("tv/{id}/season/{season_number}/episode/{episode_number}/account_states");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", TvEpisodeMethods.AccountStates.GetDescription());
        AddSessionId(req, SessionType.UserSession);

        using var response = await req.Get<TvEpisodeAccountState>(cancellationToken).ConfigureAwait(false);

        return await response.GetDataObject().ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve a specific episode using TMDb id of the associated tv show.
    /// </summary>
    /// <param name="tvShowId">TMDb id of the tv show the desired episode belongs to.</param>
    /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number of the episode you want to retrieve.</param>
    /// <param name="extraMethods">Enum flags indicating any additional data that should be fetched in the same request.</param>
    /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es. </param>
    /// <param name="includeImageLanguage">If specified the api will attempt to return localized image results eg. en,it,es.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The requested TV episode with its details and any requested additional data.</returns>
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
    /// Retrieves a list of TV episodes that have been screened theatrically.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A container with episodes that were screened theatrically.</returns>
    public async Task<ResultContainer<TvEpisodeInfo>?> GetTvEpisodesScreenedTheatricallyAsync(int tvShowId, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("tv/{tv_id}/screened_theatrically");
        req.AddUrlSegment("tv_id", tvShowId.ToString(CultureInfo.InvariantCulture));

        return await req.GetOfT<ResultContainer<TvEpisodeInfo>>(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns a credits object for the specified episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the target tv show.</param>
    /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number of the episode you want to retrieve information for.</param>
    /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es. </param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Credits information including cast, crew, and guest stars for the episode.</returns>
    public async Task<CreditsWithGuestStars?> GetTvEpisodeCreditsAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvEpisodeMethodInternal<CreditsWithGuestStars>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Credits, dateFormat: "yyyy-MM-dd", language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns an object that contains all known exteral id's for the specified episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the target tv show.</param>
    /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number of the episode you want to retrieve information for.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>External IDs for the episode from various sources like IMDb, TVDB, etc.</returns>
    public async Task<ExternalIdsTvEpisode?> GetTvEpisodeExternalIdsAsync(int tvShowId, int seasonNumber, int episodeNumber, CancellationToken cancellationToken = default)
    {
        return await GetTvEpisodeMethodInternal<ExternalIdsTvEpisode>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.ExternalIds, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all images all related to the season of specified episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the target tv show.</param>
    /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number of the episode you want to retrieve information for.</param>
    /// <param name="language">
    /// If specified the api will attempt to return a localized result. ex: en,it,es.
    /// For images this means that the image might contain language specifc text.
    /// </param>
    /// <param name="includeImageLanguage">If you want to include a fallback language you can use the include_image_language parameter. This should be a comma separated value like so: include_image_language=en,null.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Still images (screenshots) from the episode.</returns>
    public async Task<StillImages?> GetTvEpisodeImagesAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
    {
        return await GetTvEpisodeMethodInternal<StillImages>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Images, language: language, includeImageLanguage: includeImageLanguage, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves videos (trailers, clips, etc.) for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="language">Language to filter the video results.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A container with video information for the episode.</returns>
    public async Task<ResultContainer<Video>?> GetTvEpisodeVideosAsync(int tvShowId, int seasonNumber, int episodeNumber, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvEpisodeMethodInternal<ResultContainer<Video>>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Videos, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves available translations for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A container with translation information for the episode.</returns>
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
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>True if the rating was successfully removed, false otherwise.</returns>
    /// <remarks>Requires a valid guest or user session.</remarks>
    /// <exception cref="GuestSessionRequiredException">Thrown when the current client object doesn't have a guest or user session assigned.</exception>
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
    /// Sets or updates the user's rating for a TV episode.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="episodeNumber">The episode number.</param>
    /// <param name="rating">The rating value between 0.5 and 10 in increments of 0.5.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>True if the rating was successfully set, false otherwise.</returns>
    /// <remarks>Requires a valid guest or user session.</remarks>
    /// <exception cref="GuestSessionRequiredException">Thrown when the current client object doesn't have a guest or user session assigned.</exception>
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
