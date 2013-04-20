using RestSharp;
using TMDbLib.Objects.Lists;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public List GetList(string id)
        {
            RestRequest req = new RestRequest("list/{id}");
            req.AddUrlSegment("id", id);

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<List> resp = _client.Get<List>(req);

            return resp.Data;
        }
    }
}