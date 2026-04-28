using System.Collections;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Certifications;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.Configuration;
using TMDbLib.Objects.Credit;
using TMDbLib.Objects.Discover;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Genres;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.RestRequests;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Timezones;
using TMDbLib.Objects.Trending;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Utilities.JsonSerializerContexts;

[JsonSourceGenerationOptions]
// Account
[JsonSerializable(typeof(Avatar))]
[JsonSerializable(typeof(Gravatar))]
[JsonSerializable(typeof(AccountSortBy))]
[JsonSerializable(typeof(AccountDetails))]

// Authentication
[JsonSerializable(typeof(SessionType))]
[JsonSerializable(typeof(Token))]
[JsonSerializable(typeof(UserSession))]
[JsonSerializable(typeof(GuestSession))]

// Certifications
[JsonSerializable(typeof(CertificationItem))]
[JsonSerializable(typeof(CertificationsContainer))]

// Changes
[JsonSerializable(typeof(ChangeItemBase))]
[JsonSerializable(typeof(ChangeItemAdded))]
[JsonSerializable(typeof(ChangeItemCreated))]
[JsonSerializable(typeof(ChangeItemDeleted))]
[JsonSerializable(typeof(ChangeItemDestroyed))]
[JsonSerializable(typeof(ChangeItemUpdated))]
[JsonSerializable(typeof(ChangeAction))]

// Collections
[JsonSerializable(typeof(Collection))]
[JsonSerializable(typeof(CollectionMethods))]

// Companiers
[JsonSerializable(typeof(Company))]
[JsonSerializable(typeof(CompanyMethods))]

// Configuration
[JsonSerializable(typeof(APIConfiguration))]
[JsonSerializable(typeof(APIConfigurationImages))]

// Countries
[JsonSerializable(typeof(Objects.Countries.Country), TypeInfoPropertyName = "Country")]

// Credit
[JsonSerializable(typeof(Credit))]
[JsonSerializable(typeof(CreditEpisode))]
[JsonSerializable(typeof(CreditMedia))]
[JsonSerializable(typeof(CreditSeason))]

// Discover
[JsonSerializable(typeof(DictionaryBase))]
[JsonSerializable(typeof(DiscoverTv))]
[JsonSerializable(typeof(DiscoverMovie))]
[JsonSerializable(typeof(WatchMonetizationType))]
[JsonSerializable(typeof(DiscoverTvShowSortBy))]
[JsonSerializable(typeof(DiscoverMovieSortBy))]

// Exceptions
[JsonSerializable(typeof(TMDbStatusMessage))]

// Find
[JsonSerializable(typeof(FindContainer))]
[JsonSerializable(typeof(FindPerson))]
[JsonSerializable(typeof(FindTvSeason))]
[JsonSerializable(typeof(FindExternalSource))]

// General
[JsonSerializable(typeof(AccountState))]
[JsonSerializable(typeof(AlternativeName))]
[JsonSerializable(typeof(AlternativeNames))]
[JsonSerializable(typeof(AlternativeTitle))]
[JsonSerializable(typeof(ConfigImageTypes))]
[JsonSerializable(typeof(CreditType))]
[JsonSerializable(typeof(CrewBase))]
[JsonSerializable(typeof(Crew))]
[JsonSerializable(typeof(CrewAggregate))]
[JsonSerializable(typeof(CrewJob))]
[JsonSerializable(typeof(DateRange))]
[JsonSerializable(typeof(ExternalIds))]
[JsonSerializable(typeof(ExternalIdsMovie))]
[JsonSerializable(typeof(ExternalIdsTvShow))]
[JsonSerializable(typeof(ExternalIdsTvSeason))]
[JsonSerializable(typeof(ExternalIdsTvEpisode))]
[JsonSerializable(typeof(ExternalIdsPerson))]
[JsonSerializable(typeof(Genre))]
[JsonSerializable(typeof(ImageData))]
[JsonSerializable(typeof(Images))]
[JsonSerializable(typeof(ImagesWithId))]
[JsonSerializable(typeof(Job))]
[JsonSerializable(typeof(Keyword))]
[JsonSerializable(typeof(MediaType))]
[JsonSerializable(typeof(PosterImages))]
[JsonSerializable(typeof(PostReply))]
[JsonSerializable(typeof(ProductionCompany))]
[JsonSerializable(typeof(ProductionCountry))]
[JsonSerializable(typeof(SortOrder))]
[JsonSerializable(typeof(StillImages))]
[JsonSerializable(typeof(TMDbConfig))]
[JsonSerializable(typeof(Translation))]
[JsonSerializable(typeof(TranslationData))]
[JsonSerializable(typeof(TranslationsContainer))]
[JsonSerializable(typeof(TranslationsContainerTv))]
[JsonSerializable(typeof(Video))]
[JsonSerializable(typeof(WatchProviderItem))]
[JsonSerializable(typeof(WatchProviderRegion))]
[JsonSerializable(typeof(WatchProviders))]

// Genres
[JsonSerializable(typeof(GenreContainer))]

