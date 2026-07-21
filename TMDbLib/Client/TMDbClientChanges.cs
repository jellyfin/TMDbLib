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
    private async Task<T?> GetChangesInternal<T>(string type, int page = 0, int? id = null, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
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

        var req = _client.Create(resource);
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

        var res = await req.GetOfT<T>(cancellationToken).ConfigureAwait(false);

        if (res is SearchContainer<ChangesListItem> asSearch && asSearch.Results is not null)
        {
            // https://github.com/jellyfin/TMDbLib/issues/296
            asSearch.Results.RemoveAll(s => s.Id == 0);
        }

        return res;
    }

    /// <summary>
    /// Gets a list of movie ids that have been edited.
    /// </summary>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="startDate">The start date filter.</param>
    /// <param name="endDate">The end date filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Edited movie ids. Defaults to last 24 hours, max 14 days, 100 items per page.</returns>
    /// <remarks>The change log was updated on October 5, 2012 and only shows movies edited since then.</remarks>
    public async Task<SearchContainer<ChangesListItem>?> GetMoviesChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        return await GetChangesInternal<SearchContainer<ChangesListItem>>("movie", page, startDate: startDate, endDate: endDate, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a list of person ids that have been edited.
    /// </summary>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="startDate">The start date filter.</param>
    /// <param name="endDate">The end date filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Edited person ids. Defaults to last 24 hours, max 14 days, 100 items per page.</returns>
    /// <remarks>The change log was updated on October 5, 2012 and only shows people edited since then.</remarks>
    public async Task<SearchContainer<ChangesListItem>?> GetPeopleChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        return await GetChangesInternal<SearchContainer<ChangesListItem>>("person", page, startDate: startDate, endDate: endDate, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets a list of TV show ids that have been edited.
    /// </summary>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="startDate">The start date filter.</param>
    /// <param name="endDate">The end date filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Edited TV show ids. Defaults to last 24 hours, max 14 days, 100 items per page.</returns>
    /// <remarks>The TV change log was updated on May 13, 2014; only edits since then are useful.</remarks>
    public async Task<SearchContainer<ChangesListItem>?> GetTvChangesAsync(int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        return await GetChangesInternal<SearchContainer<ChangesListItem>>("tv", page, startDate: startDate, endDate: endDate, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets the changes for a movie.
    /// </summary>
    /// <param name="movieId">The TMDb id of the movie.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="startDate">The start date filter.</param>
    /// <param name="endDate">The end date filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The movie's changes.</returns>
    public async Task<IList<Change>?> GetMovieChangesAsync(int movieId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var changesContainer = await GetChangesInternal<ChangesContainer>("movie", page, movieId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer?.Changes;
    }

    /// <summary>
    /// Gets the changes for a person.
    /// </summary>
    /// <param name="personId">The TMDb id of the person.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="startDate">The start date filter.</param>
    /// <param name="endDate">The end date filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The person's changes.</returns>
    public async Task<IList<Change>?> GetPersonChangesAsync(int personId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var changesContainer = await GetChangesInternal<ChangesContainer>("person", page, personId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer?.Changes;
    }

    /// <summary>
    /// Gets the changes for a TV show.
    /// </summary>
    /// <param name="tvShowId">The TMDb id of the TV show.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="startDate">The start date filter.</param>
    /// <param name="endDate">The end date filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The TV show's changes.</returns>
    public async Task<IList<Change>?> GetTvShowChangesAsync(int tvShowId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var changesContainer = await GetChangesInternal<ChangesContainer>("tv", page, tvShowId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer?.Changes;
    }

    /// <summary>
    /// Gets the changes for a TV season.
    /// </summary>
    /// <param name="seasonId">The TMDb id of the TV season.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="startDate">The start date filter.</param>
    /// <param name="endDate">The end date filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The season's changes.</returns>
    public async Task<IList<Change>?> GetTvSeasonChangesAsync(int seasonId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var changesContainer = await GetChangesInternal<ChangesContainer>("tv/season", page, seasonId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer?.Changes;
    }

    /// <summary>
    /// Gets the changes for a TV episode.
    /// </summary>
    /// <param name="episodeId">The TMDb id of the TV episode.</param>
    /// <param name="page">The page number. Use 0 for the default.</param>
    /// <param name="startDate">The start date filter.</param>
    /// <param name="endDate">The end date filter.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The episode's changes.</returns>
    public async Task<IList<Change>?> GetTvEpisodeChangesAsync(int episodeId, int page = 0, DateTime? startDate = null, DateTime? endDate = null, CancellationToken cancellationToken = default)
    {
        var changesContainer = await GetChangesInternal<ChangesContainer>("tv/episode", page, episodeId, startDate, endDate, cancellationToken).ConfigureAwait(false);
        return changesContainer?.Changes;
    }
}
