using System.Collections.Generic;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Jobs;
using TMDbLib.Rest;

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
            RestRequest req = _client2.Create("job/list");

            RestResponse<JobContainer> response = await req.ExecuteGet<JobContainer>().ConfigureAwait(false);

            return (await response.GetDataObject()).Jobs;
        }
    }
}
