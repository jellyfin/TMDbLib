using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb TV show functionality.
/// </summary>
public class ClientTvShowTests : TestBase
{
    private static readonly Dictionary<TvShowMethods, Func<TvShow, object>> Methods;

    static ClientTvShowTests()
    {
        Methods = new Dictionary<TvShowMethods, Func<TvShow, object>>
        {
            [TvShowMethods.Credits] = tvShow => tvShow.Credits,
            [TvShowMethods.Images] = tvShow => tvShow.Images,
            [TvShowMethods.ExternalIds] = tvShow => tvShow.ExternalIds,
            [TvShowMethods.ContentRatings] = tvShow => tvShow.ContentRatings,
            [TvShowMethods.AlternativeTitles] = tvShow => tvShow.AlternativeTitles,
            [TvShowMethods.Keywords] = tvShow => tvShow.Keywords,
            [TvShowMethods.Changes] = tvShow => tvShow.Changes,
            [TvShowMethods.AccountStates] = tvShow => tvShow.AccountStates,
            [TvShowMethods.Recommendations] = tvShow => tvShow.Recommendations,
            [TvShowMethods.WatchProviders] = tvShow => tvShow.WatchProviders,
            [TvShowMethods.EpisodeGroups] = tvShow => tvShow.EpisodeGroups,
            [TvShowMethods.CreditsAggregate] = tvShow => tvShow.AggregateCredits
        };
    }

    /// <summary>
    /// Tests that a TV show can be retrieved without any extra methods.
    /// </summary>
    [Fact]
    public async Task TestTvShowExtrasNoneAsync()
    {
        var tvShow = await TMDbClient.GetTvShowAsync(IdHelper.BreakingBad);

        await Verify(tvShow);

        // Test all extras, ensure none of them are populated
        foreach (Func<TvShow, object> selector in Methods.Values)
        {
            Assert.Null(selector(tvShow));
        }
    }

    /// <summary>
    /// Tests that all extra methods can be retrieved together for a TV show.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvShowExtrasAllAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Account states will only show up if we've done something
        await TMDbClient.TvShowSetRatingAsync(IdHelper.KeepingUpAppearances, 5);

