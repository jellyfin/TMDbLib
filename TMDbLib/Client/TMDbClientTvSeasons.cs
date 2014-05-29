using System;
using System.Globalization;
using System.Linq;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLib.Utilities;

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
        public TvSeason GetTvSeason(int tvShowId, int seasonNumber, TvSeasonMethods extraMethods = TvSeasonMethods.Undefined, string language = null)
        {
            RestQueryBuilder req = new RestQueryBuilder("tv/{id}/season/{season_number}");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));

            if (language != null)
                req.AddParameter("language", language);

            string appends = string.Join(",",
                                         Enum.GetValues(typeof(TvSeasonMethods))
                                             .OfType<TvSeasonMethods>()
                                             .Except(new[] { TvSeasonMethods.Undefined })
                                             .Where(s => extraMethods.HasFlag(s))
                                             .Select(s => s.GetDescription()));

            if (appends != string.Empty)
                req.AddParameter("append_to_response", appends);

            ResponseContainer<TvSeason> response = _client.Get<TvSeason>(req);

            return response.Data;
        }

        /// <summary>
        /// Returns a credits object for the season of the tv show associated with the provided TMDb id.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season you want to retrieve information for. Note use 0 for specials.</param>
        /// <param name="language">If specified the api will attempt to return a localized result. ex: en,it,es </param>
        public Credits GetTvSeasonCredits(int tvShowId, int seasonNumber, string language = null)
        {
            return GetTvSeasonMethod<Credits>(tvShowId, seasonNumber, TvSeasonMethods.Credits, dateFormat: "yyyy-MM-dd", language: language);
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
        public PosterImages GetTvSeasonImages(int tvShowId, int seasonNumber, string language = null)
        {
            return GetTvSeasonMethod<PosterImages>(tvShowId, seasonNumber, TvSeasonMethods.Images, language: language);
        }

        /// <summary>
        /// Returns an object that contains all known exteral id's for the season of the tv show related to the specified TMDB id.
        /// </summary>
        /// <param name="tvShowId">The TMDb id of the target tv show.</param>
        /// <param name="seasonNumber">The season number of the season you want to retrieve information for. Note use 0 for specials.</param>
        public ExternalIds GetTvSeasonExternalIds(int tvShowId, int seasonNumber)
        {
            return GetTvSeasonMethod<ExternalIds>(tvShowId, seasonNumber, TvSeasonMethods.ExternalIds);
        }

        private T GetTvSeasonMethod<T>(int tvShowId, int seasonNumber, TvSeasonMethods tvShowMethod, string dateFormat = null, string language = null) where T : new()
        {
            RestQueryBuilder req = new RestQueryBuilder("tv/{id}/season/{season_number}/{method}");
            req.AddUrlSegment("id", tvShowId.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("season_number", seasonNumber.ToString(CultureInfo.InvariantCulture));
            req.AddUrlSegment("method", tvShowMethod.GetDescription());

            // TODO: Dateformat
            //if (dateFormat != null)
            //    req.DateFormat = dateFormat;

            if (language != null)
                req.AddParameter("language", language);

            ResponseContainer<T> resp = _client.Get<T>(req);

            return resp.Data;
        }
    }
}
