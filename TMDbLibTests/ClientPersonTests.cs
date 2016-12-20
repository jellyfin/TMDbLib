using System;
using System.Collections.Generic;
using System.Linq;
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
        private static Dictionary<PersonMethods, Func<Person, object>> _methods;

        public ClientPersonTests()
        {
            _methods = new Dictionary<PersonMethods, Func<Person, object>>
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
        public void TestPersonsExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / changes", " / external_ids", " / images", " / movie_credits", " / tagged_images", " / tv_credits");

            Person person = Config.Client.GetPersonAsync(IdHelper.BruceWillis).Result;

            Assert.NotNull(person);

            Assert.Equal("Bruce Willis", person.Name);

            // Test all extras, ensure none of them exist
            foreach (Func<Person, object> selector in _methods.Values)
            {
                Assert.Null(selector(person));
            }
        }

        [Fact]
        public void TestPersonsExtrasExclusive()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / changes", " / external_ids", " / images", " / movie_credits", " / tagged_images", " / tv_credits", "external_ids / id", "images / id", "movie_credits / id", "tv_credits / id");

            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => Config.Client.GetPersonAsync(id, extras).Result, IdHelper.BruceWillis);
        }

        [Fact]
        public void TestPersonsExtrasAll()
        {
            IgnoreMissingJson("external_ids / id", "images / id", "movie_credits / id", "tv_credits / id");

            PersonMethods combinedEnum = _methods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Person item = Config.Client.GetPersonAsync(IdHelper.BruceWillis, combinedEnum).Result;

            TestMethodsHelper.TestAllNotNull(_methods, item);
        }

        [Fact]
        public void TestPersonsGetWithPartialDate()
        {
            IgnoreMissingJson(" / changes", " / external_ids", " / images", " / movie_credits", " / tagged_images", " / tv_credits");

            Person item = Config.Client.GetPersonAsync(IdHelper.PersonPartialDate).Result;

            Assert.NotNull(item);
            Assert.Null(item.Birthday);
            Assert.Null(item.Deathday);
        }

        [Fact]
        public void TestPersonsGet()
        {
            IgnoreMissingJson(" / changes", " / external_ids", " / images", " / movie_credits", " / tagged_images", " / tv_credits");

            Person item = Config.Client.GetPersonAsync(IdHelper.BruceWillis).Result;

            Assert.NotNull(item);
            Assert.Equal(false, item.Adult);
            Assert.NotNull(item.Biography);
            Assert.Equal(PersonGender.Male, item.Gender);
            Assert.Equal(new DateTime(1955, 3, 19), item.Birthday);
            Assert.False(item.Deathday.HasValue);
            Assert.Equal("http://www.b-willis.com/", item.Homepage);
            Assert.Equal(62, item.Id);
            Assert.Equal("nm0000246", item.ImdbId);
            Assert.Equal("Bruce Willis", item.Name);
            Assert.Equal("Idar-Oberstein, Germany", item.PlaceOfBirth);
            Assert.True(item.Popularity > 0);
            Assert.True(TestImagesHelpers.TestImagePath(item.ProfilePath), "item.ProfilePath was not a valid image path, was: " + item.ProfilePath);

            Assert.NotNull(item.AlsoKnownAs);
            Assert.Equal(2, item.AlsoKnownAs.Count);
            Assert.True(item.AlsoKnownAs.Contains("Брюс Уиллис"));
            Assert.True(item.AlsoKnownAs.Contains("브루스 윌리스"));
        }

        [Fact]
        public void TestPersonsGetPersonTvCredits()
        {
            TvCredits item = Config.Client.GetPersonTvCreditsAsync(IdHelper.BruceWillis).Result;

            Assert.NotNull(item);
            Assert.NotNull(item.Cast);
            Assert.NotNull(item.Crew);

            Assert.Equal(IdHelper.BruceWillis, item.Id);

            TvRole cast = item.Cast.SingleOrDefault(s => s.Character == "David Addison Jr.");
            Assert.NotNull(cast);
            Assert.Equal("David Addison Jr.", cast.Character);
            Assert.Equal("52571e7f19c2957114107d48", cast.CreditId);
            Assert.Equal(71, cast.EpisodeCount);
            Assert.Equal(new DateTime(1985, 3, 3), cast.FirstAirDate);
            Assert.Equal(1998, cast.Id);
            Assert.Equal("Moonlighting", cast.Name);
            Assert.Equal("Moonlighting", cast.OriginalName);
            Assert.True(TestImagesHelpers.TestImagePath(cast.PosterPath), "cast.PosterPath was not a valid image path, was: " + cast.PosterPath);

            TvJob job = item.Crew.SingleOrDefault(s => s.CreditId == "525826eb760ee36aaa81b23b");
            Assert.NotNull(job);
            Assert.Equal("525826eb760ee36aaa81b23b", job.CreditId);
            Assert.Equal("Production", job.Department);
            Assert.Equal(37, job.EpisodeCount);
            Assert.Equal(new DateTime(1996, 9, 23), job.FirstAirDate);
            Assert.Equal(13297, job.Id);
            Assert.Equal("Producer", job.Job);
            Assert.Equal("Bruno the Kid", job.Name);
            Assert.Equal("Bruno the Kid", job.OriginalName);
            Assert.True(TestImagesHelpers.TestImagePath(job.PosterPath), "job.PosterPath was not a valid image path, was: " + job.PosterPath);
        }

        [Fact]
        public void TestPersonsGetPersonMovieCredits()
        {
            MovieCredits item = Config.Client.GetPersonMovieCreditsAsync(IdHelper.BruceWillis).Result;

            Assert.NotNull(item);
            Assert.NotNull(item.Cast);
            Assert.NotNull(item.Crew);

            Assert.Equal(IdHelper.BruceWillis, item.Id);

            MovieRole cast = item.Cast.SingleOrDefault(s => s.CreditId == "52fe4329c3a36847f803f193");
            Assert.NotNull(cast);
            Assert.Equal(false, cast.Adult);
            Assert.Equal("Lieutenant Muldoon", cast.Character);
            Assert.Equal("52fe4329c3a36847f803f193", cast.CreditId);
            Assert.Equal(1992, cast.Id);
            Assert.Equal("Planet Terror", cast.OriginalTitle);
            Assert.True(TestImagesHelpers.TestImagePath(cast.PosterPath), "cast.PosterPath was not a valid image path, was: " + cast.PosterPath);
            Assert.Equal(new DateTime(2007, 4, 6), cast.ReleaseDate);
            Assert.Equal("Planet Terror", cast.Title);

            MovieJob job = item.Crew.SingleOrDefault(s => s.CreditId == "52fe432ec3a36847f8040603");
            Assert.NotNull(job);
            Assert.Equal(false, job.Adult);
            Assert.Equal("52fe432ec3a36847f8040603", job.CreditId);
            Assert.Equal("Production", job.Department);
            Assert.Equal(2026, job.Id);
            Assert.Equal("Producer", job.Job);
            Assert.Equal(new DateTime(2005, 3, 9), job.ReleaseDate);
            Assert.True(TestImagesHelpers.TestImagePath(job.PosterPath), "job.PosterPath was not a valid image path, was: " + job.PosterPath);
            Assert.Equal("Hostage", job.Title);
            Assert.Equal("Hostage", job.OriginalTitle);
        }

        [Fact]
        public void TestPersonsGetPersonExternalIds()
        {
            ExternalIdsPerson item = Config.Client.GetPersonExternalIdsAsync(IdHelper.BruceWillis).Result;

            Assert.NotNull(item);
            Assert.Equal(IdHelper.BruceWillis, item.Id);
            Assert.Equal("nm0000246", item.ImdbId);
            Assert.Equal("/m/0h7pj", item.FreebaseMid);
            Assert.Equal("/en/bruce_willis", item.FreebaseId);
            Assert.Equal("10183", item.TvrageId);
            Assert.Null(item.FacebookId);

            item = Config.Client.GetPersonExternalIdsAsync(IdHelper.JoshACagan).Result;

            Assert.NotNull(item);
            Assert.Equal(IdHelper.JoshACagan, item.Id);
            Assert.Null(item.FacebookId);
            Assert.Equal("joshacagan", item.TwitterId);
            Assert.Equal("joshacagan", item.InstagramId);
        }

        [Fact]
        public void TestPersonsGetPersonCredits()
        {
            //GetPersonCredits(int id, string language)
            MovieCredits resp = Config.Client.GetPersonMovieCreditsAsync(IdHelper.BruceWillis).Result;
            Assert.NotNull(resp);

            MovieCredits respItalian = Config.Client.GetPersonMovieCreditsAsync(IdHelper.BruceWillis, "it").Result;
            Assert.NotNull(respItalian);

            Assert.Equal(resp.Cast.Count, respItalian.Cast.Count);
            Assert.Equal(resp.Crew.Count, respItalian.Crew.Count);
            Assert.Equal(resp.Id, respItalian.Id);

            // There must be at least one movie with a different title
            bool allTitlesIdentical = true;
            for (int index = 0; index < resp.Cast.Count; index++)
            {
                Assert.Equal(resp.Cast[index].Id, respItalian.Cast[index].Id);
                Assert.Equal(resp.Cast[index].OriginalTitle, respItalian.Cast[index].OriginalTitle);

                if (resp.Cast[index].Title != respItalian.Cast[index].Title)
                    allTitlesIdentical = false;
            }

            for (int index = 0; index < resp.Crew.Count; index++)
            {
                Assert.Equal(resp.Crew[index].Id, respItalian.Crew[index].Id);
                Assert.Equal(resp.Crew[index].OriginalTitle, respItalian.Crew[index].OriginalTitle);

                if (resp.Crew[index].Title != respItalian.Crew[index].Title)
                    allTitlesIdentical = false;
            }

            Assert.False(allTitlesIdentical);
        }

        [Fact]
        public void TestPersonsGetPersonChanges()
        {
            // Not all ChangeItem's have an iso_639_1
            IgnoreMissingJson(" / iso_639_1");

            // FindAsync latest changed person
            SearchContainer<ChangesListItem> latestChanges = Config.Client.GetChangesPeopleAsync().Sync();
            int latestChanged = latestChanges.Results.Last().Id;

            // Fetch changelog
            DateTime lower = DateTime.UtcNow.AddDays(-14);
            DateTime higher = DateTime.UtcNow;
            List<Change> respRange = Config.Client.GetPersonChangesAsync(latestChanged, lower, higher).Result;

            Assert.NotNull(respRange);
            Assert.True(respRange.Count > 0);

            // As TMDb works in days, we need to adjust our values also
            lower = lower.AddDays(-1);
            higher = higher.AddDays(1);

            foreach (Change change in respRange)
                foreach (ChangeItemBase changeItem in change.Items)
                {
                    DateTime date = changeItem.Time;
                    Assert.True(lower <= date);
                    Assert.True(date <= higher);
                }
        }

        [Fact]
        public void TestPersonsImages()
        {
            // Get config
            Config.Client.GetConfig();

            // Get images
            ProfileImages images = Config.Client.GetPersonImagesAsync(IdHelper.BruceWillis).Result;

            Assert.NotNull(images);
            Assert.NotNull(images.Profiles);
            Assert.Equal(IdHelper.BruceWillis, images.Id);

            // Test image url generator
            TestImagesHelpers.TestImages(Config, images);

            ImageData image = images.Profiles.SingleOrDefault(s => s.FilePath == "/kI1OluWhLJk3pnR19VjOfABpnTY.jpg");

            Assert.NotNull(image);
            Assert.True(Math.Abs(0.666666666666667 - image.AspectRatio) < double.Epsilon);
            Assert.True(TestImagesHelpers.TestImagePath(image.FilePath), "image.FilePath was not a valid image path, was: " + image.FilePath);
            Assert.Equal(1500, image.Height);
            Assert.Null(image.Iso_639_1);
            Assert.Equal(1000, image.Width);
            Assert.True(image.VoteAverage > 0);
            Assert.True(image.VoteCount > 0);
        }

        [Fact]
        public void TestPersonsTaggedImages()
        {
            // Get config
            Config.Client.GetConfig();

            // Get images
            TestHelpers.SearchPages(i => Config.Client.GetPersonTaggedImagesAsync(IdHelper.BruceWillis, i).Result);

            SearchContainer<TaggedImage> images = Config.Client.GetPersonTaggedImagesAsync(IdHelper.BruceWillis, 1).Result;

            Assert.NotNull(images);
            Assert.NotNull(images.Results);

            TaggedImage image = images.Results.SingleOrDefault(s => s.FilePath == "/my81Hjt7NpZhaMX9bHi4wVhFy0v.jpg");

            Assert.NotNull(image);
            Assert.True(Math.Abs(1.77777777777778 - image.AspectRatio) < double.Epsilon);
            Assert.True(TestImagesHelpers.TestImagePath(image.FilePath), "image.FilePath was not a valid image path, was: " + image.FilePath);
            Assert.Equal(1080, image.Height);
            Assert.Equal("4ea5d0792c058837cb000431", image.Id);
            Assert.Null(image.Iso_639_1);
            Assert.True(image.VoteAverage > 0);
            Assert.True(image.VoteCount > 0);
            Assert.Equal(1920, image.Width);
            Assert.Equal("backdrop", image.ImageType);
            Assert.Equal(MediaType.Movie, image.MediaType);

            Assert.NotNull(image.Media);
            Assert.IsType<SearchMovie>(image.Media);

            SearchMovie mediaBase = (SearchMovie)image.Media;
            Assert.Equal(false, mediaBase.Adult);
            Assert.True(TestImagesHelpers.TestImagePath(mediaBase.BackdropPath), "image.Media.BackdropPath was not a valid image path, was: " + mediaBase.BackdropPath);
            Assert.Equal(187, mediaBase.Id);
            Assert.Equal("en", mediaBase.OriginalLanguage);
            Assert.Equal("Sin City", mediaBase.OriginalTitle);
            Assert.Equal("Welcome to Sin City. This town beckons to the tough, the corrupt, the brokenhearted. Some call it dark… Hard-boiled. Then there are those who call it home — Crooked cops, sexy dames, desperate vigilantes. Some are seeking revenge, others lust after redemption, and then there are those hoping for a little of both. A universe of unlikely and reluctant heroes still trying to do the right thing in a city that refuses to care.", mediaBase.Overview);
            Assert.Equal(new DateTime(2005, 3, 31), mediaBase.ReleaseDate);
            Assert.True(TestImagesHelpers.TestImagePath(mediaBase.PosterPath), "image.Media.PosterPath was not a valid image path, was: " + mediaBase.PosterPath);
            Assert.True(mediaBase.Popularity > 0);
            Assert.Equal("Sin City", mediaBase.Title);
            Assert.Equal(false, mediaBase.Video);
            Assert.True(mediaBase.VoteAverage > 0);
            Assert.True(mediaBase.VoteCount > 0);

            Assert.NotNull(mediaBase.GenreIds);
            Assert.Equal(3, mediaBase.GenreIds.Count);
            Assert.True(mediaBase.GenreIds.Contains(28));
            Assert.True(mediaBase.GenreIds.Contains(53));
            Assert.True(mediaBase.GenreIds.Contains(80));
        }

        [Fact]
        public void TestPersonsList()
        {
            foreach (PersonListType type in Enum.GetValues(typeof(PersonListType)).OfType<PersonListType>())
            {
                SearchContainer<PersonResult> list = Config.Client.GetPersonListAsync(type).Result;

                Assert.NotNull(list);
                Assert.True(list.Results.Count > 0);
                Assert.Equal(1, list.Page);

                SearchContainer<PersonResult> listPage2 = Config.Client.GetPersonListAsync(type, 2).Result;

                Assert.NotNull(listPage2);
                Assert.True(listPage2.Results.Count > 0);
                Assert.Equal(2, listPage2.Page);

                SearchContainer<PersonResult> list2 = Config.Client.GetPersonListAsync(type).Result;

                Assert.NotNull(list2);
                Assert.True(list2.Results.Count > 0);
                Assert.Equal(1, list2.Page);

                // At least one person should differ
                Assert.True(list.Results.Any(s => list2.Results.Any(x => x.Name != s.Name)));
            }
        }

        [Fact]
        public void TestGetLatestPerson()
        {
            IgnoreMissingJson(" / changes", " / external_ids", " / images", " / movie_credits", " / tagged_images", " / tv_credits");

            Person item = Config.Client.GetLatestPersonAsync().Sync();
            Assert.NotNull(item);
        }
    }
}
