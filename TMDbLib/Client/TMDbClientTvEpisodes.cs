using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
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
        public TvEpisode GetTvEpisode(int tvShowId, int seasonNumber, int episodeNumber, TvEpisodeMethods extraMethods = TvEpisodeMethods.Undefined, string language = null)
        {
            RestRequest req = new RestRequest("tv/{id}/season/{season_number}/episode/{episode_number}");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

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

            IRestResponse<TvEpisode> response = _client.Get<TvEpisode>(req);

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

            return response.Data;
        }

        /// <summary>
        /// Returns a credits object for the specified episode.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
        /// <param name="episodeNumber">The episode number of the episode you want to retrieve information for.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        public Credits GetTvEpisodeCredits(int tvShowId, int seasonNumber, int episodeNumber, string language = null)
        {
            return GetTvEpisodeMethod<Credits>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Credits, dateFormat: "yyyy-MM-dd", language: language);
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
        public StillImages GetTvEpisodeImages(int tvShowId, int seasonNumber, int episodeNumber, string language = null)
        {
            return GetTvEpisodeMethod<StillImages>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Images, language: language);
        }

        /// <summary>
        /// Returns an object that contains all known exteral id's for the specified episode.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season the episode belongs to. Note use 0 for specials.</param>
        /// <param name="episodeNumber">The episode number of the episode you want to retrieve information for.</param>
        public ExternalIds GetTvEpisodeExternalIds(int tvShowId, int seasonNumber, int episodeNumber)
        {
            return GetTvEpisodeMethod<ExternalIds>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.ExternalIds);
        }

        public ResultContainer<Video> GetTvEpisodeVideos(int tvShowId, int seasonNumber, int episodeNumber)
        {
            return GetTvEpisodeMethod<ResultContainer<Video>>(tvShowId, seasonNumber, episodeNumber, TvEpisodeMethods.Videos);
        }

        public TvEpisodeAccountState GetTvEpisodeAccountState(int tvShowId, int seasonNumber, int episodeNumber)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest req = new RestRequest("tv/{id}/season/{season_number}/episode/{episode_number}/account_states");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", MovieMethods.AccountStates.GetDescription());
            req.AddParameter("session_id", SessionId);

            IRestResponse<TvEpisodeAccountState> response = _client.Get<TvEpisodeAccountState>(req);

            // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
            if (response.Data != null)
            {
                DeserializeAccountStatesRating(response.Data, response.Content);
            }

            return response.Data;
        }

        public bool TvEpisodeSetRating(int tvShowId, int seasonNumber, int episodeNumber, double rating)
        {
            RequireSessionId(SessionType.GuestSession);

            RestRequest req = new RestRequest("tv/{id}/season/{season_number}/episode/{episode_number}/rating") { RequestFormat = DataFormat.Json };
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("episode_number", episodeNumber.ToString(CultureInfo.InvariantCulture));

            if (SessionType == SessionType.UserSession)
                req.AddParameter("session_id", SessionId, ParameterType.QueryString);
            else
                req.AddParameter("guest_session_id", SessionId, ParameterType.QueryString);

            req.AddBody(new { value = rating });

            IRestResponse<PostReply> response = _client.Post<PostReply>(req);

            // status code 1 = "Success"
            // status code 12 = "The item/record was updated successfully" - Used when an item was previously rated by the user
            return response.Data != null && (response.Data.StatusCode == 1 || response.Data.StatusCode == 12);
        }

        public ChangesContainer GetTvEpisodeChanges(int episodeId)
        {
            RestRequest req = new RestRequest("tv/episode/{id}/changes");
            req.AddUrlSegment("id", episodeId.ToString(CultureInfo.InvariantCulture));

            IRestResponse<ChangesContainer> response = _client.Get<ChangesContainer>(req);

            return response.Data;
        }

        private T GetTvEpisodeMethod<T>(int tvShowId, int seasonNumber, int episodeNumber, TvEpisodeMethods tvShowMethod, string dateFormat = null, string language = null) where T : new()
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

            IRestResponse<T> resp = _client.Get<T>(req);

            return resp.Data;
        }

        private static void DeserializeAccountStatesRating(TvEpisodeAccountState accountState, string responseContent)
        {
            const string selector = @"""rated"":{""value"":(?<value>\d+(?:\.\d{1,2}))}";
            Regex regex = new Regex(selector, RegexOptions.IgnoreCase);
            Match match = regex.Match(responseContent);
            if (match.Success)
            {
                accountState.Rating = Double.Parse(match.Groups["value"].Value,
                    CultureInfo.InvariantCulture.NumberFormat);
            }
        }
    }
}
