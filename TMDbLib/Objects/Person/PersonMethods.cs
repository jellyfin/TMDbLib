using System;
using System.ComponentModel;

namespace TMDbLib.Objects.People
{
    [Flags]
    public enum PersonMethods
    {
        [Description("Undefined")]
        Undefined = 0,
        [Description("credits")]
        Credits = 1,
        [Description("images")]
        Images = 2,
        [Description("changes")]
        Changes = 4
    }
}