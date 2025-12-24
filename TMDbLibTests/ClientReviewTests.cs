using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Reviews;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using System.Globalization;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb review functionality.
/// </summary>
public class ClientReviewTests : TestBase
{
    /// <summary>
    /// Tests that full details for a review can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestReviewFullDetails()
    {
        var review = await TMDbClient.GetReviewAsync(IdHelper.TheDarkKnightRisesReviewId);

        await Verify(review);
    }

    /// <summary>
    /// Verifies that retrieving a non-existent review returns null.
    /// </summary>
    [Fact]
    public async Task TestReviewMissing()
    {
        var review = await TMDbClient.GetReviewAsync(IdHelper.MissingID.ToString(CultureInfo.InvariantCulture));

        Assert.Null(review);
    }
}
