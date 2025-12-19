using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.People;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb person functionality.
/// </summary>
public class ClientPersonTests : TestBase
{
    private static readonly Dictionary<PersonMethods, Func<Person, object>> Methods;

    static ClientPersonTests()
    {
        Methods = new Dictionary<PersonMethods, Func<Person, object>>
        {
            [PersonMethods.MovieCredits] = person => person.MovieCredits,
            [PersonMethods.TvCredits] = person => person.TvCredits,
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
        Person person = await TMDbClient.GetPersonAsync(IdHelper.BruceWillis);

        // Test all extras, ensure none of them exist
        foreach (Func<Person, object> selector in Methods.Values)
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
        await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetPersonAsync(IdHelper.BruceWillis, extras));
    }
    /// <summary>
    /// Tests that all person extra methods can be requested together.
    /// </summary>
    [Fact]
    public async Task TestPersonsExtrasAllAsync()
    {
        await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetPersonAsync(IdHelper.FrankSinatra, combined), async person => await Verify(person));
    }
    /// <summary>
    /// Verifies that retrieving a non-existent person returns null.
    /// </summary>
    [Fact]
    public async Task TestPersonMissingAsync()
    {
        Person person = await TMDbClient.GetPersonAsync(IdHelper.MissingID);

        Assert.Null(person);
    }
    /// <summary>
    /// Tests that TV credits for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestPersonsGetPersonTvCreditsAsync()
    {
        TvCredits item = await TMDbClient.GetPersonTvCreditsAsync(IdHelper.BruceWillis);

        Assert.NotNull(item);
        Assert.NotEmpty(item.Cast);
        Assert.NotEmpty(item.Crew);

        TvRole cast = item.Cast.Single(s => s.CreditId == "52571e7f19c2957114107d48");
        TvJob crew = item.Crew.Single(s => s.CreditId == "525826eb760ee36aaa81b23b");

        await Verify(new
        {
            cast,
            crew
        });
    }
    /// <summary>
    /// Tests that movie credits for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetPersonMovieCreditsAsync()
    {
        MovieCredits item = await TMDbClient.GetPersonMovieCreditsAsync(IdHelper.BruceWillis);

        Assert.NotNull(item);
        Assert.NotEmpty(item.Cast);
        Assert.NotEmpty(item.Crew);

        MovieRole cast = item.Cast.Single(s => s.CreditId == "52fe4329c3a36847f803f193");
        MovieJob crew = item.Crew.Single(s => s.CreditId == "52fe432ec3a36847f8040603");

        await Verify(new
        {
            cast,
            crew
        });
    }
    /// <summary>
    /// Tests that external IDs for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetPersonExternalIdsAsync()
    {
        ExternalIdsPerson item = await TMDbClient.GetPersonExternalIdsAsync(IdHelper.BruceWillis);

        await Verify(item);
    }
    /// <summary>
    /// Tests that the latest changes to people can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetChangesPeopleAsync()
    {
        SearchContainer<ChangesListItem> latestChanges = await TMDbClient.GetPeopleChangesAsync();

        Assert.NotEmpty(latestChanges.Results);
    }
    /// <summary>
    /// Tests that profile images for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetPersonImagesAsync()
    {
        ProfileImages images = await TMDbClient.GetPersonImagesAsync(IdHelper.BruceWillis);

        ImageData image = images.Profiles.Single(s => s.FilePath == "/cPP5y15p6iU83MxQ3tEcbr5hqNR.jpg");
        await Verify(image);

        TestImagesHelpers.TestImagePaths(images.Profiles);
    }
    /// <summary>
    /// Tests that tagged images for a person can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestPersonsTaggedImagesAsync()
    {
        SearchContainer<TaggedImage> images = await TMDbClient.GetPersonTaggedImagesAsync(IdHelper.BruceWillis, 1);

        Assert.NotEmpty(images.Results);

        TestImagesHelpers.TestImagePaths(images.Results);

        TaggedImage image = images.Results.Single(s => s.FilePath == "/svIDTNUoajS8dLEo7EosxvyAsgJ.jpg");

        Assert.IsType<SearchMovie>(image.Media);
        await Verify(image);
    }
    /// <summary>
    /// Tests that the list of popular people can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestPersonsListAsync()
    {
        SearchContainer<SearchPerson> list = await TMDbClient.GetPersonPopularListAsync();

        Assert.NotEmpty(list.Results);
    }
    /// <summary>
    /// Tests that the latest person added to TMDb can be retrieved.
    /// </summary>
    [Fact]
    public async Task TestGetLatestPersonAsync()
    {
        Person item = await TMDbClient.GetLatestPersonAsync();

        Assert.NotNull(item);
    }
    /// <summary>
    /// Tests that a person can be retrieved in a different language.
    /// </summary>
    [Fact]
    public async Task TestGetTranslatedPersonAsync()
    {
        Person person = await TMDbClient.GetPersonAsync(IdHelper.BruceWillis, "da");

        await Verify(person);
    }
}
