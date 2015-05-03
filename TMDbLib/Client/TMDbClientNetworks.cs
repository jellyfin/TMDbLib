using System.Globalization;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Retrieves a network by it's TMDb id. A network is a distributer of media content ex. HBO, AMC
        /// </summary>
        /// <param name="networkId">The id of the network object to retrieve</param>
        public async Task<Network> GetNetwork(int networkId)
        {
            RestRequest request = new RestRequest("network/{networkId}");
            request.AddUrlSegment("networkId", networkId.ToString(CultureInfo.InvariantCulture));

            IRestResponse<Network> response = await _client.ExecuteGetTaskAsync<Network>(request).ConfigureAwait(false);

            return response.Data;
        }
    }
}
