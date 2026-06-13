using System.Collections.Generic;
using TMDbLib.Objects.Timezones;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Validates the <c>/configuration/timezones</c> response shape.
/// </summary>
public class TimezoneEntryTest : TestBase
{
    /// <summary>
    /// A single format entry round-trips into <see cref="TimezoneEntry"/> with both
    /// fields populated.
    /// </summary>
    [Fact]
    public void TimezoneEntry_DeserializesNewShape()
    {
        const string json = """
        { "iso_3166_1": "AD", "zones": ["Europe/Andorra"] }
        """;

        var result = Serializer.DeserializeFromString<TimezoneEntry>(json);

        Assert.NotNull(result);
        Assert.Equal("AD", result.Iso_3166_1);
        Assert.NotNull(result.Zones);
        Assert.Single(result.Zones);
        Assert.Equal("Europe/Andorra", result.Zones[0]);
    }

    /// <summary>
    /// The whole list (as TMDb returns it) deserializes into a list of entries, and the
    /// client-side projection into <see cref="Timezones.List"/> preserves ordering and zones.
    /// </summary>
    [Fact]
    public void TimezoneEntryArray_DeserializesAndProjectsToDictionary()
    {
        const string json = """
        [
          { "iso_3166_1": "AD", "zones": ["Europe/Andorra"] },
          { "iso_3166_1": "US", "zones": ["America/New_York", "America/Los_Angeles"] }
        ]
        """;

        var entries = Serializer.DeserializeFromString<List<TimezoneEntry>>(json);

        Assert.NotNull(entries);
        Assert.Equal(2, entries.Count);

        // Project the same way the client does, then assert the resulting dictionary.
        var dict = new Dictionary<string, List<string>>();
        foreach (var entry in entries)
        {
            Assert.NotNull(entry.Iso_3166_1);
            dict[entry.Iso_3166_1!] = entry.Zones ?? new List<string>();
        }

        Assert.Equal(new[] { "Europe/Andorra" }, dict["AD"]);
        Assert.Equal(new[] { "America/New_York", "America/Los_Angeles" }, dict["US"]);
    }
}
