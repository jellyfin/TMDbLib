using System;
using System.Threading.Tasks;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Rest;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        private async Task<T> GetChanges<T>(string type, int page = 0, DateTime? startDate = null, DateTime? endDate = null) where T : new()
        {
            RestRequest req = _client.Create("{type}/changes");
            req.AddUrlSegment("type", type);

            if (page >= 1)
                req.AddParameter("page", page.ToString());
            if (startDate.HasValue)
                req.AddParameter("start_date", startDate.Value.ToString("yyyy-MM-dd"));
            if (endDate != null)
                req.AddParameter("end_date", endDate.Value.ToString("yyyy-MM-dd"));

            RestResponse<T> resp = await req.ExecuteGet<T>().ConfigureAwait(false);

            return resp;
        }

        /// <summary>
		/// Get a list of movie ids that have been edited. 
		/// By default we show the last 24 hours and only 100 items per page. 
		/// The maximum number of days that can be returned in a single request is 14. 
		/// You can then use the movie changes API to get the actual data that has been changed. (.GetMovieChangesAsync)
		/// </summary>
		/// <remarks>the change log system to support this was changed on October 5, 2012 and will only show movies that have been edited since.</remarks>
        public async Task<SearchContainer<ChangesListItem>> GetChangesMoviesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await GetChanges<SearchContainer<ChangesListItem>>("movie", page, startDate, endDate).ConfigureAwait(false);
        }

        /// <summary>
		/// Get a list of people ids that have been edited. 
		/// By default we show the last 24 hours and only 100 items per page. 
		/// The maximum number of days that can be returned in a single request is 14. 
		/// You can then use the person changes API to get the actual data that has been changed.(.GetPersonChangesAsync)
		/// </summary>
		/// <remarks>the change log system to support this was changed on October 5, 2012 and will only show people that have been edited since.</remarks>
        public async Task<SearchContainer<ChangesListItem>> GetChangesPeopleAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null)
        {
            return await GetChanges<SearchContainer<ChangesListItem>>("person", page, startDate, endDate).ConfigureAwait(false);
        }

        /// <summary>
		/// Get a list of TV show ids that have been edited. 
		/// By default we show the last 24 hours and only 100 items per page. 
		/// The maximum number of days that can be returned in a single request is 14. 
		/// You can then use the TV changes API to get the actual data that has been changed. (.GetTvShowChangesAsync)
		/// </summary>
		/// <remarks>
		/// the change log system to properly support TV was updated on May 13, 2014. 
		/// You'll likely only find the edits made since then to be useful in the change log system.
		/// </remarks>
		public async Task<SearchContainer<ChangesListItem>> GetChangesTvAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null)
		{
            return await GetChanges<SearchContainer<ChangesListItem>>("tv", page, startDate, endDate).ConfigureAwait(false);
		}
    }
}