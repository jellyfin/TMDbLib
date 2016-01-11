using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;
using TMDbLib.Rest;
using TMDbLib.Utilities;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Retrieve a specific episode using TMDb id of the associated tv show.
        /// </summary>
        /// <param name="tvShowId">TMDb id of the tv show the desired episode belongs to.</param>
        /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
        /// <param name="episodeNumber">The episode number of the episode you want to retrieve.</param>
        /// <param name="extraMethods">Enum flags indicating any additional data that should be fetched in the same request.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        public async Task<TvEpisode> GetTvEpisode(int tvShowId, int seasonNumber, int episodeNumber, TvEpisodeMethods extraMethods = TvEpisodeMethods.Undefined, string language = null)
        {
            if (extraMethods.HasFlag(TvEpisodeMethods.AccountStates))
                RequireSessionId(SessionType.UserSession);

            TmdbRestRequest req = _client2.Create("tv/{id}/season/{season_number}/episode/{episode_number}");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

            if (extraMethods.HasFlag(TvEpisodeMethods.AccountStates))
                AddSessionId(req, SessionType.UserSession);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(TvEpisodeMethods))
                                             .OfType<TvEpisodeMethods>()
                                             .Except(new[] { TvEpisodeMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            TmdbRestResponse<TvEpisode> resp = await req.ExecuteGet<TvEpisode>().ConfigureAwait(false);

            var item = await resp.GetDataObject();

            // No data to patch up so return
            if (item == null)
                return null;

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (item.Videos != null)
                item.Videos.Id = item.Id ?? 0;

            if (item.Credits != null)
                item.Credits.Id = item.Id ?? 0;

            if (item.Images != null)
                item.Images.Id = item.Id ?? 0;

            if (item.ExternalIds != null)
                item.ExternalIds.Id = item.Id ?? 0;

            if (item.AccountStates != null)
            {
                item.AccountStates.Id = item.Id ?? 0;
                // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
                CustomDeserialization.DeserializeAccountStatesRating(item.AccountStates, await resp.GetContent());
            }

            return item;
        }

        /// <summary>
        /// Returns a credits object for the specified episode.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
        /// <param name="episodeNumber">The episode number of the episode you want to retrieve information for.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        public async Task<Credits> GetTvEpisodeCredits(int tvShowId, int seasonNumber, int episodeNumber, string language = null)
        {
            return await GetTvEpisodeMethod<Credits>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Credits, dateFormat: "yyyy-MM-dd", language: language);
        }

        /// <summary>
        /// Retrieves all images all related to the season of specified episode.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
        /// <param name="episodeNumber">The episode number of the episode you want to retrieve information for.</param>
        /// <param name="language">
        /// If specified the api will attempt to return a localized result. ex: en,it,es.
        /// For images this means that the image might contain language specifc text
        /// </param>
        public async Task<StillImages> GetTvEpisodeImages(int tvShowId, int seasonNumber, int episodeNumber, string language = null)
        {
            return await GetTvEpisodeMethod<StillImages>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Images, language: language);
        }

        /// <summary>
        /// Returns an object that contains all known exteral id's for the specified episode.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
        /// <param name="episodeNumber">The episode number of the episode you want to retrieve information for.</param>
        public async Task<ExternalIds> GetTvEpisodeExternalIds(int tvShowId, int seasonNumber, int episodeNumber)
        {
            return await GetTvEpisodeMethod<ExternalIds>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.ExternalIds);
        }

        public async Task<ResultContainer<Video>> GetTvEpisodeVideos(int tvShowId, int seasonNumber, int episodeNumber)
        {
            return await GetTvEpisodeMethod<ResultContainer<Video>>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Videos);
        }

        public async Task<TvEpisodeAccountState> GetTvEpisodeAccountState(int tvShowId, int seasonNumber, int episodeNumber)
        {
            RequireSessionId(SessionType.UserSession);

            TmdbRestRequest req = _client2.Create("tv/{id}/season/{season_number}/episode/{episode_number}/account_states");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", TvEpisodeMethods.AccountStates.GetDescription());
            AddSessionId(req, SessionType.UserSession);

            TmdbRestResponse<TvEpisodeAccountState> response = await req.ExecuteGet<TvEpisodeAccountState>();

            TvEpisodeAccountState item = await response.GetDataObject();

            // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
            if (item != null)
            {
                CustomDeserialization.DeserializeAccountStatesRating(item, await response.GetContent());
            }

            return item;
        }

        public async Task<bool> TvEpisodeSetRating(int tvShowId, int seasonNumber, int episodeNumber, double rating)
        {
            RequireSessionId(SessionType.GuestSession);

            TmdbRestRequest req = _client2.Create("tv/{id}/season/{season_number}/episode/{episode_number}/rating");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

            AddSessionId(req);

            req.SetBody(new { value = rating });

            TmdbRestResponse<PostReply> response = await req.ExecutePost<PostReply>();

            // status code 1 = "Success"
            // status code 12 = "The item/record was updated successfully" - Used when an item was previously rated by the user
            PostReply item = await response.GetDataObject();

            // TODO: Original code had a check for item=null
            return item.StatusCode == 1 || item.StatusCode == 12;
        }

        public async Task<bool> TvEpisodeRemoveRating(int tvShowId, int seasonNumber, int episodeNumber)
        {
            RequireSessionId(SessionType.GuestSession);

            TmdbRestRequest req = _client2.Create("tv/{id}/season/{season_number}/episode/{episode_number}/rating");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

            AddSessionId(req);

            TmdbRestResponse<PostReply> response = await req.ExecuteDelete<PostReply>();

            // status code 13 = "The item/record was deleted successfully."
            PostReply item = await response.GetDataObject();

            // TODO: Original code had a check for item=null
            return item.StatusCode == 13;
        }

        public async Task<ChangesContainer> GetTvEpisodeChanges(int episodeId)
        {
            TmdbRestRequest req = _client2.Create("tv/episode/{id}/changes");
            req.AddUrlSegment("id", episodeId.ToString(CultureInfo.InvariantCulture));

            TmdbRestResponse<ChangesContainer> response = await req.ExecuteGet<ChangesContainer>();

            return response;
        }
        
        private async Task<T> GetTvEpisodeMethod<T>(int tvShowId, int seasonNumber, int episodeNumber, TvEpisodeMethods tvShowMethod, string dateFormat = null, string language = null) where T : new()
        {
            TmdbRestRequest req = _client2.Create("tv/{id}/season/{season_number}/episode/{episode_number}/{method}");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

            req.AddUrlSegment("method", tvShowMethod.GetDescription());

            // TODO: Dateformat?
            //if (dateFormat != null)
            //    req.DateFormat = dateFormat;

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            TmdbRestResponse<T> resp = await req.ExecuteGet<T>().ConfigureAwait(false);

            return resp;
        }
    }
}
