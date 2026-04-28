using System;
using TMDbLib.Objects.General;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Tests deserialization of <see cref="Video.PublishedAt"/> across the two
/// formats TMDb has historically returned for the field. See discussion in
/// jellyfin/jellyfin#16722 (and prior #13171, #13173): TMDb intermittently
/// switches between ISO-8601 and a "yyyy-MM-dd HH:mm:ss UTC" form for
/// <c>published_at</c> on the videos endpoint.
/// </summary>
public class VideoPublishedAtTest : TestBase
{
    /// <summary>
    /// ISO-8601 with milliseconds — the format used by TMDbLib's recorded
    /// test fixtures and (historically) by the TMDb API.
    /// </summary>
    [Fact]
    public void DeserializesIso8601PublishedAt()
    {
        var json = "{\"id\":\"abc\",\"published_at\":\"2024-02-01T15:27:17.000Z\"}";

        var video = Serializer.DeserializeFromString<Video>(json);

        Assert.NotNull(video);
        Assert.Equal(new DateTime(2024, 2, 1, 15, 27, 17, DateTimeKind.Utc),
            video!.PublishedAt.ToUniversalTime());
    }

    /// <summary>
    /// "yyyy-MM-dd HH:mm:ss UTC" — the format the live TMDb /videos endpoint
    /// returns today (and intermittently in the past). Without a converter on
    /// <see cref="Video.PublishedAt"/>, Newtonsoft.Json's strict DateTime
    /// reader rejects this form with <c>JsonReaderException: Could not convert
    /// string to DateTime</c>.
    /// </summary>
    [Fact]
    public void DeserializesUtcSuffixedPublishedAt()
    {
        var json = "{\"id\":\"abc\",\"published_at\":\"2024-02-01 15:27:17 UTC\"}";

        var video = Serializer.DeserializeFromString<Video>(json);

        Assert.NotNull(video);
        Assert.Equal(new DateTime(2024, 2, 1, 15, 27, 17, DateTimeKind.Utc),
            video!.PublishedAt.ToUniversalTime());
    }
}
