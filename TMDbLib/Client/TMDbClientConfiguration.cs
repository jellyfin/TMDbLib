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

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<APIConfiguration> GetAPIConfiguration(CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("configuration");

            RestResponse<APIConfiguration> response = await req.ExecuteGet<APIConfiguration>(cancellationToken).ConfigureAwait(false);

            return (await response.GetDataObject().ConfigureAwait(false));
        }

        public async Task<List<Country>> GetCountriesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("configuration/countries");

            RestResponse<List<Country>> response = await req.ExecuteGet<List<Country>>(cancellationToken).ConfigureAwait(false);

            return (await response.GetDataObject().ConfigureAwait(false));
        }

        public async Task<List<Language>> GetLanguagesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("configuration/languages");

            RestResponse<List<Language>> response = await req.ExecuteGet<List<Language>>(cancellationToken).ConfigureAwait(false);

            return (await response.GetDataObject().ConfigureAwait(false));
        }
        
        public async Task<List<string>> GetPrimaryTranslationsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("configuration/primary_translations");

            RestResponse<List<string>> response = await req.ExecuteGet<List<string>>(cancellationToken).ConfigureAwait(false);

            return (await response.GetDataObject().ConfigureAwait(false));
        }

        /// <summary>
        /// FindAsync movies, people and tv shows by an external id.
        /// The following trypes can be found based on the specified external id's
        /// - Movies: Imdb
        /// - People: Imdb, FreeBaseMid, FreeBaseId, TvRage
        /// - TV Series: Imdb, FreeBaseMid, FreeBaseId, TvRage, TvDb
        /// </summary>
        /// <param name="source">The source the specified id belongs to</param>
        /// <param name="id">The id of the object you wish to located</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A list of all objects in TMDb that matched your id</returns>
        public async Task<Timezones> GetTimezonesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("timezones/list");

            RestResponse<List<Dictionary<string, List<string>>>> resp = await req.ExecuteGet<List<Dictionary<string, List<string>>>>(cancellationToken).ConfigureAwait(false);

            List<Dictionary<string, List<string>>> item = await resp.GetDataObject().ConfigureAwait(false);

            if (item == null)
                return null;

            Timezones result = new Timezones();
            result.List = new Dictionary<string, List<string>>();

            foreach (Dictionary<string, List<string>> dictionary in item)
            {
                KeyValuePair<string, List<string>> item1 = dictionary.First();

                result.List[item1.Key] = item1.Value;
            }

            return result;
        }

        /// <summary>
        /// Retrieves a list of departments and positions within
        /// </summary>
        /// <returns>Valid jobs and their departments</returns>
        public async Task<List<Job>> GetJobsAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("configuration/jobs");

            RestResponse<List<Job>> response = await req.ExecuteGet<List<Job>>(cancellationToken).ConfigureAwait(false);

            return (await response.GetDataObject().ConfigureAwait(false));
        }
    }
}
