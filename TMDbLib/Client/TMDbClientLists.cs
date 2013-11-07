using RestSharp;
using TMDbLib.Objects.Lists;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public List GetList(string listId)
        {
            RestRequest req = new RestRequest("list/{listId}");
            req.AddUrlSegment("listId", listId);

            req.DateFormat = "yyyy-MM-dd";

            IRestResponse<List> resp = _client.Get<List>(req);

            return resp.Data;
        }
    }
}