using System.ComponentModel;

namespace TMDbLib.Objects.Find
{
    public enum FindExternalSource
    {
        [Description("imdb")]
        Imdb,
        [Description("freebase_mid")]
        FreeBaseMid,
        [Description("freebase_id")]
        FreeBaseId,
        [Description("tvrage")]
        TvRage,
        [Description("tvdb")]
        TvDb
    }
}