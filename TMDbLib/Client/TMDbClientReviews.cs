using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using TMDbLib.Objects.Reviews;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        public Review GetReview(string reviewId)
        {
            RestRequest request = new RestRequest("review/{reviewId}");
            request.AddUrlSegment("reviewId", reviewId);

            request.DateFormat = "yyyy-MM-dd";

            IRestResponse<Review> resp = _client.Get<Review>(request);

            return resp.Data;
        }
    }
}
