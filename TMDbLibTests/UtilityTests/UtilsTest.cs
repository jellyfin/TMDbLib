using TMDbLib.Objects.General;
using TMDbLib.Utilities;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests;

/// <summary>
/// Contains tests for the utility helper methods.
/// </summary>
public class UtilsTest : TestBase
{
    /// <summary>
    /// Tests that GetDescription returns the enum name when no EnumValue attribute is present.
    /// </summary>
    [Fact]
    public void EnumDescriptionNonDescriptionTest()
    {
        Assert.Equal("Unknown", MediaType.Unknown.GetDescription());
    }

    /// <summary>
    /// Tests that GetDescription returns the EnumValue attribute value when present.
    /// </summary>
    [Fact]
    public void EnumDescriptionTest()
    {
        Assert.Equal("movie", MediaType.Movie.GetDescription());
        Assert.Equal("tv_episode", MediaType.TvEpisode.GetDescription());
    }
}
