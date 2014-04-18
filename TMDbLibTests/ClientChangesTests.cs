using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientChangesTests
    {
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestChangesMovies()
        {
            // Basic check
            SearchContainer<ChangesListItem> changesPage1 = _config.Client.GetChangesMovies();

            Assert.IsNotNull(changesPage1);
            Assert.IsTrue(changesPage1.Results.Count > 0);
            Assert.IsTrue(changesPage1.TotalResults > changesPage1.Results.Count);
            Assert.AreEqual(1, changesPage1.Page);

            // Page 2
            SearchContainer<ChangesListItem> changesPage2 = _config.Client.GetChangesMovies(2);

            Assert.IsNotNull(changesPage2);
            Assert.AreEqual(2, changesPage2.Page);

            // Check date range (max)
            DateTime higher = DateTime.UtcNow.AddDays(-7);
            SearchContainer<ChangesListItem> changesMaxDate = _config.Client.GetChangesMovies(endDate: higher);

            Assert.IsNotNull(changesMaxDate);
            Assert.AreEqual(1, changesMaxDate.Page);
            Assert.AreNotEqual(changesPage1.TotalResults, changesMaxDate.TotalResults);

            // Check date range (lower)
            DateTime lower = DateTime.UtcNow.AddDays(-6);       // Use 6 days to avoid clashes with the 'higher'
            SearchContainer<ChangesListItem> changesLowDate = _config.Client.GetChangesMovies(startDate: lower);

            Assert.IsNotNull(changesLowDate);
            Assert.AreEqual(1, changesLowDate.Page);
            Assert.AreNotEqual(changesPage1.TotalResults, changesLowDate.TotalResults);
        }

        [TestMethod]
        public void TestChangesPeople()
        {
            // Basic check
            SearchContainer<ChangesListItem> changesPage1 = _config.Client.GetChangesPeople();

            Assert.IsNotNull(changesPage1);
            Assert.IsTrue(changesPage1.Results.Count > 0);
            Assert.IsTrue(changesPage1.TotalResults > changesPage1.Results.Count);
            Assert.AreEqual(1, changesPage1.Page);

            // Page 2
            SearchContainer<ChangesListItem> changesPage2 = _config.Client.GetChangesPeople(2);

            Assert.IsNotNull(changesPage2);
            Assert.AreEqual(2, changesPage2.Page);

            // Check date range (max)
            DateTime higher = DateTime.UtcNow.AddDays(-7);
            SearchContainer<ChangesListItem> changesMaxDate = _config.Client.GetChangesPeople(endDate: higher);

            Assert.IsNotNull(changesMaxDate);
            Assert.AreEqual(1, changesMaxDate.Page);
            Assert.AreNotEqual(changesPage1.TotalResults, changesMaxDate.TotalResults);

            // Check date range (lower)
            DateTime lower = DateTime.UtcNow.AddDays(-6);       // Use 6 days to avoid clashes with the 'higher'
            SearchContainer<ChangesListItem> changesLowDate = _config.Client.GetChangesPeople(startDate: lower);

            Assert.IsNotNull(changesLowDate);
            Assert.AreEqual(1, changesLowDate.Page);
            Assert.AreNotEqual(changesPage1.TotalResults, changesLowDate.TotalResults);

            // None of the id's in changesLowDate should exist in changesMaxDate, and vice versa
            Assert.IsTrue(changesLowDate.Results.All(lowItem => changesMaxDate.Results.All(maxItem => maxItem.Id != lowItem.Id)));
            Assert.IsTrue(changesMaxDate.Results.All(maxItem => changesLowDate.Results.All(lowItem => maxItem.Id != lowItem.Id)));
        }
    }
}
