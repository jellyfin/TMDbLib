using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMDbLib.Client;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLib.Utilities;

namespace TMDbLib.Objects.Discover;

/// <summary>
/// Provides methods for discovering TV shows with various filtering and sorting options.
/// </summary>
public class DiscoverTv : DiscoverBase<SearchTv>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoverTv"/> class.
    /// </summary>
    /// <param name="client">The TMDb client instance.</param>
    public DiscoverTv(TMDbClient client)
        : base("discover/tv", client)
    {
    }

    /// <summary>
    /// Available options are vote_average.desc, vote_average.asc, first_air_date.desc, first_air_date.asc, popularity.desc, popularity.asc.
    /// </summary>
    /// <param name="sortBy">The sort option to use.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv OrderBy(DiscoverTvShowSortBy sortBy)
    {
        Parameters["sort_by"] = sortBy.GetDescription();
        return this;
    }

    /// <summary>
    /// Specify a timeone to calculate proper date offsets. A list of valid timezones can be found by using the timezones/list method.
    /// </summary>
    /// <param name="timezone">The timezone to use.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv UseTimezone(string timezone)
    {
        Parameters["timezone"] = timezone;
        return this;
    }

    /// <summary>
    /// The minimum episode air date to include. Expected format is YYYY-MM-DD. Can be used in conjunction with a specified timezone.
    /// </summary>
    /// <param name="date">The minimum air date.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereAirDateIsAfter(DateTime date)
    {
        Parameters["air_date.gte"] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// The maximum episode air date to include. Expected format is YYYY-MM-DD. Can be used in conjunction with a specified timezone.
    /// </summary>
    /// <param name="date">The maximum air date.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereAirDateIsBefore(DateTime date)
    {
        Parameters["air_date.lte"] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// The minimum release to include. Expected format is YYYY-MM-DD.
    /// </summary>
    /// <param name="date">The minimum first air date.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereFirstAirDateIsAfter(DateTime date)
    {
        Parameters["first_air_date.gte"] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// The maximum release to include. Expected format is YYYY-MM-DD.
    /// </summary>
    /// <param name="date">The maximum first air date.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereFirstAirDateIsBefore(DateTime date)
    {
        Parameters["first_air_date.lte"] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Filter the results release dates to matches that include this value. Expected value is a year.
    /// </summary>
    /// <param name="year">The year to filter by.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereFirstAirDateIsInYear(int year)
    {
        Parameters["first_air_date_year"] = year.ToString("0000", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Only include TV shows with the specified genres. Expected value is an integer (the id of a genre). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.
    /// </summary>
    /// <param name="genres">The genres to include.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereGenresInclude(IEnumerable<Genre> genres)
    {
        return WhereGenresInclude(genres.Select(s => s.Id));
    }

    /// <summary>
    /// Only include TV shows with the specified genres. Expected value is an integer (the id of a genre). Multiple values can be specified. Comma separated indicates an 'AND' query, while a pipe (|) separated value indicates an 'OR'.
    /// </summary>
    /// <param name="genreIds">The genre IDs to include.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereGenresInclude(IEnumerable<int> genreIds)
    {
        Parameters["with_genres"] = string.Join(",", genreIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Filter TV shows to include a specific network. Expected value is an integer (the id of a network). They can be comma separated to indicate an 'AND' query.
    /// </summary>
    /// <param name="networks">The networks to include.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereNetworksInclude(IEnumerable<Network> networks)
    {
        return WhereNetworksInclude(networks.Select(s => s.Id));
    }

    /// <summary>
    /// Filter TV shows to include a specific network. Expected value is an integer (the id of a network). They can be comma separated to indicate an 'AND' query.
    /// </summary>
    /// <param name="networkIds">The network IDs to include.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereNetworksInclude(IEnumerable<int> networkIds)
    {
        Parameters["with_networks"] = string.Join(",", networkIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include TV shows that are equal to, or have a higher average rating than this value. Expected value is a float.
    /// </summary>
    /// <param name="score">The minimum vote average.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereVoteAverageIsAtLeast(double score)
    {
        // TODO: Apply culture to the ToString
        Parameters["vote_average.gte"] = score.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Only include TV shows that are equal to, or have a lower average rating than this value. Expected value is a float.
    /// </summary>
    /// <param name="score">The maximum vote average.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereVoteAverageIsAtMost(double score)
    {
        Parameters["vote_average.lte"] = score.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Only include TV shows that are equal to, or have a vote count higher than this value. Expected value is an integer.
    /// </summary>
    /// <param name="count">The minimum vote count.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereVoteCountIsAtLeast(int count)
    {
        Parameters["vote_count.gte"] = count.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Only include TV shows that are equal to, or have a vote count lower than this value. Expected value is an integer.
    /// </summary>
    /// <param name="count">The maximum vote count.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereVoteCountIsAtMost(int count)
    {
        Parameters["vote_count.lte"] = count.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Specifies which language to use for translatable fields.
    /// </summary>
    /// <param name="language">The language code.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereOriginalLanguageIs(string language)
    {
        Parameters["with_original_language"] = language;
        return this;
    }

    /// <summary>
    /// Only include TV shows that are equal to, or have a runtime higher than this value. Expected value is an integer (minutes).
    /// </summary>
    /// <param name="minutes">The minimum runtime in minutes.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereRuntimeIsAtLeast(int minutes)
    {
        Parameters["with_runtime.gte"] = minutes.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Only include TV shows that are equal to, or have a runtime lower than this value. Expected value is an integer (minutes).
    /// </summary>
    /// <param name="minutes">The maximum runtime in minutes.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereRuntimeIsAtMost(int minutes)
    {
        Parameters["with_runtime.lte"] = minutes.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Toggle the inclusion of TV shows with null first air data. Expected value is a boolean, true or false.
    /// </summary>
    /// <param name="include">Whether to include TV shows with null first air dates.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv IncludeNullFirstAirDates(bool include)
    {
        Parameters["include_null_first_air_dates"] = include.ToString();
        return this;
    }

    /// <summary>
    /// Exclude TV shows with the specified genres. Expected value is a list of genres.
    /// </summary>
    /// <param name="genres">The genres to exclude.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereGenresExclude(IEnumerable<Genre> genres)
    {
        return WhereGenresExclude(genres.Select(s => s.Id));
    }

    /// <summary>
    /// Exclude TV shows with the specified genres. Expected value is a list of integer (the id of a genre).
    /// </summary>
    /// <param name="genreIds">The genre IDs to exclude.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereGenresExclude(IEnumerable<int> genreIds)
    {
        Parameters["without_genres"] = string.Join(",", genreIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include TV shows with the specified companies. Expected value is an list of companies.
    /// </summary>
    /// <param name="companies">The companies to include.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereCompaniesInclude(IEnumerable<Company> companies)
    {
        return WhereCompaniesInclude(companies.Select(s => s.Id));
    }

    /// <summary>
    /// Only include TV shows with the specified companies. Expected value is a list of integer (the id of a company).
    /// </summary>
    /// <param name="companyIds">The company IDs to include.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereCompaniesInclude(IEnumerable<int> companyIds)
    {
        Parameters["with_companies"] = string.Join(",", companyIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Filter results to include items that have been screened theatrically.
    /// </summary>
    /// <param name="theatrical">Whether to include only TV shows that have been screened theatrically.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereScreenedTheatrically(bool theatrical)
    {
        Parameters["screened_theatrically"] = theatrical.ToString();
        return this;
    }

    /// <summary>
    /// Filter TV shows to include a specific keyword. Expected value is a list of keywords.
    /// </summary>
    /// <param name="keywords">The keywords to include.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereKeywordsInclude(IEnumerable<Keyword> keywords)
    {
        return WhereKeywordsInclude(keywords.Select(s => s.Id));
    }

    /// <summary>
    /// Filter TV shows to include a specific keyword. Expected value is a list of integer (the id of a keyword).
    /// </summary>
    /// <param name="keywordIds">The keyword IDs to include.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereKeywordsInclude(IEnumerable<int> keywordIds)
    {
        Parameters["with_keywords"] = string.Join(",", keywordIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Filter TV shows to exclude a specific keyword. Expected value is a list of keywords.
    /// </summary>
    /// <param name="keywords">The keywords to exclude.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereKeywordsExclude(IEnumerable<Keyword> keywords)
    {
        return WhereKeywordsInclude(keywords.Select(s => s.Id));
    }

    /// <summary>
    /// Filter TV shows to exclude a specific keyword. Expected value is a list of integer (the id of a keyword).
    /// </summary>
    /// <param name="keywordIds">The keyword IDs to exclude.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereKeywordsExclude(IEnumerable<int> keywordIds)
    {
        Parameters["without_keywords"] = string.Join("|", keywordIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Specifies which language to use for translatable fields.
    /// </summary>
    /// <param name="language">The language code.</param>
    /// <returns>The current <see cref="DiscoverTv"/> instance for method chaining.</returns>
    public DiscoverTv WhereLanguageIs(string language)
    {
        Parameters["language"] = language;
        return this;
    }
}
