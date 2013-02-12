using System;
using System.ComponentModel;

namespace TMDbLib.Objects.Companies
{
    [Flags]
    public enum CompanyMethods
    {
        [Description("Undefined")]
        Undefined = 0,
        [Description("movies")]
        Movies = 1
    }
}