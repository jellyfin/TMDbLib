using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Rest;
using TMDbLib.Utilities.Serializer;
using ParameterType = TMDbLib.Rest.ParameterType;
using RestClient = TMDbLib.Rest.RestClient;
using RestRequest = TMDbLib.Rest.RestRequest;

namespace TMDbLib.Client;

/// <summary>
/// The main client for accessing The Movie Database (TMDb) API v3.
/// </summary>
public partial class TMDbClient : IDisposable
{
    private const string ApiVersion = "3";
    private const string ProductionUrl = "api.themoviedb.org";

    private readonly ITMDbSerializer _serializer;
    private readonly HttpMessageHandler? _httpMessageHandler;
    private RestClient _client = null!;
    private TMDbConfig? _config;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbClient"/> class.
    /// </summary>
    /// <param name="apiKey">Your TMDb API key.</param>
    /// <param name="useSsl">Whether to use HTTPS.</param>
    /// <param name="baseUrl">The base URL for the TMDb API.</param>
    /// <param name="serializer">The JSON serializer to use, or null for the default.</param>
    /// <param name="proxy">Optional web proxy.</param>
    public TMDbClient(string apiKey, bool useSsl = true, string baseUrl = ProductionUrl, ITMDbSerializer? serializer = null, IWebProxy? proxy = null)
        : this(apiKey, useSsl, baseUrl, serializer, proxy, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbClient"/> class with a custom HTTP message handler for testing.
    /// </summary>
    /// <param name="apiKey">Your TMDb API key.</param>
    /// <param name="useSsl">Whether to use HTTPS.</param>
    /// <param name="baseUrl">The base URL for the TMDb API.</param>
    /// <param name="serializer">The JSON serializer to use, or null for the default.</param>
    /// <param name="proxy">Optional web proxy.</param>
    /// <param name="httpMessageHandler">Optional HTTP message handler, primarily for testing.</param>
    internal TMDbClient(string apiKey, bool useSsl, string baseUrl, ITMDbSerializer? serializer, IWebProxy? proxy, HttpMessageHandler? httpMessageHandler)
    {
        DefaultLanguage = null;
        DefaultImageLanguage = null;
        DefaultCountry = null;

        _serializer = serializer ?? TMDbJsonSerializer.Instance;
        _httpMessageHandler = httpMessageHandler;

        // Setup proxy to use during requests
        // Proxy is optional. If passed, will be used in every request.
        WebProxy = proxy;

        Initialize(baseUrl, useSsl, apiKey);
    }

    /// <summary>
    /// Gets the account details for the current user session.
    /// </summary>
    /// <remarks>Automatically populated when setting a user session.</remarks>
    public AccountDetails? ActiveAccount { get; private set; }

    /// <summary>
    /// Gets the TMDb API key.
    /// </summary>
    public string ApiKey { get; private set; } = null!;

    /// <summary>
    /// Gets the TMDb API configuration.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when configuration has not been loaded.</exception>
    public TMDbConfig Config
    {
        get
        {
            if (!HasConfig)
            {
                throw new InvalidOperationException("Call GetConfig() or SetConfig() first");
            }

            return _config!;
        }
        private set => _config = value;
    }

    /// <summary>
    /// Gets or sets the default ISO 3166-1 country code (e.g. US).
    /// </summary>
    public string? DefaultCountry { get; set; }

    /// <summary>
    /// Gets or sets the default ISO 639-1 language code (e.g. en).
    /// </summary>
    public string? DefaultLanguage { get; set; }

    /// <summary>
    /// Gets or sets the default ISO 639-1 image language code (e.g. en).
    /// </summary>
    public string? DefaultImageLanguage { get; set; }

    /// <summary>
    /// Gets a value indicating whether the client has loaded TMDb configuration.
    /// </summary>
    public bool HasConfig { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether exceptions should be thrown for certain TMDb API errors such as Not Found.
    /// </summary>
    public bool ThrowApiExceptions
    {
        get => _client.ThrowApiExceptions;
        set => _client.ThrowApiExceptions = value;
    }

    /// <summary>
    /// Gets or sets the maximum number of times a call to TMDb will be retried.
    /// </summary>
    /// <remarks>Default is 0.</remarks>
    public int MaxRetryCount
    {
        get => _client.MaxRetryCount;
        set => _client.MaxRetryCount = value;
    }

    /// <summary>
    /// Gets or sets the timeout for requests to the TMDb API.
    /// </summary>
    public TimeSpan Timeout
    {
        get => _client.Timeout;
        set => _client.Timeout = value;
    }

    /// <summary>
    /// Gets the session id used when TMDb requires authentication.
    /// </summary>
    /// <remarks>Use 'SetSessionInformation' to assign this value.</remarks>
    public string? SessionId { get; private set; }

    /// <summary>
    /// Gets the session type, which determines the level of access granted by the API.
    /// </summary>
    /// <remarks>Use 'SetSessionInformation' to assign this value.</remarks>
    public SessionType SessionType { get; private set; }

    /// <summary>
    /// Gets the optional web proxy used for TMDb API requests.
    /// </summary>
    /// <remarks>
    /// Set via the constructor. For convenience, this library offers a <see cref="IWebProxy"/> implementation;
    /// see <see cref="Utilities.TMDbAPIProxy"/>.
    /// </remarks>
    public IWebProxy? WebProxy { get; private set; }

    /// <summary>
    /// Assigns a session id to a request, throwing if no valid session is found.
    /// </summary>
    /// <param name="req">The request.</param>
    /// <param name="targetType">The required session type, or Unassigned to use the current session.</param>
    /// <param name="parameterType">The location of the parameter in the query.</param>
    private void AddSessionId(RestRequest req, SessionType targetType = SessionType.Unassigned, ParameterType parameterType = ParameterType.QueryString)
    {
        if ((targetType == SessionType.Unassigned && SessionType == SessionType.GuestSession) ||
            (targetType == SessionType.GuestSession))
        {
            // Either
            // - We needed ANY session ID and had a Guest session id
            // - We needed a Guest session id and had it
            req.AddParameter("guest_session_id", SessionId!, parameterType);
            return;
        }

        if ((targetType == SessionType.Unassigned && SessionType == SessionType.UserSession) ||
           (targetType == SessionType.UserSession))
        {
            // Either
            // - We needed ANY session ID and had a User session id
            // - We needed a User session id and had it
            req.AddParameter("session_id", SessionId!, parameterType);
            return;
        }

        // We did not have the required session type ready
        throw new UserSessionRequiredException();
    }

    /// <summary>
    /// Gets the TMDb API configuration.
    /// </summary>
    /// <returns>The TMDb configuration.</returns>
    /// <exception cref="Exception">Thrown when configuration cannot be retrieved.</exception>
    public async Task<TMDbConfig> GetConfigAsync()
    {
        var config = await _client.Create("configuration").GetOfT<TMDbConfig>(CancellationToken.None).ConfigureAwait(false)
            ?? throw new InvalidOperationException("Unable to retrieve configuration");

        // Store config
        Config = config;
        HasConfig = true;

        return config;
    }

    /// <summary>
    /// Builds the URL for an image.
    /// </summary>
    /// <param name="size">The image size (e.g. "w500", "original").</param>
    /// <param name="filePath">The image file path.</param>
    /// <param name="useSsl">Whether to use HTTPS.</param>
    /// <returns>The complete image URI.</returns>
    /// <exception cref="InvalidOperationException">Thrown when configuration has not been loaded.</exception>
    public Uri GetImageUrl(string size, string filePath, bool useSsl = false)
    {
        var images = Config.Images ?? throw new InvalidOperationException("Image configuration not available");
        var baseUrl = useSsl ? images.SecureBaseUrl : images.BaseUrl;
        return new Uri(baseUrl + size + filePath);
    }

    /// <summary>
    /// Downloads image bytes from TMDb's image servers.
    /// </summary>
    /// <param name="size">The image size (e.g. "w500", "original").</param>
    /// <param name="filePath">The image file path.</param>
    /// <param name="useSsl">Whether to use HTTPS.</param>
    /// <param name="token">Cancellation token.</param>
    /// <returns>The image bytes.</returns>
    /// <exception cref="InvalidOperationException">Thrown when configuration has not been loaded.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<byte[]> GetImageBytesAsync(string size, string filePath, bool useSsl = false, CancellationToken token = default)
    {
        var url = GetImageUrl(size, filePath, useSsl);

        using var response = await _client.HttpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsByteArrayAsync(token).ConfigureAwait(false);
    }

    private void Initialize(string baseUrl, bool useSsl, string apiKey)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentException(null, nameof(baseUrl));
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException(null, nameof(apiKey));
        }

        ApiKey = apiKey;

        // Cleanup the provided url so that we don't get any issues when we are configuring the client
        if (baseUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
        {
            baseUrl = baseUrl.Substring("http://".Length);
        }
        else if (baseUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            baseUrl = baseUrl.Substring("https://".Length);
        }

        var httpScheme = useSsl ? "https" : "http";

        _client?.Dispose();
        _client = new RestClient(new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}/{2}/", httpScheme, baseUrl, ApiVersion)), _serializer, WebProxy, _httpMessageHandler);
        _client.AddDefaultQueryString("api_key", apiKey);
    }

    /// <summary>
    /// Verifies that the client has the required session, throwing if not.
    /// </summary>
    /// <param name="sessionType">The required session type.</param>
    /// <exception cref="UserSessionRequiredException">Thrown when no user session is assigned.</exception>
    /// <exception cref="GuestSessionRequiredException">Thrown when no guest session is assigned.</exception>
    private void RequireSessionId(SessionType sessionType)
    {
        if (string.IsNullOrWhiteSpace(SessionId))
        {
            if (sessionType == SessionType.GuestSession)
            {
                throw new UserSessionRequiredException();
            }
            else
            {
                throw new GuestSessionRequiredException();
            }
        }

        if (sessionType == SessionType.UserSession && SessionType == SessionType.GuestSession)
        {
            throw new UserSessionRequiredException();
        }
    }

    /// <summary>
    /// Sets the TMDb API configuration manually.
    /// </summary>
    /// <param name="config">The configuration to use.</param>
    public void SetConfig(TMDbConfig config)
    {
        // Store config
        Config = config;
        HasConfig = true;
    }

    /// <summary>
    /// Sets the client's authentication session.
    /// </summary>
    /// <param name="sessionId">The session id for authenticated calls.</param>
    /// <param name="sessionType">The session type.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// - Use 'AuthenticationGetUserSessionAsync' and 'AuthenticationCreateGuestSessionAsync' to obtain session ids.
    /// - User sessions grant access to more methods than guest sessions, which are currently limited to rating media.
    /// </remarks>
    public async Task SetSessionInformationAsync(string sessionId, SessionType sessionType)
    {
        ActiveAccount = null;
        SessionId = sessionId;
        if (!string.IsNullOrWhiteSpace(sessionId) && sessionType == SessionType.Unassigned)
        {
            throw new ArgumentException("When setting the session id it must always be either a guest or user session");
        }

        SessionType = string.IsNullOrWhiteSpace(sessionId) ? SessionType.Unassigned : sessionType;

        // Populate the related account information
        if (sessionType == SessionType.UserSession)
        {
            try
            {
                ActiveAccount = await AccountGetDetailsAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Unable to complete the full process so reset all values and throw the exception
                ActiveAccount = null;
                SessionId = null;
                SessionType = SessionType.Unassigned;
                throw;
            }
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
    }

    /// <summary>
    /// Disposes the client.
    /// </summary>
    /// <param name="disposing">Whether managed resources should be disposed.</param>
    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _client?.Dispose();
        }

        _disposed = true;
    }
}
