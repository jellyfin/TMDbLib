using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Reviews;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public async Task<Review> GetReview(string reviewId)
        {
            RestRequest request = new RestRequest("review/{reviewId}");
            request.AddUrlSegment("reviewId", reviewId);

            request.DateFormat = "yyyy-MM-dd";

            IRestResponse<Review> resp = await _client.ExecuteGetTaskAsync<Review>(request).ConfigureAwait(false);

            return resp.Data;
        }
    }
}
