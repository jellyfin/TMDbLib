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

        Token original = new Token
        {
            ExpiresAt = DateTime.UtcNow.Date
        };
        original.ExpiresAt = original.ExpiresAt.AddMilliseconds(-original.ExpiresAt.Millisecond);

        string json = Serializer.SerializeToString(original);
        Token result = Serializer.DeserializeFromString<Token>(json);

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
        Token token = await TMDbClient.AuthenticationRequestAutenticationTokenAsync();

        DateTime low = DateTime.UtcNow.AddHours(-2);
        DateTime high = DateTime.UtcNow.AddHours(2);

        Assert.InRange(token.ExpiresAt, low, high);
    }
}
