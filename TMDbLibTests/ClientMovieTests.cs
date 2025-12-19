using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Cast = TMDbLib.Objects.Movies.Cast;
using Credits = TMDbLib.Objects.Movies.Credits;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb movie functionality.
/// </summary>
public class ClientMovieTests : TestBase
{
    private static readonly Dictionary<MovieMethods, Func<Movie, object>> Methods;

    static ClientMovieTests()
    {
        Methods = new Dictionary<MovieMethods, Func<Movie, object>>
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
        Movie movie = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard);

        await Verify(movie);

        // Test all extras, ensure none of them exist
        foreach (Func<Movie, object> selector in Methods.Values)
        {
            Assert.Null(selector(movie));
        }
    }

    /// <summary>
    /// Tests that each movie extra method can be requested exclusively.
    /// </summary>
    [Fact]
    public async Task TestMoviesExtrasExclusive()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard, extras));
    }

    /// <summary>
    /// Tests that all movie extra methods can be requested together using an IMDb ID.
    /// </summary>
    [Fact]
    public async Task TestMoviesImdbExtrasAllAsync()
    {
        Dictionary<MovieMethods, Func<Movie, object>> tmpMethods = new Dictionary<MovieMethods, Func<Movie, object>>(Methods);
        tmpMethods.Remove(MovieMethods.Videos);

        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Account states will only show up if we've done something
        await TMDbClient.MovieSetRatingAsync(IdHelper.TheDarkKnightRises, 5);

        await TestMethodsHelper.TestGetAll(tmpMethods, combined => TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRisesImdb, combined), movie => Verify(movie));
    }

    /// <summary>
    /// Tests that movies can be retrieved in different languages.
    /// </summary>
    [Fact]
    public async Task TestMoviesLanguage()
    {
        Movie movie = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard);
        Movie movieItalian = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard, "it");

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
        AlternativeTitles respUs = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "US");
        AlternativeTitles respFrench = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "FR");

        TMDbClient.DefaultCountry = "CA";

        AlternativeTitles respCaDefault = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard);

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
        ResultContainer<ReleaseDatesContainer> resp = await TMDbClient.GetMovieReleaseDatesAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that cast and crew credits for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieCasts()
    {
        Credits resp = await TMDbClient.GetMovieCreditsAsync(IdHelper.AGoodDayToDieHard);
        Assert.NotNull(resp);

        Cast cast = resp.Cast.Single(s => s.CreditId == "52fe4751c3a36847f812f049");
        Crew crew = resp.Crew.Single(s => s.CreditId == "5336b04a9251417db4000c80");

        await Verify(new
        {
            cast,
            crew
        });

        TestImagesHelpers.TestImagePaths(resp.Cast.Select(s => s.ProfilePath).Where(s => s != null));
        TestImagesHelpers.TestImagePaths(resp.Crew.Select(s => s.ProfilePath).Where(s => s != null));
    }

    /// <summary>
    /// Tests that external IDs for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetExternalIds()
    {
        ExternalIdsMovie externalIds = await TMDbClient.GetMovieExternalIdsAsync(IdHelper.BladeRunner2049);

        await Verify(externalIds);
    }

    /// <summary>
    /// Tests that images for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieImages()
    {
        ImagesWithId resp = await TMDbClient.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard);

        TestImagesHelpers.TestImagePaths(resp);

        ImageData backdrop = resp.Backdrops.Single(s => s.FilePath == "/js3J4SBiRfLvmRzaHoTA2tpKROw.jpg");
        ImageData poster = resp.Posters.Single(s => s.FilePath == "/c4G6lW5hAWmwveThfLSqs52yHB1.jpg");
        ImageData logo = resp.Logos.Single(s => s.FilePath == "/sZcHIwp0UD7aqOKzPkOqtd63F9r.png");

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
        ImagesWithId images = await TMDbClient.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard, "en-US", "en");

        TestImagesHelpers.TestImagePaths(images);

        ImageData backdrop = images.Backdrops.Single(s => s.FilePath == "/js3J4SBiRfLvmRzaHoTA2tpKROw.jpg");
        ImageData poster = images.Posters.Single(s => s.FilePath == "/9Zq88w35f1PpM22TIbf2amtjHD6.jpg");
        ImageData logo = images.Logos.Single(s => s.FilePath == "/sZcHIwp0UD7aqOKzPkOqtd63F9r.png");

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
        Movie resp = await TMDbClient.GetMovieAsync(IdHelper.Avatar, "de-DE", "de", MovieMethods.Images);
        Images images = resp.Images;

        TestImagesHelpers.TestImagePaths(images);

        ImageData backdrop = images.Backdrops.Single(s => s.FilePath == "/4U9fN2jsQ94GQfDGeLEe8UaReRO.jpg");
        ImageData poster = images.Posters.Single(s => s.FilePath == "/8VV4YUwOGxgolFZTo2SgNwsfznR.jpg");
        ImageData logo = images.Logos.Single(s => s.FilePath == "/jIWzq9B4KPH9hyUISlma02ijTFb.png");

        await Verify(new
        {
            backdrop,
            poster,
            logo
        });
    }

    /// <summary>
    /// Tests that keywords for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieKeywords()
    {
        KeywordsContainer resp = await TMDbClient.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that release information for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieReleases()
    {
        Releases resp = await TMDbClient.GetMovieReleasesAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that videos for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieVideos()
    {
        ResultContainer<Video> resp = await TMDbClient.GetMovieVideosAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that watch providers for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieWatchProviders()
    {
        SingleResultContainer<Dictionary<string, WatchProviders>> resp = await TMDbClient.GetMovieWatchProvidersAsync(IdHelper.AGoodDayToDieHard);

        Assert.NotNull(resp);

        Dictionary<string, WatchProviders> watchProvidersByRegion = resp.Results;
        Assert.NotEmpty(watchProvidersByRegion);

        // Not making further assertions since this data is highly dynamic.
    }

    /// <summary>
    /// Tests that translations for a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieTranslations()
    {
        TranslationsContainer resp = await TMDbClient.GetMovieTranslationsAsync(IdHelper.AGoodDayToDieHard);

        await Verify(resp);
    }

    /// <summary>
    /// Tests that similar movies for a given movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieSimilarMovies()
    {
        SearchContainer<SearchMovie> resp = await TMDbClient.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard);
        SearchContainer<SearchMovie> respGerman = await TMDbClient.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard, "de");

        SearchMovie single = resp.Results.Single(s => s.Id == 708);
        SearchMovie singleGerman = respGerman.Results.Single(s => s.Id == 708);

        await Verify(new
        {
            single,
            singleGerman
        });
    }

    /// <summary>
    /// Tests that recommended movies for a given movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieRecommendationsMovies()
    {
        SearchContainer<SearchMovie> resp = await TMDbClient.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard);
        SearchContainer<SearchMovie> respGerman = await TMDbClient.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard, "de");

        SearchMovie single = resp.Results.Single(s => s.Id == 1571);
        SearchMovie singleGerman = respGerman.Results.Single(s => s.Id == 1571);

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
        SearchContainerWithId<ReviewBase> resp = await TMDbClient.GetMovieReviewsAsync(IdHelper.AGoodDayToDieHard);

        ReviewBase single = resp.Results.Single(s => s.Id == "5ae9d7ae0e0a26394e008aeb");

        await Verify(single);
    }

    /// <summary>
    /// Tests that lists containing a movie can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieLists()
    {
        await TestHelpers.SearchPagesAsync<SearchContainerWithId<ListResult>, ListResult>(page => TMDbClient.GetMovieListsAsync(IdHelper.AGoodDayToDieHard, page));

        SearchContainerWithId<ListResult> resp = await TMDbClient.GetMovieListsAsync(IdHelper.AGoodDayToDieHard);

        Assert.Equal(IdHelper.AGoodDayToDieHard, resp.Id);
        Assert.NotEmpty(resp.Results);
    }

    /// <summary>
    /// Tests that changes to a movie can be retrieved within a date range.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetMovieChangesAsync()
    {
        IList<Change> changes = await TMDbClient.GetMovieChangesAsync(IdHelper.Avatar, startDate: DateTime.UtcNow.AddYears(-1));

        Assert.NotEmpty(changes);
    }

    /// <summary>
    /// Verifies that retrieving a non-existent movie returns null.
    /// </summary>
    [Fact]
    public async Task TestMoviesMissing()
    {
        Movie movie = await TMDbClient.GetMovieAsync(IdHelper.MissingID);
        Assert.Null(movie);
    }

    /// <summary>
    /// Tests that the list of popular movies can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesPopularList()
    {
        await TestHelpers.SearchPagesAsync(page => TMDbClient.GetMoviePopularListAsync(page: page));

        SearchContainer<SearchMovie> list = await TMDbClient.GetMoviePopularListAsync("de");
        Assert.NotEmpty(list.Results);
    }

    /// <summary>
    /// Tests that the list of top-rated movies can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesTopRatedList()
    {
        await TestHelpers.SearchPagesAsync(page => TMDbClient.GetMovieTopRatedListAsync(page: page));

        SearchContainer<SearchMovie> list = await TMDbClient.GetMovieTopRatedListAsync("de");
        Assert.NotEmpty(list.Results);
    }

    /// <summary>
    /// Tests that the list of now-playing movies can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesNowPlayingList()
    {
        await TestHelpers.SearchPagesAsync<SearchContainerWithDates<SearchMovie>, SearchMovie>(page => TMDbClient.GetMovieNowPlayingListAsync(page: page));

        SearchContainer<SearchMovie> list = await TMDbClient.GetMovieNowPlayingListAsync("de");
        Assert.NotEmpty(list.Results);
    }

    /// <summary>
    /// Tests that the list of upcoming movies can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestMoviesUpcomingList()
    {
        await TestHelpers.SearchPagesAsync<SearchContainerWithDates<SearchMovie>, SearchMovie>(page => TMDbClient.GetMovieUpcomingListAsync(page: page));

        SearchContainer<SearchMovie> list = await TMDbClient.GetMovieUpcomingListAsync("de");
        Assert.NotEmpty(list.Results);
    }

    /// <summary>
    /// Tests that a movie can be marked and unmarked as a favorite.
    /// </summary>
    [Fact]
    public async Task TestMoviesAccountStateFavoriteSetAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.SetValidateRemoveTest(
            () => TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true),
            () => TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false),
            async shouldBeSet =>
            {
                AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

                Assert.Equal(shouldBeSet, accountState.Favorite);
            });
    }

    /// <summary>
    /// Tests that a movie can be added to and removed from the watchlist.
    /// </summary>
    [Fact]
    public async Task TestMoviesAccountStateWatchlistSetAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.SetValidateRemoveTest(
            () => TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true),
            () => TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false),
            async shouldBeSet =>
            {
                AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

                Assert.Equal(shouldBeSet, accountState.Watchlist);
            });
    }

    /// <summary>
    /// Tests that a movie rating can be set and removed.
    /// </summary>
    [Fact]
    public async Task TestMoviesAccountStateRatingSetAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.SetValidateRemoveTest(
            () => TMDbClient.MovieSetRatingAsync(IdHelper.MadMaxFuryRoad, 7.5),
            () => TMDbClient.MovieRemoveRatingAsync(IdHelper.MadMaxFuryRoad),
            async shouldBeSet =>
            {
                AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

                Assert.Equal(shouldBeSet, accountState.Rating.HasValue);
            });
    }

    /// <summary>
    /// Verifies that invalid movie ratings are rejected while valid ratings are accepted.
    /// </summary>
    [Fact]
    public async Task TestMoviesSetRatingBadRating()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        Assert.False(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 7.1));

        Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 8.0));

        Assert.False(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 10.5));

        Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 1.5));

        Assert.False(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 0));

        Assert.False(await TMDbClient.MovieRemoveRatingAsync(IdHelper.Avatar));
    }

    /// <summary>
    /// Verifies that HTML entities in movie data are properly decoded.
    /// </summary>
    [Fact]
    public async Task TestMoviesGetHtmlEncodedText()
    {
        Movie item = await TMDbClient.GetMovieAsync(IdHelper.Furious7, "de");

        Assert.NotNull(item);

        Assert.DoesNotContain("&amp;", item.Overview, StringComparison.Ordinal);
    }

    /// <summary>
    /// Tests that account state including rating can be retrieved with movie extra methods.
    /// </summary>
    [Fact]
    public async Task TestMoviesExtrasAccountStateAsync()
    {
        // Test the custom parsing code for Account State rating
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        Movie movie = await TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRises, MovieMethods.AccountStates);

        if (movie.AccountStates?.Rating == null)
        {
            await TMDbClient.MovieSetRatingAsync(IdHelper.TheDarkKnightRises, 5);

            // Allow TMDb to update cache
            await Task.Delay(2000);

            movie = await TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRises, MovieMethods.AccountStates);
        }
        Assert.NotNull(movie.AccountStates);
        Assert.True(movie.AccountStates.Rating.HasValue);
        Assert.True(Math.Abs(movie.AccountStates.Rating.Value - 5) < double.Epsilon);
    }
}
