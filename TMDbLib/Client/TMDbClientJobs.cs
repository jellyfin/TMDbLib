using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Jobs;

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
            RestRequest req = new RestRequest("job/list");

            IRestResponse<JobContainer> response = await _client.ExecuteGetTaskAsync<JobContainer>(req).ConfigureAwait(false);

            if (response == null || response.Data == null)
            {
                return null;
            }

            return response.Data.Jobs;
        }
    }
}
