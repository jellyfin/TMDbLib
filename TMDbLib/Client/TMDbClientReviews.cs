using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Reviews;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Gets the details of a review.
    /// </summary>
    /// <param name="reviewId">The id of the review.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The review details.</returns>
    public async Task<Review?> GetReviewAsync(string reviewId, CancellationToken cancellationToken = default)
    {
        var request = _client.Create("review/{reviewId}");
        request.AddUrlSegment("reviewId", reviewId);

        var resp = await request.GetOfT<Review>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
