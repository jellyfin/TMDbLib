using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Reviews;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    /// <summary>
    /// Retrieves detailed information about a review.
    /// </summary>
    /// <param name="reviewId">The id of the review.</param>
    /// <param name="language">Language to localize the results in.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>The detailed review information.</returns>
    public async Task<Review?> GetReviewAsync(string reviewId, string? language = null, CancellationToken cancellationToken = default)
    {
        var request = _client.Create("review/{reviewId}");
        request.AddUrlSegment("reviewId", reviewId);

        if (language is not null)
        {
            request.AddQueryString("language", language);
        }

        // TODO: Dateformat?
        // request.DateFormat = "yyyy-MM-dd";

        var resp = await request.GetOfT<Review>(cancellationToken).ConfigureAwait(false);

        return resp;
    }
}
