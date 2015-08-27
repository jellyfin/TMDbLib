using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.TvShows;
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

            RestRequest request = new RestRequest("tv/{id}/season/{season_number}/episode/{episode_number}");
            request.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            request.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

            if (extraMethods.HasFlag(TvEpisodeMethods.AccountStates))
                AddSessionId(request, SessionType.UserSession);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(TvEpisodeMethods))
                                             .OfType<TvEpisodeMethods>()
                                             .Except(new[] { TvEpisodeMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                request.AddParameter("append_to_response", appends);

            IRestResponse<TvEpisode> response = await _client.ExecuteGetTaskAsync<TvEpisode>(request).ConfigureAwait(false);

            // No data to patch up so return
            if (response.Data == null)
                return null;

            // Patch up data, so that the end user won't notice that we share objects between request-types.
            if (response.Data.Videos != null)
                response.Data.Videos.Id = response.Data.Id ?? 0;

            if (response.Data.Credits != null)
                response.Data.Credits.Id = response.Data.Id ?? 0;

            if (response.Data.Images != null)
                response.Data.Images.Id = response.Data.Id ?? 0;

            if (response.Data.ExternalIds != null)
                response.Data.ExternalIds.Id = response.Data.Id ?? 0;

            if (response.Data.AccountStates != null)
            {
                response.Data.AccountStates.Id = response.Data.Id ?? 0;
                // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
                CustomDeserialization.DeserializeAccountStatesRating(response.Data.AccountStates, response.Content);
            }

            return response.Data;
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

            RestRequest req = new RestRequest("tv/{id}/season/{season_number}/episode/{episode_number}/account_states");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", TvEpisodeMethods.AccountStates.GetDescription());
            AddSessionId(req, SessionType.UserSession);

            IRestResponse<TvEpisodeAccountState> response = await _client.ExecuteGetTaskAsync<TvEpisodeAccountState>(req);

            // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
            if (response.Data != null)
            {
                CustomDeserialization.DeserializeAccountStatesRating(response.Data, response.Content);
            }

            return response.Data;
        }

        public async Task<bool> TvEpisodeSetRating(int tvShowId, int seasonNumber, int episodeNumber, double rating)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest req = new RestRequest("tv/{id}/season/{season_number}/episode/{episode_number}/rating") { RequestFormat = DataFormat.Json };
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

            AddSessionId(req);

            req.AddBody(new { value = rating });

            IRestResponse<PostReply> response = await _client.ExecutePostTaskAsync<PostReply>(req);

            // status code 1 = "Success"
            // status code 12 = "The item/record was updated successfully" - Used when an item was previously rated by the user
            return response.Data != null && (response.Data.StatusCode == 1 || response.Data.StatusCode == 12);
        }

        public async Task<bool> TvEpisodeRemoveRating(int tvShowId, int seasonNumber, int episodeNumber)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest req = new RestRequest("tv/{id}/season/{season_number}/episode/{episode_number}/rating");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

            AddSessionId(req);

            IRestResponse<PostReply> response = await _client.ExecuteDeleteTaskAsync<PostReply>(req);

            // status code 13 = "The item/record was deleted successfully."
            return response.Data != null && response.Data.StatusCode == 13;
        }

        public async Task<ChangesContainer> GetTvEpisodeChanges(int episodeId)
        {
            RestRequest req = new RestRequest("tv/episode/{id}/changes");
            req.AddUrlSegment("id", episodeId.ToString(CultureInfo.InvariantCulture));

            IRestResponse<ChangesContainer> response = await _client.ExecuteGetTaskAsync<ChangesContainer>(req);

            return response.Data;
        }
        
        private async Task<T> GetTvEpisodeMethod<T>(int tvShowId, int seasonNumber, int episodeNumber, TvEpisodeMethods tvShowMethod, string dateFormat = null, string language = null) where T : new()
        {
            RestRequest req = new RestRequest("tv/{id}/season/{season_number}/episode/{episode_number}/{method}");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

            req.AddUrlSegment("method", tvShowMethod.GetDescription());

            if (dateFormat != null)
                req.DateFormat = dateFormat;

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            IRestResponse<T> resp = await _client.ExecuteGetTaskAsync<T>(req).ConfigureAwait(false);

            return resp.Data;
        }
    }
}
