# TMDbLib Integration Guide

## Overview

TMDbLib is a comprehensive .NET client library for The Movie Database (TMDb) API. This guide provides all the methods available in the client with their input parameters and return types to help you integrate this library into your projects.

## Setup and Installation

### Installation
```bash
Install-Package TMDbLib
```

### Basic Client Setup
```csharp
using TMDbLib.Client;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.General;

// Initialize client
var client = new TMDbClient("YOUR_API_KEY");

// Optional: Set default language for all requests
client.DefaultLanguage = "en-US";
client.DefaultImageLanguage = "en";
client.DefaultCountry = "US";
```

## Authentication

### Guest Session (Limited Access)
```csharp
// Input: None
// Output: GuestSession
var guestSession = await client.AuthenticationCreateGuestSessionAsync();
await client.SetSessionInformationAsync(guestSession.Id, SessionType.GuestSession);
```

### User Session (Full Access)
```csharp
// Input: username (string), password (string)
// Output: UserSession
var userSession = await client.AuthenticationGetUserSessionAsync("username", "password");
await client.SetSessionInformationAsync(userSession.SessionId, SessionType.UserSession);
```

### Request Token Method
```csharp
// Step 1: Request token
// Input: None
// Output: Token
var token = await client.AuthenticationRequestAutenticationTokenAsync();

// Step 2: Validate token with user credentials
// Input: requestToken (string), username (string), password (string)
// Output: Task (void)
await client.AuthenticationValidateUserTokenAsync(token.RequestToken, "username", "password");

// Step 3: Get session
// Input: requestToken (string)
// Output: UserSession
var session = await client.AuthenticationGetUserSessionAsync(token.RequestToken);
```

## Movies

### Get Movie Details
```csharp
// Input: movieId (int), extraMethods (MovieMethods - optional)
// Output: Movie
var movie = await client.GetMovieAsync(123);

// With extra data
var movieWithCredits = await client.GetMovieAsync(123, MovieMethods.Credits | MovieMethods.Videos);

// Using IMDb ID
// Input: imdbId (string), extraMethods (MovieMethods - optional)
// Output: Movie
var movieByImdb = await client.GetMovieAsync("tt0111161");

// With language and image language
// Input: movieId (int), language (string), includeImageLanguage (string), extraMethods (MovieMethods)
// Output: Movie
var localizedMovie = await client.GetMovieAsync(123, "fr-FR", "en,fr", MovieMethods.Credits);
```

### Movie Additional Data
```csharp
// Get movie credits
// Input: movieId (int)
// Output: Credits
var credits = await client.GetMovieCreditsAsync(123);

// Get movie images
// Input: movieId (int), language (string - optional), includeImageLanguage (string - optional)
// Output: ImagesWithId
var images = await client.GetMovieImagesAsync(123);
var localizedImages = await client.GetMovieImagesAsync(123, "en-US", "en,fr");

// Get movie videos
// Input: movieId (int)
// Output: ResultContainer<Video>
var videos = await client.GetMovieVideosAsync(123);

// Get movie keywords
// Input: movieId (int)
// Output: KeywordsContainer
var keywords = await client.GetMovieKeywordsAsync(123);

// Get movie reviews
// Input: movieId (int), language (string - optional), page (int - optional)
// Output: SearchContainer<ReviewBase>
var reviews = await client.GetMovieReviewsAsync(123);

// Get movie recommendations
// Input: movieId (int), language (string - optional), page (int - optional)
// Output: SearchContainer<SearchMovie>
var recommendations = await client.GetMovieRecommendationsAsync(123);

// Get similar movies
// Input: movieId (int), language (string - optional), page (int - optional)
// Output: SearchContainer<SearchMovie>
var similarMovies = await client.GetMovieSimilarAsync(123);

// Get movie alternative titles
// Input: movieId (int), country (string - optional)
// Output: AlternativeTitles
var altTitles = await client.GetMovieAlternativeTitlesAsync(123);

// Get movie external IDs
// Input: movieId (int)
// Output: ExternalIdsMovie
var externalIds = await client.GetMovieExternalIdsAsync(123);

// Get movie release dates
// Input: movieId (int)
// Output: ReleaseDatesContainer
var releaseDates = await client.GetMovieReleaseDatesAsync(123);

// Get movie translations
// Input: movieId (int)
// Output: TranslationsContainer
var translations = await client.GetMovieTranslationsAsync(123);

// Get movie account states (requires user session)
// Input: movieId (int)
// Output: AccountState
var accountState = await client.GetMovieAccountStateAsync(123);

// Get movie lists
// Input: movieId (int), language (string - optional), page (int - optional)
// Output: SearchContainerWithId<ListResult>
var movieLists = await client.GetMovieListsAsync(123);

// Get movie watch providers
// Input: movieId (int)
// Output: SingleResultContainer<Dictionary<string, WatchProviders>>
var watchProviders = await client.GetMovieWatchProvidersAsync(123);
```

### Popular/Top Rated/Upcoming Movies
```csharp
// Get popular movies
// Input: language (string - optional), page (int - optional)
// Output: SearchContainer<SearchMovie>
var popularMovies = await client.GetMoviePopularAsync();

// Get top rated movies
// Input: language (string - optional), page (int - optional)
// Output: SearchContainer<SearchMovie>
var topRatedMovies = await client.GetMovieTopRatedAsync();

// Get upcoming movies
// Input: language (string - optional), page (int - optional), region (string - optional)
// Output: SearchContainer<SearchMovie>
var upcomingMovies = await client.GetMovieUpcomingAsync();

// Get now playing movies
// Input: language (string - optional), page (int - optional), region (string - optional)
// Output: SearchContainer<SearchMovie>
var nowPlayingMovies = await client.GetMovieNowPlayingAsync();

// Get latest movie
// Input: None
// Output: Movie
var latestMovie = await client.GetMovieLatestAsync();
```