        await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetTvShowAsync(IdHelper.KeepingUpAppearances, combined), show => Verify(show));
    }

    /// <summary>
    /// Tests that credits can be retrieved separately for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowSeparateExtrasCreditsAsync()
    {
        var credits = await TMDbClient.GetTvShowCreditsAsync(IdHelper.BreakingBad);

        await Verify(credits);
    }

    /// <summary>
    /// Tests that aggregate credits can be retrieved for a TV show across all seasons and episodes.
    /// </summary>
    [Fact]
    public async Task TestAggregateCreditsExtractAllAsync()
    {
        var credits = await TMDbClient.GetAggregateCredits(IdHelper.Lupin);

        await Verify(credits);
    }

    /// <summary>
    /// Tests that external IDs can be retrieved separately for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowSeparateExtrasExternalIdsAsync()
    {
        var externalIds = await TMDbClient.GetTvShowExternalIdsAsync(IdHelper.GameOfThrones);

        await Verify(externalIds);
    }

    /// <summary>
    /// Tests that content ratings can be retrieved separately for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowSeparateExtrasContentRatingsAsync()
    {
        var contentRatings = await TMDbClient.GetTvShowContentRatingsAsync(IdHelper.BreakingBad);

        await Verify(contentRatings);
    }

    /// <summary>
    /// Tests that alternative titles can be retrieved separately for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowSeparateExtrasAlternativeTitlesAsync()
    {
        var alternativeTitles = await TMDbClient.GetTvShowAlternativeTitlesAsync(IdHelper.BreakingBad);

        await Verify(alternativeTitles);
    }

    /// <summary>
    /// Tests that keywords can be retrieved separately for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowSeparateExtrasKeywordsAsync()
    {
        var keywords = await TMDbClient.GetTvShowKeywordsAsync(IdHelper.BreakingBad);

        var single = keywords.Results.Single(s => s.Id == 15484);

        await Verify(single);
    }

    /// <summary>
    /// Tests that translations can be retrieved separately for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowSeparateExtrasTranslationsAsync()
    {
        var translations = await TMDbClient.GetTvShowTranslationsAsync(IdHelper.BreakingBad);

        var single = translations.Translations.Single(s => s.Iso_3166_1 == "DK");

        await Verify(single);
    }

    /// <summary>
    /// Tests that videos can be retrieved separately for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowSeparateExtrasVideosAsync()
    {
        var videos = await TMDbClient.GetTvShowVideosAsync(IdHelper.BreakingBad);

        var single = videos.Results.Single(s => s.Id == "5759db2fc3a3683e7c003df7");

        await Verify(single);
    }

    /// <summary>
    /// Tests that account states can be retrieved with a TV show, including rating information.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvShowSeparateExtrasAccountStateAsync()
    {
        // Test the custom parsing code for Account State rating
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        var show = await TMDbClient.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates);
        if (show.AccountStates is null || !show.AccountStates.Rating.HasValue)
        {
            await TMDbClient.TvShowSetRatingAsync(IdHelper.BigBangTheory, 5);

            // Allow TMDb to update cache
            await Task.Delay(2000);

            show = await TMDbClient.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates);
        }
        Assert.NotNull(show.AccountStates);
        Assert.True(show.AccountStates.Rating.HasValue);
        Assert.True(Math.Abs(show.AccountStates.Rating.Value - 5) < double.Epsilon);
    }

    /// <summary>
    /// Tests that images including backdrops, posters, and logos can be retrieved separately for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowSeparateExtrasImagesAsync()
    {
        var images = await TMDbClient.GetTvShowImagesAsync(IdHelper.BreakingBad);

        TestImagesHelpers.TestImagePaths(images);

        var backdrop = images.Backdrops.Single(s => s.FilePath == "/tsRy63Mu5cu8etL1X7ZLyf7UP1M.jpg");
        var poster = images.Posters.Single(s => s.FilePath == "/ggFHVNu6YYI5L9pCfOacjizRGt.jpg");
        var logo = images.Logos.Single(s => s.FilePath == "/chw44B2VnLha8iiTdyZcIW0ZELC.png");

        await Verify(new
        {
            backdrop,
            poster,
            logo
        });
    }

    /// <summary>
    /// Tests that images can be filtered by language when retrieving TV show images separately.
    /// </summary>
    [Fact]
    public async Task TestTvShowGetImagesWithImageLanguageAsync()
    {
        var images = await TMDbClient.GetTvShowImagesAsync(IdHelper.BreakingBad, "en-US", "en");

        TestImagesHelpers.TestImagePaths(images);

        var backdrop = images.Backdrops.Single(s => s.FilePath == "/otCnAN1edreRudT5E2OHk8beiDu.jpg");
        var poster = images.Posters.Single(s => s.FilePath == "/ggFHVNu6YYI5L9pCfOacjizRGt.jpg");
        var logo = images.Logos.Single(s => s.FilePath == "/bM2bNffZlZ2UxZqwHaxr5VS3UUI.svg");

        await Verify(new
        {
            backdrop,
            poster,
            logo
        });
    }

    /// <summary>
    /// Tests that episode groups can be retrieved with a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowGetTvShowWithEpisodeGroups()
    {
        var resp = await TMDbClient.GetTvShowAsync(IdHelper.Seinfeld, TvShowMethods.EpisodeGroups);

        await Verify(resp.EpisodeGroups);
    }

    /// <summary>
    /// Tests that images can be filtered by language when retrieving a TV show with extra methods.
    /// </summary>
    [Fact]
    public async Task TestTvShowGetTvShowWithImageLanguageAsync()
    {
        var resp = await TMDbClient.GetTvShowAsync(IdHelper.BreakingBad, includeImageLanguage: "pt", extraMethods: TvShowMethods.Images);

        TestImagesHelpers.TestImagePaths(resp.Images);

        ImageData? backdrop = null;
        var poster = resp.Images.Posters.Single(s => s.FilePath == "/30erzlzIOtOK3k3T3BAl1GiVMP1.jpg");
        ImageData? logo = null;

        await Verify(new
        {
            backdrop,
            poster,
            logo
        });
    }

    /// <summary>
    /// Tests that popular TV shows can be retrieved with proper pagination.
    /// </summary>
    [Fact]
    public async Task TestTvShowPopular()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.GetTvShowPopularAsync(i));

        var result = await TMDbClient.GetTvShowPopularAsync();

        Assert.NotEmpty(result.Results);
        Assert.NotNull(result.Results[0].Name);
        Assert.NotNull(result.Results[0].OriginalName);
        Assert.NotNull(result.Results[0].FirstAirDate);
        Assert.NotNull(result.Results[0].PosterPath);
        Assert.NotNull(result.Results[0].BackdropPath);
    }

    /// <summary>
    /// Tests that a TV show's season count is correctly retrieved.
    /// </summary>
    [Fact]
    public async Task TestTvShowSeasonCountAsync()
    {
        // TODO: Is this test obsolete?
        var tvShow = await TMDbClient.GetTvShowAsync(1668);

        await Verify(tvShow);
    }

    /// <summary>
    /// Tests that videos can be retrieved as part of a TV show request.
    /// </summary>
    [Fact]
    public async Task TestTvShowVideosAsync()
    {
        var tvShow = await TMDbClient.GetTvShowAsync(1668, TvShowMethods.Videos);

        await Verify(tvShow);
    }

    /// <summary>
    /// Tests that watch provider information can be retrieved for a TV show by region.
    /// </summary>
    [Fact]
    public async Task TestTvShowGetMovieWatchProviders()
    {
        var resp = await TMDbClient.GetTvShowWatchProvidersAsync(IdHelper.GameOfThrones);

        Assert.NotNull(resp);

        var watchProvidersByRegion = resp.Results;
        Assert.NotNull(watchProvidersByRegion);

        // Not making further assertions since this data is highly dynamic.
    }

    /// <summary>
    /// Tests that all available translations can be retrieved for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowTranslationsAsync()
    {
        var translations = await TMDbClient.GetTvShowTranslationsAsync(IdHelper.BreakingBad);

        await Verify(translations);
    }

    /// <summary>
    /// Tests that similar TV shows can be retrieved based on a given TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowSimilarsAsync()
    {
        var tvShows = await TMDbClient.GetTvShowSimilarAsync(IdHelper.BreakingBad);

        Assert.NotEmpty(tvShows.Results);

        // Just verify we get valid TV show data
        var tvShow = tvShows.Results.First();
        Assert.True(tvShow.Id > 0);
        Assert.NotNull(tvShow.Name);
    }

    /// <summary>
    /// Tests that recommended TV shows can be retrieved based on a given TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowRecommendationsAsync()
    {
        var tvShows = await TMDbClient.GetTvShowRecommendationsAsync(IdHelper.BreakingBad);

        Assert.NotEmpty(tvShows.Results);

        // Just verify we get valid TV show data
        var tvShow = tvShows.Results.First();
        Assert.True(tvShow.Id > 0);
        Assert.NotNull(tvShow.Name);
    }

    /// <summary>
    /// Tests that top-rated TV shows can be retrieved with proper pagination.
    /// </summary>
    [Fact]
    public async Task TestTvShowTopRated()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.GetTvShowTopRatedAsync(i));

        var result = await TMDbClient.GetTvShowTopRatedAsync();
        Assert.NotNull(result.Results[0].Name);
        Assert.NotNull(result.Results[0].OriginalName);
        Assert.NotNull(result.Results[0].FirstAirDate);
        Assert.NotNull(result.Results[0].PosterPath ?? result.Results[0].BackdropPath);
    }

    /// <summary>
    /// Tests that the latest TV show added to TMDb can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestTvShowLatest()
    {
        var tvShow = await TMDbClient.GetLatestTvShowAsync();

        Assert.NotNull(tvShow);
    }

    /// <summary>
    /// Tests that reviews can be retrieved for a TV show with proper pagination.
    /// </summary>
    [Fact]
    public async Task TestTvShowReviews()
    {
        await TestHelpers.SearchPagesAsync<SearchContainerWithId<ReviewBase>, ReviewBase>(i => TMDbClient.GetTvShowReviewsAsync(IdHelper.BreakingBad, page: i));

        var reviews = await TMDbClient.GetTvShowReviewsAsync(IdHelper.BreakingBad);

        var single = reviews.Results.Single(s => s.Id == "5accdbe6c3a3687e2702d058");

        await Verify(single);
    }

    /// <summary>
    /// Tests that TV show lists of various types can be retrieved with proper pagination.
    /// </summary>
    [Fact]
    public async Task TestTvShowLists()
    {
        foreach (TvShowListType type in Enum.GetValues(typeof(TvShowListType)).OfType<TvShowListType>())
        {
            await TestHelpers.SearchPagesAsync(i => TMDbClient.GetTvShowListAsync(type, i));
        }
    }

    /// <summary>
    /// Tests that a TV show can be added to and removed from a user's favorites list.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvShowAccountStateFavoriteSet()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.SetValidateRemoveTest(async () =>
        {
            // Favourite the movie
            await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true);
        }, async () =>
        {
            // Un-favorite the movie
            await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false);
        }, async shouldBe =>
        {
            var accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.Equal(shouldBe, accountState.Favorite);
        });
    }

    /// <summary>
    /// Tests that a TV show can be added to and removed from a user's watchlist.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvShowAccountStateWatchlistSet()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.SetValidateRemoveTest(async () =>
        {
            // Add to watchlist
            await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true);
        }, async () =>
        {
            // Remove from watchlist
            await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false);
        }, async shouldBe =>
        {
            var accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.Equal(shouldBe, accountState.Watchlist);
        });
    }

    /// <summary>
    /// Tests that a TV show rating can be set and retrieved through account states.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvShowAccountStateRatingSet()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Set a rating
        Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 5));

        // Allow TMDb to cache our changes
        await Task.Delay(2000);

        // Verify the rating was set
        var accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);
        Assert.Equal(IdHelper.BreakingBad, accountState.Id);
        Assert.NotNull(accountState.Rating);
    }

    /// <summary>
    /// Tests that TV show rating validation accepts valid values.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvShowSetRating()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Valid ratings should succeed - use integer value that is known to work
        Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 5));
    }

    /// <summary>
    /// Tests that a TV show can be rated using a guest session.
    /// </summary>
    [Fact]
    public async Task TestTvShowSetRatingGuestSession()
    {
        // There is no way to validate the change besides the success return of the api call since the guest session doesn't have access to anything else
        await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

        // Try changing the rating - use integer value that is known to work
        Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 5));
    }

    /// <summary>
    /// Tests that null is returned when attempting to retrieve a non-existent TV show.
    /// </summary>
    [Fact]
    public async Task TestTvShowMissingAsync()
    {
        var tvShow = await TMDbClient.GetTvShowAsync(IdHelper.MissingID);

        Assert.Null(tvShow);
    }
}
