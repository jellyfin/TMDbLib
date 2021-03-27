using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientChangesTests : TestBase
    {
        [Fact]
        public async Task TestChangesMoviesAsync()
        {
            // Basic check
            SearchContainer<ChangesListItem> changesPage1 = await Config.Client.GetChangesMoviesAsync();

            Assert.NotNull(changesPage1);
            Assert.True(changesPage1.Results.Count > 0);
            Assert.True(changesPage1.TotalResults > changesPage1.Results.Count);
            Assert.Equal(1, changesPage1.Page);

            // Page 2
            SearchContainer<ChangesListItem> changesPage2 = await Config.Client.GetChangesMoviesAsync(2);

            Assert.NotNull(changesPage2);
            Assert.Equal(2, changesPage2.Page);

            // Check date range (max)
            DateTime higher = DateTime.UtcNow.AddDays(-7);
            SearchContainer<ChangesListItem> changesMaxDate = await Config.Client.GetChangesMoviesAsync(endDate: higher);

            Assert.NotNull(changesMaxDate);
            Assert.Equal(1, changesMaxDate.Page);
            Assert.NotEqual(changesPage1.TotalResults, changesMaxDate.TotalResults);

            // Check date range (lower)
            DateTime lower = DateTime.UtcNow.AddDays(-6);       // Use 6 days to avoid clashes with the 'higher'
            SearchContainer<ChangesListItem> changesLowDate = await Config.Client.GetChangesMoviesAsync(startDate: lower);

            Assert.NotNull(changesLowDate);
            Assert.Equal(1, changesLowDate.Page);
            Assert.NotEqual(changesPage1.TotalResults, changesLowDate.TotalResults);
        }

        [Fact]
        public async Task TestChangesPeopleAsync()
        {
            // Basic check
            SearchContainer<ChangesListItem> changesPage1 = await Config.Client.GetChangesPeopleAsync();

            Assert.NotNull(changesPage1);
            Assert.True(changesPage1.Results.Count > 0);
            Assert.True(changesPage1.TotalResults > changesPage1.Results.Count);
            Assert.Equal(1, changesPage1.Page);

            // Page 2
            SearchContainer<ChangesListItem> changesPage2 = await Config.Client.GetChangesPeopleAsync(2);

            Assert.NotNull(changesPage2);
            Assert.Equal(2, changesPage2.Page);

            // Check date range (max)
            DateTime higher = DateTime.UtcNow.AddDays(-7);
            SearchContainer<ChangesListItem> changesMaxDate = await Config.Client.GetChangesPeopleAsync(endDate: higher);

            Assert.NotNull(changesMaxDate);
            Assert.Equal(1, changesMaxDate.Page);
            Assert.NotEqual(changesPage1.TotalResults, changesMaxDate.TotalResults);

            // Check date range (lower)
            DateTime lower = DateTime.UtcNow.AddDays(-6);       // Use 6 days to avoid clashes with the 'higher'
            SearchContainer<ChangesListItem> changesLowDate = await Config.Client.GetChangesPeopleAsync(startDate: lower);

            Assert.NotNull(changesLowDate);
            Assert.Equal(1, changesLowDate.Page);
            Assert.NotEqual(changesPage1.TotalResults, changesLowDate.TotalResults);

            // None of the id's in changesLowDate should exist in changesMaxDate, and vice versa
            Assert.True(changesLowDate.Results.All(lowItem => changesMaxDate.Results.All(maxItem => maxItem.Id != lowItem.Id)));
            Assert.True(changesMaxDate.Results.All(maxItem => changesLowDate.Results.All(lowItem => maxItem.Id != lowItem.Id)));
        }


        [Fact]
        public async Task TestChangesTvShowsAsync()
        {
            // Basic check
            SearchContainer<ChangesListItem> changesPage1 = await Config.Client.GetChangesTvAsync();

            Assert.NotNull(changesPage1);
            Assert.NotNull(changesPage1.Results);
            Assert.True(changesPage1.Results.Count > 0);
            Assert.True(changesPage1.TotalResults >= changesPage1.Results.Count);
            Assert.Equal(1, changesPage1.Page);

            if (changesPage1.TotalPages > 1)
            {
                Assert.True(changesPage1.TotalResults > changesPage1.Results.Count);
                // Page 2
                SearchContainer<ChangesListItem> changesPage2 = await Config.Client.GetChangesTvAsync(2);

                Assert.NotNull(changesPage2);
                Assert.Equal(2, changesPage2.Page);
            }

            // Check date range (max)
            DateTime higher = DateTime.UtcNow.AddDays(-8);
            SearchContainer<ChangesListItem> changesMaxDate = await Config.Client.GetChangesTvAsync(endDate: higher);

            Assert.NotNull(changesMaxDate);
            Assert.Equal(1, changesMaxDate.Page);
            Assert.NotEqual(changesPage1.TotalResults, changesMaxDate.TotalResults);

            // Check date range (lower)
            DateTime lower = DateTime.UtcNow.AddDays(-6);       // Use 6 days to avoid clashes with the 'higher'
            SearchContainer<ChangesListItem> changesLowDate = await Config.Client.GetChangesTvAsync(startDate: lower);

            Assert.NotNull(changesLowDate);
            Assert.Equal(1, changesLowDate.Page);
            Assert.NotEqual(changesPage1.TotalResults, changesLowDate.TotalResults);

            // None of the id's in changesLowDate should exist in changesMaxDate, and vice versa
            foreach (ChangesListItem changeItem in changesLowDate.Results)
            {
                bool existsInOtherList = changesMaxDate.Results.Any(x => x.Id == changeItem.Id);

                Assert.False(existsInOtherList, "Item id " + changeItem.Id + " is duplicated");
            }

            foreach (ChangesListItem changeItem in changesMaxDate.Results)
            {
                bool existsInOtherList = changesLowDate.Results.Any(x => x.Id == changeItem.Id);

                Assert.False(existsInOtherList, "Item id " + changeItem.Id + " is duplicated");
            }
        }
    }
}
