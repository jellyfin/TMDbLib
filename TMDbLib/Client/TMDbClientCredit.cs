using System.Threading.Tasks;
using TMDbLib.Objects.Credit;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Credit> GetCreditsAsync(string id)
        {
            return await GetCreditsAsync(id, DefaultLanguage).ConfigureAwait(false);
        }

        public async Task<Credit> GetCreditsAsync(string id, string language)
        {
            RestRequest req = _client.Create("credit/{id}");

            if (!string.IsNullOrEmpty(language))
                req.AddParameter("language", language);

            req.AddUrlSegment("id", id);

            RestResponse<Credit> resp = await req.ExecuteGet<Credit>().ConfigureAwait(false);

            return resp;
        }
    }
}