### Movie Rating/Watchlist/Favorites (Requires User Session)
```csharp
// Rate a movie
// Input: movieId (int), rating (double - 0.5 to 10.0)
// Output: PostReply
var rateResult = await client.MovieSetRatingAsync(123, 8.5);

// Remove movie rating
// Input: movieId (int)
// Output: PostReply
var removeRatingResult = await client.MovieRemoveRatingAsync(123);

// Add to watchlist
// Input: movieId (int), watchlist (bool)
// Output: PostReply
var watchlistResult = await client.AccountChangeMovieWatchlistStatusAsync(123, true);

// Add to favorites
// Input: movieId (int), favorite (bool)
// Output: PostReply
var favoriteResult = await client.AccountChangeFavoriteStatusAsync(123, true);
```

## TV Shows

### Get TV Show Details
```csharp
// Input: tvShowId (int), extraMethods (TvShowMethods - optional), language (string - optional), includeImageLanguage (string - optional)
// Output: TvShow
var tvShow = await client.GetTvShowAsync(456);

// With extra data
var tvShowWithCredits = await client.GetTvShowAsync(456, TvShowMethods.Credits | TvShowMethods.ExternalIds);

// Localized
var localizedTvShow = await client.GetTvShowAsync(456, TvShowMethods.Undefined, "fr-FR", "en,fr");
```

### TV Show Additional Data
```csharp
// Get TV show credits
// Input: tvShowId (int)
// Output: Credits
var tvCredits = await client.GetTvShowCreditsAsync(456);

// Get TV show images
// Input: tvShowId (int), language (string - optional), includeImageLanguage (string - optional)
// Output: ImagesWithId
var tvImages = await client.GetTvShowImagesAsync(456);

// Get TV show videos
// Input: tvShowId (int), language (string - optional), includeVideoLanguage (string - optional)
// Output: ResultContainer<Video>
var tvVideos = await client.GetTvShowVideosAsync(456);

// Get TV show keywords
// Input: tvShowId (int)
// Output: ResultContainer<Keyword>
var tvKeywords = await client.GetTvShowKeywordsAsync(456);

// Get TV show reviews
// Input: tvShowId (int), language (string - optional), page (int - optional)
// Output: SearchContainer<ReviewBase>
var tvReviews = await client.GetTvShowReviewsAsync(456);

// Get TV show recommendations
// Input: tvShowId (int), language (string - optional), page (int - optional)
// Output: SearchContainer<SearchTv>
var tvRecommendations = await client.GetTvShowRecommendationsAsync(456);

// Get similar TV shows
// Input: tvShowId (int), language (string - optional), page (int - optional)
// Output: SearchContainer<SearchTv>
var similarTvShows = await client.GetTvShowSimilarAsync(456);

// Get TV show alternative titles
// Input: tvShowId (int)
// Output: ResultContainer<AlternativeTitle>
var tvAltTitles = await client.GetTvShowAlternativeTitlesAsync(456);

// Get TV show external IDs
// Input: tvShowId (int)
// Output: ExternalIdsTvShow
var tvExternalIds = await client.GetTvShowExternalIdsAsync(456);

// Get TV show translations
// Input: tvShowId (int)
// Output: TranslationsContainer
var tvTranslations = await client.GetTvShowTranslationsAsync(456);

// Get TV show account states (requires user session)
// Input: tvShowId (int)
// Output: AccountState
var tvAccountState = await client.GetTvShowAccountStateAsync(456);

// Get TV show content ratings
// Input: tvShowId (int)
// Output: ResultContainer<ContentRating>
var contentRatings = await client.GetTvShowContentRatingsAsync(456);

// Get TV show watch providers
// Input: tvShowId (int)
// Output: SingleResultContainer<Dictionary<string, WatchProviders>>
var tvWatchProviders = await client.GetTvShowWatchProvidersAsync(456);
```

### TV Seasons
```csharp
// Get TV season details
// Input: tvShowId (int), seasonNumber (int), extraMethods (TvSeasonMethods - optional), language (string - optional), includeImageLanguage (string - optional)
// Output: TvSeason
var season = await client.GetTvSeasonAsync(456, 1);

// Get season credits
// Input: tvShowId (int), seasonNumber (int)
// Output: CreditsWithGuestStars
var seasonCredits = await client.GetTvSeasonCreditsAsync(456, 1);

// Get season images
// Input: tvShowId (int), seasonNumber (int), language (string - optional), includeImageLanguage (string - optional)
// Output: ImagesWithId
var seasonImages = await client.GetTvSeasonImagesAsync(456, 1);

// Get season videos
// Input: tvShowId (int), seasonNumber (int), language (string - optional), includeVideoLanguage (string - optional)
// Output: ResultContainer<Video>
var seasonVideos = await client.GetTvSeasonVideosAsync(456, 1);

// Get season external IDs
// Input: tvShowId (int), seasonNumber (int)
// Output: ExternalIdsTvSeason
var seasonExternalIds = await client.GetTvSeasonExternalIdsAsync(456, 1);
```

### TV Episodes
```csharp
// Get TV episode details
// Input: tvShowId (int), seasonNumber (int), episodeNumber (int), extraMethods (TvEpisodeMethods - optional), language (string - optional), includeImageLanguage (string - optional)
// Output: TvEpisode
var episode = await client.GetTvEpisodeAsync(456, 1, 1);

// Get episode credits
// Input: tvShowId (int), seasonNumber (int), episodeNumber (int)
// Output: CreditsWithGuestStars
var episodeCredits = await client.GetTvEpisodeCreditsAsync(456, 1, 1);

// Get episode images
// Input: tvShowId (int), seasonNumber (int), episodeNumber (int), language (string - optional), includeImageLanguage (string - optional)
// Output: ImagesWithId
var episodeImages = await client.GetTvEpisodeImagesAsync(456, 1, 1);

// Get episode videos
// Input: tvShowId (int), seasonNumber (int), episodeNumber (int), language (string - optional), includeVideoLanguage (string - optional)
// Output: ResultContainer<Video>
var episodeVideos = await client.GetTvEpisodeVideosAsync(456, 1, 1);

// Get episode external IDs
// Input: tvShowId (int), seasonNumber (int), episodeNumber (int)
// Output: ExternalIdsTvEpisode
var episodeExternalIds = await client.GetTvEpisodeExternalIdsAsync(456, 1, 1);

// Get episode translations
// Input: tvShowId (int), seasonNumber (int), episodeNumber (int)
// Output: TranslationsContainer
var episodeTranslations = await client.GetTvEpisodeTranslationsAsync(456, 1, 1);

// Get episode account states (requires user session)
// Input: tvShowId (int), seasonNumber (int), episodeNumber (int)
// Output: TvEpisodeAccountState
var episodeAccountState = await client.GetTvEpisodeAccountStateAsync(456, 1, 1);
```

