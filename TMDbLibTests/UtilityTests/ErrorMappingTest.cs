using System.Net;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Rest;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Verifies that <see cref="RestRequest.MapErrorToException"/> picks the right exception type
/// for combinations of HTTP status code and TMDb body status_code.
/// </summary>
public class ErrorMappingTest : TestBase
{
    private static TMDbStatusMessage Msg(int code) => new() { StatusCode = code, StatusMessage = "test" };

    /// <summary>
    /// Codes that are explicitly authentication / authorization failures should produce a
    /// <see cref="TMDbAuthenticationException"/> regardless of the underlying HTTP status.
    /// </summary>
    [Theory]
    [InlineData(3)]   // AuthenticationFailedPermissions
    [InlineData(7)]   // InvalidApiKey
    [InlineData(10)]  // SuspendedApiKey
    [InlineData(14)]  // AuthenticationFailed
    [InlineData(16)]  // DeviceDenied
    [InlineData(17)]  // SessionDenied
    [InlineData(30)]  // InvalidCredentials
    [InlineData(33)]  // InvalidRequestToken
    [InlineData(35)]  // InvalidToken
    [InlineData(36)]  // TokenNoWritePermission
    [InlineData(38)]  // NoEditPermission
    [InlineData(39)]  // PrivateResource
    [InlineData(45)]  // UserSuspended
    public void AuthCodes_MapToAuthenticationException(int code)
    {
        var ex = RestRequest.MapErrorToException(HttpStatusCode.OK, Msg(code));

        var auth = Assert.IsType<TMDbAuthenticationException>(ex);
        Assert.NotNull(auth.StatusMessage);
        Assert.Equal(code, auth.StatusMessage!.StatusCode);
    }

    /// <summary>
    /// Codes that indicate the caller sent bad input should map to <see cref="TMDbValidationException"/>.
    /// </summary>
    [Theory]
    [InlineData(5)]   // InvalidParameters
    [InlineData(18)]  // ValidationFailed
    [InlineData(20)]  // InvalidDateRange
    [InlineData(22)]  // InvalidPage
    [InlineData(23)]  // InvalidDate
    [InlineData(27)]  // TooManyAppendToResponse
    [InlineData(28)]  // InvalidTimezone
    [InlineData(29)]  // ConfirmationRequired
    [InlineData(41)]  // RequestTokenNotApproved
    [InlineData(47)]  // InputInvalid
    public void ValidationCodes_MapToValidationException(int code)
    {
        var ex = RestRequest.MapErrorToException(HttpStatusCode.OK, Msg(code));

        Assert.IsType<TMDbValidationException>(ex);
    }

    /// <summary>
    /// Not-found body codes should fold into the existing <see cref="NotFoundException"/>.
    /// </summary>
    [Theory]
    [InlineData(6)]   // InvalidId
    [InlineData(34)]  // ResourceNotFound
    [InlineData(37)]  // SessionNotFound
    public void NotFoundCodes_MapToNotFoundException(int code)
    {
        var ex = RestRequest.MapErrorToException(HttpStatusCode.OK, Msg(code));

        Assert.IsType<NotFoundException>(ex);
    }

    /// <summary>
    /// Service-unavailable body codes (offline, maintenance, backend timeout/down) map to
    /// <see cref="TMDbServiceUnavailableException"/>.
    /// </summary>
    [Theory]
    [InlineData(9)]   // ServiceOffline
    [InlineData(24)]  // BackendTimeout
    [InlineData(43)]  // BackendUnreachable
    [InlineData(46)]  // ApiMaintenance
    public void TransientCodes_MapToServiceUnavailableException(int code)
    {
        var ex = RestRequest.MapErrorToException(HttpStatusCode.OK, Msg(code));

        Assert.IsType<TMDbServiceUnavailableException>(ex);
    }

    /// <summary>
    /// Server-side error codes map to <see cref="TMDbServerException"/>.
    /// </summary>
    [Theory]
    [InlineData(11)]  // InternalError
    [InlineData(15)]  // Failed
    [InlineData(44)]  // IdInvalid
    public void ServerCodes_MapToServerException(int code)
    {
        var ex = RestRequest.MapErrorToException(HttpStatusCode.OK, Msg(code));

        Assert.IsType<TMDbServerException>(ex);
    }

    /// <summary>
    /// Code 8 ("duplicate entry") gets its own exception type.
    /// </summary>
    [Fact]
    public void DuplicateEntryCode_MapsToDuplicateEntryException()
    {
        var ex = RestRequest.MapErrorToException(HttpStatusCode.OK, Msg(8));

        Assert.IsType<TMDbDuplicateEntryException>(ex);
    }

    /// <summary>
    /// When there is no body code, the HTTP status alone should drive a sensible
    /// fallback exception.
    /// </summary>
    [Theory]
    [InlineData(HttpStatusCode.Unauthorized, typeof(TMDbAuthenticationException))]
    [InlineData(HttpStatusCode.Forbidden, typeof(TMDbAuthenticationException))]
    [InlineData(HttpStatusCode.NotFound, typeof(NotFoundException))]
    [InlineData(HttpStatusCode.BadRequest, typeof(TMDbValidationException))]
    [InlineData((HttpStatusCode)422, typeof(TMDbValidationException))]
    [InlineData(HttpStatusCode.ServiceUnavailable, typeof(TMDbServiceUnavailableException))]
    [InlineData(HttpStatusCode.GatewayTimeout, typeof(TMDbServiceUnavailableException))]
    [InlineData(HttpStatusCode.BadGateway, typeof(TMDbServiceUnavailableException))]
    [InlineData(HttpStatusCode.InternalServerError, typeof(TMDbServerException))]
    [InlineData(HttpStatusCode.NotImplemented, typeof(TMDbServerException))]
    public void HttpStatus_FallsBackToTypedException(HttpStatusCode http, System.Type expectedType)
    {
        var ex = RestRequest.MapErrorToException(http, null);

        Assert.IsType(expectedType, ex);
    }

    /// <summary>
    /// An HTTP status we don't specifically handle (e.g. 418) without a body code
    /// should fall through to <see cref="GeneralHttpException"/>.
    /// </summary>
    [Fact]
    public void UnknownHttpStatus_NoBodyCode_FallsBackToGeneralHttpException()
    {
        var ex = RestRequest.MapErrorToException((HttpStatusCode)418, null);

        var general = Assert.IsType<GeneralHttpException>(ex);
        Assert.Equal((HttpStatusCode)418, general.HttpStatusCode);
    }
}
