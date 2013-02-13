using TMDbLib.Client;

namespace TMDbLibTests
{
    public class TestConfig
    {
        public TMDbClient Client { get; set; }

        public TestConfig()
        {
            Client = new TMDbClient("APIKEY");
        }
    }
}
