using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using RestSharp;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;

namespace TMDbLib.Client
{
    public partial class TMDbClient
    {
        /// <summary>
        /// Can be used to discover new tv shows matching certain criteria
        /// </summary>
        public DiscoverTv DiscoverTvShows()
        {
            return new DiscoverTv(this);
        }

        /// <summary>
        /// Can be used to discover movies matching certain criteria
        /// </summary>
        public DiscoverMovie DiscoverMovies()
        {
            return new DiscoverMovie(this);
        }

        internal SearchContainer<T> DiscoverPerform<T>(string endpoint, string language, int page, NameValueCollection parameters)
        {
            RestRequest request = new RestRequest(endpoint);

            if (page != 1 && page > 1)
                request.AddParameter("page", page);

            if (!string.IsNullOrWhiteSpace(language))
                request.AddParameter("language", language);

            foreach (string key in parameters.Keys)
                request.AddParameter(key, parameters[key]);

            IRestResponse<SearchContainer<T>> response = _client.Get<SearchContainer<T>>(request);
            return response.Data;
        }

        ///// <summary>
        ///// Can be used to discover new tv shows matching certain criteria
        ///// </summary>
        //public SearchContainer<SearchTv> DiscoverTvShows(DiscoverTv discover = null, string language = null, int page = 0)
        //{
        //    RestRequest request = new RestRequest("discover/tv");

        //    if (page != 1 && page > 1)
        //        request.AddParameter("page", page);

        //    if (!string.IsNullOrWhiteSpace(language))
        //        request.AddParameter("language", language);

        //    if (discover != null)
        //        foreach (KeyValuePair<string, string> parameter in discover.GetAllParameters())
        //            request.AddParameter(parameter.Key, parameter.Value);

        //    IRestResponse<SearchContainer<SearchTv>> response = _client.Get<SearchContainer<SearchTv>>(request);
        //    return response.Data;
        //}

        ///// <summary>
        ///// Can be used to discover movies matching certain criteria
        ///// </summary>
        //public SearchContainer<SearchMovie> DiscoverMovies(DiscoverMovie discover = null, string language = null, int page = 0)
        //{
        //    RestRequest request = new RestRequest("discover/movie");

        //    if (page != 1 && page > 1)
        //        request.AddParameter("page", page);

        //    if (!string.IsNullOrWhiteSpace(language))
        //        request.AddParameter("language", language);

        //    if (discover != null)
        //        foreach (KeyValuePair<string, string> parameter in discover.GetAllParameters())
        //            request.AddParameter(parameter.Key, parameter.Value);

        //    IRestResponse<SearchContainer<SearchMovie>> response = _client.Get<SearchContainer<SearchMovie>>(request);
        //    return response.Data;
        //}

        ///// <summary>
        ///// Can be used to discover new tv shows matching certain criteria
        ///// </summary>
        ///// <param name="page">If specified the requested page of results will be returned.</param>
        ///// <param name="language">ISO 639-1 code to localize the results in.</param>
        ///// <param name="sortBy">Order the results by the provided option</param>
        ///// <param name="firstAirDateYear">Filter the results based on the year the show first aired. Expected format YYYY.</param>
        ///// <param name="voteCountGreaterThan">Only include TV shows that are equal to, or have a vote count higher than this value.</param>
        ///// <param name="voteAverageGreaterThan">Only include TV shows that are equal to, or have a higher average rating than this value.</param>
        ///// <param name="withGenres">Only include TV shows with the specified genres. Expected value is an integer (the id of a genre). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.</param>
        ///// <param name="withNetworks">Filter TV shows by a specific network that distributed them. Expected value is an integer (the id of a network). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.</param>
        ///// <param name="firstAirDateGreaterThan">The minimum airdate of tv shows to include.</param>
        ///// <param name="firstAirDateLessThan">The maximum airdate of tv shows to include.</param>
        ///// <returns>Will return a list of tv shows that corespond to the provided parameters</returns>
        //[Obsolete]
        //public SearchContainer<SearchTv> DiscoverTvShows(
        //    int page = 1,
        //    string language = null,
        //    DiscoverTvShowSortBy sortBy = DiscoverTvShowSortBy.Undefined,
        //    int? firstAirDateYear = null,
        //    int? voteCountGreaterThan = null,
        //    int? voteAverageGreaterThan = null,
        //    string withGenres = null,
        //    string withNetworks = null,
        //    DateTime? firstAirDateGreaterThan = null,
        //    DateTime? firstAirDateLessThan = null)
        //{
        //    DiscoverTv query = new DiscoverTv();

