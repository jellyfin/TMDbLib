using System.Threading.Tasks;
using TMDbLib.Objects.Reviews;
using TMDbLib.Rest;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Review> GetReview(string reviewId)
        {
            TmdbRestRequest request  = _client2.Create("review/{reviewId}");
            request.AddUrlSegment("reviewId", reviewId);

            // TODO: Dateformat?
            //request.DateFormat = "yyyy-MM-dd";

            TmdbRestResponse<Review> resp = await request.ExecuteGet<Review>().ConfigureAwait(false);

            return resp;
        }
    }
}
