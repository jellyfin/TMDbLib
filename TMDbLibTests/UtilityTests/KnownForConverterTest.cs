using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the KnownFor polymorphic converter.
/// </summary>
public class KnownForConverterTest : TestBase
{
    /// <summary>
    /// Tests that the KnownFor converter correctly deserializes movie objects.
    /// </summary>
    [Fact]
    public async Task KnownForConverter_Movie()
    {
        var original = new TmdbMovieSummary
        {
            OriginalTitle = "Hello world"
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<TmdbMediaSummary>(json) as TmdbMovieSummary;

        Assert.Equal(original.Title, result?.Title);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the KnownFor converter correctly deserializes TV show objects.
    /// </summary>
    [Fact]
    public async Task KnownForConverter_Tv()
    {
        var original = new TmdbTvSummary
        {
            OriginalName = "Hello world"
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<TmdbMediaSummary>(json) as TmdbTvSummary;

        Assert.Equal(original.OriginalName, result?.OriginalName);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Verifies that the KnownFor converter correctly deserializes different media types from
    /// person search responses.
    /// </summary>
    [Fact]
    public async Task TestJsonKnownForConverter()
    {
        var result = await TMDbClient.SearchPersonAsync("Bryan Cranston", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(result?.Results);

        var knownForList = result.Results.SelectMany(s => s.KnownFor ?? []).ToList();
        Assert.NotEmpty(knownForList);

        Assert.Contains(knownForList, item => item.MediaType == MediaType.Movie && item is TmdbMovieSummary);

        // TV shows may or may not be present depending on the API response
        if (knownForList.Any(item => item.MediaType == MediaType.Tv))
        {
            Assert.Contains(knownForList, item => item.MediaType == MediaType.Tv && item is TmdbTvSummary);
        }
    }
}