### Popular/Top Rated/Airing TV Shows
```csharp
// Get popular TV shows
// Input: language (string - optional), page (int - optional)
// Output: SearchContainer<SearchTv>
var popularTvShows = await client.GetTvShowPopularAsync();

// Get top rated TV shows
// Input: language (string - optional), page (int - optional)
// Output: SearchContainer<SearchTv>
var topRatedTvShows = await client.GetTvShowTopRatedAsync();

// Get TV shows airing today
// Input: language (string - optional), page (int - optional)
// Output: SearchContainer<SearchTv>
var airingTodayTvShows = await client.GetTvShowAiringTodayAsync();

// Get TV shows on the air
// Input: language (string - optional), page (int - optional)
// Output: SearchContainer<SearchTv>
var onTheAirTvShows = await client.GetTvShowOnTheAirAsync();

// Get latest TV show
// Input: None
// Output: TvShow
var latestTvShow = await client.GetTvShowLatestAsync();
```

### TV Show Rating/Watchlist/Favorites (Requires User Session)
```csharp
// Rate a TV show
// Input: tvShowId (int), rating (double - 0.5 to 10.0)
// Output: PostReply
var rateTvResult = await client.TvShowSetRatingAsync(456, 9.0);

// Remove TV show rating
// Input: tvShowId (int)
// Output: PostReply
var removeTvRatingResult = await client.TvShowRemoveRatingAsync(456);

// Rate a TV episode
// Input: tvShowId (int), seasonNumber (int), episodeNumber (int), rating (double - 0.5 to 10.0)
// Output: PostReply
var rateEpisodeResult = await client.TvEpisodeSetRatingAsync(456, 1, 1, 8.0);

// Remove TV episode rating
// Input: tvShowId (int), seasonNumber (int), episodeNumber (int)
// Output: PostReply
var removeEpisodeRatingResult = await client.TvEpisodeRemoveRatingAsync(456, 1, 1);

// Add TV show to watchlist
// Input: tvShowId (int), watchlist (bool)
// Output: PostReply
var tvWatchlistResult = await client.AccountChangeTvShowWatchlistStatusAsync(456, true);

// Add TV show to favorites
// Input: tvShowId (int), favorite (bool)
// Output: PostReply
var tvFavoriteResult = await client.AccountChangeTvShowFavoriteStatusAsync(456, true);
```

## People

### Get Person Details
```csharp
// Input: personId (int), extraMethods (PersonMethods - optional), language (string - optional)
// Output: Person
var person = await client.GetPersonAsync(789);

// With extra data
var personWithCredits = await client.GetPersonAsync(789, PersonMethods.MovieCredits | PersonMethods.TvCredits);

// Localized
var localizedPerson = await client.GetPersonAsync(789, PersonMethods.Undefined, "es-ES");
```

### Person Additional Data
```csharp
// Get person movie credits
// Input: personId (int), language (string - optional)
// Output: MovieCredits
var movieCredits = await client.GetPersonMovieCreditsAsync(789);

// Get person TV credits
// Input: personId (int), language (string - optional)
// Output: TvCredits
var tvCredits = await client.GetPersonTvCreditsAsync(789);

// Get person combined credits
// Input: personId (int), language (string - optional)
// Output: CombinedCredits
var combinedCredits = await client.GetPersonCombinedCreditsAsync(789);

// Get person images
// Input: personId (int)
// Output: ProfileImages
var personImages = await client.GetPersonImagesAsync(789);

// Get person external IDs
// Input: personId (int)
// Output: ExternalIdsPerson
var personExternalIds = await client.GetPersonExternalIdsAsync(789);

// Get person translations
// Input: personId (int)
// Output: TranslationsContainer
var personTranslations = await client.GetPersonTranslationsAsync(789);

// Get latest person
// Input: None
// Output: Person
var latestPerson = await client.GetLatestPersonAsync();
```

### Popular People
```csharp
// Get popular people
// Input: language (string - optional), page (int - optional)
// Output: SearchContainer<SearchPerson>
var popularPeople = await client.GetPersonPopularAsync();
```

## Search

### Search Movies
```csharp
// Input: query (string), language (string - optional), page (int - optional), includeAdult (bool - optional), year (int - optional), region (string - optional), primaryReleaseYear (int - optional)
// Output: SearchContainer<SearchMovie>
var movieSearchResults = await client.SearchMovieAsync("The Matrix");

// With additional parameters
var detailedMovieSearch = await client.SearchMovieAsync("The Matrix", "en-US", 1, false, 1999, "US", 1999);
```

### Search TV Shows
```csharp
// Input: query (string), language (string - optional), page (int - optional), includeAdult (bool - optional), firstAirDateYear (int - optional)
// Output: SearchContainer<SearchTv>
var tvSearchResults = await client.SearchTvShowAsync("Breaking Bad");

// With additional parameters
var detailedTvSearch = await client.SearchTvShowAsync("Breaking Bad", "en-US", 1, false, 2008);
```

