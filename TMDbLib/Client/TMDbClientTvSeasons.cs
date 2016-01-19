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
        /// Retrieve a season for a specifc tv Show by id.
        /// </summary>
        /// <param name="tvShowId">TMDb id of the tv show the desired season belongs to.</param>
        /// <param name="seasonNumber">The season number of the season you want to retrieve. Note use 0 for specials.</param>
        /// <param name="extraMethods">Enum flags indicating any additional data that should be fetched in the same request.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        /// <returns>The requested season for the specified tv show</returns>
        public async Task<TvSeason> GetTvSeason(int tvShowId, int seasonNumber, TvSeasonMethods extraMethods = TvSeasonMethods.Undefined, string language = null)
        {
            if (extraMethods.HasFlag(TvSeasonMethods.AccountStates))
                RequireSessionId(SessionType.UserSession);

            RestRequest req = _client.Create("tv/{id}/season/{season_number}");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));

            if (extraMethods.HasFlag(TvSeasonMethods.AccountStates))
                AddSessionId(req, SessionType.UserSession);

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(TvSeasonMethods))
                                             .OfType<TvSeasonMethods>()
                                             .Except(new[] { TvSeasonMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            RestResponse<TvSeason> response = await req.ExecuteGet<TvSeason>().ConfigureAwait(false);

            TvSeason item = await response.GetDataObject().ConfigureAwait(false);

            // Nothing to patch up
            if (item == null)
                return null;

            if (item.Episodes != null)
                item.EpisodeCount = item.Episodes.Count;

            if (item.Credits != null)
                item.Credits.Id = item.Id ?? 0;

            if (item.ExternalIds != null)
                item.ExternalIds.Id = item.Id ?? 0;

            if (item.AccountStates != null)
            {
                item.AccountStates.Id = item.Id ?? 0;
                // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
                CustomDeserialization.DeserializeAccountStatesRating(item.AccountStates, await response.GetContent().ConfigureAwait(false));
            }

            return item;
        }

        /// <summary>
        /// Returns a credits object for the season of the tv show associated with the provided TMDb id.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season you want to retrieve information for. Note use 0 for specials.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        public async Task<Credits> GetTvSeasonCredits(int tvShowId, int seasonNumber, string language = null)
        {
            return await GetTvSeasonMethod<Credits>(tvShowId, seasonNumber, TvSeasonMethods.Credits, dateFormat: "yyyy-MM-dd", language: language).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves all images all related to the season of specified tv show.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season you want to retrieve information for. Note use 0 for specials.</param>
        /// <param name="language">
        /// If specified the api will attempt to return a localized result. ex: en,it,es.
        /// For images this means that the image might contain language specifc text
        /// </param>
        public async Task<PosterImages> GetTvSeasonImages(int tvShowId, int seasonNumber, string language = null)
        {
            return await GetTvSeasonMethod<PosterImages>(tvShowId, seasonNumber, TvSeasonMethods.Images, language: language).ConfigureAwait(false);
        }

        public async Task<ResultContainer<Video>> GetTvSeasonVideos(int tvShowId, int seasonNumber, string language = null)
        {
            return await GetTvSeasonMethod<ResultContainer<Video>>(tvShowId, seasonNumber, TvSeasonMethods.Videos, language: language).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns an object that contains all known exteral id's for the season of the tv show related to the specified TMDB id.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season you want to retrieve information for. Note use 0 for specials.</param>
        public async Task<ExternalIds> GetTvSeasonExternalIds(int tvShowId, int seasonNumber)
        {
            return await GetTvSeasonMethod<ExternalIds>(tvShowId, seasonNumber, TvSeasonMethods.ExternalIds).ConfigureAwait(false);
        }

        public async Task<ResultContainer<TvEpisodeAccountState>> GetTvSeasonAccountState(int tvShowId, int seasonNumber)
        {
            RequireSessionId(SessionType.UserSession);

            RestRequest req = _client.Create("tv/{id}/season/{season_number}/account_states");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", TvEpisodeMethods.AccountStates.GetDescription());
            AddSessionId(req, SessionType.UserSession);

            RestResponse<ResultContainer<TvEpisodeAccountState>> response = await req.ExecuteGet<ResultContainer<TvEpisodeAccountState>>().ConfigureAwait(false);

            ResultContainer<TvEpisodeAccountState> item = await response.GetDataObject().ConfigureAwait(false);

            // Do some custom deserialization, since TMDb uses a property that changes type we can't use automatic deserialization
            if (item != null)
            {
                CustomDeserialization.DeserializeAccountStatesRating(item, await response.GetContent().ConfigureAwait(false));
            }

            return item;
        }

        public async Task<ChangesContainer> GetTvSeasonChanges(int seasonId)
        {
            RestRequest req = _client.Create("tv/season/{id}/changes");
            req.AddUrlSegment("id", seasonId.ToString(CultureInfo.InvariantCulture));

            RestResponse<ChangesContainer> response = await req.ExecuteGet<ChangesContainer>().ConfigureAwait(false);

            return response;
        }
        
        private async Task<T> GetTvSeasonMethod<T>(int tvShowId, int seasonNumber, TvSeasonMethods tvShowMethod, string dateFormat = null, string language = null) where T : new()
        {
            RestRequest req = _client.Create("tv/{id}/season/{season_number}/{method}");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", tvShowMethod.GetDescription());

            // TODO: Dateformat?
            //if (dateFormat != null)
            //    req.DateFormat = dateFormat;

            language = language ?? DefaultLanguage;
            if (!string.IsNullOrWhiteSpace(language))
                req.AddParameter("language", language);

            RestResponse<T> response = await req.ExecuteGet<T>().ConfigureAwait(false);

            return response;
        }
    }
}
