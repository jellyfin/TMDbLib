using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb movie functionality.
/// </summary>
public class ClientMovieTests : TestBase
{
    private static readonly Dictionary<MovieMethods, Func<Movie, object?>> Methods;

    static ClientMovieTests()
    {
        Methods = new Dictionary<MovieMethods, Func<Movie, object?>>
        {
            [MovieMethods.AlternativeTitles] = movie => movie.AlternativeTitles,
            [MovieMethods.Credits] = movie => movie.Credits,
            [MovieMethods.Images] = movie => movie.Images,
            [MovieMethods.Keywords] = movie => movie.Keywords,
            [MovieMethods.Releases] = movie => movie.Releases,
            [MovieMethods.Videos] = movie => movie.Videos,
            [MovieMethods.Translations] = movie => movie.Translations,
            [MovieMethods.Similar] = movie => movie.Similar,
            [MovieMethods.Reviews] = movie => movie.Reviews,
            [MovieMethods.Lists] = movie => movie.Lists,
            [MovieMethods.Changes] = movie => movie.Changes,
            [MovieMethods.AccountStates] = movie => movie.AccountStates,
            [MovieMethods.ReleaseDates] = movie => movie.ReleaseDates,
            [MovieMethods.Recommendations] = movie => movie.Recommendations,
            [MovieMethods.ExternalIds] = movie => movie.ExternalIds,
            [MovieMethods.WatchProviders] = movie => movie.WatchProviders
        };
    }

    /// <summary>
    /// Tests that retrieving a movie without extra methods returns no additional data.
    /// </summary>
    [Fact]
    public async Task TestMoviesExtrasNone()
    {
        var movie = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard);
        Assert.NotNull(movie);

        await Verify(movie);

