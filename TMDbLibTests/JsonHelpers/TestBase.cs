using Newtonsoft.Json;
using TMDbLib.Client;

namespace TMDbLibTests.JsonHelpers
{
    public abstract class TestBase
    {
        protected readonly TestConfig TestConfig;

        protected TMDbClient TMDbClient => TestConfig.Client;

        protected TestBase()
        {
            JsonSerializerSettings sett = new JsonSerializerSettings();

            TestConfig = new TestConfig(serializer: JsonSerializer.Create(sett));
        }
    }
}