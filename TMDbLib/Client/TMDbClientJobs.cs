using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Jobs;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Retrieves a list of departments and positions within
        /// </summary>
        /// <returns>Valid jobs and their departments</returns>
        public async Task<List<Job>> GetJobs()
        {
            TmdbRestRequest req = _client2.Create("job/list");

            TmdbRestResponse<JobContainer> response = await req.ExecuteGetTaskAsync<JobContainer>().ConfigureAwait(false);

            return (await response.GetDataObject()).Jobs;
        }
    }
}
