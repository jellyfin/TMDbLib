using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.Movies;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the ChangeItem converter.
/// </summary>
public class ChangeItemConverterTest : TestBase
{
    /// <summary>
    /// Tests that the ChangeItem converter correctly deserializes ChangeItemAdded objects.
    /// </summary>
    [Fact]
    public async Task ChangeItemConverter_ChangeItemAdded()
    {
        var original = new ChangeItemAdded
        {
            Iso_639_1 = "en",
            Value = "Hello world"
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<ChangeItemBase>(json) as ChangeItemAdded;

        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the ChangeItem converter correctly deserializes ChangeItemCreated objects.
    /// </summary>
    [Fact]
    public async Task ChangeItemConverter_ChangeItemCreated()
    {
        var original = new ChangeItemCreated
        {
            Iso_639_1 = "en"
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<ChangeItemBase>(json) as ChangeItemCreated;

        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the ChangeItem converter correctly deserializes ChangeItemDeleted objects.
    /// </summary>
    [Fact]
    public async Task ChangeItemConverter_ChangeItemDeleted()
    {
        var original = new ChangeItemDeleted
        {
            Iso_639_1 = "en",
            OriginalValue = "Hello world"
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<ChangeItemBase>(json) as ChangeItemDeleted;

        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the ChangeItem converter correctly deserializes ChangeItemUpdated objects.
    /// </summary>
    [Fact]
    public async Task ChangeItemConverter_ChangeItemUpdated()
    {
        var original = new ChangeItemUpdated
        {
            Iso_639_1 = "en",
            OriginalValue = "Hello world",
            Value = "Hello world 1234"
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<ChangeItemBase>(json) as ChangeItemUpdated;

        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Verifies that the ChangeItem converter correctly deserializes different change types from API responses.
    /// </summary>
    [Fact]
    public async Task TestChangeItemConverter()
    {
        var latestMovie = await TMDbClient.GetMovieLatestAsync();
        Assert.NotNull(latestMovie);

        var changes = await TMDbClient.GetMovieChangesAsync(latestMovie.Id);
        Assert.NotNull(changes);
        var changeItems = changes.SelectMany(s => s.Items ?? []).ToList();

        // The latest movie may or may not have changes - skip if empty
        if (changeItems.Count == 0)
        {
            return; // Skip test if no changes available
        }

        var actions = new[] { ChangeAction.Added, ChangeAction.Created, ChangeAction.Updated, ChangeAction.Deleted };

        Assert.All(changeItems, item => Assert.Contains(item.Action, actions));

        var items = changeItems.Where(s => s.Action == ChangeAction.Added);
        Assert.All(items, item => Assert.IsType<ChangeItemAdded>(item));

        items = changeItems.Where(s => s.Action == ChangeAction.Updated);
        Assert.All(items, item => Assert.IsType<ChangeItemUpdated>(item));

        items = changeItems.Where(s => s.Action == ChangeAction.Created);
        Assert.All(items, item => Assert.IsType<ChangeItemCreated>(item));

        items = changeItems.Where(s => s.Action == ChangeAction.Deleted);
        Assert.All(items, item => Assert.IsType<ChangeItemDeleted>(item));
    }
}
