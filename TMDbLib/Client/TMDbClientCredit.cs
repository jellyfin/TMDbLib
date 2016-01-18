using System.Threading.Tasks;
using TMDbLib.Objects.Credit;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Credit> GetCredits(string id)
        {
            return await GetCredits(id, DefaultLanguage);
        }

        public async Task<Credit> GetCredits(string id, string language)
        {
            RestRequest req = _client2.Create("credit/{id}");

            if (!string.IsNullOrEmpty(language))
                req.AddParameter("language", language);

            req.AddUrlSegment("id", id);

            RestResponse<Credit> resp = await req.ExecuteGet<Credit>().ConfigureAwait(false);

            return resp;
        }
    }
}
