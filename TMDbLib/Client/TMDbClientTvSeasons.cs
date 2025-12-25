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
            req.AddParameter("include_image_language", includeImageLanguage);
        }

        var response = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Retrieves the account states for all episodes in a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A container with account states for each episode in the season.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when the current client object doesn't have a user session assigned.</exception>
    public async Task<ResultContainer<TvEpisodeAccountStateWithNumber>?> GetTvSeasonAccountStateAsync(int tvShowId, int seasonNumber, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var req = _client.Create("tv/{id}/season/{season_number}/account_states");
        req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", TvEpisodeMethods.AccountStates.GetDescription());
        AddSessionId(req, SessionType.UserSession);

        using var response = await req.Get<ResultContainer<TvEpisodeAccountStateWithNumber>>(cancellationToken).ConfigureAwait(false);

        return await response.GetDataObject().ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieve a season for a specifc tv Show by id.
    /// </summary>
    /// <param name="tvShowId">TMDb id of the tv show the desired season belongs to.</param>
    /// <param name="seasonNumber">The season number of the season you want to retrieve. Note use 0 for specials.</param>
    /// <param name="extraMethods">Enum flags indicating any additional data that should be fetched in the same request.</param>
    /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es. </param>
    /// <param name="includeImageLanguage">If specified the api will attempt to return localized image results eg. en,it,es.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The requested season for the specified tv show.</returns>
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
    /// Returns a credits object for the season of the tv show associated with the provided TMDb id.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the target tv show.</param>
    /// <param name="seasonNumber">The season number of the season you want to retrieve information for. Note use 0 for specials.</param>
    /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es. </param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Credits information including cast and crew for the season.</returns>
    public async Task<Credits?> GetTvSeasonCreditsAsync(int tvShowId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<Credits>(tvShowId, seasonNumber, TvSeasonMethods.Credits, dateFormat: "yyyy-MM-dd", language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Returns an object that contains all known exteral id's for the season of the tv show related to the specified TMDB id.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the target tv show.</param>
    /// <param name="seasonNumber">The season number of the season you want to retrieve information for. Note use 0 for specials.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>External IDs for the season from various sources like TVDB, etc.</returns>
    public async Task<ExternalIdsTvSeason?> GetTvSeasonExternalIdsAsync(int tvShowId, int seasonNumber, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<ExternalIdsTvSeason>(tvShowId, seasonNumber, TvSeasonMethods.ExternalIds, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves all images all related to the season of specified tv show.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the target tv show.</param>
    /// <param name="seasonNumber">The season number of the season you want to retrieve information for. Note use 0 for specials.</param>
    /// <param name="language">
    /// If specified the api will attempt to return a localized result. ex: en,it,es.
    /// For images this means that the image might contain language specifc text.
    /// </param>
    /// <param name="includeImageLanguage">If you want to include a fallback language (especially useful for backdrops) you can use the include_image_language parameter. This should be a comma separated value like so: include_image_language=en,null.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>Poster images for the season.</returns>
    public async Task<PosterImages?> GetTvSeasonImagesAsync(int tvShowId, int seasonNumber, string? language = null, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<PosterImages>(tvShowId, seasonNumber, TvSeasonMethods.Images, language: language, includeImageLanguage: includeImageLanguage, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves videos (trailers, teasers, clips, etc.) for a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A container with video information for the season.</returns>
    public async Task<ResultContainer<Video>?> GetTvSeasonVideosAsync(int tvShowId, int seasonNumber, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<ResultContainer<Video>>(tvShowId, seasonNumber, TvSeasonMethods.Videos, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves available translations for a TV season.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="seasonNumber">The season number. Use 0 for specials.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A container with translation information for the season.</returns>
    public async Task<TranslationsContainer?> GetTvSeasonTranslationsAsync(int tvShowId, int seasonNumber, CancellationToken cancellationToken = default)
    {
        return await GetTvSeasonMethodInternal<TranslationsContainer>(tvShowId, seasonNumber, TvSeasonMethods.Translations, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
