using RestSharp;
using TMDbLib.Objects.Credit;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Credit GetCredits(string id)
        {
            return GetCredits(id, DefaultLanguage);
        }

        public Credit GetCredits(string id, string language)
        {
            RestRequest req = new RestRequest("credit/{id}");

            if (!string.IsNullOrEmpty(language))
                req.AddParameter("language", language);

            req.AddUrlSegment("id", id);

            IRestResponse<Credit> resp = _client.Get<Credit>(req);

            return resp.Data;
        }
    }
}
