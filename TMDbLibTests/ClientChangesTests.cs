using System;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using Xunit;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb changes functionality.
/// </summary>
public class ClientChangesTests : TestBase
{
    /// <summary>
    /// Tests that retrieving movie changes returns results and newer changes are present.
    /// </summary>
    [Fact]
    public async Task TestChangesMoviesAsync()
    {
        SearchContainer<ChangesListItem> page1 = await TMDbClient.GetMoviesChangesAsync(1);
        SearchContainer<ChangesListItem> oldChanges = await TMDbClient.GetMoviesChangesAsync(endDate: DateTime.UtcNow.AddMonths(-1));

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
        SearchContainer<ChangesListItem> page1 = await TMDbClient.GetPeopleChangesAsync(1);
        SearchContainer<ChangesListItem> oldChanges = await TMDbClient.GetPeopleChangesAsync(endDate: DateTime.UtcNow.AddMonths(-1));

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
        SearchContainer<ChangesListItem> page1 = await TMDbClient.GetTvChangesAsync(1);
        SearchContainer<ChangesListItem> oldChanges = await TMDbClient.GetTvChangesAsync(endDate: DateTime.UtcNow.AddMonths(-1));

        Assert.NotEmpty(page1.Results);
        Assert.NotEmpty(oldChanges.Results);

        // At least one item must be newer in page1
        Assert.Contains(page1.Results, x => oldChanges.Results.All(s => s.Id != x.Id));
    }
}
