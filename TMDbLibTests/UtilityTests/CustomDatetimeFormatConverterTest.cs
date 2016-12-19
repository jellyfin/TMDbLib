using System;
using Newtonsoft.Json;
using TMDbLib.Objects.Authentication;
using TMDbLib.Utilities.Converters;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class CustomDatetimeFormatConverterTest : TestBase
    {
        [Fact]
        public void CustomDatetimeFormatConverter_Data()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new CustomDatetimeFormatConverter());

            DateTime original = DateTime.UtcNow;

            string json = JsonConvert.SerializeObject(original);
            DateTime result = JsonConvert.DeserializeObject<DateTime>(json, settings);

            Assert.Equal(original, result);
        }

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