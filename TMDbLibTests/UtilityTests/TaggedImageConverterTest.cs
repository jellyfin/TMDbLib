using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the TaggedImage converter.
/// </summary>
public class TaggedImageConverterTest : TestBase
{
    /// <summary>
    /// Tests that the TaggedImage converter correctly deserializes movie media types.
    /// </summary>
    [Fact]
    public async Task TaggedImageConverter_Movie()
    {
        var originalMedia = new SearchMovie { OriginalTitle = "Hello world" };

        var original = new TaggedImage
        {
            MediaType = originalMedia.MediaType,
            Media = originalMedia
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<TaggedImage>(json);

        Assert.NotNull(result);
        Assert.IsType<SearchMovie>(result.Media);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the TaggedImage converter correctly deserializes TV show media types.
    /// </summary>
    [Fact]
    public async Task TaggedImageConverter_Tv()
    {
        var originalMedia = new SearchTv { OriginalName = "Hello world" };

        var original = new TaggedImage
        {
            MediaType = MediaType.Tv,
            Media = originalMedia
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<TaggedImage>(json);

        Assert.NotNull(result);
        Assert.IsType<SearchTv>(result.Media);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Verifies that the TaggedImage converter correctly deserializes different media types from API responses.
    /// </summary>
    [Theory]
    [InlineData(IdHelper.HughLaurie)] // Has Movie media
    [InlineData(IdHelper.TomHanks)] // Has Episode media
    [InlineData(IdHelper.AnnaTorv)] // Has Tv, Season media
    public async Task TestJsonTaggedImageConverter(int personId)
    {
        // Get images
        var result = await TMDbClient.GetPersonTaggedImagesAsync(personId, 1);

        Assert.NotNull(result);
        Assert.NotNull(result.Results);
        Assert.NotEmpty(result.Results);
        Assert.Equal(personId, result.Id);

        Assert.All(result.Results, item =>
        {
            if (item.MediaType == MediaType.Movie)
            {
                Assert.IsType<SearchMovie>(item.Media);
            }
            else if (item.MediaType == MediaType.Tv)
            {
                Assert.IsType<SearchTv>(item.Media);
            }
            else if (item.MediaType == MediaType.Episode)
            {
                Assert.IsType<SearchTvEpisode>(item.Media);
            }
            else if (item.MediaType == MediaType.Season)
            {
                Assert.IsType<SearchTvSeason>(item.Media);
            }
            else
            {
                Assert.Fail($"Unexpected type {item.GetType().Name}");
            }
        });
    }
}
