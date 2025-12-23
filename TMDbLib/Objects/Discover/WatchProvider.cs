namespace TMDbLib.Objects.General;

/// <summary>
/// Containswatch provider IDs for use with Discover filtering.
/// Provider IDs were sourced from TMDb's watch/providers endpoint.
/// Note: Provider availability varies by region. Use with <c>WhereWatchRegionIs()</c>.
/// </summary>
/// <remarks>
/// These IDs represent the base platform providers. Many platforms have additional
/// "channel" variants (e.g., "Paramount+ Amazon Channel") which have different IDs.
/// Available provider IDs change over time, last updated: 2025-12-23.
/// </remarks>
public static class WatchProvider
{
    /// <summary>
    /// Netflix provider IDs.
    /// </summary>
    public static class Netflix
    {
        /// <summary>Netflix standard subscription.</summary>
        public const int Standard = 8;

        /// <summary>Netflix Kids.</summary>
        public const int Kids = 175;

        /// <summary>Netflix Standard with Ads.</summary>
        public const int StandardWithAds = 1796;

        /// <summary>All Netflix provider IDs.</summary>
        public static readonly int[] All = [Standard, Kids, StandardWithAds];
    }

    /// <summary>
    /// Amazon provider IDs.
    /// </summary>
    public static class Amazon
    {
        /// <summary>Amazon Prime Video.</summary>
        public const int PrimeVideo = 119;

        /// <summary>Amazon Video (purchase/rental).</summary>
        public const int Video = 10;

        /// <summary>Amazon Prime Video Free with Ads.</summary>
        public const int PrimeVideoFreeWithAds = 613;

        /// <summary>Amazon Prime Video with Ads.</summary>
        public const int PrimeVideoWithAds = 2100;

        /// <summary>Amazon MX Player.</summary>
        public const int MXPlayer = 1898;

        /// <summary>All Amazon provider IDs (excluding channel add-ons).</summary>
        public static readonly int[] All = [PrimeVideo, Video, PrimeVideoFreeWithAds, PrimeVideoWithAds, MXPlayer];
    }

    /// <summary>
    /// Disney provider IDs.
    /// </summary>
    public static class Disney
    {
        /// <summary>Disney+ streaming service.</summary>
        public const int Plus = 337;

        /// <summary>Disney+ alternate regional ID.</summary>
        public const int PlusAlt = 122;

        /// <summary>DisneyNow.</summary>
        public const int Now = 508;

        /// <summary>All Disney provider IDs.</summary>
        public static readonly int[] All = [Plus, PlusAlt, Now];
    }

    /// <summary>
    /// Max (formerly HBO Max) provider IDs.
    /// </summary>
    public static class Max
    {
        /// <summary>Max (formerly HBO Max) streaming service.</summary>
        public const int Standard = 1899;

        /// <summary>HBO Max Amazon Channel.</summary>
        public const int AmazonChannel = 1825;

        /// <summary>HBO Max Amazon Channel alternate.</summary>
        public const int AmazonChannelAlt = 2472;

        /// <summary>HBO Max on U-NEXT (Japan).</summary>
        public const int OnUNext = 2284;

        /// <summary>All Max/HBO Max provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AmazonChannelAlt, OnUNext];
    }

    /// <summary>
    /// Hulu provider IDs.
    /// </summary>
    public static class Hulu
    {
        /// <summary>Hulu streaming service.</summary>
        public const int Standard = 15;

        /// <summary>All Hulu provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Apple provider IDs.
    /// </summary>
    public static class Apple
    {
        /// <summary>Apple TV+ streaming service.</summary>
        public const int TV = 350;

        /// <summary>Apple TV Amazon Channel.</summary>
        public const int TVAmazonChannel = 2243;

        /// <summary>All Apple provider IDs.</summary>
        public static readonly int[] All = [TV, TVAmazonChannel];
    }

    /// <summary>
    /// Paramount provider IDs.
    /// </summary>
    public static class Paramount
    {
        /// <summary>Paramount+ streaming service.</summary>
        public const int Plus = 531;

        /// <summary>Paramount+ Premium tier.</summary>
        public const int PlusPremium = 2303;

        /// <summary>Paramount+ Basic with Ads.</summary>
        public const int PlusBasicWithAds = 2304;

        /// <summary>Paramount+ Essential tier.</summary>
        public const int PlusEssential = 2616;

        /// <summary>Paramount+ Amazon Channel.</summary>
        public const int PlusAmazonChannel = 582;