### Search People
```csharp
// Input: query (string), language (string - optional), page (int - optional), includeAdult (bool - optional), region (string - optional)
// Output: SearchContainer<SearchPerson>
var personSearchResults = await client.SearchPersonAsync("Tom Hanks");

// With additional parameters
var detailedPersonSearch = await client.SearchPersonAsync("Tom Hanks", "en-US", 1, false, "US");
```

### Search Collections
```csharp
// Input: query (string), language (string - optional), page (int - optional)
// Output: SearchContainer<SearchCollection>
var collectionSearchResults = await client.SearchCollectionAsync("James Bond");
```

### Multi Search
```csharp
// Input: query (string), language (string - optional), page (int - optional), includeAdult (bool - optional), year (int - optional), region (string - optional)
// Output: SearchContainer<SearchBase>
var multiSearchResults = await client.SearchMultiAsync("Marvel");
```

## Discover

### Discover Movies
```csharp
// Get a discover movie builder
var discoverMovies = client.DiscoverMoviesAsync();

// Build query with filters
var discoveredMovies = await discoverMovies
    .WhereGenresInclude(28, 12) // Action and Adventure
    .WhereReleaseDateIsAfter(new DateTime(2020, 1, 1))
    .WhereVoteAverageIsAtLeast(7.0)
    .OrderBy(DiscoverMovieSortBy.PopularityDesc)
    .Query();
```

### Discover TV Shows
```csharp
// Get a discover TV show builder
var discoverTvShows = client.DiscoverTvShowsAsync();

// Build query with filters
var discoveredTvShows = await discoverTvShows
    .WhereGenresInclude(18) // Drama
    .WhereFirstAirDateIsAfter(new DateTime(2020, 1, 1))
    .WhereVoteAverageIsAtLeast(8.0)
    .OrderBy(DiscoverTvShowSortBy.PopularityDesc)
    .Query();
```

## Collections

### Get Collection Details
```csharp
// Input: collectionId (int), extraMethods (CollectionMethods - optional), language (string - optional), includeImageLanguages (string - optional)
// Output: Collection
var collection = await client.GetCollectionAsync(10);

// With extra data
var collectionWithImages = await client.GetCollectionAsync(10, CollectionMethods.Images);

// Localized
var localizedCollection = await client.GetCollectionAsync(10, CollectionMethods.Undefined, "fr-FR", "en,fr");
```

### Collection Additional Data
```csharp
// Get collection images
// Input: collectionId (int), language (string - optional), includeImageLanguages (string - optional)
// Output: ImagesWithId
var collectionImages = await client.GetCollectionImagesAsync(10);

// Get collection translations
// Input: collectionId (int)
// Output: TranslationsContainer
var collectionTranslations = await client.GetCollectionTranslationsAsync(10);
```

## Companies

### Get Company Details
```csharp
// Input: companyId (int), extraMethods (CompanyMethods - optional)
// Output: Company
var company = await client.GetCompanyAsync(420);

// With extra data
var companyWithImages = await client.GetCompanyAsync(420, CompanyMethods.Images);
```

### Company Additional Data
```csharp
// Get company images
// Input: companyId (int)
// Output: ImagesWithId
var companyImages = await client.GetCompanyImagesAsync(420);

// Get company alternative names
// Input: companyId (int)
// Output: AlternativeNames
var companyAltNames = await client.GetCompanyAlternativeNamesAsync(420);
```

## Networks

### Get Network Details
```csharp
// Input: networkId (int)
// Output: Network
var network = await client.GetNetworkAsync(213);
```

### Network Additional Data
```csharp
// Get network images
// Input: networkId (int)
// Output: ImagesWithId
var networkImages = await client.GetNetworkImagesAsync(213);

// Get network alternative names
// Input: networkId (int)
// Output: AlternativeNames
var networkAltNames = await client.GetNetworkAlternativeNamesAsync(213);
```

## Keywords

### Get Keyword Details
```csharp
// Input: keywordId (int)
// Output: Keyword
var keyword = await client.GetKeywordAsync(1721);
```

### Keyword Movies
```csharp
// Input: keywordId (int), language (string - optional), page (int - optional)
// Output: SearchContainer<SearchMovie>
var keywordMovies = await client.GetKeywordMoviesAsync(1721);
```

## Genres

### Get Movie Genres
```csharp
// Input: language (string - optional)
// Output: GenreContainer
var movieGenres = await client.GetMovieGenresAsync();
```

### Get TV Genres
```csharp
// Input: language (string - optional)
// Output: GenreContainer
var tvGenres = await client.GetTvGenresAsync();
```

## Certifications

### Get Movie Certifications
```csharp
// Input: None
// Output: CertificationsContainer
var movieCertifications = await client.GetMovieCertificationsAsync();
```

### Get TV Certifications
```csharp
// Input: None
// Output: CertificationsContainer
var tvCertifications = await client.GetTvCertificationsAsync();
```

## Configuration

### Get API Configuration
```csharp
// Input: None
// Output: APIConfiguration
var apiConfig = await client.GetAPIConfiguration();
```

### Get Countries
```csharp
// Input: None
// Output: List<Country>
var countries = await client.GetCountriesAsync();
```

### Get Languages
```csharp
// Input: None
// Output: List<Language>
var languages = await client.GetLanguagesAsync();
```

### Get Primary Translations
```csharp
// Input: None
// Output: List<string>
var primaryTranslations = await client.GetPrimaryTranslationsAsync();
```

### Get Timezones
```csharp
// Input: None
// Output: List<Timezones>
var timezones = await client.GetTimezonesAsync();
```

## Trending

### Get Trending Movies
```csharp
// Input: timeWindow (TimeWindow), page (int - optional), language (string - optional)
// Output: SearchContainer<SearchMovie>
var trendingMovies = await client.GetTrendingMoviesAsync(TimeWindow.Day);
```

### Get Trending TV Shows
```csharp
// Input: timeWindow (TimeWindow), page (int - optional), language (string - optional)
// Output: SearchContainer<SearchTv>
var trendingTvShows = await client.GetTrendingTvAsync(TimeWindow.Week);
```

