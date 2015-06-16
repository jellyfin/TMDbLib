using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientPersonTests
    {
        private const int BruceWillis = 62;

        private static Dictionary<PersonMethods, Func<Person, object>> _methods;
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        /// <summary>
        /// Run once, on test class initialization
        /// </summary>
        [ClassInitialize]
        public static void InitialInitiator(TestContext context)
        {
            _methods = new Dictionary<PersonMethods, Func<Person, object>>();
            _methods[PersonMethods.TvCredits] = person => person.Credits;
            _methods[PersonMethods.Changes] = person => person.Changes;
            _methods[PersonMethods.Images] = person => person.Images;
        }

        [TestMethod]
        public void TestPersonsExtrasNone()
        {
            Person person = _config.Client.GetPerson(BruceWillis);

            Assert.IsNotNull(person);

            Assert.AreEqual("Bruce Willis", person.Name);

            // Test all extras, ensure none of them exist
            foreach (Func<Person, object> selector in _methods.Values)
            {
                Assert.IsNull(selector(person));
            }
        }

        [TestMethod]
        public void TestPersonsExtrasExclusive()
        {
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => _config.Client.GetPerson(id, extras), BruceWillis);
        }

        [TestMethod]
        public void TestPersonsExtrasAll()
        {
            PersonMethods combinedEnum = _methods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Person item = _config.Client.GetPerson(BruceWillis, combinedEnum);

            TestMethodsHelper.TestAllNotNull(_methods, item);
        }

        [TestMethod]
        public void TestPersonsGet()
        {
            Person item = _config.Client.GetPerson(BruceWillis);

            Assert.IsNotNull(item);
            Assert.AreEqual(false, item.Adult);
            Assert.IsNotNull(item.Biography);
            Assert.AreEqual(new DateTime(1955, 3, 19), item.Birthday);
            Assert.IsFalse(item.Deathday.HasValue);
            Assert.AreEqual("http://www.b-willis.com/", item.Homepage);
            Assert.AreEqual(62, item.Id);
            Assert.AreEqual("nm0000246", item.ImdbId);
            Assert.AreEqual("Bruce Willis", item.Name);
            Assert.AreEqual("Idar-Oberstein, Germany", item.PlaceOfBirth);
            Assert.IsTrue(item.Popularity > 0);
            Assert.AreEqual("/kI1OluWhLJk3pnR19VjOfABpnTY.jpg", item.ProfilePath);

            Assert.IsNotNull(item.AlsoKnownAs);
            Assert.AreEqual(3, item.AlsoKnownAs.Count);
            Assert.IsTrue(item.AlsoKnownAs.Contains("Брюс Уиллис"));
            Assert.IsTrue(item.AlsoKnownAs.Contains("W.B. Willis"));
            Assert.IsTrue(item.AlsoKnownAs.Contains("Walter Bruce Willis"));
        }

        [TestMethod]
        public void TestPersonsGetPersonTvCredits()
        {
            TvCredits item = _config.Client.GetPersonTvCredits(BruceWillis);

            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Cast);
            Assert.IsNotNull(item.Crew);

            Assert.AreEqual(BruceWillis, item.Id);

            TvRole cast = item.Cast.SingleOrDefault(s => s.Character == "David Addison Jr.");
            Assert.IsNotNull(cast);
            Assert.AreEqual("David Addison Jr.", cast.Character);
            Assert.AreEqual("52571e7f19c2957114107d48", cast.CreditId);
            Assert.AreEqual(69, cast.EpisodeCount);
            Assert.AreEqual(new DateTime(1985, 3, 3), cast.FirstAirDate);
            Assert.AreEqual(1998, cast.Id);
            Assert.AreEqual("Moonlighting", cast.Name);
            Assert.AreEqual("Moonlighting", cast.OriginalName);
            Assert.AreEqual("/kGsJf8k6a069WsaAWu7pHlOohg5.jpg", cast.PosterPath);

            TvJob job = item.Crew.SingleOrDefault(s => s.CreditId == "525826eb760ee36aaa81b23b");
            Assert.IsNotNull(job);
            Assert.AreEqual("525826eb760ee36aaa81b23b", job.CreditId);
            Assert.AreEqual("Production", job.Department);
            Assert.AreEqual(37, job.EpisodeCount);
            Assert.AreEqual(new DateTime(1996, 9, 23), job.FirstAirDate);
            Assert.AreEqual(13297, job.Id);
            Assert.AreEqual("Producer", job.Job);
            Assert.AreEqual("Bruno the Kid", job.Name);
            Assert.AreEqual("Bruno the Kid", job.OriginalName);
            Assert.AreEqual("/5HkZHG7FkHhay6UlmQ4NyeqpbKR.jpg", job.PosterPath);
        }

        [TestMethod]
        public void TestPersonsGetPersonMovieCredits()
        {
            MovieCredits item = _config.Client.GetPersonMovieCredits(BruceWillis);

            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Cast);
            Assert.IsNotNull(item.Crew);

            Assert.AreEqual(BruceWillis, item.Id);

            MovieRole cast = item.Cast.SingleOrDefault(s => s.CreditId == "52fe4329c3a36847f803f193");
            Assert.IsNotNull(cast);
            Assert.AreEqual(false, cast.Adult);
            Assert.AreEqual("Lieutenant Muldoon", cast.Character);
            Assert.AreEqual("52fe4329c3a36847f803f193", cast.CreditId);
            Assert.AreEqual(1992, cast.Id);
            Assert.AreEqual("Planet Terror", cast.OriginalTitle);
            Assert.AreEqual("/7Yjzttt0VfPphSsUg8vFUO9WaEt.jpg", cast.PosterPath);
            Assert.AreEqual(new DateTime(2007, 4, 6), cast.ReleaseDate);
            Assert.AreEqual("Planet Terror", cast.Title);

            MovieJob job = item.Crew.SingleOrDefault(s => s.CreditId == "52fe42fec3a36847f8032887");
            Assert.IsNotNull(job);
            Assert.AreEqual(false, job.Adult);
            Assert.AreEqual("52fe42fec3a36847f8032887", job.CreditId);
            Assert.AreEqual("Production", job.Department);
            Assert.AreEqual(1571, job.Id);
            Assert.AreEqual("Producer", job.Job);
            Assert.AreEqual(new DateTime(2007, 6, 21), job.ReleaseDate);
            Assert.AreEqual("/8czarUCdvqPnulkLX8mdXyrLk2D.jpg", job.PosterPath);
            Assert.AreEqual("Live Free or Die Hard", job.Title);
            Assert.AreEqual("Live Free or Die Hard", job.OriginalTitle);
        }

        [TestMethod]
        public void TestPersonsGetPersonExternalIds()
        {
            ExternalIds item = _config.Client.GetPersonExternalIds(BruceWillis);

            Assert.IsNotNull(item);

            Assert.AreEqual(BruceWillis, item.Id);
            Assert.AreEqual("nm0000246", item.ImdbId);
            Assert.AreEqual("/m/0h7pj", item.FreebaseMid);
            Assert.AreEqual("/en/bruce_willis", item.FreebaseId);
            Assert.IsFalse(item.TvdbId.HasValue);
            Assert.AreEqual(10183, item.TvrageId);
        }

        [TestMethod]
        public void TestPersonsGetPersonCredits()
        {
            //GetPersonCredits(int id, string language)
            MovieCredits resp = _config.Client.GetPersonMovieCredits(BruceWillis);
            Assert.IsNotNull(resp);

            MovieCredits respItalian = _config.Client.GetPersonMovieCredits(BruceWillis, "it");
            Assert.IsNotNull(respItalian);

            Assert.AreEqual(resp.Cast.Count, respItalian.Cast.Count);
            Assert.AreEqual(resp.Crew.Count, respItalian.Crew.Count);
            Assert.AreEqual(resp.Id, respItalian.Id);

            // There must be at least one movie with a different title
            bool allTitlesIdentical = true;
            for (int index = 0; index < resp.Cast.Count; index++)
            {
                Assert.AreEqual(resp.Cast[index].Id, respItalian.Cast[index].Id);
                Assert.AreEqual(resp.Cast[index].OriginalTitle, respItalian.Cast[index].OriginalTitle);

                if (resp.Cast[index].Title != respItalian.Cast[index].Title)
                    allTitlesIdentical = false;
            }

            for (int index = 0; index < resp.Crew.Count; index++)
            {
                Assert.AreEqual(resp.Crew[index].Id, respItalian.Crew[index].Id);
                Assert.AreEqual(resp.Crew[index].OriginalTitle, respItalian.Crew[index].OriginalTitle);

                if (resp.Crew[index].Title != respItalian.Crew[index].Title)
                    allTitlesIdentical = false;
            }

            Assert.IsFalse(allTitlesIdentical);
        }

        [TestMethod]
        public void TestPersonsGetPersonChanges()
        {
            //GetPersonChanges(int id, DateTime? startDate = null, DateTime? endDate = null)
            // Find latest changed person
            int latestChanged = _config.Client.GetChangesPeople().Results.First().Id;

            // Fetch changelog
            DateTime lower = DateTime.UtcNow.AddDays(-14);
            DateTime higher = DateTime.UtcNow;
            List<Change> respRange = _config.Client.GetPersonChanges(latestChanged, lower, higher);

            Assert.IsNotNull(respRange);
            Assert.IsTrue(respRange.Count > 0);

            // As TMDb works in days, we need to adjust our values also
            lower = lower.AddDays(-1);
            higher = higher.AddDays(1);

            foreach (Change change in respRange)
                foreach (ChangeItem changeItem in change.Items)
                {
                    DateTime date = changeItem.Time;
                    Assert.IsTrue(lower <= date);
                    Assert.IsTrue(date <= higher);
                }
        }

        [TestMethod]
        public void TestPersonsImages()
        {
            // Get config
            _config.Client.GetConfig();

            // Get images
            ProfileImages images = _config.Client.GetPersonImages(BruceWillis);

            Assert.IsNotNull(images);
            Assert.IsNotNull(images.Profiles);
            Assert.AreEqual(BruceWillis, images.Id);

            // Test image url generator
            TestImagesHelpers.TestImages(_config, images);

            ProfileImage image = images.Profiles.SingleOrDefault(s => s.FilePath == "/kI1OluWhLJk3pnR19VjOfABpnTY.jpg");

            Assert.IsNotNull(image);
            Assert.IsTrue(Math.Abs(0.666666666666667 - image.AspectRatio) < double.Epsilon);
            Assert.AreEqual("/kI1OluWhLJk3pnR19VjOfABpnTY.jpg", image.FilePath);
            Assert.AreEqual(1500, image.Height);
            Assert.AreEqual("51ee69f8760ee336455679c5", image.Id);
            Assert.IsNull(image.Iso_639_1);
            Assert.AreEqual(1000, image.Width);
            Assert.IsTrue(image.VoteAverage > 0);
            Assert.IsTrue(image.VoteCount > 0);
        }

        [TestMethod]
        public void TestPersonsTaggedImages()
        {
            // Get config
            _config.Client.GetConfig();

            // Get images
            TestHelpers.SearchPages(i => _config.Client.GetPersonTaggedImages(BruceWillis, i));

            SearchContainer<TaggedImage> images = _config.Client.GetPersonTaggedImages(BruceWillis, 1);

            Assert.IsNotNull(images);
            Assert.IsNotNull(images.Results);

            TaggedImage image = images.Results.SingleOrDefault(s => s.FilePath == "/my81Hjt7NpZhaMX9bHi4wVhFy0v.jpg");

            Assert.IsNotNull(image);
            Assert.IsTrue(Math.Abs(1.77777777777778 - image.AspectRatio) < double.Epsilon);
            Assert.AreEqual("/my81Hjt7NpZhaMX9bHi4wVhFy0v.jpg", image.FilePath);
            Assert.AreEqual(1080, image.Height);
            Assert.AreEqual("4ea5d0792c058837cb000431", image.Id);
            Assert.IsNull(image.Iso_639_1);
            Assert.IsTrue(image.VoteAverage > 0);
            Assert.IsTrue(image.VoteCount > 0);
            Assert.AreEqual(1920, image.Width);
            Assert.AreEqual("backdrop", image.ImageType);
            Assert.AreEqual(MediaType.Movie, image.MediaType);

            Assert.IsNotNull(image.Media);
            Assert.AreEqual(false, image.Media.Adult);
            Assert.AreEqual("/my81Hjt7NpZhaMX9bHi4wVhFy0v.jpg", image.Media.BackdropPath);
            Assert.AreEqual(187, image.Media.Id);
            Assert.AreEqual("en", image.Media.OriginalLanguage);
            Assert.AreEqual("Sin City", image.Media.OriginalTitle);
            Assert.AreEqual("Sin City is a neo-noir crime thriller based on Frank Miller's graphic novel series of the same name.The film is primarily based on three of Miller's works: The Hard Goodbye, about a man who embarks on a brutal rampage in search of his one-time sweetheart's killer; The Big Fat Kill, which focuses on a street war between a group of prostitutes and a group of mercenaries; and That Yellow Bastard, which follows an aging police officer who protects a young woman from a grotesquely disfigured serial killer.", image.Media.Overview);
            Assert.AreEqual(new DateTime(2005, 3, 31), image.Media.ReleaseDate);
            Assert.AreEqual("/eCJkepVJslq1nEtqURLaC1zLPAL.jpg", image.Media.PosterPath);
            Assert.IsTrue(image.Media.Popularity > 0);
            Assert.AreEqual("Sin City", image.Media.Title);
            Assert.AreEqual(false, image.Media.Video);
            Assert.IsTrue(image.Media.VoteAverage > 0);
            Assert.IsTrue(image.Media.VoteCount > 0);

            Assert.IsNotNull(image.Media.GenreIds);
            Assert.AreEqual(3, image.Media.GenreIds.Count);
            Assert.IsTrue(image.Media.GenreIds.Contains(28));
            Assert.IsTrue(image.Media.GenreIds.Contains(53));
            Assert.IsTrue(image.Media.GenreIds.Contains(80));
        }

        [TestMethod]
        public void TestPersonsList()
        {
            foreach (PersonListType type in Enum.GetValues(typeof(PersonListType)).OfType<PersonListType>())
            {
                SearchContainer<PersonResult> list = _config.Client.GetPersonList(type);

                Assert.IsNotNull(list);
                Assert.IsTrue(list.Results.Count > 0);
                Assert.AreEqual(1, list.Page);

                SearchContainer<PersonResult> listPage2 = _config.Client.GetPersonList(type, 2);

                Assert.IsNotNull(listPage2);
                Assert.IsTrue(listPage2.Results.Count > 0);
                Assert.AreEqual(2, listPage2.Page);

                SearchContainer<PersonResult> list2 = _config.Client.GetPersonList(type);

                Assert.IsNotNull(list2);
                Assert.IsTrue(list2.Results.Count > 0);
                Assert.AreEqual(1, list2.Page);

                // At least one person should differ
                Assert.IsTrue(list.Results.Any(s => list2.Results.Any(x => x.Name != s.Name)));
            }
        }

        [TestMethod]
        public void TestGetLatestPerson()
        {
            Person item = _config.Client.GetLatestPerson();
            Assert.IsNotNull(item);
        }
    }
}