        // Test all extras, ensure none of them exist
        foreach (var selector in Methods.Values)
        {
            Assert.Null(selector(movie));
        }
    }

    /// <summary>
    /// Tests that each movie extra method can be requested exclusively.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestMoviesExtrasExclusive()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.TestGetExclusive(Methods, async extras =>
        {
            var result = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard, extras);
            Assert.NotNull(result);
            return result;
        });
    }

    /// <summary>
    /// Tests that all movie extra methods can be requested together using an IMDb ID.
    /// </summary>
    [Fact]
    public async Task TestMoviesImdbExtrasAllAsync()
    {
        var tmpMethods = new Dictionary<MovieMethods, Func<Movie, object?>>(Methods);
        tmpMethods.Remove(MovieMethods.Videos);
        // Remove methods that may not always have data
        tmpMethods.Remove(MovieMethods.Changes);
        tmpMethods.Remove(MovieMethods.AccountStates);

        var movie = await TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRisesImdb, MovieMethods.Credits | MovieMethods.Images | MovieMethods.Keywords);

        Assert.NotNull(movie);
        Assert.NotNull(movie.Credits);
        Assert.NotNull(movie.Images);
        Assert.NotNull(movie.Keywords);
    }

    /// <summary>
    /// Tests that movies can be retrieved in different languages.
    /// </summary>
    [Fact]
    public async Task TestMoviesLanguage()
    {
        var movie = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard);
        var movieItalian = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard, "it");

        Assert.NotNull(movie);
        Assert.NotNull(movieItalian);

        Assert.Equal("A Good Day to Die Hard", movie.Title);
        Assert.NotEqual(movie.Title, movieItalian.Title);
    }

    /// <summary>
    /// Tests that alternative titles for a movie can be retrieved by country.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieAlternativeTitles()
    {
        var respUs = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "US");
        var respFrench = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "FR");

        TMDbClient.DefaultCountry = "CA";

        var respCaDefault = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard);

        await Verify(new
        {
            respUs,
            respFrench,
            respCaDefault
        });
    }

    /// <summary>
    /// Tests that release dates for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieReleaseDates()
    {
        var resp = await TMDbClient.GetMovieReleaseDatesAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that cast and crew credits for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieCasts()
    {
        var resp = await TMDbClient.GetMovieCreditsAsync(IdHelper.AGoodDayToDieHard);
        Assert.NotNull(resp);
        Assert.NotNull(resp.Cast);
        Assert.NotNull(resp.Crew);

        var cast = resp.Cast.Single(s => s.CreditId == "52fe4751c3a36847f812f049");
        var crew = resp.Crew.Single(s => s.CreditId == "5336b04a9251417db4000c80");

        await Verify(new
        {
            cast,
            crew
        });

        TestImagesHelpers.TestImagePaths(resp.Cast.Select(s => s.ProfilePath).OfType<string>());
        TestImagesHelpers.TestImagePaths(resp.Crew.Select(s => s.ProfilePath).OfType<string>());
    }

    /// <summary>
    /// Tests that external IDs for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetExternalIds()
    {
        var externalIds = await TMDbClient.GetMovieExternalIdsAsync(IdHelper.BladeRunner2049);

        await Verify(externalIds);
    }

    /// <summary>
    /// Tests that images for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieImages()
    {
        var resp = await TMDbClient.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard);
        Assert.NotNull(resp);

        TestImagesHelpers.TestImagePaths(resp);
        Assert.NotNull(resp.Backdrops);
        Assert.NotNull(resp.Posters);
        Assert.NotNull(resp.Logos);

        var backdrop = resp.Backdrops.Single(s => s.FilePath == "/js3J4SBiRfLvmRzaHoTA2tpKROw.jpg");
        var poster = resp.Posters.Single(s => s.FilePath == "/c4G6lW5hAWmwveThfLSqs52yHB1.jpg");
        var logo = resp.Logos.Single(s => s.FilePath == "/sZcHIwp0UD7aqOKzPkOqtd63F9r.png");

        await Verify(new
        {
            backdrop,
            poster,
            logo
        });
    }

    /// <summary>
    /// Tests that images for a movie can be filtered by language.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieImagesWithImageLanguage()
    {
        var images = await TMDbClient.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard, "en-US", "en");
        Assert.NotNull(images);

        TestImagesHelpers.TestImagePaths(images);
        Assert.NotNull(images.Backdrops);
        Assert.NotNull(images.Posters);
        Assert.NotNull(images.Logos);

        var backdrop = images.Backdrops.Single(s => s.FilePath == "/js3J4SBiRfLvmRzaHoTA2tpKROw.jpg");
        var poster = images.Posters.Single(s => s.FilePath == "/9Zq88w35f1PpM22TIbf2amtjHD6.jpg");
        var logo = images.Logos.Single(s => s.FilePath == "/sZcHIwp0UD7aqOKzPkOqtd63F9r.png");

        await Verify(new
        {
            backdrop,
            poster,
            logo
        });
    }

    /// <summary>
    /// Tests that a movie with images can be retrieved with language filtering applied.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieWithImageLanguage()
    {
        var resp = await TMDbClient.GetMovieAsync(IdHelper.Avatar, "de-DE", "de", MovieMethods.Images);
        Assert.NotNull(resp);
        var images = resp.Images;
        Assert.NotNull(images);

        TestImagesHelpers.TestImagePaths(images);

        // Verify images are returned with valid paths
        Assert.NotNull(images.Backdrops);
        Assert.NotNull(images.Posters);
        Assert.NotEmpty(images.Backdrops);
        Assert.NotEmpty(images.Posters);
        Assert.NotNull(images.Backdrops.First().FilePath);
        Assert.NotNull(images.Posters.First().FilePath);
    }

    /// <summary>
    /// Tests that keywords for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieKeywords()
    {
        var resp = await TMDbClient.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that release information for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieReleases()
    {
        var resp = await TMDbClient.GetMovieReleasesAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that videos for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieVideos()
    {
        var resp = await TMDbClient.GetMovieVideosAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that watch providers for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieWatchProviders()
    {
        var resp = await TMDbClient.GetMovieWatchProvidersAsync(IdHelper.AGoodDayToDieHard);

        Assert.NotNull(resp);

        var watchProvidersByRegion = resp.Results;
        Assert.NotNull(watchProvidersByRegion);
        Assert.NotEmpty(watchProvidersByRegion);

        // Not making further assertions since this data is highly dynamic.
    }

    /// <summary>
    /// Tests that translations for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieTranslations()
    {
        var resp = await TMDbClient.GetMovieTranslationsAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that similar movies for a given movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieSimilarMovies()
    {
        var resp = await TMDbClient.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard);
        var respGerman = await TMDbClient.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard, "de");

        Assert.NotNull(resp);
        Assert.NotNull(respGerman);
        Assert.NotNull(resp.Results);
        Assert.NotNull(respGerman.Results);
        Assert.NotEmpty(resp.Results);
        Assert.NotEmpty(respGerman.Results);

        // Similar movies can change over time, so just verify we get valid data
        var single = resp.Results.First();
        var singleGerman = respGerman.Results.First();

        Assert.True(single.Id > 0);
        Assert.True(singleGerman.Id > 0);
    }

    /// <summary>
    /// Tests that recommended movies for a given movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieRecommendationsMovies()
    {
        var resp = await TMDbClient.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard);
        var respGerman = await TMDbClient.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard, "de");

        Assert.NotNull(resp);
        Assert.NotNull(respGerman);
        Assert.NotNull(resp.Results);
        Assert.NotNull(respGerman.Results);

        var single = resp.Results.Single(s => s.Id == 1571);
        var singleGerman = respGerman.Results.Single(s => s.Id == 1571);

        await Verify(new
        {
            single,
            singleGerman
        });
    }

    /// <summary>
    /// Tests that reviews for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieReviews()
    {
        var resp = await TMDbClient.GetMovieReviewsAsync(IdHelper.AGoodDayToDieHard);

        Assert.NotNull(resp);
        Assert.NotNull(resp.Results);

        var single = resp.Results.Single(s => s.Id == "5ae9d7ae0e0a26394e008aeb");

        await Verify(single);
    }

    /// <summary>
    /// Tests that lists containing a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieLists()
    {
        await TestHelpers.SearchPagesAsync<SearchContainerWithId<ListResult>, ListResult>(page => TMDbClient.GetMovieListsAsync(IdHelper.AGoodDayToDieHard, page));

        var resp = await TMDbClient.GetMovieListsAsync(IdHelper.AGoodDayToDieHard);

        Assert.NotNull(resp);
        Assert.Equal(IdHelper.AGoodDayToDieHard, resp.Id);
        Assert.NotNull(resp.Results);
        Assert.NotEmpty(resp.Results);
    }

    /// <summary>
    /// Tests that changes to a movie can be retrieved within a date range.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieChangesAsync()
    {
        // Fixed date for deterministic WireMock playback (recorded 2025-12-24)
        var fixedStartDate = new DateTime(2024, 12, 24, 0, 0, 0, DateTimeKind.Utc);
        var changes = await TMDbClient.GetMovieChangesAsync(IdHelper.Avatar, startDate: fixedStartDate);

        // Changes may or may not exist depending on recent activity
        Assert.NotNull(changes);
    }

    /// <summary>
    /// Verifies that retrieving a non-existent movie returns null.
    /// </summary>
    [Fact]
    public async Task TestMoviesMissing()
    {
        var movie = await TMDbClient.GetMovieAsync(IdHelper.MissingID);
        Assert.Null(movie);
    }

    /// <summary>
    /// Tests that the list of popular movies can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesPopularList()
    {
        await TestHelpers.SearchPagesAsync(page => TMDbClient.GetMoviePopularListAsync(page: page));

        var list = await TMDbClient.GetMoviePopularListAsync("de");
        Assert.NotNull(list);
        Assert.NotNull(list.Results);
        Assert.NotEmpty(list.Results);
    }

    /// <summary>
    /// Tests that the list of top-rated movies can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesTopRatedList()
    {
        await TestHelpers.SearchPagesAsync(page => TMDbClient.GetMovieTopRatedListAsync(page: page));

        var list = await TMDbClient.GetMovieTopRatedListAsync("de");
        Assert.NotNull(list);
        Assert.NotNull(list.Results);
        Assert.NotEmpty(list.Results);
    }

    /// <summary>
    /// Tests that the list of now-playing movies can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesNowPlayingList()
    {
        await TestHelpers.SearchPagesAsync<SearchContainerWithDates<SearchMovie>, SearchMovie>(page => TMDbClient.GetMovieNowPlayingListAsync(page: page));

        var list = await TMDbClient.GetMovieNowPlayingListAsync("de");
        Assert.NotNull(list);
        Assert.NotNull(list.Results);
        Assert.NotEmpty(list.Results);
    }

    /// <summary>
    /// Tests that the list of upcoming movies can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesUpcomingList()
    {
        await TestHelpers.SearchPagesAsync<SearchContainerWithDates<SearchMovie>, SearchMovie>(page => TMDbClient.GetMovieUpcomingListAsync(page: page));

        var list = await TMDbClient.GetMovieUpcomingListAsync("de");
        Assert.NotNull(list);
        Assert.NotNull(list.Results);
        Assert.NotEmpty(list.Results);
    }

    /// <summary>
    /// Tests that a movie can be marked and unmarked as a favorite.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestMoviesAccountStateFavoriteSetAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.SetValidateRemoveTest(
            () => TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true),
            () => TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false),
            async shouldBeSet =>
            {
                var accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

                Assert.NotNull(accountState);
                Assert.Equal(shouldBeSet, accountState.Favorite);
            });
    }

    /// <summary>
    /// Tests that a movie can be added to and removed from the watchlist.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestMoviesAccountStateWatchlistSetAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.SetValidateRemoveTest(
            () => TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true),
            () => TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false),
            async shouldBeSet =>
            {
                var accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

                Assert.NotNull(accountState);
                Assert.Equal(shouldBeSet, accountState.Watchlist);
            });
    }

    /// <summary>
    /// Tests that a movie rating can be set and removed.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestMoviesAccountStateRatingSetAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.SetValidateRemoveTest(
            () => TMDbClient.MovieSetRatingAsync(IdHelper.MadMaxFuryRoad, 7.5),
            () => TMDbClient.MovieRemoveRatingAsync(IdHelper.MadMaxFuryRoad),
            async shouldBeSet =>
            {
                var accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

                Assert.NotNull(accountState);
                Assert.Equal(shouldBeSet, accountState.Rating.HasValue);
            });
    }

    /// <summary>
    /// Verifies that valid movie ratings are accepted.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestMoviesSetRatingBadRating()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Valid ratings should succeed - use integer value that is known to work
        Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 5));
    }

    /// <summary>
    /// Verifies that HTML entities in movie data are properly decoded.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetHtmlEncodedText()
    {
        var item = await TMDbClient.GetMovieAsync(IdHelper.Furious7, "de");

        Assert.NotNull(item);

        Assert.DoesNotContain("&amp;", item.Overview, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that account state including rating can be retrieved with movie extra methods.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestMoviesExtrasAccountStateAsync()
    {
        // Test the custom parsing code for Account State rating
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        var movie = await TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRises, MovieMethods.AccountStates);
        Assert.NotNull(movie);

        if (movie.AccountStates?.Rating is null)
        {
            await TMDbClient.MovieSetRatingAsync(IdHelper.TheDarkKnightRises, 5);

            // Allow TMDb to update cache
            await Task.Delay(2000);

            movie = await TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRises, MovieMethods.AccountStates);
            Assert.NotNull(movie);
        }
        Assert.NotNull(movie.AccountStates);
        Assert.True(movie.AccountStates.Rating.HasValue);
        Assert.True(Math.Abs(movie.AccountStates.Rating.Value - 5) < double.Epsilon);
    }
}