### Get Trending People
```csharp
// Input: timeWindow (TimeWindow), page (int - optional), language (string - optional)
// Output: SearchContainer<SearchPerson>
var trendingPeople = await client.GetTrendingPeopleAsync(TimeWindow.Day);
```

### Get Trending All
```csharp
// Input: timeWindow (TimeWindow), page (int - optional), language (string - optional)
// Output: SearchContainer<SearchBase>
var trendingAll = await client.GetTrendingAllAsync(TimeWindow.Day);
```

## Find

### Find by External ID
```csharp
// Input: source (FindExternalSource), id (string), language (string - optional)
// Output: FindContainer
var findResults = await client.FindAsync(FindExternalSource.Imdb, "tt0111161");

// Available sources: Imdb, TvDb, FreebaseId, FreebaseMid, TvRage, Facebook, Twitter, Instagram
```

## Account (Requires User Session)

### Get Account Details
```csharp
// Input: None
// Output: AccountDetails
var accountDetails = await client.AccountGetDetailsAsync();
```

### Get Account Lists
```csharp
// Input: page (int - optional)
// Output: SearchContainer<AccountList>
var accountLists = await client.AccountGetListsAsync();
```

### Get Favorite Movies
```csharp
// Input: page (int - optional), sortBy (AccountSortBy - optional), language (string - optional)
// Output: SearchContainer<SearchMovie>
var favoriteMovies = await client.AccountGetFavoriteMoviesAsync();
```

### Get Favorite TV Shows
```csharp
// Input: page (int - optional), sortBy (AccountSortBy - optional), language (string - optional)
// Output: SearchContainer<SearchTv>
var favoriteTvShows = await client.AccountGetFavoriteTvAsync();
```

### Get Movie Watchlist
```csharp
// Input: page (int - optional), sortBy (AccountSortBy - optional), language (string - optional)
// Output: SearchContainer<SearchMovie>
var movieWatchlist = await client.AccountGetMovieWatchlistAsync();
```

### Get TV Watchlist
```csharp
// Input: page (int - optional), sortBy (AccountSortBy - optional), language (string - optional)
// Output: SearchContainer<SearchTv>
var tvWatchlist = await client.AccountGetTvWatchlistAsync();
```

### Get Rated Movies
```csharp
// Input: page (int - optional), sortBy (AccountSortBy - optional), language (string - optional)
// Output: SearchContainer<SearchMovie>
var ratedMovies = await client.AccountGetRatedMoviesAsync();
```

### Get Rated TV Shows
```csharp
// Input: page (int - optional), sortBy (AccountSortBy - optional), language (string - optional)
// Output: SearchContainer<SearchTv>
var ratedTvShows = await client.AccountGetRatedTvShowsAsync();
```

### Get Rated TV Episodes
```csharp
// Input: page (int - optional), sortBy (AccountSortBy - optional), language (string - optional)
// Output: SearchContainer<SearchTvEpisode>
var ratedEpisodes = await client.AccountGetRatedTvShowEpisodesAsync();
```

## Lists (Requires User Session)

### Get List Details
```csharp
// Input: listId (string), language (string - optional)
// Output: GenericList
var list = await client.GetListAsync("508629");
```

### Create List
```csharp
// Input: name (string), description (string), language (string - optional)
// Output: ListCreateReply
var createResult = await client.ListCreateAsync("My List", "A list of my favorite movies");
```

### Add Item to List
```csharp
// Input: listId (string), movieId (int)
// Output: PostReply
var addResult = await client.ListAddMovieAsync("508629", 550);
```

### Remove Item from List
```csharp
// Input: listId (string), movieId (int)
// Output: PostReply
var removeResult = await client.ListRemoveMovieAsync("508629", 550);
```

### Clear List
```csharp
// Input: listId (string), confirm (bool)
// Output: PostReply
var clearResult = await client.ListClearAsync("508629", true);
```

### Delete List
```csharp
// Input: listId (string)
// Output: PostReply
var deleteResult = await client.ListDeleteAsync("508629");
```

## Watch Providers

### Get Movie Watch Providers
```csharp
// Input: movieId (int)
// Output: SingleResultContainer<Dictionary<string, WatchProviders>>
var movieWatchProviders = await client.GetMovieWatchProvidersAsync(123);
```

### Get TV Watch Providers
```csharp
// Input: tvShowId (int)
// Output: SingleResultContainer<Dictionary<string, WatchProviders>>
var tvWatchProviders = await client.GetTvShowWatchProvidersAsync(456);
```

### Get Watch Provider Regions
```csharp
// Input: language (string - optional)
// Output: List<WatchProviderRegion>
var watchProviderRegions = await client.GetWatchProviderRegionsAsync();
```

### Get Movie Watch Provider List
```csharp
// Input: language (string - optional), watchRegion (string - optional)
// Output: WatchProviderContainer
var movieWatchProviders = await client.GetMovieWatchProviderListAsync();
```

### Get TV Watch Provider List
```csharp
// Input: language (string - optional), watchRegion (string - optional)
// Output: WatchProviderContainer
var tvWatchProviders = await client.GetTvWatchProviderListAsync();
```

## Reviews

### Get Review Details
```csharp
// Input: reviewId (string)
// Output: Review
var review = await client.GetReviewAsync("5013bc76760ee372cb00253e");
```

## Credits

### Get Credit Details
```csharp
// Input: creditId (string)
// Output: Credit
var credit = await client.GetCreditAsync("52fe4751c3a36847f8024f95");
```

## Changes

### Get Movie Changes
```csharp
// Input: movieId (int), startDate (DateTime - optional), endDate (DateTime - optional)
// Output: ChangesContainer
var movieChanges = await client.GetMovieChangesAsync(123);
```

