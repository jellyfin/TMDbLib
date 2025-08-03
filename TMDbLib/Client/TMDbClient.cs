#pragma warning disable CA2201 // Do not raise reserved exception types

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
    private RestClient _client;
    private TMDbConfig _config;
    private bool _disposed;

    /// <summary>
    /// Initializes a new instance of the <see cref="TMDbClient"/> class.
    /// </summary>
    /// <param name="apiKey">Your TMDb API key.</param>
    /// <param name="useSsl">Whether to use SSL/HTTPS for requests.</param>
    /// <param name="baseUrl">The base URL for the TMDb API.</param>
    /// <param name="serializer">The JSON serializer to use. If null, the default serializer will be used.</param>
    /// <param name="proxy">The web proxy to use for requests. Optional.</param>
    public TMDbClient(string apiKey, bool useSsl = true, string baseUrl = ProductionUrl, ITMDbSerializer serializer = null, IWebProxy proxy = null)
    {
        DefaultLanguage = null;
        DefaultImageLanguage = null;
        DefaultCountry = null;

        _serializer = serializer ?? TMDbJsonSerializer.Instance;

        // Setup proxy to use during requests
        // Proxy is optional. If passed, will be used in every request.
        WebProxy = proxy;

        Initialize(baseUrl, useSsl, apiKey);
    }

    /// <summary>
    /// Gets the account details of the user account associated with the current user session.
    /// </summary>
    /// <remarks>This value is automaticly populated when setting a user session.</remarks>
    public AccountDetails ActiveAccount { get; private set; }

    /// <summary>
    /// Gets the API key used for authentication with TMDb.
    /// </summary>
    public string ApiKey { get; private set; }

    /// <summary>
    /// Gets the TMDb API configuration.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when configuration has not been loaded. Call GetConfig() or SetConfig() first.</exception>
    public TMDbConfig Config
    {
        get
        {
            if (!HasConfig)
            {
                throw new InvalidOperationException("Call GetConfig() or SetConfig() first");
            }

            return _config;
        }
        private set => _config = value;
    }

    /// <summary>
    /// Gets or sets the ISO 3166-1 code. Ex. US.
    /// </summary>
    public string DefaultCountry { get; set; }

    /// <summary>
    /// Gets or sets the ISO 639-1 code. Ex en.
    /// </summary>
    public string DefaultLanguage { get; set; }

    /// <summary>
    /// Gets or sets the ISO 639-1 code. Ex en.
    /// </summary>
    public string DefaultImageLanguage { get; set; }

    /// <summary>
    /// Gets a value indicating whether the client has loaded TMDb configuration.
    /// </summary>
    public bool HasConfig { get; private set; }

    /// <summary>
    /// Gets or sets a value indicating whether exceptions should be thrown if TMDbs API returns certain errors, such as Not Found.
    /// </summary>
    public bool ThrowApiExceptions
    {
        get => _client.ThrowApiExceptions;
        set => _client.ThrowApiExceptions = value;
    }

    /// <summary>
    /// The maximum number of times a call to TMDb will be retried.
    /// </summary>
    /// <remarks>Default is 0.</remarks>
    public int MaxRetryCount
    {
        get => _client.MaxRetryCount;
        set => _client.MaxRetryCount = value;
    }

    /// <summary>
    /// Gets or sets timeout when requests to TMDb API.
    /// </summary>
    public TimeSpan Timeout
    {
        get => _client.Timeout;
        set => _client.Timeout = value;
    }

    /// <summary>
    /// Gets the session id that will be used when TMDb requires authentication.
    /// </summary>
    /// <remarks>Use 'SetSessionInformation' to assign this value.</remarks>
    public string SessionId { get; private set; }

    /// <summary>
    /// Gets the type of the session id, this will determine the level of access that is granted on the API.
    /// </summary>
    /// <remarks>Use 'SetSessionInformation' to assign this value.</remarks>
    public SessionType SessionType { get; private set; }

    /// <summary>
    /// Gets the Web Proxy to use during requests to TMDb API.
    /// </summary>
    /// <remarks>
    /// The Web Proxy is optional. If set, every request will be sent through it.
    /// Use the constructor for setting it.
    ///
    /// For convenience, this library also offers a <see cref="IWebProxy"/> implementation.
    /// Check <see cref="Utilities.TMDbAPIProxy"/> for more information.
    /// </remarks>
    public IWebProxy WebProxy { get; private set; }

    /// <summary>
    /// Used internally to assign a session id to a request. If no valid session is found, an exception is thrown.
    /// </summary>
    /// <param name="req">Request.</param>
    /// <param name="targetType">The target session type to set. If set to Unassigned, the method will take the currently set session.</param>
    /// <param name="parameterType">The location of the paramter in the resulting query.</param>
    private void AddSessionId(RestRequest req, SessionType targetType = SessionType.Unassigned, ParameterType parameterType = ParameterType.QueryString)
    {
        if ((targetType == SessionType.Unassigned && SessionType == SessionType.GuestSession) ||
            (targetType == SessionType.GuestSession))
        {
            // Either
            // - We needed ANY session ID and had a Guest session id
            // - We needed a Guest session id and had it
            req.AddParameter("guest_session_id", SessionId, parameterType);
            return;
        }

        if ((targetType == SessionType.Unassigned && SessionType == SessionType.UserSession) ||
           (targetType == SessionType.UserSession))
        {
            // Either
            // - We needed ANY session ID and had a User session id
            // - We needed a User session id and had it
            req.AddParameter("session_id", SessionId, parameterType);
            return;
        }

        // We did not have the required session type ready
        throw new UserSessionRequiredException();
    }

    /// <summary>
    /// Retrieves the TMDb API configuration information.
    /// </summary>
    /// <returns>The TMDb configuration object.</returns>
    /// <exception cref="Exception">Thrown when unable to retrieve configuration from the API.</exception>
    public async Task<TMDbConfig> GetConfigAsync()
    {
        TMDbConfig config = await _client.Create("configuration").GetOfT<TMDbConfig>(CancellationToken.None).ConfigureAwait(false)
            ?? throw new Exception("Unable to retrieve configuration");

        // Store config
        Config = config;
        HasConfig = true;

        return config;
    }

    /// <summary>
    /// Constructs the image URL.
    /// </summary>
    /// <param name="size">The desired image size (e.g., "w500", "original").</param>
    /// <param name="filePath">The image file path.</param>
    /// <param name="useSsl">Whether to use HTTPS (secure) URL. Defaults to false.</param>
    /// <returns>A complete URI for accessing the image.</returns>
    /// <exception cref="InvalidOperationException">Thrown when configuration has not been loaded. Call GetConfigAsync() first.</exception>
    public Uri GetImageUrl(string size, string filePath, bool useSsl = false)
    {
        string baseUrl = useSsl ? Config.Images.SecureBaseUrl : Config.Images.BaseUrl;
        return new Uri(baseUrl + size + filePath);
    }

    [Obsolete("Use " + nameof(GetImageBytesAsync))]
    public Task<byte[]> GetImageBytes(string size, string filePath, bool useSsl = false, CancellationToken token = default)
    {
        return GetImageBytesAsync(size, filePath, useSsl, token);
    }

    /// <summary>
    /// Downloads image data from TMDb's image servers.
    /// </summary>
    /// <param name="size">The desired image size (e.g., "w500", "original").</param>
    /// <param name="filePath">The image file path returned by the TMDb API.</param>
    /// <param name="useSsl">Whether to use HTTPS (secure) URL. Defaults to false.</param>
    /// <param name="token">A cancellation token to cancel the operation.</param>
    /// <returns>The image data.</returns>
    /// <exception cref="InvalidOperationException">Thrown when configuration has not been loaded. Call GetConfigAsync() first.</exception>
    /// <exception cref="HttpRequestException">Thrown when the HTTP request fails.</exception>
    public async Task<byte[]> GetImageBytesAsync(string size, string filePath, bool useSsl = false, CancellationToken token = default)
    {
        Uri url = GetImageUrl(size, filePath, useSsl);

        using HttpResponseMessage response = await _client.HttpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead, token).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();

