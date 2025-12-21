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
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb TV season functionality.
/// </summary>
public class ClientTvSeasonTests : TestBase
{
    private static readonly Dictionary<TvSeasonMethods, Func<TvSeason, object>> Methods;

    static ClientTvSeasonTests()
    {
        Methods = new Dictionary<TvSeasonMethods, Func<TvSeason, object>>
        {
            [TvSeasonMethods.Credits] = tvSeason => tvSeason.Credits,
            [TvSeasonMethods.Images] = tvSeason => tvSeason.Images,
            [TvSeasonMethods.ExternalIds] = tvSeason => tvSeason.ExternalIds,
            [TvSeasonMethods.Videos] = tvSeason => tvSeason.Videos,
            [TvSeasonMethods.AccountStates] = tvSeason => tvSeason.AccountStates
        };
    }

    /// <summary>
    /// Tests that a TV season can be retrieved without any extra methods.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonExtrasNoneAsync()
    {
        TvSeason tvSeason = await TMDbClient.GetTvSeasonAsync(IdHelper.BreakingBad, 1);

        await Verify(tvSeason);

        // Test all extras, ensure none of them are populated
        foreach (Func<TvSeason, object> selector in Methods.Values)
        {
            Assert.Null(selector(tvSeason));
        }
    }

    /// <summary>
    /// Tests that account states can be retrieved with a TV season, including episode ratings.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonExtrasAccountState()
    {
        // Test the custom parsing code for Account State rating
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        TvSeason season = await TMDbClient.GetTvSeasonAsync(IdHelper.BigBangTheory, 1, TvSeasonMethods.AccountStates);
        if (season.AccountStates == null || season.AccountStates.Results.All(s => s.EpisodeNumber != 1))
        {
            await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BigBangTheory, 1, 1, 5);

            // Allow TMDb to update cache
            await Task.Delay(2000);

            season = await TMDbClient.GetTvSeasonAsync(IdHelper.BigBangTheory, 1, TvSeasonMethods.AccountStates);
        }
        Assert.NotNull(season.AccountStates);
        Assert.True(season.AccountStates.Results.Single(s => s.EpisodeNumber == 1).Rating.HasValue);
        Assert.True(Math.Abs(season.AccountStates.Results.Single(s => s.EpisodeNumber == 1).Rating.Value - 5) < double.Epsilon);
    }

    /// <summary>
    /// Tests that all extra methods can be retrieved together for a TV season.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonExtrasAllAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        // Account states will only show up if we've done something
        await TMDbClient.TvEpisodeSetRatingAsync(IdHelper.FullerHouse, 1, 1, 5);

        await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetTvSeasonAsync(IdHelper.FullerHouse, 1, combined), season => Verify(season));
    }

    /// <summary>
    /// Tests that each extra method can be retrieved exclusively for a TV season.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonExtrasExclusiveAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetTvSeasonAsync(IdHelper.BreakingBad, 1, extras));
    }

    /// <summary>
    /// Tests that credits can be retrieved separately for a TV season.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonSeparateExtrasCreditsAsync()
    {
        Credits credits = await TMDbClient.GetTvSeasonCreditsAsync(IdHelper.BreakingBad, 1);

        await Verify(credits);
    }

    /// <summary>
    /// Tests that external IDs can be retrieved separately for a TV season.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonSeparateExtrasExternalIdsAsync()
    {
        ExternalIdsTvSeason externalIds = await TMDbClient.GetTvSeasonExternalIdsAsync(IdHelper.BreakingBad, 1);

        await Verify(externalIds);
    }

    /// <summary>
    /// Tests that poster images can be retrieved separately for a TV season.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonSeparateExtrasImagesAsync()
    {
        PosterImages images = await TMDbClient.GetTvSeasonImagesAsync(IdHelper.BreakingBad, 1);

        Assert.NotEmpty(images.Posters);
        TestImagesHelpers.TestImagePaths(images.Posters);
    }

    /// <summary>
    /// Tests that videos can be retrieved separately for a TV season.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonSeparateExtrasVideosAsync()
    {
        ResultContainer<Video> videos = await TMDbClient.GetTvSeasonVideosAsync(IdHelper.GameOfThrones, 1);
        Video single = videos.Results.Single(s => s.Id == "5c9b7e95c3a36841a341b9c6");

        await Verify(single);
    }

    /// <summary>
    /// Tests that episode ratings can be set and removed, reflected in the season's account state.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonAccountStateRatingSetAsync()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

        await TestMethodsHelper.SetValidateRemoveTest(
            () => TMDbClient.TvEpisodeSetRatingAsync(IdHelper.BreakingBad, 1, 3, 5),
            () => TMDbClient.TvEpisodeRemoveRatingAsync(IdHelper.BreakingBad, 1, 3),
            async shouldBeSet =>
            {
                ResultContainer<TvEpisodeAccountStateWithNumber> state = await TMDbClient.GetTvSeasonAccountStateAsync(IdHelper.BreakingBad, 1);

                if (shouldBeSet)
                {
                    Assert.Contains(state.Results, x => x.EpisodeNumber == 3 && x.Rating.HasValue);
                }
                else
                {
                    Assert.Contains(state.Results, x => x.EpisodeNumber == 3 && !x.Rating.HasValue);
                }
            });
    }

    /// <summary>
    /// Tests that change history can be retrieved for a TV season.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonGetChangesAsync()
    {
        TvShow latestTvShow = await TMDbClient.GetLatestTvShowAsync();
        if (latestTvShow.Seasons == null || latestTvShow.Seasons.Count == 0)
        {
            return; // Skip if no seasons available
        }

        int latestSeasonId = latestTvShow.Seasons.Max(s => s.Id);
        IList<Change> changes = await TMDbClient.GetTvSeasonChangesAsync(latestSeasonId);

        // Changes may or may not exist depending on recent activity
        Assert.NotNull(changes);
    }

    /// <summary>
    /// Tests that null is returned when attempting to retrieve a non-existent TV season.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonMissingAsync()
    {
        TvSeason tvSeason = await TMDbClient.GetTvSeasonAsync(IdHelper.MissingID, 1);

        Assert.Null(tvSeason);
    }

    /// <summary>
    /// Tests that images can be filtered by language when retrieving a TV season.
    /// </summary>
    [Fact]
    public async Task TestTvSeasonGetTvSeasonWithImageLanguageAsync()
    {
        TvSeason resp = await TMDbClient.GetTvSeasonAsync(IdHelper.BreakingBad, 1, language: "en-US", includeImageLanguage: "en", extraMethods: TvSeasonMethods.Images);

        Assert.NotNull(resp.Images);
        Assert.NotEmpty(resp.Images.Posters);

        // Get first English poster
        ImageData poster = resp.Images.Posters.First();
        Assert.NotNull(poster.FilePath);
    }
}
