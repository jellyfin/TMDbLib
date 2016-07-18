using TMDbLib.Utilities;

namespace TMDbLibTests.TestClasses
{
    enum EnumTestEnum
    {
        A,
        [EnumValue("B-Description")]
        B
    }
}