        /// <summary>Paramount+ Apple TV Channel.</summary>
        public const int PlusAppleTVChannel = 1853;

        /// <summary>Paramount+ Roku Premium Channel.</summary>
        public const int PlusRokuChannel = 633;

        /// <summary>Paramount Pictures.</summary>
        public const int Pictures = 187;

        /// <summary>All Paramount provider IDs.</summary>
        public static readonly int[] All = [Plus, PlusPremium, PlusBasicWithAds, PlusEssential, PlusAmazonChannel, PlusAppleTVChannel, PlusRokuChannel, Pictures];
    }

    /// <summary>
    /// Google and YouTube provider IDs.
    /// </summary>
    public static class Google
    {
        /// <summary>Google Play Movies.</summary>
        public const int PlayMovies = 3;

        /// <summary>YouTube.</summary>
        public const int YouTube = 192;

        /// <summary>YouTube Premium.</summary>
        public const int YouTubePremium = 188;

        /// <summary>YouTube Free.</summary>
        public const int YouTubeFree = 235;

        /// <summary>YouTube TV.</summary>
        public const int YouTubeTV = 2528;

        /// <summary>All Google and YouTube provider IDs.</summary>
        public static readonly int[] All = [PlayMovies, YouTube, YouTubePremium, YouTubeFree, YouTubeTV];
    }

    /// <summary>
    /// Fandango at Home (formerly Vudu) provider IDs.
    /// </summary>
    public static class FandangoAtHome
    {
        /// <summary>Fandango at Home.</summary>
        public const int Standard = 7;

        /// <summary>Fandango at Home Free.</summary>
        public const int Free = 332;

        /// <summary>All Fandango at Home provider IDs.</summary>
        public static readonly int[] All = [Standard, Free];
    }

    /// <summary>
    /// Sky provider IDs (primarily UK and Europe).
    /// </summary>
    public static class Sky
    {
        /// <summary>Sky Go.</summary>
        public const int Go = 29;

        /// <summary>Sky.</summary>
        public const int Standard = 210;

        /// <summary>Sky Store.</summary>
        public const int Store = 130;

        /// <summary>Sky X.</summary>
        public const int X = 321;

        /// <summary>Now TV.</summary>
        public const int NowTV = 39;

        /// <summary>Now TV Cinema.</summary>
        public const int NowTVCinema = 591;

        /// <summary>WOW - German Sky streaming.</summary>
        public const int WOW = 30;

        /// <summary>SkyShowtime.</summary>
        public const int Showtime = 1773;

        /// <summary>All Sky provider IDs.</summary>
        public static readonly int[] All = [Go, Standard, Store, X, NowTV, NowTVCinema, WOW, Showtime];
    }

    /// <summary>
    /// Peacock provider IDs (NBCUniversal).
    /// </summary>
    public static class Peacock
    {
        /// <summary>Peacock Premium.</summary>
        public const int Premium = 386;

        /// <summary>Peacock Premium Plus.</summary>
        public const int PremiumPlus = 387;

        /// <summary>All Peacock provider IDs.</summary>
        public static readonly int[] All = [Premium, PremiumPlus];
    }

    /// <summary>
    /// Crunchyroll provider IDs (anime streaming).
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
    /// Tubi provider IDs (free ad-supported streaming).
    /// </summary>
    public static class Tubi
    {
        /// <summary>Tubi TV.</summary>
        public const int TV = 73;

        /// <summary>All Tubi provider IDs.</summary>
        public static readonly int[] All = [TV];
    }

    /// <summary>
    /// Pluto TV provider IDs (free ad-supported streaming).
    /// </summary>
    public static class PlutoTV
    {
        /// <summary>Pluto TV.</summary>
        public const int Standard = 300;

        /// <summary>Pluto TV Live.</summary>
        public const int Live = 1965;

        /// <summary>All Pluto TV provider IDs.</summary>
        public static readonly int[] All = [Standard, Live];
    }

    /// <summary>
    /// Starz provider IDs.
    /// </summary>
    public static class Starz
    {
        /// <summary>Starz.</summary>
        public const int Standard = 43;

        /// <summary>StarzPlay.</summary>
        public const int Play = 630;

        /// <summary>Starz Amazon Channel.</summary>
        public const int AmazonChannel = 1794;

        /// <summary>Starz Apple TV Channel.</summary>
        public const int AppleTVChannel = 1855;

        /// <summary>Starz Roku Premium Channel.</summary>
        public const int RokuChannel = 634;

