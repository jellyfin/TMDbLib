using System;
using RestSharp;
using TMDbLib.Objects.Changes;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        private T GetChanges<T>(string type, int page = -1, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            RestRequest req = new RestRequest("{type}/changes");
            req.AddUrlSegment("type", type);

            if (page >= 1)
                req.AddParameter("page", page);
            if (startDate.HasValue)
                req.AddParameter("start_date", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                req.AddParameter("end_date", endDate.Value.ToString("yyyy-MM-dd"));

            IRestResponse<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        public ChangesListContainer GetChangesMovies(int page = -1, DateTime? startDate = null, DateTime? endDate = null)
        {
            return GetChanges<ChangesListContainer>("movie", page, startDate, endDate);
        }

        public ChangesListContainer GetChangesPeople(int page = -1, DateTime? startDate = null, DateTime? endDate = null)
        {
            return GetChanges<ChangesListContainer>("person", page, startDate, endDate);
        }

    }
}