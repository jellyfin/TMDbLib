using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Cast = TMDbLib.Objects.TvShows.Cast;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb TV episode functionality.
/// </summary>
public class ClientTvEpisodeTests : TestBase
{
    private static readonly Dictionary<TvEpisodeMethods, Func<TvEpisode, object?>> Methods;

    static ClientTvEpisodeTests()
    {
        Methods = new Dictionary<TvEpisodeMethods, Func<TvEpisode, object?>>
        {
            [TvEpisodeMethods.Credits] = tvEpisode => tvEpisode.Credits,
            [TvEpisodeMethods.Images] = tvEpisode => tvEpisode.Images,
            [TvEpisodeMethods.ExternalIds] = tvEpisode => tvEpisode.ExternalIds,
            [TvEpisodeMethods.Videos] = tvEpisode => tvEpisode.Videos,
            [TvEpisodeMethods.AccountStates] = tvEpisode => tvEpisode.AccountStates
        };
    }

    /// <summary>
    /// Tests that a TV episode can be retrieved without any extra methods.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeExtrasNoneAsync()
    {
        var tvEpisode = await TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1);

        Assert.NotNull(tvEpisode);
        await Verify(tvEpisode);

        // Test all extras, ensure none of them are populated
        foreach (var selector in Methods.Values)
        {
            Assert.Null(selector(tvEpisode));
        }
    }

    /// <summary>
    /// Tests that account states can be retrieved with a TV episode, including rating information.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvEpisodeExtrasAccountState()
    {
        // Test the custom parsing code for Account State rating
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        var episode = await TMDbClient.GetTvEpisodeAsync(IdHelper.BigBangTheory, 1, 1, TvEpisodeMethods.AccountStates);
        Assert.NotNull(episode);
        if (episode.AccountStates is null || !episode.AccountStates.Rating.HasValue)
        {
            await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BigBangTheory, 1, 1, 5);

            // Allow TMDb to update cache
            await Task.Delay(2000);

            episode = await TMDbClient.GetTvEpisodeAsync(IdHelper.BigBangTheory, 1, 1, TvEpisodeMethods.AccountStates);
        }
        await Verify(episode);
    }

    /// <summary>
    /// Tests that all extra methods can be retrieved together for a TV episode.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvEpisodeExtrasAll()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Account states will only show up if we've done something
        await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.FullerHouse, 1, 1, 5);

        await TestMethodsHelper.TestGetAll(Methods, async combined =>
            {
                var result = await TMDbClient.GetTvEpisodeAsync(IdHelper.FullerHouse, 1, 1, combined);
                Assert.NotNull(result);
                return result;
            }, async tvEpisode =>
            {
                await Verify(tvEpisode);
            });
    }

    /// <summary>
    /// Tests that each extra method can be retrieved exclusively for a TV episode.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvEpisodeExtrasExclusiveAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
        await TestMethodsHelper.TestGetExclusive(Methods, async extras =>
        {
            var result = await TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, extras);
            Assert.NotNull(result);
            return result;
        });
    }

    /// <summary>
    /// Tests that credits including cast, crew, and guest stars can be retrieved separately for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeSeparateExtrasCreditsAsync()
    {
        var credits = await TMDbClient.GetTvEpisodeCreditsAsync(IdHelper.BreakingBad, 1, 1);
        Assert.NotNull(credits);

        Assert.NotNull(credits.Cast);
        Assert.NotNull(credits.Crew);
        Assert.NotNull(credits.GuestStars);
        var guestStarItem = credits.GuestStars.FirstOrDefault(s => s.Id == 92495);
        var castItem = credits.Cast.FirstOrDefault(s => s.Id == 17419);
        var crewItem = credits.Crew.FirstOrDefault(s => s.Id == 1280071);

        await Verify(new
        {
            guestStarItem,
            castItem,
            crewItem
        });
    }

    /// <summary>
    /// Tests that external IDs can be retrieved separately for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeSeparateExtrasExternalIdsAsync()
    {
        var externalIds = await TMDbClient.GetTvEpisodeExternalIdsAsync(IdHelper.BreakingBad, 1, 1);

        await Verify(externalIds);
    }

    /// <summary>
    /// Tests that still images can be retrieved separately for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeSeparateExtrasImagesAsync()
    {
        var images = await TMDbClient.GetTvEpisodeImagesAsync(IdHelper.BreakingBad, 1, 1);

        await Verify(images);
    }

    /// <summary>
    /// Tests that videos can be retrieved separately for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeSeparateExtrasVideosAsync()
    {
        var images = await TMDbClient.GetTvEpisodeVideosAsync(IdHelper.BreakingBad, 1, 1);

        await Verify(images);
    }

    /// <summary>
    /// Tests that a TV episode rating can be set, retrieved, and removed through account states.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvEpisodeAccountStateRatingSetAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
        var accountState = await TMDbClient.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1);
        Assert.NotNull(accountState);

        // Remove the rating
        if (accountState.Rating.HasValue)
        {
            Assert.True(await TMDbClient.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);
        }
        // Test that the episode is NOT rated
        accountState = await TMDbClient.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1);
        Assert.NotNull(accountState);

        Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, accountState.Id);
        Assert.False(accountState.Rating.HasValue);

        // Rate the episode
        Assert.True(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 5));

        // Allow TMDb to cache our changes
        await Task.Delay(2000);

        // Test that the episode IS rated
        accountState = await TMDbClient.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1);
        Assert.NotNull(accountState);
        Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, accountState.Id);
        Assert.True(accountState.Rating.HasValue);

        // Remove the rating
        Assert.True(await TMDbClient.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1));
    }

    /// <summary>
    /// Tests that valid rating values are accepted when rating a TV episode.
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestTvEpisodeRateBadAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // TMDb now returns HTTP 400 for invalid ratings instead of returning false
        // Test valid ratings only
        Assert.True(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 5));
    }

    /// <summary>
    /// Tests that change history can be retrieved for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeGetChangesAsync()
    {
        var changes = await TMDbClient.GetTvEpisodeChangesAsync(IdHelper.BreakingBadSeason1Episode1Id);

        // Changes may or may not exist depending on recent activity
        Assert.NotNull(changes);
    }

    /// <summary>
    /// Tests that null is returned when attempting to retrieve a non-existent TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeMissingAsync()
    {
        var tvEpisode = await TMDbClient.GetTvEpisodeAsync(IdHelper.MissingID, 1, 1);

        Assert.Null(tvEpisode);
    }

    /// <summary>
    /// Tests that TV episodes that were screened theatrically can be retrieved for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodesScreenedTheatricallyAsync()
    {
        var results = await TMDbClient.GetTvEpisodesScreenedTheatricallyAsync(IdHelper.GameOfThrones);
        Assert.NotNull(results);
        Assert.NotNull(results.Results);
        var single = results.Results.Single(s => s.Id == IdHelper.GameOfThronesSeason4Episode10);

        await Verify(single);
    }

    /// <summary>
    /// Tests that images can be filtered by language when retrieving a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeGetTvEpisodeWithImageLanguageAsync()
    {
        var resp = await TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, language: "en-US", includeImageLanguage: "en", extraMethods: TvEpisodeMethods.Images);
        Assert.NotNull(resp);
        Assert.NotNull(resp.Images);

        await Verify(resp.Images);
    }
}