// Languages
[JsonSerializable(typeof(Language))]

// Lists
[JsonSerializable(typeof(TMDbList<int>))]
[JsonSerializable(typeof(TMDbList<string>))]
[JsonSerializable(typeof(ListStatus))]
[JsonSerializable(typeof(AccountList))]
[JsonSerializable(typeof(GenericList))]
[JsonSerializable(typeof(ListCreateReply))]

// Movies
[JsonSerializable(typeof(AlternativeTitle))]
[JsonSerializable(typeof(Objects.Movies.Cast), TypeInfoPropertyName = "MovieCast")]
[JsonSerializable(typeof(Objects.Movies.Country), TypeInfoPropertyName = "MovieCountry")]
[JsonSerializable(typeof(Objects.Movies.Credits), TypeInfoPropertyName = "MovieCredits")]
[JsonSerializable(typeof(KeywordsContainer))]
[JsonSerializable(typeof(ListResult))]
[JsonSerializable(typeof(Movie))]
[JsonSerializable(typeof(MovieMethods))]
[JsonSerializable(typeof(ReleaseDateItem))]
[JsonSerializable(typeof(ReleaseDatesContainer))]
[JsonSerializable(typeof(ReleaseDateType))]
[JsonSerializable(typeof(Releases))]
[JsonSerializable(typeof(SpokenLanguage))]

// People
[JsonSerializable(typeof(CombinedCreditsCastBase))]
[JsonSerializable(typeof(CombinedCreditsCrewBase))]
[JsonSerializable(typeof(CombinedCredits))]
[JsonSerializable(typeof(CombinedCreditsCastTv))]
[JsonSerializable(typeof(CombinedCreditsCastMovie))]
[JsonSerializable(typeof(CombinedCreditsCrewTv))]
[JsonSerializable(typeof(CombinedCreditsCrewMovie))]
[JsonSerializable(typeof(MovieCredits), TypeInfoPropertyName = "PeopleCredits")]
[JsonSerializable(typeof(MovieJob))]
[JsonSerializable(typeof(MovieRole))]
[JsonSerializable(typeof(Person))]
[JsonSerializable(typeof(PersonGender))]
[JsonSerializable(typeof(PersonMethods))]
[JsonSerializable(typeof(ProfileImages))]
[JsonSerializable(typeof(TaggedImage))]
[JsonSerializable(typeof(TvCredits))]
[JsonSerializable(typeof(TvJob))]
[JsonSerializable(typeof(TvRole))]

// Reviews
[JsonSerializable(typeof(AuthorDetails))]
[JsonSerializable(typeof(ReviewBase))]
[JsonSerializable(typeof(Review))]

// Search
[JsonSerializable(typeof(AccountSearchTv))]
[JsonSerializable(typeof(AccountSearchTvEpisode))]
[JsonSerializable(typeof(KnownForBase))]
[JsonSerializable(typeof(KnownForMovie))]
[JsonSerializable(typeof(KnownForTv))]
[JsonSerializable(typeof(SearchBase))]
[JsonSerializable(typeof(SearchCollection))]
[JsonSerializable(typeof(SearchCompany))]
[JsonSerializable(typeof(SearchKeyword))]
[JsonSerializable(typeof(SearchMovieTvBase))]
[JsonSerializable(typeof(SearchMovie))]
[JsonSerializable(typeof(SearchTvShowWithRating))]
[JsonSerializable(typeof(SearchPerson))]
[JsonSerializable(typeof(SearchTv))]
[JsonSerializable(typeof(SearchTvSeason))]
[JsonSerializable(typeof(SearchTvEpisode))]
[JsonSerializable(typeof(SearchTvShowWithRating))]
[JsonSerializable(typeof(TvSeasonEpisode))]

// Timezones
[JsonSerializable(typeof(Timezones))]

// Trending
[JsonSerializable(typeof(TimeWindow))]

// TvShows
[JsonSerializable(typeof(CastBase))]
[JsonSerializable(typeof(Objects.TvShows.Cast), TypeInfoPropertyName = "TvShowCast")]
[JsonSerializable(typeof(CastAggregate))]
[JsonSerializable(typeof(CastRole))]
[JsonSerializable(typeof(ContentRating))]
[JsonSerializable(typeof(CreatedBy))]
[JsonSerializable(typeof(Objects.TvShows.Credits), TypeInfoPropertyName = "TvShowCredits")]
[JsonSerializable(typeof(CreditsAggregate))]
[JsonSerializable(typeof(CreditsWithGuestStars))]
[JsonSerializable(typeof(NetworkBase))]
[JsonSerializable(typeof(Network))]
[JsonSerializable(typeof(NetworkWithLogo))]
[JsonSerializable(typeof(TvAccountState))]
[JsonSerializable(typeof(TvEpisodeBase))]
[JsonSerializable(typeof(TvEpisode))]
[JsonSerializable(typeof(TvEpisodeAccountState))]
[JsonSerializable(typeof(TvEpisodeAccountStateWithNumber))]
[JsonSerializable(typeof(TvEpisodeInfo))]
[JsonSerializable(typeof(TvEpisodeMethods))]
[JsonSerializable(typeof(TvEpisodeWithRating))]
[JsonSerializable(typeof(TvGroup))]
[JsonSerializable(typeof(TvGroupCollection))]
[JsonSerializable(typeof(TvGroupEpisode))]
[JsonSerializable(typeof(TvGroupType))]
[JsonSerializable(typeof(TvSeason))]
[JsonSerializable(typeof(TvSeasonMethods))]
[JsonSerializable(typeof(TvShow))]
[JsonSerializable(typeof(TvShowListType))]
[JsonSerializable(typeof(TvShowMethods))]

