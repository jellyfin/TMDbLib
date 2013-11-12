using System.ComponentModel;

namespace TMDbLib.Objects.Account
{
    public enum AccountMovieSortBy
    {
        Undefined = 0,
        [Description("created_at")]
        CreatedAt = 1,
    }
}
