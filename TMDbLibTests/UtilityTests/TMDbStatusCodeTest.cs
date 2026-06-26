using TMDbLib.Objects.Exceptions;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Tests for the TMDb error-payload model: deserialization of <see cref="TMDbStatusMessage"/>
/// and mapping of integer <c>status_code</c> values to the <see cref="TMDbStatusCode"/> enum.
/// </summary>
public class TMDbStatusCodeTest : TestBase
{
    /// <summary>
    /// The body shape returned by TMDb for an error response should round-trip into
    /// <see cref="TMDbStatusMessage"/> with both the raw int and the typed enum populated.
    /// </summary>
    [Theory]
    [InlineData(7, TMDbStatusCode.InvalidApiKey)]
    [InlineData(10, TMDbStatusCode.SuspendedApiKey)]
    [InlineData(25, TMDbStatusCode.RateLimitExceeded)]
    [InlineData(34, TMDbStatusCode.ResourceNotFound)]
    [InlineData(46, TMDbStatusCode.ApiMaintenance)]
    public void TMDbStatusMessage_Code_MapsKnownCode(int rawCode, TMDbStatusCode expected)
    {
        var json = $"{{ \"status_code\": {rawCode}, \"status_message\": \"x\" }}";

        var result = Serializer.DeserializeFromString<TMDbStatusMessage>(json);

        Assert.NotNull(result);
        Assert.Equal(rawCode, result.StatusCode);
        Assert.Equal(expected, result.Code);
    }

    /// <summary>
    /// Unknown / out-of-range numeric codes should not blow up; they should surface as
    /// <see cref="TMDbStatusCode.Unknown"/> while preserving the raw int.
    /// </summary>
    [Fact]
    public void TMDbStatusMessage_Code_UnknownValueReturnsUnknown()
    {
        var json = "{ \"status_code\": 9999, \"status_message\": \"made up\" }";

        var result = Serializer.DeserializeFromString<TMDbStatusMessage>(json);

        Assert.NotNull(result);
        Assert.Equal(9999, result.StatusCode);
        Assert.Equal(TMDbStatusCode.Unknown, result.Code);
    }

    /// <summary>
    /// A response with no <c>status_code</c> at all should also resolve to Unknown.
    /// </summary>
    [Fact]
    public void TMDbStatusMessage_Code_MissingFieldResolvesToUnknown()
    {
        var json = "{ \"status_message\": \"no code\" }";

        var result = Serializer.DeserializeFromString<TMDbStatusMessage>(json);

        Assert.NotNull(result);
        Assert.Equal(0, result.StatusCode);
        Assert.Equal(TMDbStatusCode.Unknown, result.Code);
    }

    /// <summary>
    /// The TMDb error catalog at https://developer.themoviedb.org/docs/errors documents codes 1–47.
    /// Spot-check that every documented value is present in the enum to guard against
    /// accidental deletions.
    /// </summary>
    [Fact]
    public void TMDbStatusCode_AllDocumentedCodesPresent()
    {
        for (var i = 1; i <= 47; i++)
        {
            Assert.True(
                System.Enum.IsDefined(typeof(TMDbStatusCode), i),
                $"TMDbStatusCode is missing documented code {i}");
        }
    }
}
