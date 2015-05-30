using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Client;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Objects.General;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientTests
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
        public void GetConfigTest()
        {
            Assert.IsFalse(_config.Client.HasConfig);
            _config.Client.GetConfig();
            Assert.IsTrue(_config.Client.HasConfig);

            Assert.IsNotNull(_config.Client.Config);
        }

        [TestMethod]
        public void GetConfigSslTest()
        {
            _config = new TestConfig(true);

            Assert.IsFalse(_config.Client.HasConfig);
            _config.Client.GetConfig();
            Assert.IsTrue(_config.Client.HasConfig);

            Assert.IsNotNull(_config.Client.Config);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException), AllowDerivedTypes = false)]
        public void GetConfigFailTest()
        {
            TMDbConfig config = _config.Client.Config;

            // Should always throw exception
            Assert.Fail();
        }

        [TestMethod]
        public void SetConfigTest()
        {
            TMDbConfig config = new TMDbConfig();
            config.ChangeKeys = new List<string>();
            config.ChangeKeys.Add("a");
            config.Images = new ConfigImageTypes();
            config.Images.BaseUrl = " ..";

            Assert.IsFalse(_config.Client.HasConfig);
            _config.Client.SetConfig(config);
            Assert.IsTrue(_config.Client.HasConfig);

            Assert.AreSame(config, _config.Client.Config);
        }

        [TestMethod]
        public void ClientConstructorUrlTest()
        {
            TMDbClient clientA = new TMDbClient(TestConfig.APIKey, false, "http://api.themoviedb.org");
            clientA.GetConfig();

            TMDbClient clientB = new TMDbClient(TestConfig.APIKey, true, "http://api.themoviedb.org");
            clientB.GetConfig();

            TMDbClient clientC = new TMDbClient(TestConfig.APIKey, false, "https://api.themoviedb.org");
            clientC.GetConfig();

            TMDbClient clientD = new TMDbClient(TestConfig.APIKey, true, "https://api.themoviedb.org");
            clientD.GetConfig();
        }

        [TestMethod]
        [ExpectedException(typeof(RequestLimitExceededException))]
        public void ClientRateLimitTest()
        {
            const int tomorrowLand = 158852;

            TMDbClient client = new TMDbClient(TestConfig.APIKey);
            client.ThrowErrorOnExeedingMaxCalls = true;

            for (int i = 0; i < 100; i++)
            {
                client.GetMovie(tomorrowLand).Wait();
            }

            Assert.Fail();
        }
    }
}
