using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Rest;

namespace TMDbLib.Client;

public partial class TMDbClient
{
    private async Task<T> GetChangesInternal<T>(string type, int page = 0, int? id = null, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        string resource;
        if (id.HasValue)
        {
            resource = "{type}/{id}/changes";
        }
        else
        {
            resource = "{type}/changes";
        }

        RestRequest req = _client.Create(resource);
        req.AddUrlSegment("type", type);

        if (id.HasValue)
        {
            req.AddUrlSegment("id", id.Value.ToString(CultureInfo.InvariantCulture));
        }

        if (page >= 1)
        {
            req.AddParameter("page", page.ToString(CultureInfo.InvariantCulture));
        }

        if (startDate.HasValue)
        {
            req.AddParameter("start_date", startDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        if (endDate is not null)
        {
            req.AddParameter("end_date", endDate.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
        }

        using RestResponse<T> resp = await req.Get<T>(cancellationToken).ConfigureAwait(false);
        T res = await resp.GetDataObject().ConfigureAwait(false);

        if (res is SearchContainer<ChangesListItem> asSearch)
        {
            // https://github.com/jellyfin/TMDbLib/issues/296
            asSearch.Results.RemoveAll(s => s.Id == 0);
        }

        return res;
    }

    /// <summary>
    /// Get a list of movie ids that have been edited.
    /// By default we show the last 24 hours and only 100 items per page.
    /// The maximum number of days that can be returned in a single request is 14.
    /// You can then use the movie changes API to get the actual data that has been changed. (.GetMovieChangesAsync).
    /// </summary>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="startDate">The start date for filtering changes.</param>
    /// <param name="endDate">The end date for filtering changes.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A search container with a list of movie IDs that have been edited.</returns>
    /// <remarks>the change log system to support this was changed on October 5, 2012 and will only show movies that have been edited since.</remarks>
    public async Task<SearchContainer<ChangesListItem>> GetMoviesChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        return await GetChangesInternal<SearchContainer<ChangesListItem>>("movie", page, startDate: startDate, endDate: endDate, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Get a list of people ids that have been edited.
    /// By default we show the last 24 hours and only 100 items per page.
    /// The maximum number of days that can be returned in a single request is 14.
    /// You can then use the person changes API to get the actual data that has been changed.(.GetPersonChangesAsync).
    /// </summary>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="startDate">The start date for filtering changes.</param>
    /// <param name="endDate">The end date for filtering changes.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A search container with a list of person IDs that have been edited.</returns>
    /// <remarks>the change log system to support this was changed on October 5, 2012 and will only show people that have been edited since.</remarks>
    public async Task<SearchContainer<ChangesListItem>> GetPeopleChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        return await GetChangesInternal<SearchContainer<ChangesListItem>>("person", page, startDate: startDate, endDate: endDate, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Get a list of TV show ids that have been edited.
    /// By default we show the last 24 hours and only 100 items per page.
    /// The maximum number of days that can be returned in a single request is 14.
    /// You can then use the TV changes API to get the actual data that has been changed. (.GetTvShowChangesAsync).
    /// </summary>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="startDate">The start date for filtering changes.</param>
    /// <param name="endDate">The end date for filtering changes.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A search container with a list of TV show IDs that have been edited.</returns>
    /// <remarks>
    /// the change log system to properly support TV was updated on May 13, 2014.
    /// You'll likely only find the edits made since then to be useful in the change log system.
    /// </remarks>
    public async Task<SearchContainer<ChangesListItem>> GetTvChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        return await GetChangesInternal<SearchContainer<ChangesListItem>>("tv", page, startDate: startDate, endDate: endDate, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Retrieves a list of changes made to a specific movie.
    /// </summary>
    /// <param name="movieId">The TMDb ID of the movie.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="startDate">The start date for filtering changes.</param>
    /// <param name="endDate">The end date for filtering changes.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of changes made to the movie.</returns>
    public async Task<IList<Change>> GetMovieChangesAsync(int movieId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        ChangesContainer changesContainer = await GetChangesInternal<ChangesContainer>("movie", page, movieId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer.Changes;
    }

    /// <summary>
    /// Retrieves a list of changes made to a specific person.
    /// </summary>
    /// <param name="personId">The TMDb ID of the person.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="startDate">The start date for filtering changes.</param>
    /// <param name="endDate">The end date for filtering changes.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of changes made to the person.</returns>
    public async Task<IList<Change>> GetPersonChangesAsync(int personId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        ChangesContainer changesContainer = await GetChangesInternal<ChangesContainer>("person", page, personId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer.Changes;
    }

    /// <summary>
    /// Retrieves a list of changes made to a specific TV show.
    /// </summary>
    /// <param name="tvShowId">The TMDb ID of the TV show.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="startDate">The start date for filtering changes.</param>
    /// <param name="endDate">The end date for filtering changes.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of changes made to the TV show.</returns>
    public async Task<IList<Change>> GetTvShowChangesAsync(int tvShowId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        ChangesContainer changesContainer = await GetChangesInternal<ChangesContainer>("tv", page, tvShowId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer.Changes;
    }

    /// <summary>
    /// Retrieves a list of changes made to a specific TV season.
    /// </summary>
    /// <param name="seasonId">The TMDb ID of the TV season.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="startDate">The start date for filtering changes.</param>
    /// <param name="endDate">The end date for filtering changes.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of changes made to the TV season.</returns>
    public async Task<IList<Change>> GetTvSeasonChangesAsync(int seasonId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        ChangesContainer changesContainer = await GetChangesInternal<ChangesContainer>("tv/season", page, seasonId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer.Changes;
    }

    /// <summary>
    /// Retrieves a list of changes made to a specific TV episode.
    /// </summary>
    /// <param name="episodeId">The TMDb ID of the TV episode.</param>
    /// <param name="page">The page of results to retrieve. Use 0 for the default page.</param>
    /// <param name="startDate">The start date for filtering changes.</param>
    /// <param name="endDate">The end date for filtering changes.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the operation.</param>
    /// <returns>A list of changes made to the TV episode.</returns>
    public async Task<IList<Change>> GetTvEpisodeChangesAsync(int episodeId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        ChangesContainer changesContainer = await GetChangesInternal<ChangesContainer>("tv/episode", page, episodeId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer.Changes;
    }
}
