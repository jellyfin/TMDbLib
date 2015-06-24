using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Credit;

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
            RestRequest req = new RestRequest("credit/{id}");

            if (!string.IsNullOrEmpty(language))
                req.AddParameter("language", language);

            req.AddUrlSegment("id", id);

            IRestResponse<Credit> resp = await _client.ExecuteGetTaskAsync<Credit>(req).ConfigureAwait(false);

            return resp.Data;
        }
    }
}
