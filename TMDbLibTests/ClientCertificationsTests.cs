using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Certifications;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientCertificationsTests : TestBase
    {
        [Fact]
        public async Task TestCertificationsListMovieAsync()
        {
            CertificationsContainer result = await TMDbClient.GetMovieCertificationsAsync();
            Assert.NotEmpty(result.Certifications);

            List<CertificationItem> certAu = result.Certifications["AU"];
            Assert.NotEmpty(certAu);

            CertificationItem ratingE = certAu.Single(s => s.Certification == "E");

            await Verify(ratingE);
        }

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
}
