using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}
