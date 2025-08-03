using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.Movies;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class ChangeItemConverterTest : TestBase
    {
        [Fact]
        public async Task ChangeItemConverter_ChangeItemAdded()
        {
            ChangeItemAdded original = new ChangeItemAdded
            {
                Iso_639_1 = "en",
                Value = "Hello world"
            };

            string json = Serializer.SerializeToString(original);
            ChangeItemAdded result = Serializer.DeserializeFromString<ChangeItemBase>(json) as ChangeItemAdded;

            await Verify(new
            {
                json,
                result
            });
        }

        [Fact]
        public async Task ChangeItemConverter_ChangeItemCreated()
        {
            ChangeItemCreated original = new ChangeItemCreated
            {
                Iso_639_1 = "en"
            };

            string json = Serializer.SerializeToString(original);
            ChangeItemCreated result = Serializer.DeserializeFromString<ChangeItemBase>(json) as ChangeItemCreated;

            await Verify(new
            {
                json,
                result
            });
        }

        [Fact]
        public async Task ChangeItemConverter_ChangeItemDeleted()
        {
            ChangeItemDeleted original = new ChangeItemDeleted
            {
                Iso_639_1 = "en",
                OriginalValue = "Hello world"
            };

            string json = Serializer.SerializeToString(original);
            ChangeItemDeleted result = Serializer.DeserializeFromString<ChangeItemBase>(json) as ChangeItemDeleted;

            await Verify(new
            {
                json,
                result
            });
        }

        [Fact]
        public async Task ChangeItemConverter_ChangeItemUpdated()
        {
            ChangeItemUpdated original = new ChangeItemUpdated
            {
                Iso_639_1 = "en",
                OriginalValue = "Hello world",
                Value = "Hello world 1234"
            };

            string json = Serializer.SerializeToString(original);
            ChangeItemUpdated result = Serializer.DeserializeFromString<ChangeItemBase>(json) as ChangeItemUpdated;

            await Verify(new
            {
                json,
                result
            });
        }

        /// <summary>
        /// Tests the ChangeItemConverter
        /// </summary>
        [Fact]
        public async Task TestChangeItemConverter()
        {
            Movie latestMovie = await TMDbClient.GetMovieLatestAsync();
            IList<Change> changes = await TMDbClient.GetMovieChangesAsync(latestMovie.Id);

            List<ChangeItemBase> changeItems = changes.SelectMany(s => s.Items).ToList();

            ChangeAction[] actions = [ChangeAction.Added, ChangeAction.Created, ChangeAction.Updated];

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
