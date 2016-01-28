using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.Account
{
    public enum AccountSortBy
    {
        Undefined = 0,
        [Display(Description = "created_at")]
        CreatedAt = 1,
    }
}
