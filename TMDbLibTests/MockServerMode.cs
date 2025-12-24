/// <summary>
/// Mock modes for test execution:
/// - PassThrough: Direct API calls (default)
/// - Record: API calls + save responses via WireMock proxy
/// - Playback: Use saved responses
///
/// Usage:
///   MOCK_MODE=Record dotnet test
///   MOCK_MODE=Playback dotnet test
///
/// Note: Stateful tests (RequiresAccountAccess) should always run with PassThrough or be excluded.
/// </summary>
public enum MockServerMode
{
    /// <summary>
    /// Direct API calls (default behavior).
    /// </summary>
    PassThrough,

    /// <summary>
    /// API calls with response recording via WireMock proxy.
    /// </summary>
    Record,

    /// <summary>
    /// Use previously recorded responses.
    /// </summary>
    Playback
}
