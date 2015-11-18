using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientNetworkTests
    {
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
            Network network = _config.Client.GetNetwork(IdHelper.Hbo).Result;

            Assert.IsNotNull(network);
            Assert.AreEqual("HBO", network.Name);
            Assert.AreEqual(IdHelper.Hbo, network.Id);
        }
    }
}
