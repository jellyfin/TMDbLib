using System;
using TMDbLib.Objects.Authentication;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class TestCustomDatetimeFormatTest : TestBase
    {
        /// <summary>
        /// Tests the CustomDatetimeFormatConverter
        /// </summary>
        [Fact]
        public void TestCustomDatetimeFormatConverter()
        {
            Token token = Config.Client.AuthenticationRequestAutenticationTokenAsync().Sync();

            DateTime low = DateTime.UtcNow.AddHours(-2);
            DateTime high = DateTime.UtcNow.AddHours(2);

            Assert.InRange(token.ExpiresAt, low, high);
        }
    }
}