using System;
using System.Collections.Generic;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

public class EnumStringValueConverterTest : TestBase
{
    public static IEnumerable<object[]> GetEnumMembers(Type type)
    {
        Array values = Enum.GetValues(type);

        foreach (Enum value in values)
        {
            yield return new object[] { value };
        }
    }
    [Theory]
    [MemberData(nameof(GetEnumMembers), typeof(ChangeAction))]
    [MemberData(nameof(GetEnumMembers), typeof(MediaType))]
    public void EnumStringValueConverter_Data(object original)
    {
        string json = Serializer.SerializeToString(original);
        object result = Serializer.DeserializeFromString(json, original.GetType());

        Assert.IsType(original.GetType(), result);
        Assert.Equal(original, result);
    }
}
