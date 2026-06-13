using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Configuration;
using TMDbLib.Objects.Countries;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Timezones;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Gets the system-wide TMDb API configuration.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The API configuration including image sizes and base URLs.</returns>
    public async Task<APIConfiguration?> GetAPIConfiguration(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("configuration");

        using var response = await req.Get<APIConfiguration>(cancellationToken).ConfigureAwait(false);

        return await response.GetDataObject().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of supported countries.
    /// </summary>
    /// <param name="language">Optional ISO 639-1 language code to localize the <c>native_name</c> field.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The supported countries.</returns>
    public async Task<List<Country>?> GetCountriesAsync(string? language = null, CancellationToken cancellationToken = default)
    {
        var req = _client.Create("configuration/countries");

        if (!string.IsNullOrWhiteSpace(language))
        {
            req.AddParameter("language", language);
        }

        using var response = await req.Get<List<Country>>(cancellationToken).ConfigureAwait(false);

        return await response.GetDataObject().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of supported languages.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The supported languages.</returns>
    public async Task<List<Language>?> GetLanguagesAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("configuration/languages");

        using var response = await req.Get<List<Language>>(cancellationToken).ConfigureAwait(false);

        return await response.GetDataObject().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the list of primary translation language codes.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>ISO 639-1 codes for primary translations.</returns>
    public async Task<List<string>?> GetPrimaryTranslationsAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("configuration/primary_translations");

        using var response = await req.Get<List<string>>(cancellationToken).ConfigureAwait(false);

        return await response.GetDataObject().ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the timezones grouped by country.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Timezones keyed by ISO 3166-1 country code.</returns>
    public async Task<Timezones?> GetTimezonesAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("configuration/timezones");

        using var resp = await req.Get<List<TimezoneEntry>>(cancellationToken).ConfigureAwait(false);

        var item = await resp.GetDataObject().ConfigureAwait(false);

        if (item is null)
        {
            return null;
        }

        var result = new Timezones
        {
            List = []
        };

        foreach (var entry in item)
        {
            if (string.IsNullOrEmpty(entry.Iso_3166_1))
            {
                continue;
            }

            result.List[entry.Iso_3166_1!] = entry.Zones ?? [];
        }

        return result;
    }

    /// <summary>
    /// Gets the list of jobs grouped by department.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The valid jobs and their departments.</returns>
    public async Task<List<Job>?> GetJobsAsync(CancellationToken cancellationToken = default)
    {
        var req = _client.Create("configuration/jobs");

        using var response = await req.Get<List<Job>>(cancellationToken).ConfigureAwait(false);

        return await response.GetDataObject().ConfigureAwait(false);
    }
}
