using Newtonsoft.Json;
using TMDbLib.Objects.Changes;
using TMDbLib.Utilities.Converters;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class EnumStringValueConverterTest : TestBase
    {
        [Fact]
        public void EnumStringValueConverter_Data()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new EnumStringValueConverter());

            ChangeAction original = ChangeAction.Added;

            string json = JsonConvert.SerializeObject(original);
            ChangeAction result = JsonConvert.DeserializeObject<ChangeAction>(json, settings);

            Assert.Equal(original, result);
        }
    }
}