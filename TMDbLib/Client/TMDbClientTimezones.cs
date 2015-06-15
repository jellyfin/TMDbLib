using System.Collections.Generic;
using System.Linq;
using RestSharp;
using TMDbLib.Objects.Timezones;

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
        public Timezones GetTimezones()
        {
            RestRequest req = new RestRequest("timezones/list");

            IRestResponse<List<Dictionary<string, List<string>>>> resp = _client.Get<List<Dictionary<string, List<string>>>>(req);

            if (resp.Data == null)
                return null;

            Timezones result = new Timezones();
            result.List = new Dictionary<string, List<string>>();

            foreach (Dictionary<string, List<string>> dictionary in resp.Data)
            {
                KeyValuePair<string, List<string>> item1 = dictionary.First();

                result.List[item1.Key] = item1.Value;
            }

            return result;
        }
    }
}