### Get TV Changes
```csharp
// Input: tvShowId (int), startDate (DateTime - optional), endDate (DateTime - optional)
// Output: ChangesContainer
var tvChanges = await client.GetTvChangesAsync(456);
```

### Get Person Changes
```csharp
// Input: personId (int), startDate (DateTime - optional), endDate (DateTime - optional)
// Output: ChangesContainer
var personChanges = await client.GetPersonChangesAsync(789);
```

## Images

### Get Image Bytes
```csharp
// Input: size (string), filePath (string), useSsl (bool - optional)
// Output: byte[]
var imageBytes = await client.GetImageBytesAsync("w500", "/path/to/image.jpg");
```

### Get Image URL
```csharp
// Input: size (string), filePath (string), useSsl (bool - optional)
// Output: Uri
var imageUrl = client.GetImageUrl("w500", "/path/to/image.jpg");
```

## Error Handling

### Common Exceptions
```csharp
try
{
    var movie = await client.GetMovieAsync(123);
}
catch (UnauthorizedAccessException)
{
    // Invalid API key or insufficient permissions
}
catch (UserSessionRequiredException)
{
    // User session required for this operation
}
catch (GuestSessionRequiredException)
{
    // Guest session required for this operation
}
catch (NotFoundException)
{
    // Resource not found
}
catch (APIException ex)
{
    // General API error
    Console.WriteLine($"API Error: {ex.Message}");
}
```

### Rate Limiting
```csharp
// The client handles rate limiting automatically
// You can configure retry behavior if needed
```

## Result Models

This section shows the structure of the main result objects you'll receive from the API calls.

### Movie
```csharp
public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string OriginalTitle { get; set; }
    public string Overview { get; set; }
    public string BackdropPath { get; set; }
    public string PosterPath { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public int? Runtime { get; set; }
    public long Budget { get; set; }
    public long Revenue { get; set; }
    public double? Popularity { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public bool Adult { get; set; }
    public string Homepage { get; set; }
    public string ImdbId { get; set; }
    public string OriginalLanguage { get; set; }
    public string Status { get; set; }
    public string Tagline { get; set; }
    
    // Collections
    public List<Genre> Genres { get; set; }
    public List<ProductionCompany> ProductionCompanies { get; set; }
    public List<ProductionCountry> ProductionCountries { get; set; }
    public List<SpokenLanguage> SpokenLanguages { get; set; }
    public SearchCollection BelongsToCollection { get; set; }
    
    // Extra data (populated when requested)
    public Credits Credits { get; set; }
    public Images Images { get; set; }
    public ResultContainer<Video> Videos { get; set; }
    public KeywordsContainer Keywords { get; set; }
    public SearchContainer<ReviewBase> Reviews { get; set; }
    public SearchContainer<SearchMovie> Recommendations { get; set; }
    public SearchContainer<SearchMovie> Similar { get; set; }
    public AlternativeTitles AlternativeTitles { get; set; }
    public ExternalIdsMovie ExternalIds { get; set; }
    public ReleaseDatesContainer ReleaseDates { get; set; }
    public TranslationsContainer Translations { get; set; }
    public AccountState AccountStates { get; set; }
    public SearchContainer<ListResult> Lists { get; set; }
    public SingleResultContainer<Dictionary<string, WatchProviders>> WatchProviders { get; set; }
    public ChangesContainer Changes { get; set; }
}
```

### TvShow
```csharp
public class TvShow
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OriginalName { get; set; }
    public string Overview { get; set; }
    public string BackdropPath { get; set; }
    public string PosterPath { get; set; }
    public DateTime? FirstAirDate { get; set; }
    public DateTime? LastAirDate { get; set; }
    public bool InProduction { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public double? Popularity { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public bool Adult { get; set; }
    public string Homepage { get; set; }
    public string OriginalLanguage { get; set; }
    public int NumberOfEpisodes { get; set; }
    public int NumberOfSeasons { get; set; }
    
    // Collections
    public List<Genre> Genres { get; set; }
    public List<int> GenreIds { get; set; }
    public List<CreatedBy> CreatedBy { get; set; }
    public List<int> EpisodeRunTime { get; set; }
    public List<string> Languages { get; set; }
    public List<NetworkWithLogo> Networks { get; set; }
    public List<ProductionCompany> ProductionCompanies { get; set; }
    public List<ProductionCountry> ProductionCountries { get; set; }
    public List<SpokenLanguage> SpokenLanguages { get; set; }
    public List<string> OriginCountry { get; set; }
    public List<TvSeason> Seasons { get; set; }
    
    // Episode info
    public TvEpisodeBase LastEpisodeToAir { get; set; }
    public TvEpisodeBase NextEpisodeToAir { get; set; }
    
    // Extra data (populated when requested)
    public Credits Credits { get; set; }
    public CreditsAggregate AggregateCredits { get; set; }
    public Images Images { get; set; }
    public ResultContainer<Video> Videos { get; set; }
    public ResultContainer<Keyword> Keywords { get; set; }
    public SearchContainer<ReviewBase> Reviews { get; set; }
    public SearchContainer<SearchTv> Recommendations { get; set; }
    public SearchContainer<SearchTv> Similar { get; set; }
    public ResultContainer<AlternativeTitle> AlternativeTitles { get; set; }
    public ExternalIdsTvShow ExternalIds { get; set; }
    public TranslationsContainer Translations { get; set; }
    public AccountState AccountStates { get; set; }
    public ResultContainer<ContentRating> ContentRatings { get; set; }
    public ResultContainer<TvGroupCollection> EpisodeGroups { get; set; }
    public SingleResultContainer<Dictionary<string, WatchProviders>> WatchProviders { get; set; }
    public ChangesContainer Changes { get; set; }
}
```

