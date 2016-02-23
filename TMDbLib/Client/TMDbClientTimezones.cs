using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Timezones;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// FindAsync movies, people and tv shows by an external id.
        /// The following trypes can be found based on the specified external id's
        /// - Movies: Imdb
        /// - People: Imdb, FreeBaseMid, FreeBaseId, TvRage
        /// - TV Series: Imdb, FreeBaseMid, FreeBaseId, TvRage, TvDb
        /// </summary>
        /// <param name="source">The source the specified id belongs to</param>
        /// <param name="id">The id of the object you wish to located</param>
        /// <returns>A list of all objects in TMDb that matched your id</returns>
        public async Task<Timezones> GetTimezonesAsync()
        {
            RestRequest req = _client.Create("timezones/list");

            RestResponse<List<Dictionary<string, List<string>>>> resp = await req.ExecuteGet<List<Dictionary<string, List<string>>>>().ConfigureAwait(false);

            List<Dictionary<string, List<string>>> item = await resp.GetDataObject().ConfigureAwait(false);

            if (item == null)
                return null;

            Timezones result = new Timezones();
            result.List = new Dictionary<string, List<string>>();

            foreach (Dictionary<string, List<string>> dictionary in item)
            {
                KeyValuePair<string, List<string>> item1 = dictionary.First();

                result.List[item1.Key] = item1.Value;
            }

            return result;
        }
    }
}
