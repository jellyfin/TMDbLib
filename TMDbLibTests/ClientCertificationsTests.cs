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
            Assert.NotNull(result);
            Assert.NotNull(result.Certifications);
            Assert.True(result.Certifications.Count > 1);

            List<CertificationItem> certAu = result.Certifications["AU"];
            Assert.NotNull(certAu);
            Assert.True(certAu.Count > 2);

            CertificationItem ratingE = certAu.Single(s => s.Certification == "E");

            Assert.NotNull(ratingE);
            Assert.Equal("E", ratingE.Certification);
            Assert.Equal("Exempt from classification. Films that are exempt from classification must not contain contentious material (i.e. material that would ordinarily be rated M or higher).", ratingE.Meaning);
            Assert.Equal(1, ratingE.Order);
        }

        [Fact]
        public async Task TestCertificationsListTvAsync()
        {
            CertificationsContainer result = await TMDbClient.GetTvCertificationsAsync();
            Assert.NotNull(result);
            Assert.NotNull(result.Certifications);
            Assert.True(result.Certifications.Count > 1);

            List<CertificationItem> certUs = result.Certifications["US"];
            Assert.NotNull(certUs);
            Assert.True(certUs.Count > 2);

            CertificationItem ratingNr = certUs.SingleOrDefault(s => s.Certification == "NR");

            Assert.NotNull(ratingNr);
            Assert.Equal("NR", ratingNr.Certification);
            Assert.Equal("No rating information.", ratingNr.Meaning);
            Assert.Equal(0, ratingNr.Order);
        }
    }
}