using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Person;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientPersonTests
    {
        private const int BruceWillis = 62;

        private Dictionary<PersonMethods, Func<Person, object>> _methods;
        private TestConfig _config;

        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();

            _methods = new Dictionary<PersonMethods, Func<Person, object>>();
            _methods[PersonMethods.Credits] = movie => movie.Credits;
            _methods[PersonMethods.Changes] = movie => movie.Changes;
            _methods[PersonMethods.Images] = movie => movie.Images;
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

            TestMethodsHelper.TestGetAll(_methods, item);
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
    }
}