### Person
```csharp
public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Biography { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime? Deathday { get; set; }
    public string PlaceOfBirth { get; set; }
    public string ProfilePath { get; set; }
    public PersonGender Gender { get; set; }
    public string KnownForDepartment { get; set; }
    public double Popularity { get; set; }
    public bool Adult { get; set; }
    public string Homepage { get; set; }
    public string ImdbId { get; set; }
    
    // Collections
    public List<string> AlsoKnownAs { get; set; }
    
    // Extra data (populated when requested)
    public MovieCredits MovieCredits { get; set; }
    public TvCredits TvCredits { get; set; }
    public CombinedCredits CombinedCredits { get; set; }
    public ProfileImages Images { get; set; }
    public ExternalIdsPerson ExternalIds { get; set; }
    public TranslationsContainer Translations { get; set; }
    public SearchContainer<TaggedImage> TaggedImages { get; set; }
    public ChangesContainer Changes { get; set; }
}
```

### SearchContainer<T>
```csharp
public class SearchContainer<T>
{
    public int Page { get; set; }
    public List<T> Results { get; set; }
    public int TotalPages { get; set; }
    public int TotalResults { get; set; }
}
```

### SearchMovie
```csharp
public class SearchMovie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string OriginalTitle { get; set; }
    public string Overview { get; set; }
    public string BackdropPath { get; set; }
    public string PosterPath { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public bool Adult { get; set; }
    public bool Video { get; set; }
    public double? Popularity { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public string OriginalLanguage { get; set; }
    public List<int> GenreIds { get; set; }
    public MediaType MediaType { get; set; } = MediaType.Movie;
}
```

### SearchTv
```csharp
public class SearchTv
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OriginalName { get; set; }
    public string Overview { get; set; }
    public string BackdropPath { get; set; }
    public string PosterPath { get; set; }
    public DateTime? FirstAirDate { get; set; }
    public double? Popularity { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public string OriginalLanguage { get; set; }
    public List<int> GenreIds { get; set; }
    public List<string> OriginCountry { get; set; }
    public MediaType MediaType { get; set; } = MediaType.Tv;
}
```

### SearchPerson
```csharp
public class SearchPerson
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ProfilePath { get; set; }
    public bool Adult { get; set; }
    public double? Popularity { get; set; }
    public string KnownForDepartment { get; set; }
    public List<KnownForBase> KnownFor { get; set; }
    public MediaType MediaType { get; set; } = MediaType.Person;
}
```

### Credits
```csharp
public class Credits
{
    public int Id { get; set; }
    public List<Cast> Cast { get; set; }
    public List<Crew> Crew { get; set; }
}
```

### Cast
```csharp
public class Cast
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OriginalName { get; set; }
    public string Character { get; set; }
    public string ProfilePath { get; set; }
    public int CastId { get; set; }
    public string CreditId { get; set; }
    public int Order { get; set; }
    public PersonGender Gender { get; set; }
    public bool Adult { get; set; }
    public string KnownForDepartment { get; set; }
    public float Popularity { get; set; }
}
```

### Crew
```csharp
public class Crew
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string OriginalName { get; set; }
    public string Job { get; set; }
    public string Department { get; set; }
    public string ProfilePath { get; set; }
    public string CreditId { get; set; }
    public PersonGender Gender { get; set; }
    public bool Adult { get; set; }
    public string KnownForDepartment { get; set; }
    public float Popularity { get; set; }
}
```

### Images
```csharp
public class Images
{
    public List<ImageData> Backdrops { get; set; }
    public List<ImageData> Posters { get; set; }
    public List<ImageData> Logos { get; set; }
}

public class ImagesWithId : Images
{
    public int Id { get; set; }
}
```

### ImageData
```csharp
public class ImageData
{
    public double AspectRatio { get; set; }
    public string FilePath { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }
    public string Iso_639_1 { get; set; } // Language code
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
}
```

### Video
```csharp
public class Video
{
    public string Id { get; set; }
    public string Key { get; set; }
    public string Name { get; set; }
    public string Site { get; set; } // YouTube, Vimeo, etc.
    public string Type { get; set; } // Trailer, Teaser, Clip, etc.
    public int Size { get; set; }
    public bool Official { get; set; }
    public DateTime PublishedAt { get; set; }
    public string Iso_3166_1 { get; set; } // Country code
    public string Iso_639_1 { get; set; } // Language code
}
```

### Genre
```csharp
public class Genre
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### AccountState
```csharp
public class AccountState
{
    public int Id { get; set; }
    public bool Favorite { get; set; }
    public bool Watchlist { get; set; }
    public double? Rating { get; set; }
}
```

### ResultContainer<T>
```csharp
public class ResultContainer<T>
{
    public int Id { get; set; }
    public List<T> Results { get; set; }
}
```

### SingleResultContainer<T>
```csharp
public class SingleResultContainer<T>
{
    public int Id { get; set; }
    public T Results { get; set; }
}
```

### WatchProviders
```csharp
public class WatchProviders
{
    public string Link { get; set; }
    public List<WatchProviderItem> FlatRate { get; set; }
    public List<WatchProviderItem> Rent { get; set; }
    public List<WatchProviderItem> Buy { get; set; }
}

public class WatchProviderItem
{
    public int ProviderId { get; set; }
    public string ProviderName { get; set; }
    public string LogoPath { get; set; }
    public int DisplayPriority { get; set; }
}
```

### ExternalIds
```csharp
public class ExternalIdsMovie
{
    public int Id { get; set; }
    public string ImdbId { get; set; }
    public string FacebookId { get; set; }
    public string InstagramId { get; set; }
    public string TwitterId { get; set; }
    public string WikidataId { get; set; }
}

public class ExternalIdsTvShow
{
    public int Id { get; set; }
    public string ImdbId { get; set; }
    public string FacebookId { get; set; }
    public string InstagramId { get; set; }
    public string TwitterId { get; set; }
    public string WikidataId { get; set; }
    public int? TvdbId { get; set; }
    public int? TvrageId { get; set; }
}

