using System;

namespace TMDbLib.Objects.Discover;

/// <summary>
/// Watch provider IDs for use with Discover filtering. Availability varies by region; combine with <c>WhereWatchRegionIs()</c>.
/// </summary>
/// <remarks>
/// IDs represent base platform providers; channel variants (e.g. "Paramount+ Amazon Channel") have separate IDs. Last updated 2026-08-19.
/// </remarks>
public static class WatchProvider
{
    /// <summary>
    /// AcornTV provider IDs.
    /// </summary>
    public static class AcornTV
    {
        /// <summary>Acorn TV.</summary>
        public const int Standard = 87;

        /// <summary>AcornTV Amazon Channel.</summary>
        public const int AmazonChannel = 196;

        /// <summary>Acorn TV Apple TV.</summary>
        public const int AppleTV = 2034;

        /// <summary>All AcornTV provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTV];
    }

    /// <summary>
    /// Amazon provider IDs.
    /// </summary>
    public static class Amazon
    {
        /// <summary>Amazon Prime Video.</summary>
        public const int PrimeVideo = 9;

        /// <summary>Amazon Video.</summary>
        public const int Video = 10;

        /// <summary>Amazon Prime Video.</summary>
        public const int PrimeVideoAlt = 119;

        /// <summary>Amazon Arthaus Channel.</summary>
        public const int ArthausChannel = 533;

        /// <summary>Amazon Prime Video Free with Ads.</summary>
        public const int PrimeVideoFreeWithAds = 613;

        /// <summary>Amazon MX Player.</summary>
        public const int MXPlayer = 1898;

        /// <summary>Amazon Prime Video with Ads.</summary>
        public const int PrimeVideoWithAds = 2100;

        /// <summary>All Amazon provider IDs.</summary>
        public static readonly int[] All = [PrimeVideo, Video, PrimeVideoAlt, ArthausChannel, PrimeVideoFreeWithAds, MXPlayer, PrimeVideoWithAds];
    }

    /// <summary>
    /// AMCPlus provider IDs.
    /// </summary>
    public static class AMCPlus
    {
        /// <summary>AMC+.</summary>
        public const int Standard = 526;

        /// <summary>AMC+ Amazon Channel.</summary>
        public const int AmazonChannel = 528;

        /// <summary>AMC+ Roku Premium Channel.</summary>
        public const int RokuChannel = 635;

        /// <summary>AMC Plus Apple TV Channel.</summary>
        public const int AppleTVChannel = 1854;

        /// <summary>AMC Channels Amazon Channel.</summary>
        public const int ChannelsAmazonChannel = 2561;

        /// <summary>All AMCPlus provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, RokuChannel, AppleTVChannel, ChannelsAmazonChannel];
    }

    /// <summary>
    /// Apple provider IDs.
    /// </summary>
    public static class Apple
    {
        /// <summary>Apple TV Store.</summary>
        public const int TVStore = 2;

        /// <summary>Apple TV.</summary>
        public const int TV = 350;

        /// <summary>Apple TV Amazon Channel.</summary>
        public const int TVAmazonChannel = 2243;

        /// <summary>All Apple provider IDs.</summary>
        public static readonly int[] All = [TVStore, TV, TVAmazonChannel];
    }

    /// <summary>
    /// BBCiPlayer provider IDs.
    /// </summary>
    public static class BBCiPlayer
    {
        /// <summary>BBC iPlayer.</summary>
        public const int Standard = 38;

        /// <summary>All BBCiPlayer provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// BritBox provider IDs.
    /// </summary>
    public static class BritBox
    {
        /// <summary>BritBox.</summary>
        public const int Standard = 151;

        /// <summary>BritBox Amazon Channel.</summary>
        public const int AmazonChannel = 197;

        /// <summary>Britbox Apple TV Channel.</summary>
        public const int AppleTVChannel = 1852;

        /// <summary>All BritBox provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTVChannel];
    }

    /// <summary>
    /// CanalPlus provider IDs.
    /// </summary>
    public static class CanalPlus
    {
        /// <summary>Canal+ Séries.</summary>
        public const int Series = 345;

        /// <summary>Canal+.</summary>
        public const int Standard = 381;

        /// <summary>Filmtastic bei Canal+.</summary>
        public const int FilmtasticBeiCanalPlus = 1929;

        /// <summary>CANAL+.</summary>
        public const int StandardAlt = 2101;

        /// <summary>Premiery Canal+.</summary>
        public const int PremieryCanalPlus = 2102;

