using Newtonsoft.Json;

namespace TMDbLibTests.JsonHelpers
{
    public abstract class TestBase 
    {
        protected readonly TestConfig Config;

        protected TestBase()
        {
            JsonSerializerSettings sett = new JsonSerializerSettings();

            Config = new TestConfig(serializer: JsonSerializer.Create(sett));
        }
    }
}