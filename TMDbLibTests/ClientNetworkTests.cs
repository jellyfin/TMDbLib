using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientNetworkTests : TestBase
    {
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public override void Initiator()
        {
            base.Initiator();

            _config = new TestConfig();
        }

        [TestMethod]
        public void TestNetworkGetById()
        {
            Network network = _config.Client.GetNetworkAsync(IdHelper.Hbo).Result;

            Assert.IsNotNull(network);
            Assert.AreEqual("HBO", network.Name);
            Assert.AreEqual(IdHelper.Hbo, network.Id);
        }
    }
}