        //    if (sortBy != DiscoverTvShowSortBy.Undefined)
        //        query.OrderBy(sortBy);

        //    if (firstAirDateYear.HasValue)
        //        query.WhereFirstAirDateIsInYear(firstAirDateYear.Value);

        //    if (voteAverageGreaterThan.HasValue)
        //        query.WhereVoteAverageIsAtLeast(voteAverageGreaterThan.Value);

        //    if (voteCountGreaterThan.HasValue)
        //        query.WhereVoteCountIsAtLeast(voteCountGreaterThan.Value);

        //    if (firstAirDateGreaterThan.HasValue)
        //        query.WhereFirstAirDateIsAfter(firstAirDateGreaterThan.Value);

        //    if (firstAirDateLessThan.HasValue)
        //        query.WhereFirstAirDateIsBefore(firstAirDateLessThan.Value);

        //    return DiscoverTvShows(query, language, page );
        //}

        ///// <summary>
        ///// Can be used to discover movies matching certain criteria
        ///// </summary>
        ///// <param name="page">If specified the requested page of results will be returned.</param>
        ///// <param name="language">ISO 639-1 code to localize the results in.</param>
        ///// <param name="sortBy">Order the results by the provided option</param>
        ///// <param name="includeAdult">Specify true to include adult movie results.</param>
        ///// <param name="year"></param>
        ///// <param name="primaryReleaseYear">Filter the results basted on the year the movie was first released. Expected format YYYY.</param>
        ///// <param name="voteCountGreaterThan">Only include movies that are equal to, or have a vote count higher than this value.</param>
        ///// <param name="voteAverageGreaterThan">Only include movies that are equal to, or have a higher average rating than this value.</param>
        ///// <param name="withGenres">Only include movies with the specified genres. Expected value is an integer (the id of a genre). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.</param>
        ///// <param name="certificationCountry">Only include movies with certifications for a specific country. When this value is specified, 'certification.lte' is required. A ISO 3166-1 is expected.</param>
        ///// <param name="certificationLessThan">Only include movies with this certification and lower. Expected value is a valid certification for the specificed 'certificationCountry'</param>
        ///// <param name="releaseDateGreaterThan">The minimum release date of movies to include.</param>
        ///// <param name="releaseDateLessThan">The maximum release date of movies to include.</param>
        ///// <param name="withCompanies">Filter movies based on the company that created them. Expected value is an integer (the id of a company). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.</param>
        ///// <returns>Will return a list of movies that corespond to the provided parameters</returns>
        //[Obsolete]
        //public SearchContainer<SearchMovie> DiscoverMovies(
        //    int? page = null,
        //    string language = null,
        //    DiscoverMovieSortBy sortBy = DiscoverMovieSortBy.Undefined,
        //    bool includeAdult = false,
        //    int? year = null,
        //    int? primaryReleaseYear = null,
        //    int? voteCountGreaterThan = null,
        //    int? voteAverageGreaterThan = null,
        //    string withGenres = null,
        //    DateTime? releaseDateGreaterThan = null,
        //    DateTime? releaseDateLessThan = null,
        //    string certificationCountry = null,
        //    string certificationLessThan = null,
        //    string withCompanies = null
        //    )
        //{
        //    DiscoverMovie query = new DiscoverMovie();

        //    if (sortBy != DiscoverMovieSortBy.Undefined)
        //        query.OrderBy(sortBy);

        //    if (includeAdult)
        //        query.IncludeAdultMovies();

        //    if (year.HasValue)
        //        query.WhereAnyReleaseDateIsInYear(year.Value);

        //    if (primaryReleaseYear.HasValue)
        //        query.WherePrimaryReleaseIsInYear(year.Value);

        //    if (voteAverageGreaterThan.HasValue)
        //        query.WhereVoteAverageIsAtLeast(voteAverageGreaterThan.Value);

        //    if (voteCountGreaterThan.HasValue)
        //        query.WhereVoteCountIsAtLeast(voteCountGreaterThan.Value);

        //    if (releaseDateGreaterThan.HasValue)
        //        query.WhereReleaseDateIsAfter(releaseDateGreaterThan.Value);

        //    if (releaseDateLessThan.HasValue)
        //        query.WhereReleaseDateIsBefore(releaseDateLessThan.Value);

        //    if (!string.IsNullOrEmpty(certificationCountry) && !string.IsNullOrEmpty(certificationLessThan))
        //        query.WhereCertificationIsAtMost(certificationCountry, certificationLessThan);

        //    return DiscoverMovies(query, language, page ?? 0);
        //}
    }
}
