using System.Globalization;
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
        public Network GetNetwork(int networkId)
        {
            var request = new RestRequest("network/{networkId}");
            request.AddUrlSegment("networkId", networkId.ToString(CultureInfo.InvariantCulture));

            IRestResponse<Network> response = _client.Get<Network>(request);

            return response.Data;
        }
    }
}
