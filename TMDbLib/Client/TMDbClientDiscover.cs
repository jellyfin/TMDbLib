using System;
using RestSharp;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLib.Utilities;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Can be used to discover new tv shows matching certain criteria
        /// </summary>
        /// <param name="page">If specified the requested page of results will be returned.</param>
        /// <param name="language">ISO 639-1 code to localize the results in.</param>
        /// <param name="sortBy">Order the results by the provided option</param>
        /// <param name="firstAirDateYear">Filter the results based on the year the show first aired. Expected format YYYY.</param>
        /// <param name="voteCountGreaterThan">Only include TV shows that are equal to, or have a vote count higher than this value.</param>
        /// <param name="voteAverageGreaterThan">Only include TV shows that are equal to, or have a higher average rating than this value.</param>
        /// <param name="withGenres">Only include TV shows with the specified genres. Expected value is an integer (the id of a genre). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.</param>
        /// <param name="withNetworks">Filter TV shows by a specific network that distributed them. Expected value is an integer (the id of a network). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.</param>
        /// <param name="firstAirDateGreaterThan">The minimum airdate of tv shows to include.</param>
        /// <param name="firstAirDateLessThan">The maximum airdate of tv shows to include.</param>
        /// <returns>Will return a list of tv shows that corespond to the provided parameters</returns>
        public SearchContainer<TvShowBase> DiscoverTvShows(
            int page = 1,
            string language = null,
            DiscoverTvShowSortBy sortBy = DiscoverTvShowSortBy.Undefined,
            int? firstAirDateYear = null,
            int? voteCountGreaterThan = null,
            int? voteAverageGreaterThan = null,
            string withGenres = null,
            string withNetworks = null,
            DateTime? firstAirDateGreaterThan = null,
            DateTime? firstAirDateLessThan = null)
        {
            RestRequest request = new RestRequest("discover/tv");

            if (page != 1 && page > 1)
                request.AddParameter("page", page);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            if (sortBy != DiscoverTvShowSortBy.Undefined)
                request.AddParameter("sort_by", sortBy.GetDescription());

            if (firstAirDateYear.HasValue)
                request.AddParameter("first_air_date_year", firstAirDateYear);

            if (voteCountGreaterThan.HasValue)
                request.AddParameter("vote_count.gte", voteCountGreaterThan);

            if (voteAverageGreaterThan.HasValue)
                request.AddParameter("vote_average.gte", voteAverageGreaterThan);

            if (!String.IsNullOrWhiteSpace(withGenres))
                request.AddParameter("with_genres", withGenres);

            if (!String.IsNullOrWhiteSpace(withNetworks))
                request.AddParameter("with_networks", withNetworks);

            if (firstAirDateGreaterThan.HasValue)
                request.AddParameter("first_air_date.gte", firstAirDateGreaterThan.Value.ToString("yyyy-MM-dd"));

            if (firstAirDateLessThan.HasValue)
                request.AddParameter("first_air_date.lte", firstAirDateLessThan.Value.ToString("yyyy-MM-dd"));

            IRestResponse<SearchContainer<TvShowBase>> response = _client.Get<SearchContainer<TvShowBase>>(request);

            return response.Data;
        }

        /// <summary>
        /// Can be used to discover movies matching certain criteria
        /// </summary>
        /// <param name="page">If specified the requested page of results will be returned.</param>
        /// <param name="language">ISO 639-1 code to localize the results in.</param>
        /// <param name="sortBy">Order the results by the provided option</param>
        /// <param name="includeAdult">Specify true to include adult movie results.</param>
        /// <param name="year"></param>
        /// <param name="primaryReleaseYear">Filter the results basted on the year the movie was first released. Expected format YYYY.</param>
        /// <param name="voteCountGreaterThan">Only include movies that are equal to, or have a vote count higher than this value.</param>
        /// <param name="voteAverageGreaterThan">Only include movies that are equal to, or have a higher average rating than this value.</param>
        /// <param name="withGenres">Only include movies with the specified genres. Expected value is an integer (the id of a genre). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.</param>
        /// <param name="certificationCountry">Only include movies with certifications for a specific country. When this value is specified, 'certification.lte' is required. A ISO 3166-1 is expected.</param>
        /// <param name="certificationLessThan">Only include movies with this certification and lower. Expected value is a valid certification for the specificed 'certificationCountry'</param>
        /// <param name="releaseDateGreaterThan">The minimum release date of movies to include.</param>
        /// <param name="releaseDateLessThan">The maximum release date of movies to include.</param>
        /// <param name="withCompanies">Filter movies based on the company that created them. Expected value is an integer (the id of a company). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.</param>
        /// <returns>Will return a list of movies that corespond to the provided parameters</returns>
        public SearchContainer<SearchMovie> DiscoverMovies(
            int? page = null,
            string language = null,
            DiscoverMovieSortBy sortBy = DiscoverMovieSortBy.Undefined,
            bool includeAdult = false,
            int? year = null,
            int? primaryReleaseYear = null,
            int? voteCountGreaterThan = null,
            int? voteAverageGreaterThan = null,
            string withGenres = null,
            DateTime? releaseDateGreaterThan = null,
            DateTime? releaseDateLessThan = null,
            string certificationCountry = null,
            string certificationLessThan = null,
            string withCompanies = null
            )
        {
            RestRequest request = new RestRequest("discover/movie");

            if (page != 1 && page > 1)
                request.AddParameter("page", page);

            language = language ?? DefaultLanguage;
            if (!String.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            if (sortBy != DiscoverMovieSortBy.Undefined)
                request.AddParameter("sort_by", sortBy.GetDescription());

            if (includeAdult)
                request.AddParameter("include_adult", includeAdult);

            if (year.HasValue)
                request.AddParameter("year", year);

            if (primaryReleaseYear.HasValue)
                request.AddParameter("primary_release_year", primaryReleaseYear);

            if (voteCountGreaterThan.HasValue)
                request.AddParameter("vote_count.gte", voteCountGreaterThan);

            if (voteAverageGreaterThan.HasValue)
                request.AddParameter("vote_average.gte", voteAverageGreaterThan);

            if (!String.IsNullOrWhiteSpace(withGenres))
                request.AddParameter("with_genres", withGenres);

            if (releaseDateGreaterThan.HasValue)
                request.AddParameter("release_date.gte", releaseDateGreaterThan.Value.ToString("yyyy-MM-dd"));

            if (releaseDateLessThan.HasValue)
                request.AddParameter("release_date.lte", releaseDateLessThan.Value.ToString("yyyy-MM-dd"));

            if (!String.IsNullOrWhiteSpace(certificationCountry))
            {
                if (String.IsNullOrWhiteSpace(certificationLessThan))
                    throw new ArgumentNullException("certificationLessThan", "The parameter 'certificationLessThan' must be populated when specifying a 'certificationCountry'.");

                request.AddParameter("certification_country", certificationCountry);
                request.AddParameter("certification.lte", certificationLessThan);
            }
            else if (!String.IsNullOrWhiteSpace(certificationLessThan))
            {
                throw new ArgumentNullException("certificationCountry", "The parameter 'certificationCountry' must be populated when specifying a 'certificationLessThan'.");
            }

            if (!String.IsNullOrWhiteSpace(withCompanies))
                request.AddParameter("with_companies", withCompanies);

            IRestResponse<SearchContainer<SearchMovie>> response = _client.Get<SearchContainer<SearchMovie>>(request);

            return response.Data;
        }
    }
}
