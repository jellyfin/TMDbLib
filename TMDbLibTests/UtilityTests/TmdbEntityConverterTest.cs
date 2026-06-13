using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Tests the polymorphic <see cref="TmdbEntity"/> converter.
/// </summary>
public class TmdbEntityConverterTest : TestBase
{
    /// <summary>
    /// Movie payloads dispatch to <see cref="SearchMovie"/>.
    /// </summary>
    [Fact]
    public async Task TmdbEntityConverter_Movie()
    {
        var original = new SearchMovie
        {
            OriginalTitle = "Hello world"
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<TmdbEntity>(json) as SearchMovie;

        Assert.Equal(original.OriginalTitle, result?.OriginalTitle);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// TV payloads dispatch to <see cref="SearchTv"/>.
    /// </summary>
    [Fact]
    public async Task TmdbEntityConverter_Tv()
    {
        var original = new SearchTv
        {
            OriginalName = "Hello world"
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<TmdbEntity>(json) as SearchTv;

        Assert.Equal(original.MediaType, result?.MediaType);
        Assert.Equal(original.OriginalName, result?.OriginalName);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Person payloads dispatch to <see cref="SearchPerson"/>.
    /// </summary>
    [Fact]
    public async Task TmdbEntityConverter_Person()
    {
        var original = new SearchPerson
        {
            Name = "Hello world"
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<TmdbEntity>(json) as SearchPerson;

        Assert.Equal(original.MediaType, result?.MediaType);
        Assert.Equal(original.Name, result?.Name);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// /search/multi end-to-end: mixed list dispatches to each concrete search type.
    /// </summary>
    [Fact]
    public async Task SearchMulti_DispatchesByMediaType()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.SearchMultiAsync("Jobs", i));
        var result = await TMDbClient.SearchMultiAsync("Jobs", cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.NotNull(result.Results);

        Assert.Contains(result.Results, item => item.MediaType == MediaType.Tv && item is SearchTv);
        Assert.Contains(result.Results, item => item.MediaType == MediaType.Movie && item is SearchMovie);
        Assert.Contains(result.Results, item => item.MediaType == MediaType.Person && item is SearchPerson);
    }
}
