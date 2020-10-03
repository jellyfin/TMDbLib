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
        public async Task GetConfigTest()
        {
            Assert.False(TMDbClient.HasConfig);
            await TMDbClient.GetConfigAsync();
            Assert.True(TMDbClient.HasConfig);

            Assert.NotNull(TMDbClient.Config);
        }

        [Fact]
        public async Task GetConfigSslTest()
        {
            TestConfig config = new TestConfig(true);

            Assert.False(config.Client.HasConfig);
            await config.Client.GetConfigAsync();
            Assert.True(config.Client.HasConfig);

            Assert.NotNull(config.Client.Config);
        }

        [Fact]
        public void GetConfigFailTest()
        {
            Assert.Throws<InvalidOperationException>(() => TMDbClient.Config);
        }

        [Fact]
        public void SetConfigTest()
        {
            TMDbConfig config = new TMDbConfig();
            config.ChangeKeys = new List<string>();
            config.ChangeKeys.Add("a");
            config.Images = new ConfigImageTypes();
            config.Images.BaseUrl = " ..";

            Assert.False(TMDbClient.HasConfig);
            TMDbClient.SetConfig(config);
            Assert.True(TMDbClient.HasConfig);

            Assert.Same(config, TMDbClient.Config);
        }

        [Fact]
        public async Task ClientConstructorUrlTest()
        {
            TMDbClient clientA = new TMDbClient(TestConfig.APIKey, false, "http://api.themoviedb.org") { MaxRetryCount = 2 };
            await clientA.GetConfigAsync();

            TMDbClient clientB = new TMDbClient(TestConfig.APIKey, true, "http://api.themoviedb.org") { MaxRetryCount = 2 };
            await clientB.GetConfigAsync();

            TMDbClient clientC = new TMDbClient(TestConfig.APIKey, false, "https://api.themoviedb.org") { MaxRetryCount = 2 };
            await clientC.GetConfigAsync();

            TMDbClient clientD = new TMDbClient(TestConfig.APIKey, true, "https://api.themoviedb.org") { MaxRetryCount = 2 };
            await clientD.GetConfigAsync();
        }

        [Fact]
        public void ClientSetBadMaxRetryValue()
        {
            TMDbClient client = new TMDbClient(TestConfig.APIKey);

            Assert.Throws<ArgumentOutOfRangeException>(() => client.MaxRetryCount = -1);
        }

        [Fact]
        public async Task ClientRateLimitTest()
        {
            const int id = IdHelper.AGoodDayToDieHard;

            TMDbClient client = new TMDbClient(TestConfig.APIKey);
            client.MaxRetryCount = 0;

            await Assert.ThrowsAsync<RequestLimitExceededException>(async () =>
            {
                try
                {
                    List<Task> tasks = new List<Task>(100);
                    for (int i = 0; i < 100; i++)
                        tasks.Add(client.GetMovieAsync(id));

                    await Task.WhenAll(tasks);
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
