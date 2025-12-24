using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<T?> GetCompanyMethodInternal<T>(int companyId, CompanyMethods companyMethod, int page = 0, string? language = null, CancellationToken cancellationToken = default)
        where T : new()
    {
        var req = _client.Create("company/{companyId}/{method}");
        req.AddUrlSegment("companyId", companyId.ToString(CultureInfo.InvariantCulture));
        req.AddUrlSegment("method", companyMethod.GetDescription());

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        language ??= DefaultLanguage;
        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        var resp = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves a company by its TMDb ID.
    /// </summary>
    /// <param name="companyId">The TMDb ID of the company.</param>
    /// <param name="extraMethods">Additional data to include in the response using the append_to_response pattern.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>The company object.</returns>
    public async Task<Company?> GetCompanyAsync(int companyId, CompanyMethods extraMethods = CompanyMethods.Undefined, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("company/{companyId}");
        req.AddUrlSegment("companyId", companyId.ToString(CultureInfo.InvariantCulture));

        var appends = string.Join(
            ",",
            Enum.GetValues(typeof(CompanyMethods))
                                         .OfType<CompanyMethods>()
                                         .Except([CompanyMethods.Undefined])
                                         .Where(s => extraMethods.HasFlag(s))
                                         .Select(s => s.GetDescription()));

        if (appends != string.Empty)
        {
            req.AddParameter("append_to_response", appends);
        }

        // req.DateFormat = "yyyy-MM-dd";

        var resp = await req.GetOfT<Company>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Retrieves a paginated list of movies produced by a specific company.
    /// </summary>
    /// <param name="companyId">The TMDb ID of the company.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A search container with movies produced by the company.</returns>
    public async Task<SearchContainerWithId<SearchMovie>?> GetCompanyMoviesAsync(int companyId, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetCompanyMoviesAsync(companyId, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a paginated list of movies produced by a specific company with language option.
    /// </summary>
    /// <param name="companyId">The TMDb ID of the company.</param>
    /// <param name="language">The ISO 639-1 language code for the movie text. Defaults to the client's DefaultLanguage.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A search container with movies produced by the company.</returns>
    public async Task<SearchContainerWithId<SearchMovie>?> GetCompanyMoviesAsync(int companyId, string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetCompanyMethodInternal<SearchContainerWithId<SearchMovie>>(companyId, CompanyMethods.Movies, page, language, cancellationToken).ConfigureAwait(false);
    }
}
