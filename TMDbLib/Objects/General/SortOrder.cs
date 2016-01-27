using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.General
{
    public enum SortOrder
    {
        Undefined = 0,
        [Display(Description = "asc")]
        Ascending = 1,
        [Display(Description = "desc")]
        Descending = 2
    }
}
