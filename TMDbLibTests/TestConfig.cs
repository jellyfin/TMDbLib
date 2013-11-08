using System.Configuration;
using TMDbLib.Client;

namespace TMDbLibTests
{
    public class TestConfig
    {
        private const string APIKey = "";

        public TMDbClient Client { get; set; }

        public TestConfig(bool useSsl = false)
        {
            if (APIKey.Length == 0)
                throw new ConfigurationException("You need to configure the API Key before running any tests. Look at the TestConfig class.");

            Client = new TMDbClient(APIKey, useSsl);
        }
    }
}