#if NETSTANDARD2_0
        return await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
#else
        return await response.Content.ReadAsByteArrayAsync(token).ConfigureAwait(false);
#endif
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

        string httpScheme = useSsl ? "https" : "http";

        _client?.Dispose();
        _client = new RestClient(new Uri(string.Format(CultureInfo.InvariantCulture, "{0}://{1}/{2}/", httpScheme, baseUrl, ApiVersion)), _serializer, WebProxy);
        _client.AddDefaultQueryString("api_key", apiKey);
    }

    /// <summary>
    /// Used internally to determine if the current client has the required session set, if not an appropriate exception will be thrown.
    /// </summary>
    /// <param name="sessionType">The type of session that is required by the calling method.</param>
    /// <exception cref="UserSessionRequiredException">Thrown if the calling method requires a user session and one isn't set on the client object.</exception>
    /// <exception cref="GuestSessionRequiredException">Thrown if the calling method requires a guest session and no session is set on the client object. (neither user or client type session).</exception>
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
    /// <param name="config">The TMDb configuration to use.</param>
    public void SetConfig(TMDbConfig config)
    {
        // Store config
        Config = config;
        HasConfig = true;
    }

    /// <summary>
    /// Use this method to set the current client's authentication information.
    /// The session id assigned here will be used by the client when ever TMDb requires it.
    /// </summary>
    /// <param name="sessionId">The session id to use when making calls that require authentication.</param>
    /// <param name="sessionType">The type of session id.</param>
    /// <remarks>
    /// - Use the 'AuthenticationGetUserSessionAsync' and 'AuthenticationCreateGuestSessionAsync' methods to optain the respective session ids.
    /// - User sessions have access to far for methods than guest sessions, these can currently only be used to rate media.
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
    /// Disposes the stream state.
    /// </summary>
    /// <param name="disposing">Whether the object is currently being disposed.</param>
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
