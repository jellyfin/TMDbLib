using System.Net;
using Newtonsoft.Json;
using TMDbLibTests.Exceptions;
using TMDbLib.Client;
using TMDbLib.Utilities.Serializer;

namespace TMDbLibTests;

/// <summary>
/// Configuration class for test execution containing API credentials and client settings.
/// </summary>
public class TestConfig
{
    /// <summary>
    /// The TMDb API key used for testing. This key should only be used for library testing.
    /// For production use, request a dedicated key from TMDb.
    /// </summary>
    public const string APIKey = "c6b31d1cdad6a56a23f0c913e2482a31";

    /// <summary>
    /// User session ID required for authenticated tests.
    /// </summary>
    public readonly string UserSessionId = "c413282cdadad9af972c06d9b13096a8b13ab1c1";

    /// <summary>
    /// Guest session ID used for guest session tests.
    /// </summary>
    public readonly string GuestTestSessionId = "d425468da2781d6799ba14c05f7327e7";

    /// <summary>
    /// Gets the configured TMDbClient instance for API calls.
    /// </summary>
    public TMDbClient Client { get; }

    /// <summary>
    /// The username for the test account.
    /// </summary>
    public string Username = "TMDbTestAccount";

    /// <summary>
    /// The password for the test account.
    /// </summary>
    public string Password = "TJX6vP7bPC%!ZrJwAqtCU5FshHEKAwzr6YvR3%CU9s7BrjqUWmjC8AMuXju*eTEu524zsxDQK5ySY6EmjAC3e54B%WvkS9FNPE3K";

    /// <summary>
    /// Initializes a new instance of the <see cref="TestConfig"/> class.
    /// </summary>
    /// <param name="useSsl">Whether to use SSL for API calls.</param>
    /// <param name="serializer">Optional custom serializer for TMDb objects.</param>
    /// <param name="proxy">Optional web proxy for HTTP requests.</param>
    /// <exception cref="ConfigurationErrorsException">Thrown when the API key is not configured.</exception>
    public TestConfig(bool useSsl = false, ITMDbSerializer serializer = null, IWebProxy proxy = null)
    {
        if (APIKey.Length == 0)
        {
            throw new ConfigurationErrorsException("You need to configure the API Key before running any tests. Look at the TestConfig class.");
        }
        Client = new TMDbClient(APIKey, useSsl, serializer: serializer, proxy: proxy)
        {
            MaxRetryCount = 1
        };
    }
}
