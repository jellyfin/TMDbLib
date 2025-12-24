using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Credit;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Retrieves credit information by its unique credit ID.
    /// </summary>
    /// <param name="id">The unique credit ID.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The credit object with information about the person and their role.</returns>
    public async Task<Credit?> GetCreditsAsync(string id, CancellationToken cancellationToken = default)
    {
        return await GetCreditsAsync(id, DefaultLanguage, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves credit information by its unique credit ID with language option.
    /// </summary>
    /// <param name="id">The unique credit ID.</param>
    /// <param name="language">The ISO 639-1 language code for the credit text. If empty, uses the client's DefaultLanguage.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The credit object with information about the person and their role.</returns>
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
