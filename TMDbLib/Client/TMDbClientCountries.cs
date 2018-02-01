using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Countries;
using TMDbLib.Objects.Languages;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<List<Country>> GetCountriesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            RestRequest req = _client.Create("configuration/countries");

            RestResponse<List<Country>> response = await req.ExecuteGet<List<Country>>(cancellationToken).ConfigureAwait(false);

            return (await response.GetDataObject().ConfigureAwait(false));
        }
    }
}