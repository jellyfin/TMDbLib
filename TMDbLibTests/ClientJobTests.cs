using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientJobTests : TestBase
    {
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public override void Initiator()
        {
            base.Initiator();

            _config = new TestConfig();
        }

        [TestMethod]
        public void TestJobList()
        {
            List<Job> jobs = _config.Client.GetJobsAsync().Result;

            Assert.IsNotNull(jobs);
            Assert.IsTrue(jobs.Count > 0);

            Assert.IsTrue(jobs.All(job => !string.IsNullOrEmpty(job.Department)));
            Assert.IsTrue(jobs.All(job => job.JobList != null));
            Assert.IsTrue(jobs.All(job => job.JobList.Count > 0));
        }
    }
}