        /// <summary>All CanalPlus provider IDs.</summary>
        public static readonly int[] All = [Series, Standard, FilmtasticBeiCanalPlus, StandardAlt, PremieryCanalPlus];
    }

    /// <summary>
    /// Channel4 provider IDs.
    /// </summary>
    public static class Channel4
    {
        /// <summary>Channel 4.</summary>
        public const int Standard = 103;

        /// <summary>Channel 4 Plus.</summary>
        public const int Plus = 2311;

        /// <summary>All Channel4 provider IDs.</summary>
        public static readonly int[] All = [Standard, Plus];
    }

    /// <summary>
    /// Crave provider IDs.
    /// </summary>
    public static class Crave
    {
        /// <summary>Crave.</summary>
        public const int Standard = 230;

        /// <summary>Crave Amazon Channel.</summary>
        public const int AmazonChannel = 2604;

        /// <summary>All Crave provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel];
    }

    /// <summary>
    /// CriterionChannel provider IDs.
    /// </summary>
    public static class CriterionChannel
    {
        /// <summary>Criterion Channel.</summary>
        public const int Standard = 258;

        /// <summary>All CriterionChannel provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Crunchyroll provider IDs.
    /// </summary>
    public static class Crunchyroll
    {
        /// <summary>Crunchyroll.</summary>
        public const int Standard = 283;

        /// <summary>Crunchyroll Amazon Channel.</summary>
        public const int AmazonChannel = 1968;

        /// <summary>All Crunchyroll provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel];
    }

    /// <summary>
    /// CuriosityStream provider IDs.
    /// </summary>
    public static class CuriosityStream
    {
        /// <summary>Curiosity Stream.</summary>
        public const int Standard = 190;

        /// <summary>CuriosityStream Amazon Channel.</summary>
        public const int AmazonChannel = 603;

        /// <summary>CuriosityStream Apple TV Channel.</summary>
        public const int AppleTVChannel = 2060;

        /// <summary>All CuriosityStream provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTVChannel];
    }

    /// <summary>
    /// DiscoveryPlus provider IDs.
    /// </summary>
    public static class DiscoveryPlus
    {
        /// <summary>Discovery+.</summary>
        public const int Standard = 510;

        /// <summary>Discovery +.</summary>
        public const int StandardAlt = 520;

        /// <summary>Discovery+.</summary>
        public const int StandardAlt2 = 524;

        /// <summary>Discovery+ Amazon Channel.</summary>
        public const int AmazonChannel = 584;

        /// <summary>All DiscoveryPlus provider IDs.</summary>
        public static readonly int[] All = [Standard, StandardAlt, StandardAlt2, AmazonChannel];
    }

    /// <summary>
    /// Disney provider IDs.
    /// </summary>
    public static class Disney
    {
        /// <summary>Disney+.</summary>
        public const int Plus = 122;

        /// <summary>Disney Plus.</summary>
        public const int PlusAlt = 337;

        /// <summary>DisneyNOW.</summary>
        public const int NOW = 508;

        /// <summary>All Disney provider IDs.</summary>
        public static readonly int[] All = [Plus, PlusAlt, NOW];
    }

    /// <summary>
    /// ESPN provider IDs.
    /// </summary>
    public static class ESPN
    {
        /// <summary>ESPN.</summary>
        public const int Standard = 1718;

        /// <summary>ESPN Plus.</summary>
        public const int Plus = 1768;

        /// <summary>All ESPN provider IDs.</summary>
        public static readonly int[] All = [Standard, Plus];
    }

    /// <summary>
    /// FandangoAtHome provider IDs.
    /// </summary>
    public static class FandangoAtHome
    {
        /// <summary>Fandango At Home.</summary>
        public const int Standard = 7;

        /// <summary>Fandango at Home Free.</summary>
        public const int Free = 332;

        /// <summary>All FandangoAtHome provider IDs.</summary>
        public static readonly int[] All = [Standard, Free];
    }

    /// <summary>
    /// FuboTV provider IDs.
    /// </summary>
    public static class FuboTV
    {
        /// <summary>fuboTV.</summary>
        public const int Standard = 257;

        /// <summary>All FuboTV provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Globoplay provider IDs.
    /// </summary>
    public static class Globoplay
    {
        /// <summary>Globoplay.</summary>
        public const int Standard = 307;

        /// <summary>All Globoplay provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Google provider IDs.
    /// </summary>
    public static class Google
    {
        /// <summary>Google Play Movies.</summary>
        public const int PlayMovies = 3;

        /// <summary>YouTube Premium.</summary>
        public const int YouTubePremium = 188;

