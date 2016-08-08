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
        [Fact]
        public void GetConfigTest()
        {
            Assert.False(Config.Client.HasConfig);
            Config.Client.GetConfig();
            Assert.True(Config.Client.HasConfig);

            Assert.NotNull(Config.Client.Config);
        }

        [Fact]
        public void GetConfigSslTest()
        {
            TestConfig config = new TestConfig(true);

            Assert.False(config.Client.HasConfig);
            config.Client.GetConfig();
            Assert.True(config.Client.HasConfig);

            Assert.NotNull(config.Client.Config);
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
            TMDbClient clientA = new TMDbClient(TestConfig.APIKey, false, "http://api.themoviedb.org");
            clientA.GetConfig();

            TMDbClient clientB = new TMDbClient(TestConfig.APIKey, true, "http://api.themoviedb.org");
            clientB.GetConfig();

            TMDbClient clientC = new TMDbClient(TestConfig.APIKey, false, "https://api.themoviedb.org");
            clientC.GetConfig();

            TMDbClient clientD = new TMDbClient(TestConfig.APIKey, true, "https://api.themoviedb.org");
            clientD.GetConfig();
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
