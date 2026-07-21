using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Gets a network by id. A network is a distributor of media content (e.g. HBO, AMC).
    /// </summary>
    /// <param name="networkId">The TMDb id of the network.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The network.</returns>
    public async Task<Network?> GetNetworkAsync(int networkId, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("network/{networkId}");
        req.AddUrlSegment("networkId", networkId.ToString(CultureInfo.InvariantCulture));

        var response = await req.GetOfT<Network>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the logos of a network.
    /// </summary>
    /// <param name="networkId">The TMDb id of the network.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The network's logos.</returns>
    public async Task<NetworkLogos?> GetNetworkImagesAsync(int networkId, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("network/{networkId}/images");
        req.AddUrlSegment("networkId", networkId.ToString(CultureInfo.InvariantCulture));

        var response = await req.GetOfT<NetworkLogos>(cancellationToken).ConfigureAwait(false);

        return response;
    }

    /// <summary>
    /// Gets the alternative names of a network.
    /// </summary>
    /// <param name="networkId">The TMDb id of the network.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The network's alternative names.</returns>
    public async Task<AlternativeNames?> GetNetworkAlternativeNamesAsync(int networkId, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("network/{networkId}/alternative_names");
        req.AddUrlSegment("networkId", networkId.ToString(CultureInfo.InvariantCulture));

        var response = await req.GetOfT<AlternativeNames>(cancellationToken).ConfigureAwait(false);

        return response;
    }
}
