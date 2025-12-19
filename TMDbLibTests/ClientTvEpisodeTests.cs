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
    private static readonly Dictionary<TvEpisodeMethods, Func<TvEpisode, object>> Methods;

    static ClientTvEpisodeTests()
    {
        Methods = new Dictionary<TvEpisodeMethods, Func<TvEpisode, object>>
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
        TvEpisode tvEpisode = await TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1);

        await Verify(tvEpisode);

        // Test all extras, ensure none of them are populated
        foreach (Func<TvEpisode, object> selector in Methods.Values)
        {
            Assert.Null(selector(tvEpisode));
        }
    }

    /// <summary>
    /// Tests that account states can be retrieved with a TV episode, including rating information.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeExtrasAccountState()
    {
        // Test the custom parsing code for Account State rating
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        TvEpisode episode = await TMDbClient.GetTvEpisodeAsync(IdHelper.BigBangTheory, 1, 1, TvEpisodeMethods.AccountStates);
        if (episode.AccountStates == null || !episode.AccountStates.Rating.HasValue)
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
    public async Task TestTvEpisodeExtrasAll()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Account states will only show up if we've done something
        await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.FullerHouse, 1, 1, 5);

        await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetTvEpisodeAsync(IdHelper.FullerHouse, 1, 1, combined), async tvEpisode =>
            {
                await Verify(tvEpisode);
            });
    }

    /// <summary>
    /// Tests that each extra method can be retrieved exclusively for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeExtrasExclusiveAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
        await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, extras));
    }

    /// <summary>
    /// Tests that credits including cast, crew, and guest stars can be retrieved separately for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeSeparateExtrasCreditsAsync()
    {
        CreditsWithGuestStars credits = await TMDbClient.GetTvEpisodeCreditsAsync(IdHelper.BreakingBad, 1, 1);
        Assert.NotNull(credits);

        Cast guestStarItem = credits.GuestStars.FirstOrDefault(s => s.Id == 92495);
        Cast castItem = credits.Cast.FirstOrDefault(s => s.Id == 17419);
        Crew crewItem = credits.Crew.FirstOrDefault(s => s.Id == 1280071);

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
        ExternalIdsTvEpisode externalIds = await TMDbClient.GetTvEpisodeExternalIdsAsync(IdHelper.BreakingBad, 1, 1);

        await Verify(externalIds);
    }

    /// <summary>
    /// Tests that still images can be retrieved separately for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeSeparateExtrasImagesAsync()
    {
        StillImages images = await TMDbClient.GetTvEpisodeImagesAsync(IdHelper.BreakingBad, 1, 1);

        await Verify(images);
    }

    /// <summary>
    /// Tests that videos can be retrieved separately for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeSeparateExtrasVideosAsync()
    {
        ResultContainer<Video> images = await TMDbClient.GetTvEpisodeVideosAsync(IdHelper.BreakingBad, 1, 1);

        await Verify(images);
    }

    /// <summary>
    /// Tests that a TV episode rating can be set, retrieved, and removed through account states.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeAccountStateRatingSetAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
        TvEpisodeAccountState accountState = await TMDbClient.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1);

        // Remove the rating
        if (accountState.Rating.HasValue)
        {
            Assert.True(await TMDbClient.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1));

            // Allow TMDb to cache our changes
            await Task.Delay(2000);
        }
        // Test that the episode is NOT rated
        accountState = await TMDbClient.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1);

        Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, accountState.Id);
        Assert.False(accountState.Rating.HasValue);

        // Rate the episode
        Assert.True(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 5));

        // Allow TMDb to cache our changes
        await Task.Delay(2000);

        // Test that the episode IS rated
        accountState = await TMDbClient.GetTvEpisodeAccountStateAsync(IdHelper.BreakingBad, 1, 1);
        Assert.Equal(IdHelper.BreakingBadSeason1Episode1Id, accountState.Id);
        Assert.True(accountState.Rating.HasValue);

        // Remove the rating
        Assert.True(await TMDbClient.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 1));
    }

    /// <summary>
    /// Tests that invalid rating values are rejected when rating a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeRateBadAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        Assert.False(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, -1));
        Assert.False(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 0));
        Assert.False(await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 1, 10.5));
    }

    /// <summary>
    /// Tests that change history can be retrieved for a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeGetChangesAsync()
    {
        IList<Change> changes = await TMDbClient.GetTvEpisodeChangesAsync(IdHelper.BreakingBadSeason1Episode1Id);

        Assert.NotEmpty(changes);

        await Verify(changes);
    }

    /// <summary>
    /// Tests that null is returned when attempting to retrieve a non-existent TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeMissingAsync()
    {
        TvEpisode tvEpisode = await TMDbClient.GetTvEpisodeAsync(IdHelper.MissingID, 1, 1);

        Assert.Null(tvEpisode);
    }

    /// <summary>
    /// Tests that TV episodes that were screened theatrically can be retrieved for a TV show.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodesScreenedTheatricallyAsync()
    {
        ResultContainer<TvEpisodeInfo> results = await TMDbClient.GetTvEpisodesScreenedTheatricallyAsync(IdHelper.GameOfThrones);
        TvEpisodeInfo single = results.Results.Single(s => s.Id == IdHelper.GameOfThronesSeason4Episode10);

        await Verify(single);
    }

    /// <summary>
    /// Tests that images can be filtered by language when retrieving a TV episode.
    /// </summary>
    [Fact]
    public async Task TestTvEpisodeGetTvEpisodeWithImageLanguageAsync()
    {
        TvEpisode resp = await TMDbClient.GetTvEpisodeAsync(IdHelper.BreakingBad, 1, 1, language: "en-US", includeImageLanguage: "en", extraMethods: TvEpisodeMethods.Images);

        await Verify(resp.Images);
    }
}
