using TMDbLib.Client;

namespace TMDbLibTests
{
    public class TestConfig
    {
        public TMDbClient Client { get; set; }

        public TestConfig(bool useSsl = false)
        {
            Client = new TMDbClient("APIKEY");
        }
    }
}
