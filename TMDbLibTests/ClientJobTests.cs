using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientJobTests
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
        public void TestJobList()
        {
            List<Job> jobs = _config.Client.GetJobs();

            Assert.IsNotNull(jobs);
            Assert.IsTrue(jobs.Count > 0);

            Assert.IsTrue(jobs.All(job => job.JobList != null));
            Assert.IsTrue(jobs.All(job => job.JobList.Count > 0));
        }
    }
}
