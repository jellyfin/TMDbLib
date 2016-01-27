using System.ComponentModel.DataAnnotations;

namespace TMDbLibTests.TestClasses
{
    enum EnumTestEnum
    {
        A,
        [Display(Description = "B-Description")]
        B
    }
}