using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;

namespace TMDbLibTests
{
    [TestClass]
    public class ConfigTests
    {
        private TestConfig _config;

        [TestInitialize]
        public void InitTest()
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
