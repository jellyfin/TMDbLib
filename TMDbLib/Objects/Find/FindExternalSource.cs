using TMDbLib.Utilities;

namespace TMDbLib.Objects.Find
{
    public enum FindExternalSource
    {
        [EnumValue("imdb_id")]
        Imdb,
        [EnumValue("freebase_mid")]
        FreeBaseMid,
        [EnumValue("freebase_id")]
        FreeBaseId,
        [EnumValue("tvrage_id")]
        TvRage,
        [EnumValue("tvdb_id")]
        TvDb
    }
}