using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Client;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientTests : TestBase
    {
        public ClientTests(TestConfig testConfig) : base(testConfig)
        {
        }

        [Fact]
        public void GetConfigTest()
        {
            Assert.False(Config.Client.HasConfig);
            Config.Client.GetConfigAsync().Sync();
            Assert.True(Config.Client.HasConfig);

            Assert.NotNull(Config.Client.Config);
        }

        [Fact(Skip = "Re-do test for SSL operations")]
        public void GetConfigSslTest()
        {
            //TestConfig config = new TestConfig(true);

            //Assert.False(config.Client.HasConfig);
            //config.Client.GetConfigAsync().Sync();
            //Assert.True(config.Client.HasConfig);

            //Assert.NotNull(config.Client.Config);
        }

        [Fact]
        public void GetConfigFailTest()
        {
            Assert.Throws<InvalidOperationException>(() => Config.Client.Config);
        }

        [Fact]
        public void SetConfigTest()
        {
            TMDbConfig config = new TMDbConfig();
            config.ChangeKeys = new List<string>();
            config.ChangeKeys.Add("a");
            config.Images = new ConfigImageTypes();
            config.Images.BaseUrl = " ..";

            Assert.False(Config.Client.HasConfig);
            Config.Client.SetConfig(config);
            Assert.True(Config.Client.HasConfig);

            Assert.Same(config, Config.Client.Config);
        }

        [Fact]
        public void ClientConstructorUrlTest()
        {
            TMDbClient clientA = new TMDbClient(TestConfig.APIKey, false, "http://api.themoviedb.org") { MaxRetryCount = 2 };
            clientA.GetConfigAsync().Sync();

            TMDbClient clientB = new TMDbClient(TestConfig.APIKey, true, "http://api.themoviedb.org") { MaxRetryCount = 2 };
            clientB.GetConfigAsync().Sync();

            TMDbClient clientC = new TMDbClient(TestConfig.APIKey, false, "https://api.themoviedb.org") { MaxRetryCount = 2 };
            clientC.GetConfigAsync().Sync();

            TMDbClient clientD = new TMDbClient(TestConfig.APIKey, true, "https://api.themoviedb.org") { MaxRetryCount = 2 };
            clientD.GetConfigAsync().Sync();
        }

        [Fact]
        public void ClientSetBadMaxRetryValue()
        {
            TMDbClient client = new TMDbClient(TestConfig.APIKey);

            Assert.Throws<ArgumentOutOfRangeException>(() => client.MaxRetryCount = -1);
        }

        [Fact]
        public void ClientRateLimitTest()
        {
            const int id = IdHelper.AGoodDayToDieHard;

            TMDbClient client = new TMDbClient(TestConfig.APIKey);
            client.MaxRetryCount = 0;

            Assert.Throws<RequestLimitExceededException>(() =>
            {
                try
                {
                    Parallel.For(0, 100, i =>
                    {
                        client.GetMovieAsync(id).Sync();
                    });
                }
                catch (AggregateException ex)
                {
                    // Unpack the InnerException
                    throw ex.InnerException;
                }
            });
        }
    }
}
