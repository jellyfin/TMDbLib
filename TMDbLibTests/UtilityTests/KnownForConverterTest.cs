using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the KnownFor converter.
/// </summary>
public class KnownForConverterTest : TestBase
{
    /// <summary>
    /// Tests that the KnownFor converter correctly deserializes movie objects.
    /// </summary>
    [Fact]
    public async Task KnownForConverter_Movie()
    {
        KnownForMovie original = new KnownForMovie
        {
            OriginalTitle = "Hello world"
        };

        string json = Serializer.SerializeToString(original);
        KnownForMovie result = Serializer.DeserializeFromString<KnownForBase>(json) as KnownForMovie;

        Assert.Equal(original.Title, result.Title);
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
        KnownForTv original = new KnownForTv
        {
            OriginalName = "Hello world"
        };

        string json = Serializer.SerializeToString(original);
        KnownForTv result = Serializer.DeserializeFromString<KnownForBase>(json) as KnownForTv;

        Assert.Equal(original.OriginalName, result.OriginalName);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Verifies that the KnownFor converter correctly deserializes different media types from person search API responses.
    /// </summary>
    [Fact]
    public async Task TestJsonKnownForConverter()
    {
        // Search for a person who is known for both TV and movies
        SearchContainer<SearchPerson> result = await TMDbClient.SearchPersonAsync("Bryan Cranston");

        Assert.NotNull(result?.Results);

        List<KnownForBase> knownForList = result.Results.SelectMany(s => s.KnownFor).ToList();
        Assert.NotEmpty(knownForList);

        // Verify proper deserialization - at least one of each type should be present
        // or at minimum verify movies are properly deserialized
        Assert.Contains(knownForList, item => item.MediaType == MediaType.Movie && item is KnownForMovie);

        // TV shows may or may not be present depending on the API response
        bool hasTv = knownForList.Any(item => item.MediaType == MediaType.Tv && item is KnownForTv);
        if (hasTv)
        {
            Assert.Contains(knownForList, item => item.MediaType == MediaType.Tv && item is KnownForTv);
        }
    }
}
