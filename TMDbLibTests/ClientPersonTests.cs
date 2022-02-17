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

namespace TMDbLibTests
{
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

        [Fact]
        public async Task TestPersonsExtrasNoneAsync()
        {
            Person person = await TMDbClient.GetPersonAsync(IdHelper.BruceWillis);

            // Test all extras, ensure none of them exist
            foreach (Func<Person, object> selector in Methods.Values)
                Assert.Null(selector(person));

            await Verify(person);
        }

        [Fact]
        public async Task TestPersonsExtrasExclusive()
        {
            await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetPersonAsync(IdHelper.BruceWillis, extras));
        }

        [Fact]
        public async Task TestPersonsExtrasAllAsync()
        {
            await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetPersonAsync(IdHelper.FrankSinatra, combined), async person => await Verify(person));
        }

        [Fact]
        public async Task TestPersonMissingAsync()
        {
            Person person = await TMDbClient.GetPersonAsync(IdHelper.MissingID);

            Assert.Null(person);
        }

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

        [Fact]
        public async Task TestGetPersonExternalIdsAsync()
        {
            ExternalIdsPerson item = await TMDbClient.GetPersonExternalIdsAsync(IdHelper.BruceWillis);

            await Verify(item);
        }

        [Fact]
        public async Task TestGetChangesPeopleAsync()
        {
            SearchContainer<ChangesListItem> latestChanges = await TMDbClient.GetPeopleChangesAsync();

            Assert.NotEmpty(latestChanges.Results);
        }

        [Fact]
        public async Task TestGetPersonImagesAsync()
        {
            ProfileImages images = await TMDbClient.GetPersonImagesAsync(IdHelper.BruceWillis);

            ImageData image = images.Profiles.Single(s => s.FilePath == "/cPP5y15p6iU83MxQ3tEcbr5hqNR.jpg");
            await Verify(image);

            TestImagesHelpers.TestImagePaths(images.Profiles);
        }

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

        [Fact]
        public async Task TestPersonsListAsync()
        {
            SearchContainer<SearchPerson> list = await TMDbClient.GetPersonPopularListAsync();

            Assert.NotEmpty(list.Results);
        }

        [Fact]
        public async Task TestGetLatestPersonAsync()
        {
            Person item = await TMDbClient.GetLatestPersonAsync();

            Assert.NotNull(item);
        }

        [Fact]
        public async Task TestGetTranslatedPersonAsync()
        {
            Person person = await TMDbClient.GetPersonAsync(IdHelper.BruceWillis, "da");

            await Verify(person);
        }
    }
}