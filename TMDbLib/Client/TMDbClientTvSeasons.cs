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
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<T?> GetTvSeasonMethodInternal<T>(int tvShowId, int seasonNumber, TvSeasonMethods tvShowMethod, string? dateFormat = null, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
        where T : new()
    {
        var req = _client.Create("tv/{id}/season/{season_number}/{method}");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
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
            var key = tvShowMethod == TvSeasonMethods.Videos
                ? "include_video_language"
                : "include_image_language";
            req.AddParameter(key, includeImageLanguage);
        }

        var response = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the account states for each episode in a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Account states per episode.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<ResultContainer<TvEpisodeAccountStateWithNumber>?> GetTvSeasonAccountStateAsync(int tvShowId, int seasonNumber, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var req = _client.Create("tv/{id}/season/{season_number}/account_states");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
        AddSessionId(req, SessionType.UserSession);

        using var response = await req.Get<ResultContainer<TvEpisodeAccountStateWithNumber>>(cancellationToken).ConfigureAwait(false);

        return await response.GetDataObject().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="extraMethods">Additional methods to append to the response.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="includeImageLanguage">Comma-separated ISO 639-1 codes for image languages.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The season.</returns>
    public async Task<TvSeason?> GetTvSeasonAsync(int tvShowId, int seasonNumber, TvSeasonMethods extraMethods = TvSeasonMethods.Undefined, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
    {
        if (extraMethods.HasFlag(TvSeasonMethods.AccountStates))
        {
            RequireSessionId(SessionType.UserSession);
        }

        var req = _client.Create("tv/{id}/season/{season_number}");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));

        if (extraMethods.HasFlag(TvSeasonMethods.AccountStates))
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
            Enum.GetValues(typeof(TvSeasonMethods))
                                         .OfType<TvSeasonMethods>()
                                         .Except([TvSeasonMethods.Undefined])
                                         .Where(s => extraMethods.HasFlag(s))
                                         .Select(s => s.GetDescription()));

        if (appends != string.Empty)
        {
            req.AddParameter("append_to_response", appends);
        }

        using var response = await req.Get<TvSeason>(cancellationToken).ConfigureAwait(false);

        if (!response.IsValid)
        {
            return null;
        }

        var item = await response.GetDataObject().ConfigureAwait(false);

        // Nothing to patch up
        if (item is null)
        {
            return null;
        }

        if (item.Images is not null)
        {
            item.Images.Id = item.Id ?? 0;
        }

        if (item.Credits is not null)
        {
            item.Credits.Id = item.Id ?? 0;
        }

        if (item.ExternalIds is not null)
        {
            item.ExternalIds.Id = item.Id ?? 0;
        }

        if (item.AccountStates is not null)
        {
            item.AccountStates.Id = item.Id ?? 0;
        }

        if (item.Videos is not null)
        {
            item.Videos.Id = item.Id ?? 0;
        }

        return item;
    }

    /// <summary>
    /// Gets the credits for a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The season's cast and crew.</returns>
    public async Task<Credits?> GetTvSeasonCreditsAsync(int tvShowId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<Credits>(tvShowId, seasonNumber, TvSeasonMethods.Credits, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the aggregate credits for a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The season's aggregate credits.</returns>
    public async Task<CreditsAggregate?> GetTvSeasonAggregateCredits(int tvShowId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<CreditsAggregate>(tvShowId, seasonNumber, TvSeasonMethods.CreditsAggregate, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the external ids for a TV season (TVDB, etc.).
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The season's external ids.</returns>
    public async Task<ExternalIdsTvSeason?> GetTvSeasonExternalIdsAsync(int tvShowId, int seasonNumber, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<ExternalIdsTvSeason>(tvShowId, seasonNumber, TvSeasonMethods.ExternalIds, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the poster images for a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="language">The ISO 639-1 language code. Images may contain language-specific text.</param>
    /// <param name="includeImageLanguage">Comma-separated ISO 639-1 codes for image fallback (e.g. "en,null").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The season's posters.</returns>
    public async Task<PosterImages?> GetTvSeasonImagesAsync(int tvShowId, int seasonNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<PosterImages>(tvShowId, seasonNumber, TvSeasonMethods.Images, language: language, includeImageLanguage: includeImageLanguage, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the videos for a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The season's videos.</returns>
    public async Task<ResultContainer<Video>?> GetTvSeasonVideosAsync(int tvShowId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<ResultContainer<Video>>(tvShowId, seasonNumber, TvSeasonMethods.Videos, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the available translations for a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The season's translations.</returns>
    public async Task<TranslationsContainer?> GetTvSeasonTranslationsAsync(int tvShowId, int seasonNumber, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<TranslationsContainer>(tvShowId, seasonNumber, TvSeasonMethods.Translations, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
