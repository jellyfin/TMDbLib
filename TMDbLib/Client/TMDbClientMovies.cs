using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;
using TMDbLib.Utilities;
using Credits = TMDbLib.Objects.Movies.Credits;

namespace TMDbLib.Client;

public sealed partial class TMDbClient
{
    private async Task<T?> GetMovieMethodInternal<T>(
        int movieId,
        MovieMethods movieMethod,
        string dateFormat = "yyyy-MM-dd",
        string? country = null,
        string? language = null,
        string? includeImageLanguage = null,
        int page = 0,
        DateTime? startDate = null,
        DateTime? endDate = null,
        CancellationToken cancellationToken = default)
        where T : new()
    {
        var req = _client.Create("movie/{movieId}/{method}");
        req.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", movieMethod.GetDescription());

        if (country is not null)
        {
            req.AddParameter("country", country);
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

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (startDate.HasValue)
        {
            req.AddParameter("start_date", startDate.Value.ToString(dateFormat, CultureInfo.InvariantCulture));
        }

        if (endDate is not null)
        {
            req.AddParameter("end_date", endDate.Value.ToString(dateFormat, CultureInfo.InvariantCulture));
        }

        var response = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the current user's account state for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's account state.</returns>
    /// <remarks>Requires a valid user session.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    public async Task<AccountState?> GetMovieAccountStateAsync(int movieId, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.UserSession);

        var req = _client.Create("movie/{movieId}/{method}");
        req.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", MovieMethods.AccountStates.GetDescription());
        AddSessionId(req, SessionType.UserSession);

        return await req.GetOfT<AccountState>(cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the alternative titles for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's alternative titles.</returns>
    public async Task<AlternativeTitles?> GetMovieAlternativeTitlesAsync(int movieId, CancellationToken cancellationToken = default)
    {
        return await GetMovieAlternativeTitlesAsync(movieId, DefaultCountry, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the alternative titles for a movie in a specific country.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="country">The ISO 3166-1 country code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's alternative titles.</returns>
    public async Task<AlternativeTitles?> GetMovieAlternativeTitlesAsync(int movieId, string? country, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<AlternativeTitles>(movieId, MovieMethods.AlternativeTitles, country: country, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a movie by TMDb id.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="extraMethods">Additional methods to append to the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie, or null if not found.</returns>
    /// <remarks>Requires a valid user session when specifying the extra method 'AccountStates' flag.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned, see remarks.</exception>
    public async Task<Movie?> GetMovieAsync(int movieId, MovieMethods extraMethods = MovieMethods.Undefined, CancellationToken cancellationToken = default)
    {
        return await GetMovieAsync(movieId, DefaultLanguage, null, extraMethods, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a movie by IMDb id.
    /// </summary>
    /// <param name="imdbId">The IMDb id of the movie.</param>
    /// <param name="extraMethods">Additional methods to append to the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie, or null if not found.</returns>
    /// <remarks>Requires a valid user session when specifying the extra method 'AccountStates' flag.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned, see remarks.</exception>
    public async Task<Movie?> GetMovieAsync(string imdbId, MovieMethods extraMethods = MovieMethods.Undefined, CancellationToken cancellationToken = default)
    {
        return await GetMovieAsync(imdbId, DefaultLanguage, null, extraMethods, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a movie by TMDb id with language options.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="includeImageLanguage">Comma-separated ISO 639-1 codes for image languages.</param>
    /// <param name="extraMethods">Additional methods to append to the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie, or null if not found.</returns>
    /// <remarks>Requires a valid user session when specifying the extra method 'AccountStates' flag.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned, see remarks.</exception>
    public async Task<Movie?> GetMovieAsync(int movieId, string? language, string? includeImageLanguage = null, MovieMethods extraMethods = MovieMethods.Undefined, CancellationToken cancellationToken = default)
    {
        return await GetMovieAsync(movieId.ToString(CultureInfo.InvariantCulture), language, includeImageLanguage, extraMethods, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a movie by IMDb id (or TMDb id as string) with language options.
    /// </summary>
    /// <param name="imdbId">The IMDb id, or TMDb id as string.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="includeImageLanguage">Comma-separated ISO 639-1 codes for image languages.</param>
    /// <param name="extraMethods">Additional methods to append to the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie, or null if not found.</returns>
    /// <remarks>Requires a valid user session when specifying the extra method 'AccountStates' flag.</remarks>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned, see remarks.</exception>
    public async Task<Movie?> GetMovieAsync(string imdbId, string? language, string? includeImageLanguage = null, MovieMethods extraMethods = MovieMethods.Undefined, CancellationToken cancellationToken = default)
    {
        if (extraMethods.HasFlag(MovieMethods.AccountStates))
        {
            RequireSessionId(SessionType.UserSession);
        }

        var req = _client.Create("movie/{movieId}");
        req.AddUrlSegment("movieId", imdbId);
        if (extraMethods.HasFlag(MovieMethods.AccountStates))
        {
            AddSessionId(req, SessionType.UserSession);
        }

        if (language is not null)
        {
            req.AddParameter("language", language);
        }

        includeImageLanguage ??= DefaultImageLanguage;
        if (includeImageLanguage is not null)
        {
            req.AddParameter("include_image_language", includeImageLanguage);
        }

        var appends = string.Join(
            ",",
            Enum.GetValues<MovieMethods>()
                                         .Except([MovieMethods.Undefined])
                                         .Where(s => extraMethods.HasFlag(s))
                                         .Select(s => s.GetDescription()));

        if (appends != string.Empty)
        {
            req.AddParameter("append_to_response", appends);
        }

        using var response = await req.Get<Movie>(cancellationToken).ConfigureAwait(false);

        if (!response.IsValid)
        {
            return null;
        }

        var item = await response.GetDataObject().ConfigureAwait(false);

        // Patch up data, so that the end user won't notice that we share objects between req-types.
        if (item is null)
        {
            return null;
        }

        if (item.Videos is not null)
        {
            item.Videos.Id = item.Id;
        }

        if (item.AlternativeTitles is not null)
        {
            item.AlternativeTitles.Id = item.Id;
        }

        if (item.Credits is not null)
        {
            item.Credits.Id = item.Id;
        }

        if (item.Releases is not null)
        {
            item.Releases.Id = item.Id;
        }

        if (item.Keywords is not null)
        {
            item.Keywords.Id = item.Id;
        }

        if (item.Translations is not null)
        {
            item.Translations.Id = item.Id;
        }

        if (item.AccountStates is not null)
        {
            item.AccountStates.Id = item.Id;
        }

        if (item.ExternalIds is not null)
        {
            item.ExternalIds.Id = item.Id;
        }

        // Overview is the only field that is HTML encoded from the source.
        item.Overview = WebUtility.HtmlDecode(item.Overview);

        return item;
    }

    /// <summary>
    /// Gets the cast and crew for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's credits.</returns>
    public async Task<Credits?> GetMovieCreditsAsync(int movieId, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<Credits>(movieId, MovieMethods.Credits, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the external ids for a movie (IMDb, Facebook, Twitter, etc.).
    /// </summary>
    /// <param name="id">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's external ids.</returns>
    public async Task<ExternalIdsMovie?> GetMovieExternalIdsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<ExternalIdsMovie>(id, MovieMethods.ExternalIds, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the images for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's posters and backdrops.</returns>
    public async Task<ImagesWithId?> GetMovieImagesAsync(int movieId, CancellationToken cancellationToken = default)
    {
        return await GetMovieImagesAsync(movieId, DefaultLanguage, null, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the images for a movie with language options.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="includeImageLanguage">Comma-separated ISO 639-1 codes for image languages.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's posters and backdrops.</returns>
    public async Task<ImagesWithId?> GetMovieImagesAsync(int movieId, string? language, string? includeImageLanguage = null, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<ImagesWithId>(movieId, MovieMethods.Images, language: language, includeImageLanguage: includeImageLanguage, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the keywords for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's keywords.</returns>
    public async Task<KeywordsContainer?> GetMovieKeywordsAsync(int movieId, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<KeywordsContainer>(movieId, MovieMethods.Keywords, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the most recently created movie on TMDb.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The latest movie.</returns>
    public async Task<Movie?> GetMovieLatestAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("movie/latest");
        using var resp = await req.Get<Movie>(cancellationToken).ConfigureAwait(false);

        var item = await resp.GetDataObject().ConfigureAwait(false);

        // Overview is the only field that is HTML encoded from the source.
        if (item is not null)
        {
            item.Overview = WebUtility.HtmlDecode(item.Overview);
        }

        return item;
    }

    /// <summary>
    /// Gets the lists that contain a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The lists containing the movie.</returns>
    public async Task<SearchContainerWithId<ListResult>?> GetMovieListsAsync(int movieId, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetMovieListsAsync(movieId, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the lists that contain a movie in a specific language.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The lists containing the movie.</returns>
    public async Task<SearchContainerWithId<ListResult>?> GetMovieListsAsync(int movieId, string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<SearchContainerWithId<ListResult>>(movieId, MovieMethods.Lists, page: page, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets recommended movies based on a movie.
    /// </summary>
    /// <param name="id">The TMDb id of the movie.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The recommended movies.</returns>
    public async Task<SearchContainer<SearchMovie>?> GetMovieRecommendationsAsync(int id, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetMovieRecommendationsAsync(id, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets recommended movies based on a movie in a specific language.
    /// </summary>
    /// <param name="id">The TMDb id of the movie.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The recommended movies.</returns>
    public async Task<SearchContainer<SearchMovie>?> GetMovieRecommendationsAsync(int id, string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<SearchContainer<SearchMovie>>(id, MovieMethods.Recommendations, language: language, page: page, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of movies currently in theaters.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Movies now playing, with the date range.</returns>
    public async Task<SearchContainerWithDates<SearchMovie>?> GetMovieNowPlayingListAsync(string? language = null, int page = 0, string? region = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("movie/now_playing");

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (language is not null)
        {
            req.AddParameter("language", language);
        }

        if (region is not null)
        {
            req.AddParameter("region", region);
        }

        var resp = await req.GetOfT<SearchContainerWithDates<SearchMovie>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the list of popular movies.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The popular movies.</returns>
    public async Task<SearchContainer<SearchMovie>?> GetMoviePopularListAsync(string? language = null, int page = 0, string? region = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("movie/popular");

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (language is not null)
        {
            req.AddParameter("language", language);
        }

        if (region is not null)
        {
            req.AddParameter("region", region);
        }

        var resp = await req.GetOfT<SearchContainer<SearchMovie>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the release dates and certifications for a movie by country.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's release dates by country.</returns>
    public async Task<ResultContainer<ReleaseDatesContainer>?> GetMovieReleaseDatesAsync(int movieId, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<ResultContainer<ReleaseDatesContainer>>(movieId, MovieMethods.ReleaseDates, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the release information for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's releases.</returns>
    public async Task<Releases?> GetMovieReleasesAsync(int movieId, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<Releases>(movieId, MovieMethods.Releases, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the user reviews for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's reviews.</returns>
    public async Task<SearchContainerWithId<ReviewBase>?> GetMovieReviewsAsync(int movieId, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetMovieReviewsAsync(movieId, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the user reviews for a movie in a specific language.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's reviews.</returns>
    public async Task<SearchContainerWithId<ReviewBase>?> GetMovieReviewsAsync(int movieId, string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<SearchContainerWithId<ReviewBase>>(movieId, MovieMethods.Reviews, page: page, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets movies similar to a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The similar movies.</returns>
    public async Task<SearchContainer<SearchMovie>?> GetMovieSimilarAsync(int movieId, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetMovieSimilarAsync(movieId, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets movies similar to a movie in a specific language.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The similar movies.</returns>
    public async Task<SearchContainer<SearchMovie>?> GetMovieSimilarAsync(int movieId, string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<SearchContainer<SearchMovie>>(movieId, MovieMethods.Similar, page: page, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of top-rated movies.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The top-rated movies.</returns>
    public async Task<SearchContainer<SearchMovie>?> GetMovieTopRatedListAsync(string? language = null, int page = 0, string? region = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("movie/top_rated");

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (language is not null)
        {
            req.AddParameter("language", language);
        }

        if (region is not null)
        {
            req.AddParameter("region", region);
        }

        var resp = await req.GetOfT<SearchContainer<SearchMovie>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the available translations for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's translations.</returns>
    public async Task<TranslationsContainer?> GetMovieTranslationsAsync(int movieId, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<TranslationsContainer>(movieId, MovieMethods.Translations, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of upcoming movies.
    /// </summary>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number.</param>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Upcoming movies, with the date range.</returns>
    public async Task<SearchContainerWithDates<SearchMovie>?> GetMovieUpcomingListAsync(string? language = null, int page = 0, string? region = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("movie/upcoming");

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (language is not null)
        {
            req.AddParameter("language", language);
        }

        if (region is not null)
        {
            req.AddParameter("region", region);
        }

        var resp = await req.GetOfT<SearchContainerWithDates<SearchMovie>>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the videos (trailers, teasers, clips, etc.) for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's videos.</returns>
    public async Task<ResultContainer<Video>?> GetMovieVideosAsync(int movieId, string? language = null, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<ResultContainer<Video>>(movieId, MovieMethods.Videos, language: language, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the watch providers for a movie by region.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's watch providers by region.</returns>
    public async Task<SingleResultContainer<Dictionary<string, WatchProviders>>?> GetMovieWatchProvidersAsync(int movieId, CancellationToken cancellationToken = default)
    {
        return await GetMovieMethodInternal<SingleResultContainer<Dictionary<string, WatchProviders>>>(movieId, MovieMethods.WatchProviders, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Removes the user's rating for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the rating was removed.</returns>
    /// <remarks>Requires a valid guest or user session.</remarks>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest or user session is assigned.</exception>
    public async Task<bool> MovieRemoveRatingAsync(int movieId, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.GuestSession);

        var req = _client.Create("movie/{movieId}/rating");
        req.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
        AddSessionId(req);

        using var response = await req.Delete<PostReply>(cancellationToken).ConfigureAwait(false);
        // status code 13 = "The item/record was deleted successfully."
        var item = await response.GetDataObject().ConfigureAwait(false);

        return item is not null && item.StatusCode == 13;
    }

    /// <summary>
    /// Sets the user's rating for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="rating">The rating, between 0.5 and 10 in increments of 0.5. Other values are rejected.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>True if the rating was set.</returns>
    /// <remarks>Requires a valid guest or user session.</remarks>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest or user session is assigned.</exception>
    public async Task<bool> MovieSetRatingAsync(int movieId, double rating, CancellationToken cancellationToken = default)
    {
        RequireSessionId(SessionType.GuestSession);

        var req = _client.Create("movie/{movieId}/rating");
        req.AddUrlSegment("movieId", movieId.ToString(CultureInfo.InvariantCulture));
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
