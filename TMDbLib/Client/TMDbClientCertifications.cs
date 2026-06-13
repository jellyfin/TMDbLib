using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Certifications;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Gets the list of movie content rating certifications.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Movie certifications organized by country.</returns>
    public async Task<CertificationsContainer?> GetMovieCertificationsAsync(CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("certification/movie/list");

        var resp = await req.GetOfT<CertificationsContainer>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the list of TV show content rating certifications.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>TV certifications organized by country.</returns>
    public async Task<CertificationsContainer?> GetTvCertificationsAsync(CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("certification/tv/list");

        var resp = await req.GetOfT<CertificationsContainer>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
