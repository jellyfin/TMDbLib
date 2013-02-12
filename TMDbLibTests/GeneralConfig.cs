using TMDbLib.Client;

namespace TMDbLibTests
{
    public static class GeneralConfig
    {
        public static TMDbClient Client { get; set; }

        static GeneralConfig()
        {
            Client = new TMDbClient("APIKEY");
        }
    }
}
