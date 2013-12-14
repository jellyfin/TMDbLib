using RestSharp;
using TMDbLib.Objects.Find;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public FindContainer Find(FindExternalSource source, string id)
        {
            RestRequest req = new RestRequest("find/{id}");
            req.AddUrlSegment("id", id);

            req.AddParameter("external_source", source.GetDescription());

            IRestResponse<FindContainer> resp = _client.Get<FindContainer>(req);

            return resp.Data;
        }
    }
}
