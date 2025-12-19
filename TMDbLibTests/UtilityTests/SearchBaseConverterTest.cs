using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the SearchBase converter.
/// </summary>
public class SearchBaseConverterTest : TestBase
{
    /// <summary>
    /// Tests that the SearchBase converter correctly deserializes movie search results.
    /// </summary>
    [Fact]
    public async Task SearchBaseConverter_Movie()
    {
        SearchMovie original = new SearchMovie
        {
            OriginalTitle = "Hello world"
        };

        string json = Serializer.SerializeToString(original);
        SearchMovie result = Serializer.DeserializeFromString<SearchBase>(json) as SearchMovie;

        Assert.Equal(original.OriginalTitle, result.OriginalTitle);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the SearchBase converter correctly deserializes TV show search results.
    /// </summary>
    [Fact]
    public async Task SearchBaseConverter_Tv()
    {
        SearchTv original = new SearchTv
        {
            OriginalName = "Hello world"
        };

        string json = Serializer.SerializeToString(original);
        SearchTv result = Serializer.DeserializeFromString<SearchBase>(json) as SearchTv;

        Assert.Equal(original.MediaType, result.MediaType);
        Assert.Equal(original.OriginalName, result.OriginalName);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the SearchBase converter correctly deserializes person search results.
    /// </summary>
    [Fact]
    public async Task SearchBaseConverter_Person()
    {
        SearchPerson original = new SearchPerson
        {
            Name = "Hello world"
        };

        string json = Serializer.SerializeToString(original);
        SearchPerson result = Serializer.DeserializeFromString<SearchBase>(json) as SearchPerson;

        Assert.Equal(original.MediaType, result.MediaType);
        Assert.Equal(original.Name, result.Name);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Verifies that the SearchBase converter correctly deserializes multi-search API responses containing different media types.
    /// </summary>
    [Fact]
    public async Task TestSearchBaseConverter()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchMultiAsync("Jobs", i));
        SearchContainer<SearchBase> result = await TMDbClient.SearchMultiAsync("Jobs");

        Assert.NotNull(result);
        Assert.NotNull(result.Results);

        Assert.Contains(result.Results, item => item.MediaType == MediaType.Tv && item is SearchTv);
        Assert.Contains(result.Results, item => item.MediaType == MediaType.Movie && item is SearchMovie);
        Assert.Contains(result.Results, item => item.MediaType == MediaType.Person && item is SearchPerson);
    }
}
