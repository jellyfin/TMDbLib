using System.Globalization;
using System.Threading.Tasks;
using TMDbLib.Objects.TvShows;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Retrieves a network by it's TMDb id. A network is a distributer of media content ex. HBO, AMC
        /// </summary>
        /// <param name="networkId">The id of the network object to retrieve</param>
        public async Task<Network> GetNetworkAsync(int networkId)
        {
            RestRequest req = _client.Create("network/{networkId}");
            req.AddUrlSegment("networkId", networkId.ToString(CultureInfo.InvariantCulture));

            RestResponse<Network> response = await req.ExecuteGet<Network>().ConfigureAwait(false);

            return response;
        }
    }
}