        /// <summary>YouTube.</summary>
        public const int YouTube = 192;

        /// <summary>YouTube Free.</summary>
        public const int YouTubeFree = 235;

        /// <summary>YouTube TV.</summary>
        public const int YouTubeTV = 2528;

        /// <summary>All Google provider IDs.</summary>
        public static readonly int[] All = [PlayMovies, YouTubePremium, YouTube, YouTubeFree, YouTubeTV];
    }

    /// <summary>
    /// Hayu provider IDs.
    /// </summary>
    public static class Hayu
    {
        /// <summary>Hayu.</summary>
        public const int Standard = 223;

        /// <summary>Hayu Amazon Channel.</summary>
        public const int AmazonChannel = 296;

        /// <summary>All Hayu provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel];
    }

    /// <summary>
    /// HIDIVE provider IDs.
    /// </summary>
    public static class HIDIVE
    {
        /// <summary>HiDive.</summary>
        public const int Standard = 430;

        /// <summary>Hidive Amazon Channel.</summary>
        public const int AmazonChannel = 2390;

        /// <summary>All HIDIVE provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel];
    }

    /// <summary>
    /// Hoopla provider IDs.
    /// </summary>
    public static class Hoopla
    {
        /// <summary>Hoopla.</summary>
        public const int Standard = 212;

        /// <summary>All Hoopla provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Hulu provider IDs.
    /// </summary>
    public static class Hulu
    {
        /// <summary>Hulu.</summary>
        public const int Standard = 15;

        /// <summary>All Hulu provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// IQIYI provider IDs.
    /// </summary>
    public static class IQIYI
    {
        /// <summary>iQIYI.</summary>
        public const int Standard = 581;

        /// <summary>All IQIYI provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// ITVX provider IDs.
    /// </summary>
    public static class ITVX
    {
        /// <summary>ITVX.</summary>
        public const int Standard = 41;

        /// <summary>ITVX Premium.</summary>
        public const int Premium = 2300;

        /// <summary>All ITVX provider IDs.</summary>
        public static readonly int[] All = [Standard, Premium];
    }

    /// <summary>
    /// JioHotstar provider IDs.
    /// </summary>
    public static class JioHotstar
    {
        /// <summary>JioHotstar.</summary>
        public const int Standard = 2336;

        /// <summary>All JioHotstar provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Joyn provider IDs.
    /// </summary>
    public static class Joyn
    {
        /// <summary>Joyn.</summary>
        public const int Standard = 304;

        /// <summary>Joyn Plus.</summary>
        public const int Plus = 421;

        /// <summary>All Joyn provider IDs.</summary>
        public static readonly int[] All = [Standard, Plus];
    }

    /// <summary>
    /// Kanopy provider IDs.
    /// </summary>
    public static class Kanopy
    {
        /// <summary>Kanopy.</summary>
        public const int Standard = 191;

        /// <summary>All Kanopy provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Max provider IDs.
    /// </summary>
    public static class Max
    {
        /// <summary>HBO Max Amazon Channel.</summary>
        public const int HBOMaxAmazonChannel = 1825;

        /// <summary>HBO Max.</summary>
        public const int HBOMax = 1899;

        /// <summary>HBO Max on U-Next.</summary>
        public const int HBOMaxOnUNext = 2284;

        /// <summary>All Max provider IDs.</summary>
        public static readonly int[] All = [HBOMaxAmazonChannel, HBOMax, HBOMaxOnUNext];
    }

    /// <summary>
    /// MGMPlus provider IDs.
    /// </summary>
    public static class MGMPlus
    {
        /// <summary>MGM Plus.</summary>
        public const int Standard = 34;

        /// <summary>MGM+ Amazon Channel.</summary>
        public const int AmazonChannel = 583;

        /// <summary>MGM Amazon Channel.</summary>
        public const int AmazonChannelAlt = 588;

        /// <summary>MGM Plus Roku Premium Channel.</summary>
        public const int RokuChannel = 636;

        /// <summary>MGM Plus Amazon Channel.</summary>
        public const int AmazonChannelAlt2 = 2141;

        /// <summary>MGM+ Apple TV Channel.</summary>
        public const int AppleTVChannel = 2142;

        /// <summary>All MGMPlus provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AmazonChannelAlt, RokuChannel, AmazonChannelAlt2, AppleTVChannel];
    }

    /// <summary>
    /// MUBI provider IDs.
    /// </summary>
    public static class MUBI
    {
        /// <summary>MUBI.</summary>
        public const int Standard = 11;

