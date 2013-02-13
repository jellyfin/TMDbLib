using System.ComponentModel;

namespace TMDbLibTests.TestClasses
{
    enum EnumTestEnum
    {
        A,
        [Description("B-Description")]
        B
    }
}