// SearchContainers
[JsonSerializable(typeof(SearchContainer<ListResult>))]
[JsonSerializable(typeof(SearchContainer<ReviewBase>))]
[JsonSerializable(typeof(SearchContainer<SearchBase>))]
[JsonSerializable(typeof(SearchContainer<SearchMovieTvBase>))]
[JsonSerializable(typeof(SearchContainer<SearchTv>))]
[JsonSerializable(typeof(SearchContainer<AccountSearchTv>))]
[JsonSerializable(typeof(SearchContainer<SearchTvShowWithRating>))]
[JsonSerializable(typeof(SearchContainer<SearchTvSeason>))]
[JsonSerializable(typeof(SearchContainer<SearchTvEpisode>))]
[JsonSerializable(typeof(SearchContainer<SearchMovie>))]
[JsonSerializable(typeof(SearchContainer<SearchMovieWithRating>))]
[JsonSerializable(typeof(SearchContainer<SearchPerson>))]
[JsonSerializable(typeof(SearchContainer<SearchCollection>))]
[JsonSerializable(typeof(SearchContainer<SearchKeyword>))]
[JsonSerializable(typeof(SearchContainer<AccountSearchTvEpisode>))]
[JsonSerializable(typeof(SearchContainer<AccountList>))]
[JsonSerializable(typeof(SearchContainer<ChangesListItem>))]
[JsonSerializable(typeof(SearchContainer<TvEpisodeWithRating>))]
[JsonSerializable(typeof(SearchContainer<SearchCompany>))]
[JsonSerializable(typeof(NetworkLogos))]

// ResultContainers
[JsonSerializable(typeof(ResultContainer<TaggedImage>))]
[JsonSerializable(typeof(ResultContainer<ReleaseDatesContainer>))]
[JsonSerializable(typeof(ResultContainer<Video>))]
[JsonSerializable(typeof(ResultContainer<TvEpisodeInfo>))]
[JsonSerializable(typeof(ResultContainer<TvEpisodeAccountStateWithNumber>))]
[JsonSerializable(typeof(ResultContainer<AlternativeTitle>))]
[JsonSerializable(typeof(ResultContainer<ContentRating>))]
[JsonSerializable(typeof(ResultContainer<Keyword>))]
[JsonSerializable(typeof(ResultContainer<WatchProviderRegion>))]
[JsonSerializable(typeof(ResultContainer<WatchProviderItem>))]
[JsonSerializable(typeof(ResultContainer<TvGroupCollection>))]

// Container
[JsonSerializable(typeof(SearchContainerWithDates<SearchMovie>))]
[JsonSerializable(typeof(SearchContainerWithId<SearchMovie>))]
[JsonSerializable(typeof(SearchContainerWithId<ListResult>))]
[JsonSerializable(typeof(SearchContainerWithId<ReviewBase>))]
[JsonSerializable(typeof(SearchContainerWithId<TaggedImage>))]
[JsonSerializable(typeof(SingleResultContainer<Dictionary<string, WatchProviders>>))]

// RestRequests
[JsonSerializable(typeof(IBody))]
[JsonSerializable(typeof(ListCreateBody))]
[JsonSerializable(typeof(MovieIdBody))]
[JsonSerializable(typeof(RatingBody))]

// Arrays
[JsonSerializable(typeof(int[]))]
[JsonSerializable(typeof(List<int>))]
[JsonSerializable(typeof(List<KnownForBase>))]
[JsonSerializable(typeof(List<Objects.TvShows.Cast>), TypeInfoPropertyName = "TvShowCastList")]
[JsonSerializable(typeof(List<Objects.Movies.Cast>), TypeInfoPropertyName = "MovieCastList")]
[JsonSerializable(typeof(List<Objects.Countries.Country>),  TypeInfoPropertyName = "CountriesCountryList")]
[JsonSerializable(typeof(List<Job>))]
[JsonSerializable(typeof(List<Language>))]
[JsonSerializable(typeof(List<Dictionary<string, List<string>>>))]
[JsonSerializable(typeof(HashSet<int>))]
[JsonSerializable(typeof(IEnumerable<int>))]
[JsonSerializable(typeof(Dictionary<string, WatchProviders>))]
internal sealed partial class TmdbJsonSerializerContext : JsonSerializerContext;
