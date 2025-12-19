using System;
using System.Collections.Generic;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the enum string value converter.
/// </summary>
public class EnumStringValueConverterTest : TestBase
{
    /// <summary>
    /// Gets all enum members of the specified type for use in theory tests.
    /// </summary>
    /// <param name="type">The enum type to get members from.</param>
    /// <returns>A collection of object arrays containing each enum member.</returns>
    public static IEnumerable<object[]> GetEnumMembers(Type type)
    {
        Array values = Enum.GetValues(type);

        foreach (Enum value in values)
        {
            yield return new object[] { value };
        }
    }

    /// <summary>
    /// Tests that the enum string value converter correctly serializes and deserializes enum values.
    /// </summary>
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
