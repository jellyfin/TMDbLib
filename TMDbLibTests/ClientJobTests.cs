using System.Collections.Generic;
using System.Linq;
using Xunit;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientJobTests : TestBase
    {
        [Fact]
        public void TestJobList()
        {
            List<Job> jobs = Config.Client.GetJobsAsync().Sync();

            Assert.NotNull(jobs);
            Assert.True(jobs.Count > 0);

            Assert.True(jobs.All(job => !string.IsNullOrEmpty(job.Department)));
            Assert.True(jobs.All(job => job.JobList != null));
            Assert.True(jobs.All(job => job.JobList.Count > 0));
        }
    }
}
