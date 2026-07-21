using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Credit;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Gets credit information by id.
    /// </summary>
    /// <param name="id">The credit id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The credit details.</returns>
    public async Task<Credit?> GetCreditsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await GetCreditsAsync(id, DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets credit information by id in a specific language.
    /// </summary>
    /// <param name="id">The credit id.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The credit details.</returns>
    public async Task<Credit?> GetCreditsAsync(string id, string? language, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("credit/{id}");

        if (!string.IsNullOrEmpty(language))
        {
            req.AddParameter("language", language);
        }

        req.AddUrlSegment("id", id);

        var resp = await req.GetOfT<Credit>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
