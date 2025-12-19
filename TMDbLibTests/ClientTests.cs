using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Client;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb client functionality.
/// </summary>
public class ClientTests : TestBase
{
    /// <summary>
    /// Tests that TMDb configuration can be retrieved and cached.
    /// </summary>
    [Fact]
    public async Task GetConfigTest()
    {
        Assert.False(TMDbClient.HasConfig);
        await TMDbClient.GetConfigAsync();
        Assert.True(TMDbClient.HasConfig);

        await Verify(TMDbClient.Config);
    }

    /// <summary>
    /// Tests that TMDb configuration can be retrieved with SSL enabled.
    /// </summary>
    [Fact]
    public async Task GetConfigSslTest()
    {
        TestConfig config = new TestConfig(true);

        Assert.False(config.Client.HasConfig);
        await config.Client.GetConfigAsync();
        Assert.True(config.Client.HasConfig);

        await Verify(config.Client.Config);
    }

    /// <summary>
    /// Verifies that accessing configuration before it is loaded throws an exception.
    /// </summary>
    [Fact]
    public void GetConfigFailTest()
    {
        Assert.Throws<InvalidOperationException>(() => TMDbClient.Config);
    }

    /// <summary>
    /// Tests that TMDb configuration can be set manually.
    /// </summary>
    [Fact]
    public void SetConfigTest()
    {
        TMDbConfig config = new TMDbConfig
        {
            ChangeKeys = new List<string>()
        };
        config.ChangeKeys.Add("a");
        config.Images = new ConfigImageTypes
        {
            BaseUrl = " .."
        };

        Assert.False(TMDbClient.HasConfig);
        TMDbClient.SetConfig(config);
        Assert.True(TMDbClient.HasConfig);

        Assert.Same(config, TMDbClient.Config);
    }
    /// <summary>
    /// Tests that the client can be constructed with various URL and SSL configurations.
    /// </summary>
    [Fact]
    public async Task ClientConstructorUrlTest()
    {
        using TMDbClient clientA = new TMDbClient(TestConfig.APIKey, false, "http://api.themoviedb.org") { MaxRetryCount = 2 };
        await clientA.GetConfigAsync();

        using TMDbClient clientB = new TMDbClient(TestConfig.APIKey, true, "http://api.themoviedb.org") { MaxRetryCount = 2 };
        await clientB.GetConfigAsync();

        using TMDbClient clientC = new TMDbClient(TestConfig.APIKey, false, "https://api.themoviedb.org") { MaxRetryCount = 2 };
        await clientC.GetConfigAsync();

        using TMDbClient clientD = new TMDbClient(TestConfig.APIKey, true, "https://api.themoviedb.org") { MaxRetryCount = 2 };
        await clientD.GetConfigAsync();
    }
    /// <summary>
    /// Verifies that setting an invalid MaxRetryCount value throws an exception.
    /// </summary>
    [Fact]
    public void ClientSetBadMaxRetryValue()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => TMDbClient.MaxRetryCount = -1);
    }
    /// <summary>
    /// Tests that image URLs can be generated with and without SSL.
    /// </summary>
    [Fact]
    public async Task ClientTestUrlGenerator()
    {
        await TMDbClient.GetConfigAsync();

        Uri uri = TMDbClient.GetImageUrl("w92", "/2B7RySy2WMVJKKEFN2XA3IFb8w0.jpg");
        Uri uriSsl = TMDbClient.GetImageUrl("w92", "/2B7RySy2WMVJKKEFN2XA3IFb8w0.jpg", true);

        await Verify(new
        {
            uri,
            uriSsl
        });
    }
    /// <summary>
    /// Tests that rate limit exceptions are properly thrown when the API rate limit is exceeded.
    /// </summary>
    [Fact(Skip = "Disabled till we can consistently reproduce a rate limit")]
    public async Task ClientRateLimitTest()
    {
        TMDbClient client = TMDbClient;
        client.MaxRetryCount = 0;

        await Assert.ThrowsAsync<RequestLimitExceededException>(async () =>
        {
            try
            {
                List<Task> tasks = new List<Task>(100);
                for (int i = 0; i < 100; i++)
                {
                    tasks.Add(client.GetMovieAsync(IdHelper.AGoodDayToDieHard));
                }
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
