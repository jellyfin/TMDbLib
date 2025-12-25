using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the AccountState converter.
/// </summary>
public class AccountStateConverterTest : TestBase
{
    /// <summary>
    /// Tests that the AccountState converter correctly deserializes when rating data is present.
    /// </summary>
    [Fact]
    public void AccountStateConverter_WithData()
    {
        // { "rated": { "value": 5 } }
        var original = new
        {
            rated = new { value = 5 }
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<AccountState>(json);

        Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the AccountState converter correctly deserializes when rating data is false.
    /// </summary>
    [Fact]
    public void AccountStateConverter_WithoutData()
    {
        // { "rated": false }
        var original = new { rated = false };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<AccountState>(json);

        Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests the AccountStateConverter on the AccountState type
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestAccountStateConverterAccountState()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
        var accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.Avatar);

        Assert.NotNull(accountState);
        await Verify(accountState);
    }

    /// <summary>
    /// Tests the AccountStateConverter on the TvEpisodeAccountState type
    /// </summary>
    [Fact]
    [Trait("Category", "RequiresAccountAccess")]
    public async Task TestAccountStateConverterTvEpisodeAccountState()
    {
        await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
        var season = await TMDbClient.GetTvSeasonAccountStateAsync(IdHelper.BigBangTheory, 1);
        Assert.NotNull(season);
        Assert.NotNull(season.Results);

        // Episode 1 has a rating
        var episodeA = season.Results.Single(s => s.EpisodeNumber == 1);
        Assert.NotNull(episodeA.Rating);

        // Episode 2 has no rating
        var episodeB = season.Results.Single(s => s.EpisodeNumber == 2);
        Assert.Null(episodeB.Rating);

        await Verify(new
        {
            episodeA,
            episodeB
        });
    }
}
