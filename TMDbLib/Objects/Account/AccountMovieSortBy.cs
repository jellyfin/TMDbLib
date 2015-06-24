using System.ComponentModel;

namespace TMDbLib.Objects.Account
{
    public enum AccountSortBy
    {
        Undefined = 0,
        [Description("created_at")]
        CreatedAt = 1,
    }
}
