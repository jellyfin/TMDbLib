using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb discover functionality.
/// </summary>
public class ClientDiscoverTests : TestBase
{
    /// <summary>
    /// Tests that discovering TV shows without parameters returns results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestDiscoverTvShowsNoParamsAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.DiscoverTvShowsAsync().Query(i));
    }

    /// <summary>
    /// Tests that discovering movies without parameters returns results and supports pagination.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesNoParamsAsync()
    {
        await TestHelpers.SearchPagesAsync(i => TMDbClient.DiscoverMoviesAsync().Query(i));
    }

    /// <summary>
    /// Tests that discovering TV shows with vote count and average filters returns filtered results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverTvShowsAsync()
    {
        DiscoverTv query = TMDbClient.DiscoverTvShowsAsync()
                .WhereVoteCountIsAtLeast(100)
                .WhereVoteAverageIsAtLeast(2);

        await TestHelpers.SearchPagesAsync(i => query.Query(i));
    }

    /// <summary>
    /// Tests that discovering movies with vote count and average filters returns filtered results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesAsync()
    {
        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
                .WhereVoteCountIsAtLeast(1000)
                .WhereVoteAverageIsAtLeast(2);

        await TestHelpers.SearchPagesAsync(i => query.Query(i));
    }

    /// <summary>
    /// Tests that discovering movies with region and release date filters returns region-specific results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesRegionAsync()
    {
        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
            .WhereReleaseDateIsInRegion("BR")
            .WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01));

        await TestHelpers.SearchPagesAsync(i => query.Query(i));
    }

    /// <summary>
    /// Tests that discovering movies with release type filters returns movies matching the specified release type.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesReleaseTypeAsync()
    {
        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
            .WithAnyOfReleaseTypes(ReleaseDateType.Premiere);

        await TestHelpers.SearchPagesAsync(i => query.Query(i));
    }

    /// <summary>
    /// Tests that discovering movies with language filters returns localized titles while maintaining same movie IDs.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesLanguageAsync()
    {
        SearchContainer<SearchMovie> query = await TMDbClient.DiscoverMoviesAsync()
            .WhereOriginalLanguageIs("en-US")
            .WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01))
            .Query();

        SearchContainer<SearchMovie> queryDanish = await TMDbClient.DiscoverMoviesAsync()
            .WhereLanguageIs("da-DK")
            .WhereOriginalLanguageIs("en-US")
            .WherePrimaryReleaseDateIsAfter(new DateTime(2017, 01, 01))
            .Query();

        // Should be the same identities, but different titles
        Assert.Equal(query.TotalResults, queryDanish.TotalResults);

        for (int i = 0; i < query.Results.Count; i++)
        {
            SearchMovie a = query.Results[i];
            SearchMovie b = queryDanish.Results[i];

            Assert.Equal(a.Id, b.Id);
            Assert.NotEqual(a.Title, b.Title);
        }
    }

    /// <summary>
    /// Tests that discovering movies with watch provider filter returns results available on that provider.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesWatchProviderAsync()
    {
        int[] filteredProviderIds = [WatchProvider.Netflix.Standard];

        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
            .IncludeWithAnyOfWatchProviders(filteredProviderIds)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchMovie> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have at least one of the filtered providers
        foreach (SearchMovie movie in results.Results)
        {
            List<int> providerIds = await GetMovieProviderIdsAsync(movie.Id, "US");
            Assert.True(
                providerIds.Any(id => filteredProviderIds.Contains(id)),
                $"Movie {movie.Id} ({movie.Title}): Expected at least one of [{string.Join(", ", filteredProviderIds)}] but got [{string.Join(", ", providerIds)}]");
        }
    }

    /// <summary>
    /// Tests that discovering movies with multiple watch providers using OR query returns results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesWatchProviderMultipleAsync()
    {
        int[] filteredProviderIds =
        [
            WatchProvider.Netflix.Standard,
            WatchProvider.Amazon.PrimeVideo
        ];

        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
            .IncludeWithAnyOfWatchProviders(filteredProviderIds)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchMovie> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have at least one of the filtered providers
        foreach (SearchMovie movie in results.Results)
        {
            List<int> providerIds = await GetMovieProviderIdsAsync(movie.Id, "US");
            Assert.True(
                providerIds.Any(id => filteredProviderIds.Contains(id)),
                $"Movie {movie.Id} ({movie.Title}): Expected at least one of [{string.Join(", ", filteredProviderIds)}] but got [{string.Join(", ", providerIds)}]");
        }
    }

    /// <summary>
    /// Tests that discovering movies with monetization type filter returns results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesMonetizationTypeAsync()
    {
        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
            .WhereAnyWatchMonetizationTypesMatch(WatchMonetizationType.Flatrate)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchMovie> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have at least one flatrate provider
        foreach (SearchMovie movie in results.Results)
        {
            WatchProviders providers = await GetMovieWatchProvidersAsync(movie.Id, "US");
            Assert.True(
                providers?.FlatRate?.Count > 0,
                $"Movie {movie.Id} ({movie.Title}): Expected flatrate providers but found none");
        }
    }

    /// <summary>
    /// Tests that discovering movies with multiple monetization types using OR query returns results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesMonetizationTypeMultipleAsync()
    {
        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
            .WhereAnyWatchMonetizationTypesMatch(WatchMonetizationType.Flatrate, WatchMonetizationType.Free)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchMovie> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have at least one flatrate or free provider
        foreach (SearchMovie movie in results.Results)
        {
            WatchProviders providers = await GetMovieWatchProvidersAsync(movie.Id, "US");
            bool hasFlatrate = providers?.FlatRate?.Count > 0;
            bool hasFree = providers?.Free?.Count > 0;
            Assert.True(
                hasFlatrate || hasFree,
                $"Movie {movie.Id} ({movie.Title}): Expected flatrate or free providers but found none");
        }
    }

    /// <summary>
    /// Tests that discovering movies with watch provider and monetization type combined returns filtered results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverMoviesWatchProviderAndMonetizationTypeAsync()
    {
        int[] filteredProviderIds = [WatchProvider.Netflix.Standard];

        DiscoverMovie query = TMDbClient.DiscoverMoviesAsync()
            .IncludeWithAnyOfWatchProviders(filteredProviderIds)
            .WhereAnyWatchMonetizationTypesMatch(WatchMonetizationType.Flatrate)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchMovie> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have Netflix in flatrate providers
        foreach (SearchMovie movie in results.Results)
        {
            WatchProviders providers = await GetMovieWatchProvidersAsync(movie.Id, "US");
            Assert.NotNull(providers?.FlatRate);
            Assert.True(
                providers.FlatRate.Any(p => filteredProviderIds.Contains(p.ProviderId ?? 0)),
                $"Movie {movie.Id} ({movie.Title}): Expected Netflix in flatrate providers but found [{string.Join(", ", providers.FlatRate.Select(p => p.ProviderId))}]");
        }
    }

    /// <summary>
    /// Tests that discovering TV shows with watch provider filter returns results available on that provider.
    /// </summary>
    [Fact]
    public async Task TestDiscoverTvShowsWatchProviderAsync()
    {
        int[] filteredProviderIds = [WatchProvider.Netflix.Standard];

        DiscoverTv query = TMDbClient.DiscoverTvShowsAsync()
            .IncludeWithAnyOfWatchProviders(filteredProviderIds)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchTv> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have at least one of the filtered providers
        foreach (SearchTv tvShow in results.Results)
        {
            List<int> providerIds = await GetTvShowProviderIdsAsync(tvShow.Id, "US");
            Assert.True(
                providerIds.Any(id => filteredProviderIds.Contains(id)),
                $"TV Show {tvShow.Id} ({tvShow.Name}): Expected at least one of [{string.Join(", ", filteredProviderIds)}] but got [{string.Join(", ", providerIds)}]");
        }
    }

    /// <summary>
    /// Tests that discovering TV shows with multiple watch providers using OR query returns results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverTvShowsWatchProviderMultipleAsync()
    {
        int[] filteredProviderIds =
        [
            WatchProvider.Netflix.Standard,
            WatchProvider.Amazon.PrimeVideo
        ];

        DiscoverTv query = TMDbClient.DiscoverTvShowsAsync()
            .IncludeWithAnyOfWatchProviders(filteredProviderIds)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchTv> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have at least one of the filtered providers
        foreach (SearchTv tvShow in results.Results)
        {
            List<int> providerIds = await GetTvShowProviderIdsAsync(tvShow.Id, "US");
            Assert.True(
                providerIds.Any(id => filteredProviderIds.Contains(id)),
                $"TV Show {tvShow.Id} ({tvShow.Name}): Expected at least one of [{string.Join(", ", filteredProviderIds)}] but got [{string.Join(", ", providerIds)}]");
        }
    }

    /// <summary>
    /// Tests that discovering TV shows with monetization type filter returns results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverTvShowsMonetizationTypeAsync()
    {
        DiscoverTv query = TMDbClient.DiscoverTvShowsAsync()
            .WhereAnyWatchMonetizationTypesMatch(WatchMonetizationType.Flatrate)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchTv> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have at least one flatrate provider
        foreach (SearchTv tvShow in results.Results)
        {
            WatchProviders providers = await GetTvShowWatchProvidersAsync(tvShow.Id, "US");
            Assert.True(
                providers?.FlatRate?.Count > 0,
                $"TV Show {tvShow.Id} ({tvShow.Name}): Expected flatrate providers but found none");
        }
    }

    /// <summary>
    /// Tests that discovering TV shows with multiple monetization types using OR query returns results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverTvShowsMonetizationTypeMultipleAsync()
    {
        DiscoverTv query = TMDbClient.DiscoverTvShowsAsync()
            .WhereAnyWatchMonetizationTypesMatch(WatchMonetizationType.Flatrate, WatchMonetizationType.Free)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchTv> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have at least one flatrate or free provider
        foreach (SearchTv tvShow in results.Results)
        {
            WatchProviders providers = await GetTvShowWatchProvidersAsync(tvShow.Id, "US");
            bool hasFlatrate = providers?.FlatRate?.Count > 0;
            bool hasFree = providers?.Free?.Count > 0;
            Assert.True(
                hasFlatrate || hasFree,
                $"TV Show {tvShow.Id} ({tvShow.Name}): Expected flatrate or free providers but found none");
        }
    }

    /// <summary>
    /// Tests that discovering TV shows with watch provider and monetization type combined returns filtered results.
    /// </summary>
    [Fact]
    public async Task TestDiscoverTvShowsWatchProviderAndMonetizationTypeAsync()
    {
        int[] filteredProviderIds = [WatchProvider.Netflix.Standard];

        DiscoverTv query = TMDbClient.DiscoverTvShowsAsync()
            .IncludeWithAnyOfWatchProviders(filteredProviderIds)
            .WhereAnyWatchMonetizationTypesMatch(WatchMonetizationType.Flatrate)
            .WhereWatchRegionIs("US");

        SearchContainer<SearchTv> results = await query.Query();

        Assert.NotEmpty(results.Results);
        Assert.True(results.TotalResults > 0);

        // Verify all results have Netflix in flatrate providers
        foreach (SearchTv tvShow in results.Results)
        {
            WatchProviders providers = await GetTvShowWatchProvidersAsync(tvShow.Id, "US");
            Assert.NotNull(providers?.FlatRate);
            Assert.True(
                providers.FlatRate.Any(p => filteredProviderIds.Contains(p.ProviderId ?? 0)),
                $"TV Show {tvShow.Id} ({tvShow.Name}): Expected Netflix in flatrate providers but found [{string.Join(", ", providers.FlatRate.Select(p => p.ProviderId))}]");
        }
    }

    /// <summary>
    /// Gets all provider IDs for a movie in a specific region.
    /// </summary>
    private async Task<List<int>> GetMovieProviderIdsAsync(int movieId, string region)
    {
        WatchProviders providers = await GetMovieWatchProvidersAsync(movieId, region);
        return GetProviderIds(providers);
    }

    /// <summary>
    /// Gets all provider IDs for a TV show in a specific region.
    /// </summary>
    private async Task<List<int>> GetTvShowProviderIdsAsync(int tvShowId, string region)
    {
        WatchProviders providers = await GetTvShowWatchProvidersAsync(tvShowId, region);
        return GetProviderIds(providers);
    }

    /// <summary>
    /// Gets watch providers for a movie in a specific region.
    /// </summary>
    private async Task<WatchProviders> GetMovieWatchProvidersAsync(int movieId, string region)
    {
        SingleResultContainer<Dictionary<string, WatchProviders>> watchProviders =
            await TMDbClient.GetMovieWatchProvidersAsync(movieId);

        watchProviders.Results.TryGetValue(region, out WatchProviders regionProviders);
        return regionProviders;
    }

    /// <summary>
    /// Gets watch providers for a TV show in a specific region.
    /// </summary>
    private async Task<WatchProviders> GetTvShowWatchProvidersAsync(int tvShowId, string region)
    {
        SingleResultContainer<Dictionary<string, WatchProviders>> watchProviders =
            await TMDbClient.GetTvShowWatchProvidersAsync(tvShowId);

        watchProviders.Results.TryGetValue(region, out WatchProviders regionProviders);
        return regionProviders;
    }

    /// <summary>
    /// Extracts all provider IDs from watch providers.
    /// </summary>
    private static List<int> GetProviderIds(WatchProviders providers)
    {
        List<int> ids = new();

        if (providers?.FlatRate != null)
            ids.AddRange(providers.FlatRate.Where(p => p.ProviderId.HasValue).Select(p => p.ProviderId.Value));
        if (providers?.Rent != null)
            ids.AddRange(providers.Rent.Where(p => p.ProviderId.HasValue).Select(p => p.ProviderId.Value));
        if (providers?.Buy != null)
            ids.AddRange(providers.Buy.Where(p => p.ProviderId.HasValue).Select(p => p.ProviderId.Value));
        if (providers?.Free != null)
            ids.AddRange(providers.Free.Where(p => p.ProviderId.HasValue).Select(p => p.ProviderId.Value));
        if (providers?.Ads != null)
            ids.AddRange(providers.Ads.Where(p => p.ProviderId.HasValue).Select(p => p.ProviderId.Value));

        return ids;
    }
}