        /// <summary>MUBI Amazon Channel.</summary>
        public const int AmazonChannel = 201;

        /// <summary>All MUBI provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel];
    }

    /// <summary>
    /// Netflix provider IDs.
    /// </summary>
    public static class Netflix
    {
        /// <summary>Netflix.</summary>
        public const int Standard = 8;

        /// <summary>Netflix Kids.</summary>
        public const int Kids = 175;

        /// <summary>Netflix Standard with Ads.</summary>
        public const int StandardWithAds = 1796;

        /// <summary>All Netflix provider IDs.</summary>
        public static readonly int[] All = [Standard, Kids, StandardWithAds];
    }

    /// <summary>
    /// Paramount provider IDs.
    /// </summary>
    public static class Paramount
    {
        /// <summary>Paramount Pictures.</summary>
        public const int Pictures = 187;

        /// <summary>Paramount Plus.</summary>
        public const int Plus = 531;

        /// <summary>Paramount+ Amazon Channel.</summary>
        public const int PlusAmazonChannel = 582;

        /// <summary>Paramount+ Roku Premium Channel.</summary>
        public const int PlusRokuChannel = 633;

        /// <summary>Paramount Plus Apple TV Channel.</summary>
        public const int PlusAppleTVChannel = 1853;

        /// <summary>Paramount Plus Premium.</summary>
        public const int PlusPremium = 2303;

        /// <summary>Paramount Plus Basic with Ads.</summary>
        public const int PlusBasicWithAds = 2304;

        /// <summary>Paramount Plus Essential.</summary>
        public const int PlusEssential = 2616;

        /// <summary>All Paramount provider IDs.</summary>
        public static readonly int[] All = [Pictures, Plus, PlusAmazonChannel, PlusRokuChannel, PlusAppleTVChannel, PlusPremium, PlusBasicWithAds, PlusEssential];
    }

    /// <summary>
    /// Peacock provider IDs.
    /// </summary>
    public static class Peacock
    {
        /// <summary>Peacock Premium.</summary>
        public const int Premium = 386;

        /// <summary>Peacock Premium Plus.</summary>
        public const int PremiumPlus = 387;

        /// <summary>Peacock Premium Plus Amazon Channel.</summary>
        public const int PremiumPlusAmazonChannel = 2553;

        /// <summary>All Peacock provider IDs.</summary>
        public static readonly int[] All = [Premium, PremiumPlus, PremiumPlusAmazonChannel];
    }

    /// <summary>
    /// Philo provider IDs.
    /// </summary>
    public static class Philo
    {
        /// <summary>Philo.</summary>
        public const int Standard = 2383;

        /// <summary>All Philo provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Plex provider IDs.
    /// </summary>
    public static class Plex
    {
        /// <summary>Plex.</summary>
        public const int Standard = 538;

        /// <summary>Plex Channel.</summary>
        public const int Channel = 2077;

        /// <summary>All Plex provider IDs.</summary>
        public static readonly int[] All = [Standard, Channel];
    }

    /// <summary>
    /// PlutoTV provider IDs.
    /// </summary>
    public static class PlutoTV
    {
        /// <summary>Pluto TV.</summary>
        public const int Standard = 300;

        /// <summary>Pluto TV Live.</summary>
        [Obsolete("No longer returned by TMDb's watch/providers endpoint. Will be removed in a future version.")]
        public const int Live = 1965;

        /// <summary>All PlutoTV provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// RakutenTV provider IDs.
    /// </summary>
    public static class RakutenTV
    {
        /// <summary>Rakuten TV.</summary>
        public const int Standard = 35;

        /// <summary>Rakuten Viki.</summary>
        public const int Viki = 344;

        /// <summary>All RakutenTV provider IDs.</summary>
        public static readonly int[] All = [Standard, Viki];
    }

    /// <summary>
    /// RokuChannel provider IDs.
    /// </summary>
    public static class RokuChannel
    {
        /// <summary>The Roku Channel.</summary>
        public const int Standard = 207;

        /// <summary>All RokuChannel provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// RTLPlus provider IDs.
    /// </summary>
    public static class RTLPlus
    {
        /// <summary>RTL+ Max Amazon Channel.</summary>
        public const int MaxAmazonChannel = 2578;

        /// <summary>RTL+.</summary>
        public const int Standard = 2750;

        /// <summary>All RTLPlus provider IDs.</summary>
        public static readonly int[] All = [MaxAmazonChannel, Standard];
    }

