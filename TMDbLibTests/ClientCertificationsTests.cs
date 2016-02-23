using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Certifications;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientCertificationsTests
    {
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestCertificationsListMovie()
        {
            CertificationsContainer result = _config.Client.GetMovieCertificationsAsync().Result;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Certifications);
            Assert.IsTrue(result.Certifications.Count > 1);

            List<CertificationItem> certAu = result.Certifications["AU"];
            Assert.IsNotNull(certAu);
            Assert.IsTrue(certAu.Count > 2);

            CertificationItem ratingE = certAu.Single(s => s.Certification == "E");

            Assert.IsNotNull(ratingE);
            Assert.AreEqual("E", ratingE.Certification);
            Assert.AreEqual("Exempt from classification. Films that are exempt from classification must not contain contentious material (i.e. material that would ordinarily be rated M or higher).", ratingE.Meaning);
            Assert.AreEqual(1, ratingE.Order);
        }

        [TestMethod]
        public void TestCertificationsListTv()
        {
            CertificationsContainer result = _config.Client.GetTvCertificationsAsync().Result;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Certifications);
            Assert.IsTrue(result.Certifications.Count > 1);

            List<CertificationItem> certUs = result.Certifications["US"];
            Assert.IsNotNull(certUs);
            Assert.IsTrue(certUs.Count > 2);

            CertificationItem ratingNr = certUs.SingleOrDefault(s => s.Certification == "NR");

            Assert.IsNotNull(ratingNr);
            Assert.AreEqual("NR", ratingNr.Certification);
            Assert.AreEqual("No rating information.", ratingNr.Meaning);
            Assert.AreEqual(0, ratingNr.Order);
        }
    }
}