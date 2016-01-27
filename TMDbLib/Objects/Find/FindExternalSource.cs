using System.ComponentModel.DataAnnotations;

namespace TMDbLib.Objects.Find
{
    public enum FindExternalSource
    {
        [Display(Description = "imdb_id")]
        Imdb,
        [Display(Description = "freebase_mid")]
        FreeBaseMid,
        [Display(Description = "freebase_id")]
        FreeBaseId,
        [Display(Description = "tvrage_id")]
        TvRage,
        [Display(Description = "tvdb_id")]
        TvDb
    }
}