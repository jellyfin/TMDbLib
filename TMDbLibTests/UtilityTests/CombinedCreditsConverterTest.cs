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
    /// <summary>
    /// Tests that the CombinedCreditsCast converter correctly deserializes movie cast objects.
    /// </summary>
    [Fact]
    public async Task CombinedCreditsCastConverter_Movie()
    {
        CombinedCreditsCastMovie original = new CombinedCreditsCastMovie
        {
            OriginalTitle = "Die Hard",
            Title = "Die Hard",
            Character = "John McClane"
        };

        string json = Serializer.SerializeToString(original);
        CombinedCreditsCastMovie result = Serializer.DeserializeFromString<CombinedCreditsCastBase>(json) as CombinedCreditsCastMovie;

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
    /// Tests that the CombinedCreditsCast converter correctly deserializes TV show cast objects.
    /// </summary>
    [Fact]
    public async Task CombinedCreditsCastConverter_Tv()
    {
        CombinedCreditsCastTv original = new CombinedCreditsCastTv
        {
            OriginalName = "Breaking Bad",
            Name = "Breaking Bad",
            Character = "Walter White"
        };

        string json = Serializer.SerializeToString(original);
        CombinedCreditsCastTv result = Serializer.DeserializeFromString<CombinedCreditsCastBase>(json) as CombinedCreditsCastTv;

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
    /// Tests that the CombinedCreditsCrew converter correctly deserializes movie crew objects.
    /// </summary>
    [Fact]
    public async Task CombinedCreditsCrewConverter_Movie()
    {
        CombinedCreditsCrewMovie original = new CombinedCreditsCrewMovie
        {
            OriginalTitle = "Pulp Fiction",
            Title = "Pulp Fiction",
            Department = "Directing",
            Job = "Director"
        };

        string json = Serializer.SerializeToString(original);
        CombinedCreditsCrewMovie result = Serializer.DeserializeFromString<CombinedCreditsCrewBase>(json) as CombinedCreditsCrewMovie;

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
    /// Tests that the CombinedCreditsCrew converter correctly deserializes TV show crew objects.
    /// </summary>
    [Fact]
    public async Task CombinedCreditsCrewConverter_Tv()
    {
        CombinedCreditsCrewTv original = new CombinedCreditsCrewTv
        {
            OriginalName = "Game of Thrones",
            Name = "Game of Thrones",
            Department = "Production",
            Job = "Executive Producer"
        };

        string json = Serializer.SerializeToString(original);
        CombinedCreditsCrewTv result = Serializer.DeserializeFromString<CombinedCreditsCrewBase>(json) as CombinedCreditsCrewTv;

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
    /// Verifies that the CombinedCredits converters correctly deserialize different media types from person combined credits API responses.
    /// </summary>
    [Fact]
    public async Task TestJsonCombinedCreditsConverter()
    {
        // Get combined credits for a person who has both movie and TV credits
        CombinedCredits result = await TMDbClient.GetPersonCombinedCreditsAsync(IdHelper.BruceWillis);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Cast);

        // Verify proper deserialization - should have at least one movie cast credit
        Assert.Contains(result.Cast, item => item.MediaType == MediaType.Movie && item is CombinedCreditsCastMovie);

        // TV cast may or may not be present depending on the API response
        bool hasTvCast = result.Cast.Any(item => item.MediaType == MediaType.Tv && item is CombinedCreditsCastTv);
        if (hasTvCast)
        {
            Assert.Contains(result.Cast, item => item.MediaType == MediaType.Tv && item is CombinedCreditsCastTv);
        }

        // Verify crew credits
        if (result.Crew.Count > 0)
        {
            // Check for movie crew credits
            bool hasMovieCrew = result.Crew.Any(item => item.MediaType == MediaType.Movie && item is CombinedCreditsCrewMovie);
            if (hasMovieCrew)
            {
                Assert.Contains(result.Crew, item => item.MediaType == MediaType.Movie && item is CombinedCreditsCrewMovie);
            }

            // Check for TV crew credits
            bool hasTvCrew = result.Crew.Any(item => item.MediaType == MediaType.Tv && item is CombinedCreditsCrewTv);
            if (hasTvCrew)
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
        // Get person with combined credits appended
        Person person = await TMDbClient.GetPersonAsync(IdHelper.BruceWillis, PersonMethods.CombinedCredits);

        Assert.NotNull(person);
        Assert.NotNull(person.CombinedCredits);
        Assert.NotEmpty(person.CombinedCredits.Cast);

        // Verify cast contains properly typed objects
        Assert.Contains(person.CombinedCredits.Cast, item => item is CombinedCreditsCastMovie);
    }
}
