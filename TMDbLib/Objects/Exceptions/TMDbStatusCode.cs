namespace TMDbLib.Objects.Exceptions;

/// <summary>
/// Enumerates the TMDb-specific status codes that may be returned in the body of an API response.
/// The numeric value matches the <c>status_code</c> field of TMDb error payloads.
/// </summary>
/// <remarks>
/// See <see href="https://developer.themoviedb.org/docs/errors"/> for the authoritative list.
/// </remarks>
public enum TMDbStatusCode
{
    /// <summary>
    /// The body did not contain a recognised <c>status_code</c>.
    /// </summary>
    Unknown = 0,

    /// <summary>1 (HTTP 200) - Success.</summary>
    Success = 1,

    /// <summary>2 (HTTP 501) - Invalid service: this service does not exist.</summary>
    InvalidService = 2,

    /// <summary>3 (HTTP 401) - Authentication failed: you do not have permissions to access the service.</summary>
    AuthenticationFailedPermissions = 3,

    /// <summary>4 (HTTP 405) - Invalid format: this service doesn't exist in that format.</summary>
    InvalidFormat = 4,

    /// <summary>5 (HTTP 422) - Invalid parameters: your request parameters are incorrect.</summary>
    InvalidParameters = 5,

    /// <summary>6 (HTTP 404) - Invalid id: the pre-requisite id is invalid or not found.</summary>
    InvalidId = 6,

    /// <summary>7 (HTTP 401) - Invalid API key: you must be granted a valid key.</summary>
    InvalidApiKey = 7,

    /// <summary>8 (HTTP 403) - Duplicate entry: the data you tried to submit already exists.</summary>
    DuplicateEntry = 8,

    /// <summary>9 (HTTP 503) - Service offline: this service is temporarily offline, try again later.</summary>
    ServiceOffline = 9,

    /// <summary>10 (HTTP 401) - Suspended API key: access to your account has been suspended.</summary>
    SuspendedApiKey = 10,

    /// <summary>11 (HTTP 500) - Internal error: something went wrong, contact TMDB.</summary>
    InternalError = 11,

    /// <summary>12 (HTTP 201) - The item/record was updated successfully.</summary>
    UpdatedSuccessfully = 12,

    /// <summary>13 (HTTP 200) - The item/record was deleted successfully.</summary>
    DeletedSuccessfully = 13,

    /// <summary>14 (HTTP 401) - Authentication failed.</summary>
    AuthenticationFailed = 14,

    /// <summary>15 (HTTP 500) - Failed.</summary>
    Failed = 15,

    /// <summary>16 (HTTP 401) - Device denied.</summary>
    DeviceDenied = 16,

    /// <summary>17 (HTTP 401) - Session denied.</summary>
    SessionDenied = 17,

    /// <summary>18 (HTTP 400) - Validation failed.</summary>
    ValidationFailed = 18,

    /// <summary>19 (HTTP 406) - Invalid accept header.</summary>
    InvalidAcceptHeader = 19,

    /// <summary>20 (HTTP 422) - Invalid date range: should be a range no longer than 14 days.</summary>
    InvalidDateRange = 20,

    /// <summary>21 (HTTP 200) - Entry not found: the item you are trying to edit cannot be found.</summary>
    EntryNotFound = 21,

    /// <summary>22 (HTTP 400) - Invalid page: pages start at 1 and max at 500.</summary>
    InvalidPage = 22,

    /// <summary>23 (HTTP 400) - Invalid date: format needs to be YYYY-MM-DD.</summary>
    InvalidDate = 23,

    /// <summary>24 (HTTP 504) - Backend request timeout.</summary>
    BackendTimeout = 24,

    /// <summary>25 (HTTP 429) - Request count over the allowed rate limit.</summary>
    RateLimitExceeded = 25,

    /// <summary>26 (HTTP 400) - You must provide a username and password.</summary>
    MissingCredentials = 26,

    /// <summary>27 (HTTP 400) - Too many append_to_response objects: maximum is 20.</summary>
    TooManyAppendToResponse = 27,

    /// <summary>28 (HTTP 400) - Invalid timezone.</summary>
    InvalidTimezone = 28,

    /// <summary>29 (HTTP 400) - Action confirmation required (confirm=true).</summary>
    ConfirmationRequired = 29,

    /// <summary>30 (HTTP 401) - Invalid username and/or password.</summary>
    InvalidCredentials = 30,

    /// <summary>31 (HTTP 401) - Account disabled.</summary>
    AccountDisabled = 31,

    /// <summary>32 (HTTP 401) - Email not verified.</summary>
    EmailNotVerified = 32,

    /// <summary>33 (HTTP 401) - Invalid request token (expired or invalid).</summary>
    InvalidRequestToken = 33,

    /// <summary>34 (HTTP 404) - The resource you requested could not be found.</summary>
    ResourceNotFound = 34,

    /// <summary>35 (HTTP 401) - Invalid token.</summary>
    InvalidToken = 35,

    /// <summary>36 (HTTP 401) - Token has not been granted write permission by the user.</summary>
    TokenNoWritePermission = 36,

    /// <summary>37 (HTTP 404) - The requested session could not be found.</summary>
    SessionNotFound = 37,

    /// <summary>38 (HTTP 401) - You don't have permission to edit this resource.</summary>
    NoEditPermission = 38,

    /// <summary>39 (HTTP 401) - This resource is private.</summary>
    PrivateResource = 39,

    /// <summary>40 (HTTP 200) - Nothing to update.</summary>
    NothingToUpdate = 40,

    /// <summary>41 (HTTP 422) - Request token has not been approved by the user.</summary>
    RequestTokenNotApproved = 41,

    /// <summary>42 (HTTP 405) - Request method is not supported for this resource.</summary>
    MethodNotSupported = 42,

    /// <summary>43 (HTTP 502) - Couldn't connect to the backend server.</summary>
    BackendUnreachable = 43,

    /// <summary>44 (HTTP 500) - The ID is invalid.</summary>
    IdInvalid = 44,

    /// <summary>45 (HTTP 403) - This user has been suspended.</summary>
    UserSuspended = 45,

    /// <summary>46 (HTTP 503) - The API is undergoing maintenance.</summary>
    ApiMaintenance = 46,

    /// <summary>47 (HTTP 400) - The input is not valid.</summary>
    InputInvalid = 47,
}
