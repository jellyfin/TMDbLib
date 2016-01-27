using System;
using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.Companies
{
    [Flags]
    public enum CompanyMethods
    {
        [Display(Description = "Undefined")]
        Undefined = 0,
        [Display(Description = "movies")]
        Movies = 1
    }
}