using TMDbLib.Client;

namespace TMDbLibTests
{
    public class TestConfig
    {
        public TMDbClient Client { get; set; }

        public TestConfig(bool useSsl = false)
        {
            Client = new TMDbClient("3df9b05bb90365ddca3edfffd009d6c1");
        }
    }
}
