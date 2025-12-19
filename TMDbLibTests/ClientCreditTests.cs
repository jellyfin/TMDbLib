using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Credit;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using System.Globalization;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb credit functionality.
/// </summary>
public class ClientCreditTests : TestBase
{
    /// <summary>
    /// Tests that retrieving TV credit information returns expected episode data.
    /// </summary>
    [Fact]
    public async Task TestGetCreditTv()
    {
        Credit result = await TMDbClient.GetCreditsAsync(IdHelper.BruceWillisMiamiVice);

        // Episode must exist
        Assert.Contains(result.Media.Episodes, s => s.Name == "No Exit");

        await Verify(result);
    }

    /// <summary>
    /// Tests that attempting to retrieve a non-existent credit returns null.
    /// </summary>
    [Fact]
    public async Task TestMissingCredit()
    {
        Credit result = await TMDbClient.GetCreditsAsync(IdHelper.MissingID.ToString(CultureInfo.InvariantCulture));

        Assert.Null(result);
    }

    /// <summary>
    /// Tests that retrieving season credit information returns expected season data.
    /// </summary>
    [Fact]
    public async Task TestGetCreditSeasons()
    {
        Credit result = await TMDbClient.GetCreditsAsync(IdHelper.HughLaurieHouse);

        // Season must exist
        Assert.Contains(result.Media.Seasons, s => s.SeasonNumber == 1);

        await Verify(result);
    }
}
