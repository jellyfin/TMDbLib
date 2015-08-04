using System;
using RestSharp;
using RestSharp.Deserializers;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public string ApiKey { get; private set; }

        /// <summary>
        /// The session id that will be used when TMDb requires authentication
        /// </summary>
        /// <remarks>Use 'SetSessionInformation' to assign this value</remarks>
        public string SessionId { get; private set; }

        /// <summary>
        /// The type of the session id, this will determine the level of access that is granted on the API
        /// </summary>
        /// <remarks>Use 'SetSessionInformation' to assign this value</remarks>
        public SessionType SessionType { get; private set; }

        /// <summary>
        /// The account details of the user account associated with the current user session
        /// </summary>
        /// <remarks>This value is automaticly populated when setting a user session</remarks>
        public AccountDetails ActiveAccount { get; private set; }

        /// <summary>
        /// ISO 639-1 code. Ex en
        /// </summary>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// ISO 3166-1 code. Ex. US
        /// </summary>
        public string DefaultCountry { get; set; }

        /// <summary>
        /// The TMDb only allows x amount of requests over a specific time span.
        /// If you exceed this limit your request will be denied untill the timer resets.
        /// By default the client will keep waiting for the time to expire and then try again.
        /// For details about the allowed limits: https://www.themoviedb.org/talk/5317af69c3a3685c4a0003b1?page=1
        /// </summary>
        public bool ThrowErrorOnExeedingMaxCalls
        {
            get { return _client.ThrowErrorOnExeedingMaxCalls; }
            set { _client.ThrowErrorOnExeedingMaxCalls = value; }
        }

        /// <summary>
        /// The maximum number of times a call to TMDb will be retied
        /// </summary>
        /// <remarks>Default is 0</remarks>
        public int MaxRetryCount
        {
            get { return _client.MaxRetryCount; }
            set { _client.MaxRetryCount = value; }
        }

        /// <summary>
        /// The base number of seconds that will be waited between retry attempts.
        /// Each retry will take progressively longer to give the service a chance to recover from what ever the problem is.
        /// Formula: RetryAttempt * RetryWaitTimeInSeconds, this is the amount of time in seconds the application will wait before retrying
        /// </summary>
        /// <remarks>Default is 10</remarks>
        public int RetryWaitTimeInSeconds
        {
            get { return _client.RetryWaitTimeInSeconds; }
            set { _client.RetryWaitTimeInSeconds = value; }
        }

        public TMDbConfig Config
        {
            get
            {
                if (!HasConfig)
                    throw new InvalidOperationException("Call GetConfig() or SetConfig() first");
                return _config;
            }
            private set { _config = value; }
        }

        public bool HasConfig { get; private set; }

        private const string ApiVersion = "3";
        private const string ProductionUrl = "api.themoviedb.org";
        private TMDbRestClient _client;
        private TMDbConfig _config;

        public TMDbClient(string apiKey, bool useSsl = false, string baseUrl = ProductionUrl)
        {
            DefaultLanguage = null;
            DefaultCountry = null;

            Initialize(baseUrl, useSsl, apiKey);
        }

        private void Initialize(string baseUrl, bool useSsl, string apiKey)
        {
            if (String.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("baseUrl");

            if (String.IsNullOrWhiteSpace(apiKey))
                throw new ArgumentException("apiKey");

            ApiKey = apiKey;

            // Cleanup the provided url so that we don't get any issues when we are configuring the client
            if (baseUrl.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
                baseUrl = baseUrl.Substring("http://".Length);
            else if (baseUrl.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
                baseUrl = baseUrl.Substring("https://".Length);

            string httpScheme = useSsl ? "https" : "http";
            _client = new TMDbRestClient(String.Format("{0}://{1}/{2}/", httpScheme, baseUrl, ApiVersion));
            _client.AddDefaultParameter("api_key", apiKey, ParameterType.QueryString);

            _client.ClearHandlers();
            _client.AddHandler("application/json", new JsonDeserializer());
        }

        public void GetConfig()
        {
            RestRequest req = new RestRequest("configuration");
            IRestResponse<TMDbConfig> resp = _client.Get<TMDbConfig>(req);

            if (resp.Data == null)
                throw new Exception("Unable to retrieve configuration");

            // Store config
            Config = resp.Data;
            HasConfig = true;
        }

        public void SetConfig(TMDbConfig config)
        {
            // Store config
            Config = config;
            HasConfig = true;
        }

        public Uri GetImageUrl(string size, string filePath, bool useSsl = false)
        {
            string baseUrl = useSsl ? Config.Images.SecureBaseUrl : Config.Images.BaseUrl;
            return new Uri(baseUrl + size + filePath);
        }

        /// <summary>
        /// Use this method to set the current client's authentication information.
        /// The session id assigned here will be used by the client when ever TMDb requires it.
        /// </summary>
        /// <param name="sessionId">The session id to use when making calls that require authentication</param>
        /// <param name="sessionType">The type of session id</param>
        /// <remarks>
        /// - Use the 'AuthenticationGetUserSession' and 'AuthenticationCreateGuestSession' methods to optain the respective session ids.
        /// - User sessions have access to far for methods than guest sessions, these can currently only be used to rate media.
        /// </remarks>
        public void SetSessionInformation(string sessionId, SessionType sessionType)
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
                    ActiveAccount = AccountGetDetails().Result;
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

        /// <summary>
        /// Used internally to determine if the current client has the required session set, if not an appropriate exception will be thrown
        /// </summary>
        /// <param name="sessionType">The type of session that is required by the calling method</param>
        /// <exception cref="UserSessionRequiredException">Thrown if the calling method requires a user session and one isn't set on the client object</exception>
        /// <exception cref="GuestSessionRequiredException">Thrown if the calling method requires a guest session and no session is set on the client object. (neither user or client type session)</exception>
        private void RequireSessionId(SessionType sessionType)
        {
            if (string.IsNullOrWhiteSpace(SessionId))
            {
                if (sessionType == SessionType.GuestSession)
                    throw new UserSessionRequiredException();
                else
                    throw new GuestSessionRequiredException();
            }

            if (sessionType == SessionType.UserSession && SessionType == SessionType.GuestSession)
                throw new UserSessionRequiredException();
        }

        /// <summary>
        /// Used internally to assign a session id to a request. If no valid session is found, an exception is thrown.
        /// </summary>
        /// <param name="req">Request</param>
        /// <param name="targetType">The target session type to set. If set to Unassigned, the method will take the currently set session.</param>
        private void AddSessionId(IRestRequest req, SessionType targetType = SessionType.Unassigned)
        {
            if ((targetType == SessionType.Unassigned && SessionType == SessionType.GuestSession) ||
                (targetType == SessionType.GuestSession))
            {
                req.AddParameter("guest_session_id", SessionId, ParameterType.QueryString);
            }

            if ((targetType == SessionType.Unassigned && SessionType == SessionType.UserSession) ||
               (targetType == SessionType.UserSession))
            {
                req.AddParameter("session_id", SessionId, ParameterType.QueryString);
            }

            // TODO: Handle this case better
            throw new InvalidOperationException("Bad combination of target type and actual session type");
        }
    }
}