        /// <summary>All Starz provider IDs.</summary>
        public static readonly int[] All = [Standard, Play, AmazonChannel, AppleTVChannel, RokuChannel];
    }

    /// <summary>
    /// MGM Plus provider IDs.
    /// </summary>
    public static class MGMPlus
    {
        /// <summary>MGM Plus.</summary>
        public const int Standard = 34;

        /// <summary>MGM Plus Amazon Channel.</summary>
        public const int AmazonChannel = 583;

        /// <summary>MGM Plus Amazon Channel alternate.</summary>
        public const int AmazonChannelAlt = 2141;

        /// <summary>MGM Plus Apple TV Channel.</summary>
        public const int AppleTVChannel = 2142;

        /// <summary>MGM Plus Roku Premium Channel.</summary>
        public const int RokuChannel = 636;

        /// <summary>All MGM Plus provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AmazonChannelAlt, AppleTVChannel, RokuChannel];
    }

    /// <summary>
    /// AMC Plus provider IDs.
    /// </summary>
    public static class AMCPlus
    {
        /// <summary>AMC+.</summary>
        public const int Standard = 526;

        /// <summary>AMC+ Amazon Channel.</summary>
        public const int AmazonChannel = 528;

        /// <summary>AMC+ Apple TV Channel.</summary>
        public const int AppleTVChannel = 1854;

        /// <summary>AMC+ Roku Premium Channel.</summary>
        public const int RokuChannel = 635;

        /// <summary>AMC Channels Amazon Channel.</summary>
        public const int ChannelsAmazon = 2561;

        /// <summary>All AMC+ provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTVChannel, RokuChannel, ChannelsAmazon];
    }

    /// <summary>
    /// Discovery Plus provider IDs.
    /// </summary>
    public static class DiscoveryPlus
    {
        /// <summary>Discovery+.</summary>
        public const int Standard = 524;

        /// <summary>Discovery+ alternate.</summary>
        public const int StandardAlt = 520;

        /// <summary>Discovery+ Amazon Channel.</summary>
        public const int AmazonChannel = 584;

        /// <summary>All Discovery+ provider IDs.</summary>
        public static readonly int[] All = [Standard, StandardAlt, AmazonChannel];
    }

    /// <summary>
    /// MUBI provider IDs (curated cinema).
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
    /// Criterion Channel provider IDs.
    /// </summary>
    public static class CriterionChannel
    {
        /// <summary>Criterion Channel.</summary>
        public const int Standard = 258;

        /// <summary>All Criterion Channel provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Shudder provider IDs (horror streaming).
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
    /// BritBox provider IDs.
    /// </summary>
    public static class BritBox
    {
        /// <summary>BritBox.</summary>
        public const int Standard = 151;

        /// <summary>BritBox Amazon Channel.</summary>
        public const int AmazonChannel = 197;

        /// <summary>BritBox Apple TV Channel.</summary>
        public const int AppleTVChannel = 1852;

        /// <summary>All BritBox provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTVChannel];
    }

    /// <summary>
    /// Curiosity Stream provider IDs (documentaries).
    /// </summary>
    public static class CuriosityStream
    {
        /// <summary>Curiosity Stream.</summary>
        public const int Standard = 190;

        /// <summary>CuriosityStream Amazon Channel.</summary>
        public const int AmazonChannel = 603;

        /// <summary>CuriosityStream Apple TV Channel.</summary>
        public const int AppleTVChannel = 2060;

        /// <summary>All Curiosity Stream provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTVChannel];
    }

    /// <summary>
    /// The Roku Channel provider IDs (free streaming).
    /// </summary>
    public static class RokuChannel
    {
        /// <summary>The Roku Channel.</summary>
        public const int Standard = 207;

        /// <summary>All Roku Channel provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Plex provider IDs (free streaming).
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
    /// Kanopy provider IDs (library-based streaming).
    /// </summary>
    public static class Kanopy
    {
        /// <summary>Kanopy.</summary>
        public const int Standard = 191;

        /// <summary>All Kanopy provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Hoopla provider IDs (library-based streaming).
    /// </summary>
    public static class Hoopla
    {
        /// <summary>Hoopla.</summary>
        public const int Standard = 212;

        /// <summary>All Hoopla provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Acorn TV provider IDs (British and international content).
    /// </summary>
    public static class AcornTV
    {
        /// <summary>Acorn TV.</summary>
        public const int Standard = 87;

        /// <summary>AcornTV Amazon Channel.</summary>
        public const int AmazonChannel = 196;

