using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Certifications;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Retrieves an up-to-date list of movie content rating certifications from TMDb.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A container with movie certifications organized by country.</returns>
    public async Task<CertificationsContainer> GetMovieCertificationsAsync(CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("certification/movie/list");

        CertificationsContainer resp = await req.GetOfT<CertificationsContainer>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves an up-to-date list of TV show content rating certifications from TMDb.
    /// </summary>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A container with TV certifications organized by country.</returns>
    public async Task<CertificationsContainer> GetTvCertificationsAsync(CancellationToken cancellationToken = default)
    {
        RestRequest req = _client.Create("certification/tv/list");

        CertificationsContainer resp = await req.GetOfT<CertificationsContainer>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
