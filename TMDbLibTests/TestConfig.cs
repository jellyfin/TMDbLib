using System.Configuration;
using TMDbLib.Client;

namespace TMDbLibTests
{
    public class TestConfig
    {
        private const string APIKey = "";
        // Required to be able to successfully run all the account tests
        public readonly string UserSessionId = "";

        public readonly string GuestTestSessionId = "0c81565c80905bbfd685782a907ee73d";

        public TMDbClient Client { get; set; }

        public TestConfig(bool useSsl = false)
        {
            if (APIKey.Length == 0)
                throw new ConfigurationErrorsException("You need to configure the API Key before running any tests. Look at the TestConfig class.");

            Client = new TMDbClient(APIKey, useSsl);
        }
    }
}