        /// <summary>Acorn TV Apple TV Channel.</summary>
        public const int AppleTVChannel = 2034;

        /// <summary>All Acorn TV provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTVChannel];
    }

    /// <summary>
    /// Sundance Now provider IDs.
    /// </summary>
    public static class SundanceNow
    {
        /// <summary>Sundance Now.</summary>
        public const int Standard = 143;

        /// <summary>Sundance Now Amazon Channel.</summary>
        public const int AmazonChannel = 205;

        /// <summary>Sundance Now Apple TV Channel.</summary>
        public const int AppleTVChannel = 2048;

        /// <summary>All Sundance Now provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel, AppleTVChannel];
    }

    /// <summary>
    /// Viaplay provider IDs (Nordic streaming).
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
    /// Crave provider IDs (Canadian streaming).
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
    /// Stan provider IDs (Australian streaming).
    /// </summary>
    public static class Stan
    {
        /// <summary>Stan.</summary>
        public const int Standard = 21;

        /// <summary>All Stan provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// BBC iPlayer provider IDs (UK).
    /// </summary>
    public static class BBCiPlayer
    {
        /// <summary>BBC iPlayer.</summary>
        public const int Standard = 38;

        /// <summary>All BBC iPlayer provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// ITVX provider IDs (UK).
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
    /// Channel 4 provider IDs (UK).
    /// </summary>
    public static class Channel4
    {
        /// <summary>Channel 4.</summary>
        public const int Standard = 103;

        /// <summary>Channel 4 Plus.</summary>
        public const int Plus = 2311;

        /// <summary>All Channel 4 provider IDs.</summary>
        public static readonly int[] All = [Standard, Plus];
    }

    /// <summary>
    /// Canal Plus provider IDs (France).
    /// </summary>
    public static class CanalPlus
    {
        /// <summary>Canal+.</summary>
        public const int Standard = 2101;

        /// <summary>Canal+ Séries.</summary>
        public const int Series = 345;

        /// <summary>Premiery Canal+.</summary>
        public const int Premiery = 2102;

        /// <summary>All Canal+ provider IDs.</summary>
        public static readonly int[] All = [Standard, Series, Premiery];
    }

    /// <summary>
    /// RTL Plus provider IDs (Germany).
    /// </summary>
    public static class RTLPlus
    {
        /// <summary>RTL+.</summary>
        public const int Standard = 298;

        /// <summary>All RTL+ provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Joyn provider IDs (Germany).
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
    /// Rakuten TV provider IDs (Europe).
    /// </summary>
    public static class RakutenTV
    {
        /// <summary>Rakuten TV.</summary>
        public const int Standard = 35;

        /// <summary>Rakuten Viki.</summary>
        public const int Viki = 344;

        /// <summary>All Rakuten TV provider IDs.</summary>
        public static readonly int[] All = [Standard, Viki];
    }

    /// <summary>
    /// Hayu provider IDs (reality TV streaming).
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
    /// HIDIVE provider IDs (anime streaming).
    /// </summary>
    public static class HIDIVE
    {
        /// <summary>HIDIVE.</summary>
        public const int Standard = 430;

        /// <summary>HIDIVE Amazon Channel.</summary>
        public const int AmazonChannel = 2390;

        /// <summary>All HIDIVE provider IDs.</summary>
        public static readonly int[] All = [Standard, AmazonChannel];
    }

    /// <summary>
    /// Zee5 provider IDs (Indian streaming).
    /// </summary>
    public static class Zee5
    {
        /// <summary>Zee5.</summary>
        public const int Standard = 232;

        /// <summary>All Zee5 provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// JioHotstar provider IDs (Indian streaming, formerly Hotstar).
    /// </summary>
    public static class JioHotstar
    {
        /// <summary>JioHotstar.</summary>
        public const int Standard = 2336;

        /// <summary>All JioHotstar provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// Philo provider IDs (US live TV streaming).
    /// </summary>
    public static class Philo
    {
        /// <summary>Philo.</summary>
        public const int Standard = 2383;

        /// <summary>All Philo provider IDs.</summary>
        public static readonly int[] All = [Standard];
    }

    /// <summary>
    /// FuboTV provider IDs (sports streaming).
    /// </summary>
    public static class FuboTV
    {
        /// <summary>FuboTV.</summary>
        public const int Standard = 257;

        /// <summary>All FuboTV provider IDs.</summary>
        public static readonly int[] All = [Standard];
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
}
