using System;
using Xunit;
using TMDbLib.Utilities;
using TMDbLibTests.JsonHelpers;
using TMDbLibTests.TestClasses;

namespace TMDbLibTests
{
    public class UtilsTest : TestBase
    {
        [Fact]
        public void EnumDescriptionNonEnumTest()
        {
            EnumTestStruct strct = new EnumTestStruct();

            Assert.Throws<ArgumentException>(() => strct.GetDescription());
        }

        [Fact]
        public void EnumDescriptionNonDescriptionTest()
        {
            EnumTestEnum enm = EnumTestEnum.A;
            string s = enm.GetDescription();

            Assert.Equal("A", s);
        }

        [Fact]
        public void EnumDescriptionTest()
        {
            EnumTestEnum enm = EnumTestEnum.B;
            string s = enm.GetDescription();

            Assert.Equal("B-Description", s);
        }
    }
}