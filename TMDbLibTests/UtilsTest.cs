using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Utilities;
using TMDbLibTests.TestClasses;

namespace TMDbLibTests
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), AllowDerivedTypes = false)]
        public void EnumDescriptionNonEnumTest()
        {
            EnumTestStruct strct = new EnumTestStruct();
            strct.GetDescription();

            // Should always throw exception
            Assert.Fail();
        }

        [TestMethod]
        public void EnumDescriptionNonDescriptionTest()
        {
            EnumTestEnum enm = EnumTestEnum.A;
            string s = enm.GetDescription();

            Assert.AreEqual("A", s);
        }

        [TestMethod]
        public void EnumDescriptionTest()
        {
            EnumTestEnum enm = EnumTestEnum.B;
            string s = enm.GetDescription();

            Assert.AreEqual("B-Description", s);
        }
    }
}