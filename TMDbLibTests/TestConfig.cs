using System;
using System.IO;
using System.Net;
using TMDbLibTests.Exceptions;
using TMDbLib.Client;
using TMDbLib.Utilities.Serializer;
using WireMock.Server;
using WireMock.Settings;

namespace TMDbLibTests;

/// <summary>
/// Configuration class for test execution containing API credentials and mock server settings.
/// </summary>
public class TestConfig
{
    /// <summary>
    /// Environment variable name for setting the mock mode.
    /// </summary>
    public const string MockModeEnvironmentVariable = "MOCK_MODE";

    /// <summary>
    /// The TMDb API key used for testing. This key should only be used for library testing.
    /// For production use, request a dedicated key from TMDb.
    /// </summary>
    public const string APIKey = "c6b31d1cdad6a56a23f0c913e2482a31";

    /// <summary>
    /// User session ID required for authenticated tests.
    /// </summary>
    public static string UserSessionId => "c413282cdadad9af972c06d9b13096a8b13ab1c1";

    /// <summary>
    /// Guest session ID used for guest session tests.
    /// </summary>
    public static string GuestTestSessionId => "d425468da2781d6799ba14c05f7327e7";

    /// <summary>
    /// Gets the configured TMDbClient instance for API calls.
    /// </summary>
    public TMDbClient Client { get; }

    /// <summary>
    /// The username for the test account.
    /// </summary>
    public static string Username => "TMDbTestAccount";

    /// <summary>
    /// The password for the test account.
    /// </summary>
    public static string Password => "TJX6vP7bPC%!ZrJwAqtCU5FshHEKAwzr6YvR3%CU9s7BrjqUWmjC8AMuXju*eTEu524zsxDQK5ySY6EmjAC3e54B%WvkS9FNPE3K";

    /// <summary>
    /// Gets the current mock server mode from environment variable.
    /// </summary>
    public static MockServerMode Mode => GetMode();

    private static readonly Lazy<WireMockServer> _server = new(CreateServer);

    /// <summary>
    /// Initializes a new instance of the <see cref="TestConfig"/> class.
    /// </summary>
    /// <param name="useSsl">Whether to use SSL for API calls.</param>
    /// <param name="serializer">Optional custom serializer for TMDb objects.</param>
    /// <param name="proxy">Optional web proxy for HTTP requests.</param>
    /// <exception cref="ConfigurationErrorsException">Thrown when the API key is not configured.</exception>
    public TestConfig(bool useSsl = false, ITMDbSerializer? serializer = null, IWebProxy? proxy = null)
    {
        if (string.IsNullOrEmpty(APIKey))
        {
            throw new ConfigurationErrorsException("You need to configure the API Key before running any tests. Look at the TestConfig class.");
        }

        if (Mode == MockServerMode.PassThrough)
        {
            Client = new TMDbClient(APIKey, useSsl, serializer: serializer, proxy: proxy);
        }
        else
        {
            Client = new TMDbClient(APIKey, false, $"localhost:{_server.Value.Port}", serializer: serializer, proxy: proxy);
        }

        Client.MaxRetryCount = 1;
    }

    private static MockServerMode GetMode() =>
        Enum.TryParse<MockServerMode>(Environment.GetEnvironmentVariable(MockModeEnvironmentVariable), true, out var m) ? m : MockServerMode.PassThrough;

    private static WireMockServer CreateServer()
    {
        var baseDir = GetMappingsPath();
        Directory.CreateDirectory(baseDir);

        // WireMock proxy saves recordings to {root}/__admin/mappings/
        // WireMock ReadStaticMappings reads from {root}/mappings/
        // For Playback, we set root to __admin so it reads from __admin/mappings/
        var fileSystemRoot = Mode == MockServerMode.Playback
            ? Path.Combine(baseDir, "__admin")
            : baseDir;

        var settings = new WireMockServerSettings
        {
            Port = 8080,
            ReadStaticMappings = Mode == MockServerMode.Playback,
            // Use deterministic handler for Record mode to generate consistent filenames
            FileSystemHandler = Mode == MockServerMode.Record
                ? new DeterministicFileSystemHandler(fileSystemRoot)
                : new WireMock.Handlers.LocalFileSystemHandler(fileSystemRoot)
        };

        if (Mode == MockServerMode.Record)
        {
            settings.ProxyAndRecordSettings = new ProxyAndRecordSettings
            {
                Url = "https://api.themoviedb.org",
                SaveMapping = false, // Don't add to in-memory mappings (prevents caching during test run)
                SaveMappingToFile = true, // Save to files for later playback
                SaveMappingForStatusCodePattern = "*",
                AppendGuidToSavedMappingFile = true, // Unique filenames for each request
                ExcludedHeaders = ["Host", "Content-Length", "Transfer-Encoding"],
                ExcludedParams = ["api_key"]
            };
        }

        var server = WireMockServer.Start(settings);

        // Manually load mappings for Playback mode (ReadStaticMappings may fail with certain filenames)
        if (Mode == MockServerMode.Playback)
        {
            var mappingsDir = Path.Combine(fileSystemRoot, "mappings");
            if (Directory.Exists(mappingsDir))
            {
                foreach (var file in Directory.GetFiles(mappingsDir, "*.json"))
                {
                    try
                    {
                        server.ReadStaticMappingAndAddOrUpdate(file);
                    }
                    catch (Exception)
                    {
                        // Skip files that can't be loaded
                    }
                }
            }
        }

        return server;
    }

    private static string GetMappingsPath()
    {
        for (var dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory); dir != null; dir = dir.Parent)
        {
            if (File.Exists(Path.Combine(dir.FullName, "TMDbLibTests.csproj")))
            {
                return Path.Combine(dir.FullName, "__wiremock__");
            }

            var sub = Path.Combine(dir.FullName, "TMDbLibTests");
            if (File.Exists(Path.Combine(sub, "TMDbLibTests.csproj")))
            {
                return Path.Combine(sub, "__wiremock__");
            }
        }

        return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "__wiremock__");
    }
}
