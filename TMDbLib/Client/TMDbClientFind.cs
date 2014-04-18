using RestSharp;
using RestSharp.Contrib;
using TMDbLib.Objects.Find;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Find movies, people and tv shows by an external id.
        /// The following trypes can be found based on the specified external id's
        /// - Movies: Imdb
        /// - People: Imdb, FreeBaseMid, FreeBaseId, TvRage
        /// - TV Series: Imdb, FreeBaseMid, FreeBaseId, TvRage, TvDb
        /// </summary>
        /// <param name="source">The source the specified id belongs to</param>
        /// <param name="id">The id of the object you wish to located</param>
        /// <returns>A list of all objects in TMDb that matched your id</returns>
        public FindContainer Find(FindExternalSource source, string id)
        {
            RestRequest req = new RestRequest("find/{id}");

            if (source == FindExternalSource.FreeBaseId || source == FindExternalSource.FreeBaseMid)
                // No url encoding for freebase Id's (they include /-slashes)
                req.AddUrlSegment("id", id);
            else
                req.AddUrlSegment("id", HttpUtility.UrlEncode(id));

            req.AddParameter("external_source", source.GetDescription());

            IRestResponse<FindContainer> resp = _client.Get<FindContainer>(req);

            return resp.Data;
        }
    }
}
