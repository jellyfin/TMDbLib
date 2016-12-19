using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.Movies;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class ChangeItemConverterTest : TestBase
    {
        /// <summary>
        /// Tests the ChangeItemConverter
        /// </summary>
        [Fact]
        public void TestChangeItemConverter()
        {
            // Not all ChangeItem's have an iso_639_1
            IgnoreMissingJson(" / iso_639_1");

            // Ignore missing movie properties
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / credits", " / images", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos");

            Movie latestMovie = Config.Client.GetMovieLatestAsync().Sync();
            List<Change> changes = Config.Client.GetMovieChangesAsync(latestMovie.Id).Sync();
            List<ChangeItemBase> changeItems = changes.SelectMany(s => s.Items).ToList();

            ChangeAction[] actions = { ChangeAction.Added, ChangeAction.Created, ChangeAction.Updated };

            Assert.NotEmpty(changeItems);
            Assert.All(changeItems, item => Assert.Contains(item.Action, actions));

            IEnumerable<ChangeItemBase> items = changeItems.Where(s => s.Action == ChangeAction.Added);
            Assert.All(items, item => Assert.IsType<ChangeItemAdded>(item));

            items = changeItems.Where(s => s.Action == ChangeAction.Updated);
            Assert.All(items, item => Assert.IsType<ChangeItemUpdated>(item));

            items = changeItems.Where(s => s.Action == ChangeAction.Created);
            Assert.All(items, item => Assert.IsType<ChangeItemCreated>(item));
        }
    }
}