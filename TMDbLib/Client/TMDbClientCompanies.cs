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
    /// Gets a company by id.
    /// </summary>
    /// <param name="companyId">The TMDb id of the company.</param>
    /// <param name="extraMethods">Additional data to append to the response.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The company.</returns>
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

        var resp = await req.GetOfT<Company>(cancellationToken).ConfigureAwait(false);

        return resp;
    }

    /// <summary>
    /// Gets the movies produced by a company.
    /// </summary>
    /// <param name="companyId">The TMDb id of the company.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The company's movies.</returns>
    public async Task<SearchContainerWithId<SearchMovie>?> GetCompanyMoviesAsync(int companyId, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetCompanyMoviesAsync(companyId, DefaultLanguage, page, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the movies produced by a company in a specific language.
    /// </summary>
    /// <param name="companyId">The TMDb id of the company.</param>
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The company's movies.</returns>
    public async Task<SearchContainerWithId<SearchMovie>?> GetCompanyMoviesAsync(int companyId, string? language, int page = 0, CancellationToken cancellationToken = default)
    {
        return await GetCompanyMethodInternal<SearchContainerWithId<SearchMovie>>(companyId, CompanyMethods.Movies, page, language, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the alternative names of a company.
    /// </summary>
    /// <param name="companyId">The TMDb id of the company.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The company's alternative names.</returns>
    public async Task<AlternativeNames?> GetCompanyAlternativeNamesAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await GetCompanyMethodInternal<AlternativeNames>(companyId, CompanyMethods.AlternativeNames, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the logos for a company.
    /// </summary>
    /// <param name="companyId">The TMDb id of the company.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The company's logos.</returns>
    public async Task<ImagesWithId?> GetCompanyImagesAsync(int companyId, CancellationToken cancellationToken = default)
    {
        return await GetCompanyMethodInternal<ImagesWithId>(companyId, CompanyMethods.Images, cancellationToken: cancellationToken).ConfigureAwait(false);
    }
}
