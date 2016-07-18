using System.Collections.Generic;
using Xunit;
using TMDbLib.Objects.Timezones;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientTimezonesTests : TestBase
    {
        [Fact]
        public void TestTimezonesList()
        {
            Timezones result = Config.Client.GetTimezonesAsync().Sync();

            Assert.NotNull(result);
            Assert.True(result.List.Count > 200);

            List<string> item = result.List["DK"];
            Assert.NotNull(item);
            Assert.Equal(1, item.Count);
            Assert.Equal("Europe/Copenhagen", item[0]);
        }
    }
}