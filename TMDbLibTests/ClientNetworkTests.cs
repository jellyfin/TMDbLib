using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.TvShows;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientNetworkTests
    {
        private const int Hbo = 49;
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestNetworkGetById()
        {
            Network network = _config.Client.GetNetwork(Hbo).Result;

            Assert.IsNotNull(network);
            Assert.AreEqual("HBO", network.Name);
            Assert.AreEqual(Hbo, network.Id);
        }
    }
}
