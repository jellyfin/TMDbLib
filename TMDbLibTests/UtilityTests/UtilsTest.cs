using System;
using TMDbLib.Utilities;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the utility helper methods.
/// </summary>
public class UtilsTest : TestBase
{
    /// <summary>
    /// Tests that GetDescription returns the enum name when no EnumValue attribute is present.
    /// </summary>
    [Fact]
    public void EnumDescriptionNonDescriptionTest()
    {
        var @enum = EnumTestEnum.A;
        var s = @enum.GetDescription();

        Assert.Equal("A", s);
    }

    /// <summary>
    /// Tests that GetDescription returns the EnumValue attribute value when present.
    /// </summary>
    [Fact]
    public void EnumDescriptionTest()
    {
        var @enum = EnumTestEnum.B;
        var s = @enum.GetDescription();

        Assert.Equal("B-Description", s);
    }

    enum EnumTestEnum
    {
        A,
        [EnumValue("B-Description")]
        B
    }

    struct EnumTestStruct
    {
    }
}
