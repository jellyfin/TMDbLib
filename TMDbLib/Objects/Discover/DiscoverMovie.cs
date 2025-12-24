using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMDbLib.Client;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities;
using Cast = TMDbLib.Objects.TvShows.Cast;

namespace TMDbLib.Objects.Discover;

/// <summary>
/// Provides methods for discovering movies with various filtering and sorting options.
/// </summary>
public class DiscoverMovie : DiscoverBase<SearchMovie>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DiscoverMovie"/> class.
    /// </summary>
    /// <param name="client">The TMDb client instance.</param>
    public DiscoverMovie(TMDbClient client)
        : base("discover/movie", client)
    {
    }

    private void ClearCertification()
    {
        Parameters.Remove("certification_country");
        Parameters.Remove("certification");
        Parameters.Remove("certification.lte");
        Parameters.Remove("certification.gte");
    }

    /// <summary>
    /// Toggle the inclusion of adult titles. Expected value is a boolean, true or false. Default is false.
    /// </summary>
    /// <param name="include">Whether to include adult titles.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeAdultMovies(bool include = true)
    {
        Parameters["include_adult"] = include.ToString();
        return this;
    }

    /// <summary>
    /// Toggle the inclusion of items marked as a video. Expected value is a boolean, true or false. Default is true.
    /// </summary>
    /// <param name="include">Whether to include video titles.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeVideoMovies(bool include = true)
    {
        Parameters["include_video"] = include.ToString();
        return this;
    }

    /// <summary>
    /// Only include movies that have this person id added as a cast member. Expected value is an integer (the id of a person).
    /// This method performs an AND query.
    /// </summary>
    /// <param name="castIds">The cast member IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfCast(IEnumerable<int> castIds)
    {
        Parameters["with_cast"] = string.Join(",", castIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that have this person id added as a cast member.
    /// This method performs an AND query.
    /// </summary>
    /// <param name="casts">The cast members to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfCast(IEnumerable<Cast> casts)
    {
        return IncludeWithAllOfCast(casts.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies that have this company id added as a crew member. Expected value is an integer (the id of a company).
    /// This method performs an AND query.
    /// </summary>
    /// <param name="companyIds">The company IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfCompany(IEnumerable<int> companyIds)
    {
        Parameters["with_companies"] = string.Join(",", companyIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that have this company id added as a crew member.
    /// This method performs an AND query.
    /// </summary>
    /// <param name="companies">The companies to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfCompany(IEnumerable<Company> companies)
    {
        return IncludeWithAllOfCompany(companies.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies that have this person id added as a crew member. Expected value is an integer (the id of a person).
    /// This method performs an AND query.
    /// </summary>
    /// <param name="crewIds">The crew member IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfCrew(IEnumerable<int> crewIds)
    {
        Parameters["with_crew"] = string.Join(",", crewIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that have this person id added as a crew member.
    /// This method performs an AND query.
    /// </summary>
    /// <param name="crews">The crew members to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfCrew(IEnumerable<Crew> crews)
    {
        return IncludeWithAllOfCrew(crews.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies with the specified genres. Expected value is an integer (the id of a genre).
    /// This method performs an AND query.
    /// </summary>
    /// <param name="genreIds">The genre IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfGenre(IEnumerable<int> genreIds)
    {
        Parameters["with_genres"] = string.Join(",", genreIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies with the specified genres.
    /// This method performs an AND query.
    /// </summary>
    /// <param name="genres">The genres to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfGenre(IEnumerable<Genre> genres)
    {
        return IncludeWithAllOfGenre(genres.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies with the specified keywords. Expected value is an integer (the id of a keyword).
    /// This method performs an AND query.
    /// </summary>
    /// <param name="keywordIds">The keyword IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfKeywords(IEnumerable<int> keywordIds)
    {
        Parameters["with_keywords"] = string.Join(",", keywordIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies with the specified keywords.
    /// This method performs an AND query.
    /// </summary>
    /// <param name="keywords">The keywords to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfKeywords(IEnumerable<Genre> keywords)
    {
        return IncludeWithAllOfKeywords(keywords.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies that have these person id's added as a cast or crew member. Expected value is an integer (the id or ids of a person).
    /// This method performs an AND query.
    /// </summary>
    /// <param name="peopleIds">The people IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfPeople(IEnumerable<int> peopleIds)
    {
        Parameters["with_people"] = string.Join(",", peopleIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that have these person id's added as a cast or crew member.
    /// This method performs an AND query.
    /// </summary>
    /// <param name="people">The people to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAllOfPeople(IEnumerable<Genre> people)
    {
        return IncludeWithAllOfPeople(people.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies that have this person id added as a cast member. Expected value is an integer (the id of a person).
    /// This method performs an OR query.
    /// </summary>
    /// <param name="castIds">The cast member IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfCast(IEnumerable<int> castIds)
    {
        Parameters["with_cast"] = string.Join("|", castIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that have this person id added as a cast member.
    /// This method performs an OR query.
    /// </summary>
    /// <param name="casts">The cast members to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfCast(IEnumerable<Cast> casts)
    {
        return IncludeWithAnyOfCast(casts.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies that have this company id added as a crew member. Expected value is an integer (the id of a company).
    /// This method performs an OR query.
    /// </summary>
    /// <param name="companyIds">The company IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfCompany(IEnumerable<int> companyIds)
    {
        Parameters["with_companies"] = string.Join("|", companyIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that have this company id added as a crew member.
    /// This method performs an OR query.
    /// </summary>
    /// <param name="companies">The companies to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfCompany(IEnumerable<Company> companies)
    {
        return IncludeWithAnyOfCompany(companies.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies that have this person id added as a crew member. Expected value is an integer (the id of a person).
    /// This method performs an OR query.
    /// </summary>
    /// <param name="crewIds">The crew member IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfCrew(IEnumerable<int> crewIds)
    {
        Parameters["with_crew"] = string.Join("|", crewIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that have this person id added as a crew member.
    /// This method performs an OR query.
    /// </summary>
    /// <param name="crews">The crew members to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfCrew(IEnumerable<Crew> crews)
    {
        return IncludeWithAnyOfCrew(crews.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies with the specified genres. Expected value is an integer (the id of a genre).
    /// This method performs an OR query.
    /// </summary>
    /// <param name="castIds">The genre IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfGenre(IEnumerable<int> castIds)
    {
        Parameters["with_genres"] = string.Join("|", castIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies with the specified genres.
    /// This method performs an OR query.
    /// </summary>
    /// <param name="genres">The genres to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfGenre(IEnumerable<Genre> genres)
    {
        return IncludeWithAnyOfGenre(genres.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies with the specified keywords. Expected value is an integer (the id of a keyword).
    /// This method performs an OR query.
    /// </summary>
    /// <param name="keywordIds">The keyword IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfKeywords(IEnumerable<int> keywordIds)
    {
        Parameters["with_keywords"] = string.Join("|", keywordIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies with the specified keywords.
    /// This method performs an OR query.
    /// </summary>
    /// <param name="keywords">The keywords to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfKeywords(IEnumerable<Genre> keywords)
    {
        return IncludeWithAnyOfKeywords(keywords.Select(s => s.Id));
    }

    /// <summary>
    /// Only include movies that have these person id's added as a cast or crew member. Expected value is an integer (the id or ids of a person).
    /// This method performs an OR query.
    /// </summary>
    /// <param name="peopleIds">The people IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfPeople(IEnumerable<int> peopleIds)
    {
        Parameters["with_people"] = string.Join("|", peopleIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that have these person id's added as a cast or crew member.
    /// This method performs an OR query.
    /// </summary>
    /// <param name="people">The people to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie IncludeWithAnyOfPeople(IEnumerable<Genre> people)
    {
        return IncludeWithAnyOfPeople(people.Select(s => s.Id));
    }

    /// <summary>
    /// Available options are: popularity.ascpopularity.descrelease_date.ascrelease_date.descrevenue.ascrevenue.descprimary_release_date.ascprimary_release_date.descoriginal_title.ascoriginal_title.descvote_average.ascvote_average.descvote_count.ascvote_count.desc.
    /// </summary>
    /// <param name="sortBy">The sort order to apply.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie OrderBy(DiscoverMovieSortBy sortBy)
    {
        Parameters["sort_by"] = sortBy.GetDescription();
        return this;
    }

    /// <summary>
    /// Filter the results by all available release dates that have the specified value added as a year. Expected value is an integer (year).
    /// </summary>
    /// <param name="year">The year to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereAnyReleaseDateIsInYear(int year)
    {
        Parameters["year"] = year.ToString("0000", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Only include movies with this certification. Expected value is a valid certification for the specificed 'certification_country'.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="certification">The certification rating.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereCertificationIs(string country, string certification)
    {
        ClearCertification();

        Parameters["certification_country"] = country;
        Parameters["certification"] = certification;

        return this;
    }

    /// <summary>
    /// Only include movies with this certification and lower. Expected value is a valid certification for the specificed 'certification_country'.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="maxCertification">The maximum certification rating.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereCertificationIsAtMost(string country, string maxCertification)
    {
        ClearCertification();

        Parameters["certification_country"] = country;
        Parameters["certification.lte"] = maxCertification;

        return this;
    }

    /// <summary>
    /// Only include movies with this certification and higher. Expected value is a valid certification for the specificed 'certification_country'.
    /// </summary>
    /// <param name="country">The country code.</param>
    /// <param name="minCertification">The minimum certification rating.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereCertificationIsAtLeast(string country, string minCertification)
    {
        ClearCertification();

        Parameters["certification_country"] = country;
        Parameters["certification.gte"] = minCertification;

        return this;
    }

    /// <summary>
    /// Filter by the primary release date and only include those which are greater than or equal to the specified value. Expected format is YYYY-MM-DD.
    /// </summary>
    /// <param name="date">The minimum release date.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WherePrimaryReleaseDateIsAfter(DateTime date)
    {
        Parameters["primary_release_date.gte"] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Filter by the primary release date and only include those which are greater than or equal to the specified value. Expected format is YYYY-MM-DD.
    /// </summary>
    /// <param name="date">The maximum release date.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WherePrimaryReleaseDateIsBefore(DateTime date)
    {
        Parameters["primary_release_date.lte"] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Filter the results so that only the primary release date year has this value. Expected value is a year.
    /// </summary>
    /// <param name="year">The year to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WherePrimaryReleaseIsInYear(int year)
    {
        Parameters["primary_release_year"] = year.ToString("0000", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Filter by all available release dates and only include those which are greater or equal to the specified value. Expected format is YYYY-MM-DD.
    /// </summary>
    /// <param name="date">The minimum release date.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereReleaseDateIsAfter(DateTime date)
    {
        Parameters["release_date.gte"] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Filter by all available release dates and only include those which are less or equal to the specified value. Expected format is YYYY-MM-DD.
    /// </summary>
    /// <param name="date">The maximum release date.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereReleaseDateIsBefore(DateTime date)
    {
        Parameters["release_date.lte"] = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Only include movies that are equal to, or have a runtime higher than this value. Expected value is an integer (minutes).
    /// </summary>
    /// <param name="minutes">The minimum runtime in minutes.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereRuntimeIsAtLeast(int minutes)
    {
        Parameters["with_runtime.gte"] = minutes.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Only include movies that are equal to, or have a runtime lower than this value. Expected value is an integer (minutes).
    /// </summary>
    /// <param name="minutes">The maximum runtime in minutes.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereRuntimeIsAtMost(int minutes)
    {
        Parameters["with_runtime.lte"] = minutes.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Filter movies by their vote average and only include those that have an average rating that is equal to or higher than the specified value. Expected value is a float.
    /// </summary>
    /// <param name="score">The minimum vote average.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereVoteAverageIsAtLeast(double score)
    {
        // TODO: Apply culture to the ToString
        Parameters["vote_average.gte"] = score.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Filter movies by their vote average and only include those that have an average rating that is equal to or lower than the specified value. Expected value is a float.
    /// </summary>
    /// <param name="score">The maximum vote average.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereVoteAverageIsAtMost(double score)
    {
        // TODO: Apply culture to the ToString
        Parameters["vote_average.lte"] = score.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Filter movies by their vote count and only include movies that have a vote count that is equal to or lower than the specified value.
    /// </summary>
    /// <param name="count">The minimum vote count.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereVoteCountIsAtLeast(int count)
    {
        Parameters["vote_count.gte"] = count.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Filter movies by their vote count and only include movies that have a vote count that is equal to or lower than the specified value. Expected value is an integer.
    /// </summary>
    /// <param name="count">The maximum vote count.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereVoteCountIsAtMost(int count)
    {
        Parameters["vote_count.lte"] = count.ToString(CultureInfo.InvariantCulture);
        return this;
    }

    /// <summary>
    /// Specifies which region to use for release date filtering (using ISO 3166-1 code).
    /// </summary>
    /// <param name="region">The region code.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereReleaseDateIsInRegion(string region)
    {
        Parameters["region"] = region;
        return this;
    }

    /// <summary>
    /// Specifies which language to use for translatable fields.
    /// </summary>
    /// <param name="language">The language code.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereLanguageIs(string language)
    {
        Parameters["language"] = language;
        return this;
    }

    /// <summary>
    /// Specifies which language to use for translatable fields.
    /// </summary>
    /// <param name="language">The original language code.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereOriginalLanguageIs(string language)
    {
        Parameters["with_original_language"] = language;
        return this;
    }

    /// <summary>
    /// Specifies that only movies with all the given release types will be returned.
    /// </summary>
    /// <param name="releaseTypes">The release types to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WithAllOfReleaseTypes(params ReleaseDateType[] releaseTypes)
    {
        Parameters["with_release_type"] = string.Join(",", releaseTypes.Select(s => ((int)s).ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Specifies that only movies with the given release types will be returned.
    /// </summary>
    /// <param name="releaseTypes">The release types to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WithAnyOfReleaseTypes(params ReleaseDateType[] releaseTypes)
    {
        Parameters["with_release_type"] = string.Join("|", releaseTypes.Select(s => ((int)s).ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that are available on all of the specified watch providers (e.g., Netflix, Amazon Prime Video, Disney+).
    /// This method performs an AND query.
    /// </summary>
    /// <param name="providerIds">The watch provider IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    /// <remarks>Use in conjunction with <see cref="WhereWatchRegionIs"/> to specify the region.</remarks>
    public DiscoverMovie IncludeWithAllOfWatchProviders(IEnumerable<int> providerIds)
    {
        Parameters["with_watch_providers"] = string.Join(",", providerIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Only include movies that are available on any of the specified watch providers (e.g., Netflix, Amazon Prime Video, Disney+).
    /// This method performs an OR query.
    /// </summary>
    /// <param name="providerIds">The watch provider IDs to filter by.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    /// <remarks>Use in conjunction with <see cref="WhereWatchRegionIs"/> to specify the region.</remarks>
    public DiscoverMovie IncludeWithAnyOfWatchProviders(IEnumerable<int> providerIds)
    {
        Parameters["with_watch_providers"] = string.Join("|", providerIds.Select(s => s.ToString(CultureInfo.InvariantCulture)));
        return this;
    }

    /// <summary>
    /// Specifies the watch region for watch provider or monetization type filtering using an ISO 3166-1 code (e.g., "US", "GB", "DE").
    /// This is required when using <see cref="IncludeWithAllOfWatchProviders(IEnumerable{int})"/>,
    /// <see cref="IncludeWithAnyOfWatchProviders(IEnumerable{int})"/>, <see cref="WhereAllWatchMonetizationTypesMatch(WatchMonetizationType[])"/>,
    /// or <see cref="WhereAnyWatchMonetizationTypesMatch(WatchMonetizationType[])"/>.
    /// </summary>
    /// <param name="region">The ISO 3166-1 region code.</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    public DiscoverMovie WhereWatchRegionIs(string region)
    {
        Parameters["watch_region"] = region;
        return this;
    }

    /// <summary>
    /// Only include movies available with all of the specified monetization types.
    /// This method performs an AND query.
    /// </summary>
    /// <param name="monetizationTypes">The monetization types to filter by (flatrate, free, ads, rent, buy).</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    /// <remarks>Use in conjunction with <see cref="WhereWatchRegionIs"/> to specify the region.</remarks>
    public DiscoverMovie WhereAllWatchMonetizationTypesMatch(params WatchMonetizationType[] monetizationTypes)
    {
        Parameters["with_watch_monetization_types"] = string.Join(",", monetizationTypes.Select(s => s.GetDescription()));
        return this;
    }

    /// <summary>
    /// Only include movies available with any of the specified monetization types.
    /// This method performs an OR query.
    /// </summary>
    /// <param name="monetizationTypes">The monetization types to filter by (flatrate, free, ads, rent, buy).</param>
    /// <returns>The current <see cref="DiscoverMovie"/> instance for method chaining.</returns>
    /// <remarks>Use in conjunction with <see cref="WhereWatchRegionIs"/> to specify the region.</remarks>
    public DiscoverMovie WhereAnyWatchMonetizationTypesMatch(params WatchMonetizationType[] monetizationTypes)
    {
        Parameters["with_watch_monetization_types"] = string.Join("|", monetizationTypes.Select(s => s.GetDescription()));
        return this;
    }
}
