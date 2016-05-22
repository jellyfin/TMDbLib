using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Client;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientTests : TestBase
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
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ClientSetBadMaxRetryValue()
        {
            TMDbClient client = new TMDbClient(TestConfig.APIKey);

            client.MaxRetryCount = -1;

            Assert.Fail();
        }

        [TestMethod]
        [ExpectedException(typeof(RequestLimitExceededException))]
        public void ClientRateLimitTest()
        {
            const int id = IdHelper.AGoodDayToDieHard;

            TMDbClient client = new TMDbClient(TestConfig.APIKey);
            client.MaxRetryCount = 0;

            try
            {
                Parallel.For(0, 100, i =>
                {
                    try
                    {
                        client.GetMovieAsync(id).Wait();
                    }
                    catch (AggregateException ex)
                    {
                        // Unpack the InnerException
                        throw ex.InnerException;
                    }
                });
            }
            catch (AggregateException ex)
            {
                // Unpack the InnerException
                throw ex.InnerException;
            }

            Assert.Fail();
        }
    }
}
