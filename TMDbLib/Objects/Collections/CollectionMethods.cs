using System;
using System.ComponentModel;

namespace TMDbLib.Objects.Collections
{
    [Flags]
    public enum CollectionMethods
    {
        //[Description("Undefined")]
        Undefined = 0,
        //[Description("images")]
        Images = 1
    }
}