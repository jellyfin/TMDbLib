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
        CertificationsContainer result = await TMDbClient.GetMovieCertificationsAsync();
        Assert.NotEmpty(result.Certifications);

        List<CertificationItem> certUs = result.Certifications["US"];
        Assert.NotEmpty(certUs);

        // Use a common US rating that's likely to exist
        CertificationItem rating = certUs.FirstOrDefault(s => s.Certification == "PG-13")
            ?? certUs.First();

        await Verify(rating);
    }

    /// <summary>
    /// Tests that retrieving TV show certifications returns a valid list of certification items.
    /// </summary>
    [Fact]
    public async Task TestCertificationsListTvAsync()
    {
        CertificationsContainer result = await TMDbClient.GetTvCertificationsAsync();
        Assert.NotEmpty(result.Certifications);

        List<CertificationItem> certUs = result.Certifications["US"];
        Assert.NotEmpty(certUs);

        CertificationItem ratingNr = certUs.SingleOrDefault(s => s.Certification == "NR");

        await Verify(ratingNr);
    }
}