    /// <summary>
    /// Showmax provider IDs.
    /// </summary>
    [Obsolete("No longer returned by TMDb's watch/providers endpoint. Will be removed in a future version.")]
    public static class Showmax
    {
        /// <summary>Showmax.</summary>
        public const int Standard = 55;

        /// <summary>All Showmax provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Shudder provider IDs.
    /// </summary>
    public static class Shudder
    {
        /// <summary>Shudder.</summary>
        public const int Standard = 99;

        /// <summary>Shudder Amazon Channel.</summary>
        public const int AmazonChannel = 204;

        /// <summary>Shudder Apple TV Channel.</summary>
        public const int AppleTVChannel = 2049;

        /// <summary>All Shudder provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTVChannel];
    }

    /// <summary>
    /// Sky provider IDs.
    /// </summary>
    public static class Sky
    {
        /// <summary>Sky Go.</summary>
        public const int Go = 29;

        /// <summary>WOW.</summary>
        public const int WOW = 30;

        /// <summary>Now TV.</summary>
        public const int NowTV = 39;

        /// <summary>Sky Store.</summary>
        public const int Store = 130;

        /// <summary>Sky.</summary>
        public const int Standard = 210;

        /// <summary>Sky X.</summary>
        public const int X = 321;

        /// <summary>Now TV Cinema.</summary>
        public const int NowTVCinema = 591;

        /// <summary>SkyShowtime.</summary>
        public const int Showtime = 1773;

        /// <summary>TV2 Skyshowtime.</summary>
        public const int TV2Skyshowtime = 2624;

        /// <summary>All Sky provider IDs.</summary>
        public static readonly int[] All = [Go, WOW, NowTV, Store, Standard, X, NowTVCinema, Showtime, TV2Skyshowtime];
    }

    /// <summary>
    /// SonyLiv provider IDs.
    /// </summary>
    public static class SonyLiv
    {
        /// <summary>Sony Liv.</summary>
        public const int Standard = 237;

        /// <summary>All SonyLiv provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Stan provider IDs.
    /// </summary>
    public static class Stan
    {
        /// <summary>Stan.</summary>
        public const int Standard = 21;

        /// <summary>All Stan provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Starz provider IDs.
    /// </summary>
    public static class Starz
    {
        /// <summary>Starz.</summary>
        public const int Standard = 43;

        /// <summary>STARZPLAY.</summary>
        public const int PLAY = 630;

        /// <summary>Starz Roku Premium Channel.</summary>
        public const int RokuChannel = 634;

        /// <summary>Starz Amazon Channel.</summary>
        public const int AmazonChannel = 1794;

        /// <summary>Starz Apple TV Channel.</summary>
        public const int AppleTVChannel = 1855;

        /// <summary>All Starz provider IDs.</summary>
        public static readonly int[] All = [Standard, PLAY, RokuChannel, AmazonChannel, AppleTVChannel];
    }

    /// <summary>
    /// SundanceNow provider IDs.
    /// </summary>
    public static class SundanceNow
    {
        /// <summary>Sundance Now.</summary>
        public const int Standard = 143;

        /// <summary>Sundance Now Amazon Channel.</summary>
        public const int AmazonChannel = 205;

        /// <summary>Sundance Now Apple TV Channel.</summary>
        public const int AppleTVChannel = 2048;

        /// <summary>All SundanceNow provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTVChannel];
    }

    /// <summary>
    /// Tubi provider IDs.
    /// </summary>
    public static class Tubi
    {
        /// <summary>Tubi TV.</summary>
        public const int TV = 73;

        /// <summary>All Tubi provider IDs.</summary>
        public static readonly int[] All = [TV];
    }

    /// <summary>
    /// UNext provider IDs.
    /// </summary>
    public static class UNext
    {
        /// <summary>U-NEXT.</summary>
        public const int Standard = 84;

        /// <summary>All UNext provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Viaplay provider IDs.
    /// </summary>
    public static class Viaplay
    {
        /// <summary>Viaplay.</summary>
        public const int Standard = 76;

        /// <summary>Viaplay Amazon Channel.</summary>
        public const int AmazonChannel = 2296;

        /// <summary>All Viaplay provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel];
    }

    /// <summary>
    /// ViX provider IDs.
    /// </summary>
    public static class ViX
    {
        /// <summary>VIX.</summary>
        public const int Standard = 457;

        /// <summary>All ViX provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Zee5 provider IDs.
    /// </summary>
    public static class Zee5
    {
        /// <summary>Zee5.</summary>
        public const int Standard = 232;

        /// <summary>All Zee5 provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }
}
