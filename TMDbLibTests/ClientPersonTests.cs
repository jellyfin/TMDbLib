using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;
using Credits = TMDbLib.Objects.People.Credits;

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
            _methods[PersonMethods.Credits] = person => person.Credits;
            _methods[PersonMethods.Changes] = person => person.Changes;
            _methods[PersonMethods.Images] = person => person.Images;
        }

        [TestMethod]
        public void TestPersonsExtrasNone()
        {
            Person person = _config.Client.GetPerson(BruceWillis);

            Assert.IsNotNull(person);

            // TODO: Test all properties
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
        public void TestPersonsGetPersonCredits()
        {
            //GetPersonCredits(int id, string language)
            Credits resp = _config.Client.GetPersonCredits(BruceWillis);
            Assert.IsNotNull(resp);

            Credits respItalian = _config.Client.GetPersonCredits(BruceWillis, "it");
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

            // Test image url generator
            ProfileImages images = _config.Client.GetPersonImages(BruceWillis);

            Assert.AreEqual(BruceWillis, images.Id);
            Assert.IsTrue(images.Profiles.Count > 0);

            TestImagesHelpers.TestImages(_config, images);
        }

        [TestMethod]
        public void TestPersonList()
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
    }
}
