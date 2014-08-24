using System;
using System.Configuration;
using TMDbLib.Client;

namespace TMDbLibTests
{
    public class TestConfig
    {
        // This API key should only be used to test the library, for further use please request a dedicated key from the TMDb
        public const string APIKey = "c6b31d1cdad6a56a23f0c913e2482a31";

        // Required to be able to successfully run all authenticated tests
        public readonly string UserSessionId = "c413282cdadad9af972c06d9b13096a8b13ab1c1";
        public readonly string GuestTestSessionId = "d425468da2781d6799ba14c05f7327e7";

        public TMDbClient Client { get; set; }

		public string Username = "";

		public string Password = "";

        public TestConfig(bool useSsl = false)
        {
            if (APIKey.Length == 0)
                throw new ConfigurationErrorsException("You need to configure the API Key before running any tests. Look at the TestConfig class.");

            Client = new TMDbClient(APIKey, useSsl);
        }
    }
}