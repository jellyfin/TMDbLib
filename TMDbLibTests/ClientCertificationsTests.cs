using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Certifications;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb certifications functionality.
/// </summary>
public class ClientCertificationsTests : TestBase
{
    /// <summary>
    /// Tests that retrieving movie certifications returns a valid list of certification items.
    /// </summary>
    [Fact]
    public async Task TestCertificationsListMovieAsync()
    {
        var result = await TMDbClient.GetMovieCertificationsAsync();
        Assert.NotEmpty(result.Certifications);

        var certUs = result.Certifications["US"];
        Assert.NotEmpty(certUs);

        // Use a common US rating that's likely to exist
        var rating = certUs.FirstOrDefault(s => s.Certification == "PG-13")
            ?? certUs.First();

        await Verify(rating);
    }

    /// <summary>
    /// Tests that retrieving TV show certifications returns a valid list of certification items.
    /// </summary>
    [Fact]
    public async Task TestCertificationsListTvAsync()
    {
        var result = await TMDbClient.GetTvCertificationsAsync();
        Assert.NotEmpty(result.Certifications);

        var certUs = result.Certifications["US"];
        Assert.NotEmpty(certUs);

        var ratingNr = certUs.SingleOrDefault(s => s.Certification == "NR");

        await Verify(ratingNr);
    }
}
