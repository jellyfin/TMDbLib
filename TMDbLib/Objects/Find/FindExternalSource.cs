using TMDbLib.Utilities;

namespace TMDbLib.Objects.Find;

/// <summary>
/// Represents the external sources that can be used for finding content.
/// </summary>
public enum FindExternalSource
{
    /// <summary>
    /// IMDb external ID source.
    /// </summary>
    [EnumValue("imdb_id")]
    Imdb,

    /// <summary>
    /// TVDb external ID source.
    /// </summary>
    [EnumValue("tvdb_id")]
    TvDb,

    /// <summary>
    /// Freebase MID external ID source.
    /// </summary>
    [EnumValue("freebase_mid")]
    FreebaseMid,

    /// <summary>
    /// Freebase ID external ID source.
    /// </summary>
    [EnumValue("freebase_id")]
    FreebaseId,

    /// <summary>
    /// TVRage external ID source.
    /// </summary>
    [EnumValue("tvrage_id")]
    TvRage,

    /// <summary>
    /// Wikidata external ID source.
    /// </summary>
    [EnumValue("wikidata_id")]
    Wikidata,

    /// <summary>
    /// Facebook external ID source.
    /// </summary>
    [EnumValue("facebook_id")]
    Facebook,

    /// <summary>
    /// Instagram external ID source.
    /// </summary>
    [EnumValue("instagram_id")]
    Instagram,

    /// <summary>
    /// Twitter external ID source.
    /// </summary>
    [EnumValue("twitter_id")]
    Twitter,

    /// <summary>
    /// YouTube external ID source.
    /// </summary>
    [EnumValue("youtube_id")]
    YouTube,

    /// <summary>
    /// TikTok external ID source.
    /// </summary>
    [EnumValue("tiktok_id")]
    TikTok,
}
