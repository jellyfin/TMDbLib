using System;
using TMDbLib.Utilities;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

public class UtilsTest : TestBase
{
    [Fact]
    public void EnumDescriptionNonEnumTest()
    {
        EnumTestStruct @struct = new EnumTestStruct();

        Assert.Throws<ArgumentException>(() => @struct.GetDescription());
    }
    [Fact]
    public void EnumDescriptionNonDescriptionTest()
    {
        EnumTestEnum @enum = EnumTestEnum.A;
        string s = @enum.GetDescription();

        Assert.Equal("A", s);
    }
    [Fact]
    public void EnumDescriptionTest()
    {
        EnumTestEnum @enum = EnumTestEnum.B;
        string s = @enum.GetDescription();

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
