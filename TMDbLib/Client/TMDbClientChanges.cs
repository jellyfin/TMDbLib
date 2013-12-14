using System;
using RestSharp;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        private T GetChanges<T>(string type, int page = 0, DateTime? startDate = null, DateTime? endDate = null) where T : new()
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

        public SearchContainer<ChangesListItem> GetChangesMovies(int page = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            return GetChanges<SearchContainer<ChangesListItem>>("movie", page, startDate, endDate);
        }

        public SearchContainer<ChangesListItem> GetChangesPeople(int page = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            return GetChanges<SearchContainer<ChangesListItem>>("person", page, startDate, endDate);
        }
    }
}