public class ExternalIdsPerson
{
    public int Id { get; set; }
    public string ImdbId { get; set; }
    public string FacebookId { get; set; }
    public string InstagramId { get; set; }
    public string TwitterId { get; set; }
    public string WikidataId { get; set; }
    public int? TvdbId { get; set; }
    public int? TvrageId { get; set; }
}
```

### Review
```csharp
public class ReviewBase
{
    public string Id { get; set; }
    public string Author { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public string Url { get; set; }
}

public class Review : ReviewBase
{
    public int MediaId { get; set; }
    public string MediaTitle { get; set; }
    public MediaType MediaType { get; set; }
    public string Iso_639_1 { get; set; }
}
```

### Collection
```csharp
public class Collection
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Overview { get; set; }
    public string BackdropPath { get; set; }
    public string PosterPath { get; set; }
    public List<SearchMovie> Parts { get; set; }
    
    // Extra data
    public Images Images { get; set; }
    public TranslationsContainer Translations { get; set; }
}
```

### Company
```csharp
public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Headquarters { get; set; }
    public string Homepage { get; set; }
    public string LogoPath { get; set; }
    public string OriginCountry { get; set; }
    public Company ParentCompany { get; set; }
    
    // Extra data
    public Images Images { get; set; }
    public AlternativeNames AlternativeNames { get; set; }
}
```

### Network
```csharp
public class Network
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Headquarters { get; set; }
    public string Homepage { get; set; }
    public string LogoPath { get; set; }
    public string OriginCountry { get; set; }
    
    // Extra data
    public Images Images { get; set; }
    public AlternativeNames AlternativeNames { get; set; }
}
```

### Keyword
```csharp
public class Keyword
{
    public int Id { get; set; }
    public string Name { get; set; }
}
```

### TvSeason
```csharp
public class TvSeason
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Overview { get; set; }
    public DateTime? AirDate { get; set; }
    public int EpisodeCount { get; set; }
    public string PosterPath { get; set; }
    public int SeasonNumber { get; set; }
    public double? VoteAverage { get; set; }
    public List<TvEpisode> Episodes { get; set; }
    
    // Extra data
    public CreditsWithGuestStars Credits { get; set; }
    public Images Images { get; set; }
    public ResultContainer<Video> Videos { get; set; }
    public ExternalIdsTvSeason ExternalIds { get; set; }
}
```

### TvEpisode
```csharp
public class TvEpisode
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Overview { get; set; }
    public DateTime? AirDate { get; set; }
    public int EpisodeNumber { get; set; }
    public string ProductionCode { get; set; }
    public int? Runtime { get; set; }
    public int SeasonNumber { get; set; }
    public int ShowId { get; set; }
    public string StillPath { get; set; }
    public double? VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public List<CrewBase> Crew { get; set; }
    public List<GuestStar> GuestStars { get; set; }
    
    // Extra data
    public CreditsWithGuestStars Credits { get; set; }
    public Images Images { get; set; }
    public ResultContainer<Video> Videos { get; set; }
    public ExternalIdsTvEpisode ExternalIds { get; set; }
    public TranslationsContainer Translations { get; set; }
    public TvEpisodeAccountState AccountStates { get; set; }
}
```

### Error Types
```csharp
public class APIException : Exception
{
    public int StatusCode { get; set; }
    public string StatusMessage { get; set; }
}

public class NotFoundException : APIException { }
public class UserSessionRequiredException : Exception { }
public class GuestSessionRequiredException : Exception { }
public class UnauthorizedAccessException : Exception { }
```

### Enums
```csharp
public enum MediaType
{
    Movie,
    Tv,
    Person,
    Collection,
    TvEpisode,
    TvSeason
}

public enum PersonGender
{
    NotSpecified = 0,
    Female = 1,
    Male = 2,
    NonBinary = 3
}

public enum SessionType
{
    Unassigned,
    GuestSession,
    UserSession
}

public enum TimeWindow
{
    Day,
    Week
}

public enum FindExternalSource
{
    Imdb,
    TvDb,
    FreebaseId,
    FreebaseMid,
    TvRage,
    Facebook,
    Twitter,
    Instagram
}
```

## Advanced Usage

### Custom Serializer
```csharp
using TMDbLib.Utilities.Serializer;

// Use custom serializer
var customSerializer = new TMDbJsonSerializer();
var client = new TMDbClient("API_KEY", serializer: customSerializer);
```

### Proxy Support
```csharp
var proxy = new WebProxy("http://proxy.example.com:8080");
var client = new TMDbClient("API_KEY", proxy: proxy);
```

### Cancellation Tokens
```csharp
// All methods support cancellation tokens
var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
var movie = await client.GetMovieAsync(123, cancellationToken: cancellationToken);
```

## Enums and Constants

### TimeWindow
- `Day` - Trending for the day
- `Week` - Trending for the week

### SessionType
- `Unassigned` - No session
- `GuestSession` - Guest session (limited access)
- `UserSession` - User session (full access)

### MovieMethods (Flags)
- `AccountStates`, `AlternativeTitles`, `Changes`, `Credits`, `Images`, `Keywords`, `Lists`, `Recommendations`, `ReleaseDates`, `Reviews`, `Similar`, `Translations`, `Videos`, `WatchProviders`

### TvShowMethods (Flags)
- `AccountStates`, `AlternativeTitles`, `Changes`, `ContentRatings`, `Credits`, `EpisodeGroups`, `ExternalIds`, `Images`, `Keywords`, `Recommendations`, `Reviews`, `Similar`, `Translations`, `Videos`, `WatchProviders`

### PersonMethods (Flags)
- `Changes`, `CombinedCredits`, `ExternalIds`, `Images`, `MovieCredits`, `TaggedImages`, `Translations`, `TvCredits`

### FindExternalSource
- `Imdb`, `TvDb`, `FreebaseId`, `FreebaseMid`, `TvRage`, `Facebook`, `Twitter`, `Instagram`

This guide covers all the major functionality of the TMDbLib client. Each method includes clear input parameters and return types to help you integrate the library into your projects effectively. 