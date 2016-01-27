using System;
using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.Collections
{
    [Flags]
    public enum CollectionMethods
    {
        [Display(Description = "Undefined")]
        Undefined = 0,
        [Display(Description = "images")]
        Images = 1
    }
}