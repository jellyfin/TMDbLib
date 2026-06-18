using System;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb changes functionality.
/// </summary>
public class ClientChangesTests : TestBase
{
    // Fixed date for deterministic WireMock playback (recorded 2025-12-24)
    private static readonly DateTime FixedEndDate = new(2025, 11, 24, 0, 0, 0, DateTimeKind.Utc);

    /// <summary>
    /// Tests that retrieving movie changes returns results and newer changes are present.
    /// </summary>
    [Fact]
    public async Task TestChangesMoviesAsync()
    {
        var page1 = await TMDbClient.GetMoviesChangesAsync(1, cancellationToken: TestContext.Current.CancellationToken);
        var oldChanges = await TMDbClient.GetMoviesChangesAsync(endDate: FixedEndDate, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(page1);
        Assert.NotNull(page1.Results);
        Assert.NotNull(oldChanges);
        Assert.NotNull(oldChanges.Results);
        Assert.NotEmpty(page1.Results);
        Assert.NotEmpty(oldChanges.Results);

        // At least one item must be newer in page1
        Assert.Contains(page1.Results, x => oldChanges.Results.All(s => s.Id != x.Id));
    }

    /// <summary>
    /// Tests that retrieving people changes returns results and newer changes are present.
    /// </summary>
    [Fact]
    public async Task TestChangesPeopleAsync()
    {
        var page1 = await TMDbClient.GetPeopleChangesAsync(1, cancellationToken: TestContext.Current.CancellationToken);
        var oldChanges = await TMDbClient.GetPeopleChangesAsync(endDate: FixedEndDate, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(page1);
        Assert.NotNull(page1.Results);
        Assert.NotNull(oldChanges);
        Assert.NotNull(oldChanges.Results);
        Assert.NotEmpty(page1.Results);
        Assert.NotEmpty(oldChanges.Results);

        // At least one item must be newer in page1
        Assert.Contains(page1.Results, x => oldChanges.Results.All(s => s.Id != x.Id));
    }

    /// <summary>
    /// Tests that retrieving TV show changes returns results and newer changes are present.
    /// </summary>
    [Fact]
    public async Task TestChangesTvShowsAsync()
    {
        var page1 = await TMDbClient.GetTvChangesAsync(1, cancellationToken: TestContext.Current.CancellationToken);
        var oldChanges = await TMDbClient.GetTvChangesAsync(endDate: FixedEndDate, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(page1);
        Assert.NotNull(page1.Results);
        Assert.NotNull(oldChanges);
        Assert.NotNull(oldChanges.Results);
        Assert.NotEmpty(page1.Results);
        Assert.NotEmpty(oldChanges.Results);

        // At least one item must be newer in page1
        Assert.Contains(page1.Results, x => oldChanges.Results.All(s => s.Id != x.Id));
    }
}
