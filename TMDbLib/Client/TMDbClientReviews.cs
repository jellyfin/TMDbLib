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
    /// <param name="language">The ISO 639-1 language code.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The review details.</returns>
    public async Task<Review?> GetReviewAsync(string reviewId, string? language = null, CancellationToken cancellationToken = default)
    {
        var request = _client.Create("review/{reviewId}");
        request.AddUrlSegment("reviewId", reviewId);

        if (language is not null)
        {
            request.AddQueryString("language", language);
        }

        var resp = await request.GetOfT<Review>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
