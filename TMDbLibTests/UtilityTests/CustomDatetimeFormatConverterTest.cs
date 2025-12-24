using System;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the custom datetime format converter.
/// </summary>
public class CustomDatetimeFormatConverterTest : TestBase
{
    /// <summary>
    /// Tests that the custom datetime format converter correctly serializes and deserializes datetime values.
    /// </summary>
    [Fact]
    public void CustomDatetimeFormatConverter_Data()
    {
        // Use a fixed date to ensure deterministic test output
        var original = new Token
        {
            ExpiresAt = new DateTime(2020, 1, 15, 12, 30, 45, DateTimeKind.Utc)
        };

        var json = Serializer.SerializeToString(original);
        var result = Serializer.DeserializeFromString<Token>(json);

        Assert.NotNull(result);
        Assert.Equal(original.ExpiresAt, result.ExpiresAt);

        Verify(new
        {
            json,
            result
        });
    }

    /// <summary>
    /// Verifies that the custom datetime format converter correctly deserializes datetime values from API responses.
    /// </summary>
    [Fact]
    public async Task TestCustomDatetimeFormatConverter()
    {
        var token = await TMDbClient.AuthenticationRequestAutenticationTokenAsync();

        Assert.NotNull(token);
        // Verify the datetime was parsed correctly - should be a reasonable date
        // Accept any date after the first time the responses were recorded up toa few days in the future
        // This handles both playback mode (recorded date) and live mode (~1 hour from now)
        var low = new DateTime(2025, 12, 24, 0, 0, 0, DateTimeKind.Utc);
        var high = DateTime.UtcNow.AddDays(2);

        Assert.InRange(token.ExpiresAt, low, high);
    }
}
