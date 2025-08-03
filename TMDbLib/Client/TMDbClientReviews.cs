using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Reviews;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    public async Task<Review> GetReviewAsync(string reviewId, string language = null, CancellationToken cancellationToken = default)
    {
        RestRequest request = _client.Create("review/{reviewId}");
        request.AddUrlSegment("reviewId", reviewId);

        if (language is not null)
        {
            request.AddQueryString("language", language);
        }

        // TODO: Dateformat?
        // request.DateFormat = "yyyy-MM-dd";

        Review resp = await request.GetOfT<Review>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
