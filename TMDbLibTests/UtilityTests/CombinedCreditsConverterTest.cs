using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the CombinedCredits converters.
/// </summary>
public class CombinedCreditsConverterTest : TestBase
{
    private static string WrapCast(string itemJson) => $"{{\"cast\": [{itemJson}]}}";

    private static string WrapCrew(string itemJson) => $"{{\"crew\": [{itemJson}]}}";

    /// <summary>
    /// Tests that the CombinedCreditsCast converter correctly deserializes movie cast items
    /// via the wrapper polymorphism.
    /// </summary>
    [Fact]
    public async Task CombinedCreditsCastConverter_Movie()
    {
        var original = new CombinedCreditsCastMovie
        {
            OriginalTitle = "Die Hard",
            Title = "Die Hard",
            Character = "John McClane"
        };

        var json = WrapCast(Serializer.SerializeToString(original));
        var container = Serializer.DeserializeFromString<CombinedCredits>(json);
        var result = container?.Cast?[0] as CombinedCreditsCastMovie;

        Assert.NotNull(result);
        Assert.Equal(original.OriginalTitle, result.OriginalTitle);
        Assert.Equal(original.Title, result.Title);
        Assert.Equal(original.Character, result.Character);
        Assert.Equal(MediaType.Movie, result.MediaType);

        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the CombinedCreditsCast converter correctly deserializes TV show cast items.
    /// </summary>
    [Fact]
    public async Task CombinedCreditsCastConverter_Tv()
    {
        var original = new CombinedCreditsCastTv
        {
            OriginalName = "Breaking Bad",
            Name = "Breaking Bad",
            Character = "Walter White"
        };

        var json = WrapCast(Serializer.SerializeToString(original));
        var container = Serializer.DeserializeFromString<CombinedCredits>(json);
        var result = container?.Cast?[0] as CombinedCreditsCastTv;

        Assert.NotNull(result);
        Assert.Equal(original.OriginalName, result.OriginalName);
        Assert.Equal(original.Name, result.Name);
        Assert.Equal(original.Character, result.Character);
        Assert.Equal(MediaType.Tv, result.MediaType);

        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the CombinedCreditsCrew converter correctly deserializes movie crew items.
    /// </summary>
    [Fact]
    public async Task CombinedCreditsCrewConverter_Movie()
    {
        var original = new CombinedCreditsCrewMovie
        {
            OriginalTitle = "Pulp Fiction",
            Title = "Pulp Fiction",
            Department = "Directing",
            Job = "Director"
        };

        var json = WrapCrew(Serializer.SerializeToString(original));
        var container = Serializer.DeserializeFromString<CombinedCredits>(json);
        var result = container?.Crew?[0] as CombinedCreditsCrewMovie;

        Assert.NotNull(result);
        Assert.Equal(original.OriginalTitle, result.OriginalTitle);
        Assert.Equal(original.Title, result.Title);
        Assert.Equal(original.Department, result.Department);
        Assert.Equal(original.Job, result.Job);
        Assert.Equal(MediaType.Movie, result.MediaType);

        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Tests that the CombinedCreditsCrew converter correctly deserializes TV show crew items.
    /// </summary>
    [Fact]
    public async Task CombinedCreditsCrewConverter_Tv()
    {
        var original = new CombinedCreditsCrewTv
        {
            OriginalName = "Game of Thrones",
            Name = "Game of Thrones",
            Department = "Production",
            Job = "Executive Producer"
        };

        var json = WrapCrew(Serializer.SerializeToString(original));
        var container = Serializer.DeserializeFromString<CombinedCredits>(json);
        var result = container?.Crew?[0] as CombinedCreditsCrewTv;

        Assert.NotNull(result);
        Assert.Equal(original.OriginalName, result.OriginalName);
        Assert.Equal(original.Name, result.Name);
        Assert.Equal(original.Department, result.Department);
        Assert.Equal(original.Job, result.Job);
        Assert.Equal(MediaType.Tv, result.MediaType);

        await Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Verifies that the CombinedCredits converters correctly deserialize different media types
    /// from person combined credits responses.
    /// </summary>
    [Fact]
    public async Task TestJsonCombinedCreditsConverter()
    {
        var result = await TMDbClient.GetPersonCombinedCreditsAsync(IdHelper.BruceWillis, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(result);
        Assert.NotNull(result.Cast);
        Assert.NotEmpty(result.Cast);

        Assert.Contains(result.Cast, item => item.MediaType == MediaType.Movie && item is CombinedCreditsCastMovie);

        if (result.Cast.Any(item => item.MediaType == MediaType.Tv))
        {
            Assert.Contains(result.Cast, item => item.MediaType == MediaType.Tv && item is CombinedCreditsCastTv);
        }

        Assert.NotNull(result.Crew);
        if (result.Crew.Count > 0)
        {
            if (result.Crew.Any(item => item.MediaType == MediaType.Movie))
            {
                Assert.Contains(result.Crew, item => item.MediaType == MediaType.Movie && item is CombinedCreditsCrewMovie);
            }

            if (result.Crew.Any(item => item.MediaType == MediaType.Tv))
            {
                Assert.Contains(result.Crew, item => item.MediaType == MediaType.Tv && item is CombinedCreditsCrewTv);
            }
        }
    }

    /// <summary>
    /// Tests retrieving combined credits via the append_to_response mechanism.
    /// </summary>
    [Fact]
    public async Task TestPersonCombinedCreditsAppendToResponse()
    {
        var person = await TMDbClient.GetPersonAsync(IdHelper.BruceWillis, PersonMethods.CombinedCredits, cancellationToken: TestContext.Current.CancellationToken);

        Assert.NotNull(person);
        Assert.NotNull(person.CombinedCredits);
        Assert.NotNull(person.CombinedCredits.Cast);
        Assert.NotEmpty(person.CombinedCredits.Cast);

        Assert.Contains(person.CombinedCredits.Cast, item => item is CombinedCreditsCastMovie);
    }
}
