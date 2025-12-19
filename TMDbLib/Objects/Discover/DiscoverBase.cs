using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.Discover;

/// <summary>
/// Base class for discover functionality providing common query operations.
/// </summary>
/// <typeparam name="T">The type of search result items.</typeparam>
public abstract class DiscoverBase<T>
{
    private readonly TMDbClient _client;
    private readonly string _endpoint;
    private readonly SimpleNamedValueCollection parameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoverBase{T}"/> class.
    /// </summary>
    /// <param name="endpoint">The API endpoint for the discover operation.</param>
    /// <param name="client">The TMDb client instance.</param>
    protected DiscoverBase(string endpoint, TMDbClient client)
    {
        _endpoint = endpoint;
        _client = client;
        parameters = [];
    }

    /// <summary>
    /// Gets the collection of query parameters for the discover operation.
    /// </summary>
    protected SimpleNamedValueCollection Parameters => parameters;

    /// <summary>
    /// Executes the discover query using the default language.
    /// </summary>
    /// <param name="page">The page number to retrieve (default is 0).</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the search results.</returns>
    public async Task<SearchContainer<T>> Query(int page = 0, CancellationToken cancellationToken = default)
    {
        return await Query(_client.DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the discover query with a specified language.
    /// </summary>
    /// <param name="language">The language code for localized results.</param>
    /// <param name="page">The page number to retrieve (default is 0).</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation, containing the search results.</returns>
    public async Task<SearchContainer<T>> Query(string language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await _client.DiscoverPerformAsync<T>(_endpoint, language, page, Parameters, cancellationToken).ConfigureAwait(false);
    }
}
