using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Converters;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class EnumStringValueConverterTest : TestBase
    {
        public EnumStringValueConverterTest(TestConfig testConfig) : base(testConfig)
        {
        }

        public static IEnumerable<object[]> GetEnumMembers(Type type)
        {
            IEnumerable<FieldInfo> members = type.GetTypeInfo().DeclaredFields.Where(s => s.IsStatic);

            foreach (FieldInfo member in members)
            {
                yield return new[] { member.GetValue(null) };
            }
        }

        [Theory]
        [MemberData(nameof(GetEnumMembers), typeof(ChangeAction))]
        [MemberData(nameof(GetEnumMembers), typeof(MediaType))]
        public void EnumStringValueConverter_Data(object original)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new EnumStringValueConverter());

            string json = JsonConvert.SerializeObject(original, settings);
            object result = JsonConvert.DeserializeObject(json, original.GetType(), settings);

            Assert.IsType(original.GetType(), result);
            Assert.Equal(original, result);
        }
    }
}