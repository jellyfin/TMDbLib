using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb person functionality.
/// </summary>
public class ClientPersonTests : TestBase
{
    private static readonly Dictionary<PersonMethods, Func<Person, object?>> Methods;

    static ClientPersonTests()
    {
        Methods = new Dictionary<PersonMethods, Func<Person, object?>>
        {
            [PersonMethods.MovieCredits] = person => person.MovieCredits,
            [PersonMethods.TvCredits] = person => person.TvCredits,
            [PersonMethods.CombinedCredits] = person => person.CombinedCredits,
            [PersonMethods.ExternalIds] = person => person.ExternalIds,
            [PersonMethods.Images] = person => person.Images,
            [PersonMethods.TaggedImages] = person => person.TaggedImages,
            [PersonMethods.Changes] = person => person.Changes
        };
    }

    /// <summary>
    /// Tests that retrieving a person without extra methods returns no additional data.
    /// </summary>
    [Fact]
    public async Task TestPersonsExtrasNoneAsync()
    {
        var person = await TMDbClient.GetPersonAsync(IdHelper.BruceWillis);
        Assert.NotNull(person);

        // Test all extras, ensure none of them exist
        foreach (var selector in Methods.Values)
        {
            Assert.Null(selector(person));
        }
        await Verify(person);
    }

    /// <summary>
    /// Tests that each person extra method can be requested exclusively.
    /// </summary>
    [Fact]
    public async Task TestPersonsExtrasExclusive()
    {
        await TestMethodsHelper.TestGetExclusive(Methods, async extras =>
        {
            var result = await TMDbClient.GetPersonAsync(IdHelper.BruceWillis, extras);
            Assert.NotNull(result);
            return result;
        });
    }

    /// <summary>
    /// Tests that all person extra methods can be requested together.
    /// </summary>
    [Fact]
    public async Task TestPersonsExtrasAllAsync()
    {
        await TestMethodsHelper.TestGetAll(Methods, async combined =>
        {
            var result = await TMDbClient.GetPersonAsync(IdHelper.FrankSinatra, combined);
            Assert.NotNull(result);
            return result;
        }, async person =>
        {
            Assert.NotNull(person);
            await Verify(person);
        });
    }

    /// <summary>
    /// Verifies that retrieving a non-existent person returns null.
    /// </summary>
    [Fact]
    public async Task TestPersonMissingAsync()
    {
        var person = await TMDbClient.GetPersonAsync(IdHelper.MissingID);

        Assert.Null(person);
    }

    /// <summary>
    /// Tests that TV credits for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestPersonsGetPersonTvCreditsAsync()
    {
        var item = await TMDbClient.GetPersonTvCreditsAsync(IdHelper.BruceWillis);

        Assert.NotNull(item);
        Assert.NotNull(item.Cast);
        Assert.NotEmpty(item.Cast);

        // Verify we get valid TV credit data
        var cast = item.Cast.First();
        Assert.NotNull(cast.CreditId);

        // Crew may or may not have entries
        Assert.NotNull(item.Crew);
        if (item.Crew.Count > 0)
        {
            var crew = item.Crew.First();
            Assert.NotNull(crew.CreditId);
        }
    }

    /// <summary>
    /// Tests that movie credits for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetPersonMovieCreditsAsync()
    {
        var item = await TMDbClient.GetPersonMovieCreditsAsync(IdHelper.BruceWillis);

        Assert.NotNull(item);
        Assert.NotNull(item.Cast);
        Assert.NotNull(item.Crew);
        Assert.NotEmpty(item.Cast);
        Assert.NotEmpty(item.Crew);

        var cast = item.Cast.Single(s => s.CreditId == "52fe4329c3a36847f803f193");
        var crew = item.Crew.Single(s => s.CreditId == "52fe432ec3a36847f8040603");

        await Verify(new
        {
            cast,
            crew
        });
    }

    /// <summary>
    /// Tests that combined credits for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetPersonCombinedCreditsAsync()
    {
        var item = await TMDbClient.GetPersonCombinedCreditsAsync(IdHelper.BruceWillis);

        Assert.NotNull(item);
        Assert.NotNull(item.Cast);
        Assert.NotEmpty(item.Cast);

        // Verify we get valid combined credit data with proper types
        var movieCast = item.Cast.OfType<CombinedCreditsCastMovie>().First();
        Assert.NotNull(movieCast.CreditId);
        Assert.NotNull(movieCast.Title);

        // TV credits may or may not have entries
        var tvCast = item.Cast.OfType<CombinedCreditsCastTv>().FirstOrDefault();
        if (tvCast is not null)
        {
            Assert.NotNull(tvCast.CreditId);
        }

        // Crew may or may not have entries
        Assert.NotNull(item.Crew);
        if (item.Crew.Count > 0)
        {
            var movieCrew = item.Crew.OfType<CombinedCreditsCrewMovie>().FirstOrDefault();
            if (movieCrew is not null)
            {
                Assert.NotNull(movieCrew.CreditId);
            }
        }
    }

    /// <summary>
    /// Tests that external IDs for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetPersonExternalIdsAsync()
    {
        var item = await TMDbClient.GetPersonExternalIdsAsync(IdHelper.BruceWillis);

        await Verify(item);
    }

    /// <summary>
    /// Tests that the latest changes to people can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetChangesPeopleAsync()
    {
        var latestChanges = await TMDbClient.GetPeopleChangesAsync();

        Assert.NotNull(latestChanges);
        Assert.NotNull(latestChanges.Results);
        Assert.NotEmpty(latestChanges.Results);
    }

    /// <summary>
    /// Tests that profile images for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetPersonImagesAsync()
    {
        var images = await TMDbClient.GetPersonImagesAsync(IdHelper.BruceWillis);
        Assert.NotNull(images);

        Assert.NotNull(images.Profiles);
        var image = images.Profiles.Single(s => s.FilePath == "/cPP5y15p6iU83MxQ3tEcbr5hqNR.jpg");
        await Verify(image);

        TestImagesHelpers.TestImagePaths(images.Profiles);
    }

    /// <summary>
    /// Tests that tagged images for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestPersonsTaggedImagesAsync()
    {
        var images = await TMDbClient.GetPersonTaggedImagesAsync(IdHelper.BruceWillis, 1);
        Assert.NotNull(images);

        Assert.NotNull(images.Results);
        Assert.NotEmpty(images.Results);

        TestImagesHelpers.TestImagePaths(images.Results);

        var image = images.Results.Single(s => s.FilePath == "/svIDTNUoajS8dLEo7EosxvyAsgJ.jpg");

        Assert.IsType<SearchMovie>(image.Media);
        await Verify(image);
    }

    /// <summary>
    /// Tests that the list of popular people can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestPersonsListAsync()
    {
        var list = await TMDbClient.GetPersonPopularListAsync();
        Assert.NotNull(list);

        Assert.NotNull(list.Results);
        Assert.NotEmpty(list.Results);
    }

    /// <summary>
    /// Tests that the latest person added to TMDb can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetLatestPersonAsync()
    {
        var item = await TMDbClient.GetLatestPersonAsync();

        Assert.NotNull(item);
    }

    /// <summary>
    /// Tests that a person can be retrieved in a different language.
    /// </summary>
    [Fact]
    public async Task TestGetTranslatedPersonAsync()
    {
        var person = await TMDbClient.GetPersonAsync(IdHelper.BruceWillis, "da");
        Assert.NotNull(person);

        await Verify(person);
    }
}
