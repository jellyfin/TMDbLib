using System;
using System.Collections.Generic;
using System.Linq;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.Discover
{
    public class DiscoverTv : DiscoverBase<SearchTv>
    {
        public DiscoverTv(TMDbClient client)
            : base("discover/tv", client)
        {

        }

        private void ClearAirDate()
        {
            Parameters.Remove("air_date.gte");
            Parameters.Remove("air_date.lte");
        }

        private void ClearFirstAirDate()
        {
            Parameters.Remove("first_air_date.gte");
            Parameters.Remove("first_air_date.lte");
            Parameters.Remove("first_air_date_year");
        }

        /// <summary>
        /// Available options are vote_average.desc, vote_average.asc, first_air_date.desc, first_air_date.asc, popularity.desc, popularity.asc
        /// </summary>
        public DiscoverTv OrderBy(DiscoverTvShowSortBy sortBy)
        {
            Parameters["sort_by"] = sortBy.GetDescription();
            return this;
        }

        /// <summary>
        /// Specify a timeone to calculate proper date offsets. A list of valid timezones can be found by using the timezones/list method.
        /// </summary>
        public DiscoverTv UseTimezone(string timezone)
        {
            Parameters["timezone"] = timezone;
            return this;
        }

        /// <summary>
        /// The minimum episode air date to include. Expected format is YYYY-MM-DD. Can be used in conjunction with a specified timezone.
        /// </summary>
        public DiscoverTv WhereAirDateIsAfter(DateTime date)
        {
            ClearAirDate();

            Parameters["air_date.gte"] = date.ToString("yyyy-MM-dd");
            return this;
        }

        /// <summary>
        /// The maximum episode air date to include. Expected format is YYYY-MM-DD. Can be used in conjunction with a specified timezone.
        /// </summary>
        public DiscoverTv WhereAirDateIsBefore(DateTime date)
        {
            ClearAirDate();

            Parameters["air_date.lte"] = date.ToString("yyyy-MM-dd");
            return this;
        }

        /// <summary>
        /// The minimum release to include. Expected format is YYYY-MM-DD.
        /// </summary>
        public DiscoverTv WhereFirstAirDateIsAfter(DateTime date)
        {
            ClearFirstAirDate();

            Parameters["first_air_date.gte"] = date.ToString("yyyy-MM-dd");
            return this;
        }

        /// <summary>
        /// The maximum release to include. Expected format is YYYY-MM-DD.
        /// </summary>
        public DiscoverTv WhereFirstAirDateIsBefore(DateTime date)
        {
            ClearFirstAirDate();

            Parameters["first_air_date.lte"] = date.ToString("yyyy-MM-dd");
            return this;
        }

        /// <summary>
        /// Filter the results release dates to matches that include this value. Expected value is a year.
        /// </summary>
        public DiscoverTv WhereFirstAirDateIsInYear(int year)
        {
            ClearFirstAirDate();

            Parameters["first_air_date_year"] = year.ToString("0000");
            return this;
        }

        /// <summary>
        /// Only include TV shows with the specified genres. Expected value is an integer (the id of a genre). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.
        /// </summary>
        public DiscoverTv WhereGenresInclude(IEnumerable<Genre> genres)
        {
            return WhereGenresInclude(genres.Select(s => s.Id));
        }

        /// <summary>
        /// Only include TV shows with the specified genres. Expected value is an integer (the id of a genre). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.
        /// </summary>
        public DiscoverTv WhereGenresInclude(IEnumerable<int> genreIds)
        {
            Parameters["with_genres"] = string.Join(",", genreIds.Select(s => s.ToString()));
            return this;
        }

        /// <summary>
        /// Filter TV shows to include a specific network. Expected value is an integer (the id of a network). They can be comma separated to indicate an 'AND' query.
        /// </summary>
        public DiscoverTv WhereNetworksInclude(IEnumerable<Network> networks)
        {
            return WhereNetworksInclude(networks.Select(s => s.Id));
        }

        /// <summary>
        /// Filter TV shows to include a specific network. Expected value is an integer (the id of a network). They can be comma separated to indicate an 'AND' query.
        /// </summary>
        public DiscoverTv WhereNetworksInclude(IEnumerable<int> networkIds)
        {
            Parameters["with_networks"] = string.Join(",", networkIds.Select(s => s.ToString()));
            return this;
        }

        /// <summary>
        /// Only include TV shows that are equal to, or have a higher average rating than this value. Expected value is a float.
        /// </summary>
        public DiscoverTv WhereVoteAverageIsAtLeast(double score)
        {
            // TODO: Apply culture to the ToString
            Parameters["vote_average.gte"] = score.ToString();
            return this;
        }

        /// <summary>
        /// Only include TV shows that are equal to, or have a vote count higher than this value. Expected value is an integer.
        /// </summary>
        public DiscoverTv WhereVoteCountIsAtLeast(int count)
        {
            Parameters["vote_count.gte"] = count.ToString();
            return this;
        }
    }
}