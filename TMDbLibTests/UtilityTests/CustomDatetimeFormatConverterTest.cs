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
        public CustomDatetimeFormatConverterTest(TestConfig config) : base(config)
        {
        }

        [Fact]
        public void CustomDatetimeFormatConverter_Data()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new CustomDatetimeFormatConverter());

            Token original = new Token();
            original.ExpiresAt = DateTime.UtcNow.Date;
            original.ExpiresAt = original.ExpiresAt.AddMilliseconds(-original.ExpiresAt.Millisecond);

            string json = JsonConvert.SerializeObject(original, settings);
            Token result = JsonConvert.DeserializeObject<Token>(json, settings);

            Assert.Equal(original.ExpiresAt, result.ExpiresAt);
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