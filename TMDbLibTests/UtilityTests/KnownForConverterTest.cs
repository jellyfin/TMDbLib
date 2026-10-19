using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Tests for the KnownFor polymorphic converter. The converter is wired property-level on
/// <see cref="SearchPerson.KnownFor"/>, so these tests exercise the converter through that wrapper.
/// </summary>
public class KnownForConverterTest : TestBase
{
    private static string WrapKnownFor(string itemJson) => $"{{\"known_for\": [{itemJson}]}}";

    /// <summary>
    /// Movie payloads dispatch to <see cref="TmdbMovieSummary"/>.
    /// </summary>
    [Fact]
    public async Task KnownForConverter_Movie()
    {
        var original = new TmdbMovieSummary
        {
            OriginalTitle = "Hello world"
        };

        var json = WrapKnownFor(Serializer.SerializeToString(original));
        var container = Serializer.DeserializeFromString<SearchPerson>(json);
        var result = container?.KnownFor?[0] as TmdbMovieSummary;

        Assert.NotNull(result);
        Assert.Equal(original.OriginalTitle, result.OriginalTitle);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// TV payloads dispatch to <see cref="TmdbTvSummary"/>.
    /// </summary>
    [Fact]
    public async Task KnownForConverter_Tv()
    {
        var original = new TmdbTvSummary
        {
            OriginalName = "Hello world"
        };

        var json = WrapKnownFor(Serializer.SerializeToString(original));
        var container = Serializer.DeserializeFromString<SearchPerson>(json);
        var result = container?.KnownFor?[0] as TmdbTvSummary;

        Assert.NotNull(result);
        Assert.Equal(original.OriginalName, result.OriginalName);
        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// A media type no summary has a shape for is left out rather than failing the whole response.
    /// </summary>
    [Fact]
    public void KnownForConverter_UnsupportedMediaType_IsSkipped()
    {
        // What /search/person answers for a person known for a collection: typed as one, keyed as a movie.
        const string CollectionItem = """
            {"adult": true, "id": 720774, "title": "Game Boys Collection", "original_title": "Game Boys Collection", "media_type": "collection"}
            """;

        var json = WrapKnownFor(CollectionItem + ", " + Serializer.SerializeToString(new TmdbMovieSummary { OriginalTitle = "Hello world" }));
        var container = Serializer.DeserializeFromString<SearchPerson>(json);

        var kept = Assert.Single(container?.KnownFor!);
        Assert.Equal("Hello world", Assert.IsType<TmdbMovieSummary>(kept).OriginalTitle);
    }

    /// <summary>
    /// End-to-end: /search/person mixed known_for items dispatch to the correct concrete types.
    /// </summary>
    [Fact]
    public async Task TestJsonKnownForConverter()
    {
        var result = await TMDbClient.SearchPersonAsync("Bryan Cranston", cancellationToken: TestContext.Current.CancellationToken);
        Assert.NotNull(result?.Results);

        var knownForList = result.Results.SelectMany(s => s.KnownFor ?? new List<TmdbMediaSummary>()).ToList();
        Assert.NotEmpty(knownForList);

        Assert.Contains(knownForList, item => item.MediaType == MediaType.Movie && item is TmdbMovieSummary);

        if (knownForList.Any(item => item.MediaType == MediaType.Tv))
        {
            Assert.Contains(knownForList, item => item.MediaType == MediaType.Tv && item is TmdbTvSummary);
        }
    }
}
