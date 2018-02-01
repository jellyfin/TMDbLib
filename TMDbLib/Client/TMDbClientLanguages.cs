using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Timezones;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<List<Language>> GetLanguagesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("configuration/languages");

            RestResponse<List<Language>> response = await req.ExecuteGet<List<Language>>(cancellationToken).ConfigureAwait(false);

            return (await response.GetDataObject().ConfigureAwait(false));
        }
    }
}