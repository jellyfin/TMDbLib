using System.Collections.Generic;
using System.Text.Json.Serialization;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Certifications;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.Companies;
using TMDbLib.Objects.Configuration;
using TMDbLib.Objects.Countries;
using TMDbLib.Objects.Credit;
using TMDbLib.Objects.Exceptions;
using TMDbLib.Objects.Find;
using TMDbLib.Objects.General;
using TMDbLib.Objects.General.Schema;
using TMDbLib.Objects.Genres;
using TMDbLib.Objects.Languages;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Requests;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.Timezones;
using TMDbLib.Objects.TvShows;

namespace TMDbLib.Utilities.Serializer;

[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true)]

// Top-level response types
[JsonSerializable(typeof(APIConfiguration))]
[JsonSerializable(typeof(AccountDetails))]
[JsonSerializable(typeof(AccountState))]
[JsonSerializable(typeof(AlternativeNames))]
[JsonSerializable(typeof(CertificationsContainer))]
[JsonSerializable(typeof(Collection))]
[JsonSerializable(typeof(Company))]
[JsonSerializable(typeof(Credit))]
[JsonSerializable(typeof(FindContainer))]
[JsonSerializable(typeof(GenericList))]
[JsonSerializable(typeof(GenreContainer))]
[JsonSerializable(typeof(GuestSession))]
[JsonSerializable(typeof(Keyword))]
[JsonSerializable(typeof(ListCreateReply))]
[JsonSerializable(typeof(ListStatus))]
[JsonSerializable(typeof(Movie))]
[JsonSerializable(typeof(Network))]
[JsonSerializable(typeof(NetworkLogos))]
[JsonSerializable(typeof(Person))]
[JsonSerializable(typeof(PostReply))]
[JsonSerializable(typeof(Review))]
[JsonSerializable(typeof(TMDbConfig))]
[JsonSerializable(typeof(TMDbStatusMessage))]
[JsonSerializable(typeof(Token))]
[JsonSerializable(typeof(TvAccountState))]
[JsonSerializable(typeof(TvEpisode))]
[JsonSerializable(typeof(TvEpisodeAccountState))]
[JsonSerializable(typeof(TvEpisodeAccountStateWithNumber))]
[JsonSerializable(typeof(TvGroupCollection))]
[JsonSerializable(typeof(TvSeason))]
[JsonSerializable(typeof(TvShow))]
[JsonSerializable(typeof(TranslationsContainerTv))]
[JsonSerializable(typeof(UserSession))]

// Closed generic containers
[JsonSerializable(typeof(List<TMDbLib.Objects.Countries.Country>), TypeInfoPropertyName = "ListCountryEntry")]
[JsonSerializable(typeof(List<Job>))]
[JsonSerializable(typeof(List<Language>))]
[JsonSerializable(typeof(List<TimezoneEntry>))]
[JsonSerializable(typeof(List<string>))]
[JsonSerializable(typeof(ResultContainer<TvEpisodeAccountStateWithNumber>))]
[JsonSerializable(typeof(ResultContainer<TvEpisodeInfo>))]
[JsonSerializable(typeof(ResultContainer<WatchProviderItem>))]
[JsonSerializable(typeof(ResultContainer<WatchProviderRegion>))]
[JsonSerializable(typeof(SearchContainer<AccountList>))]
[JsonSerializable(typeof(SearchContainer<AccountSearchTv>))]
[JsonSerializable(typeof(SearchContainer<AccountSearchTvEpisode>))]
[JsonSerializable(typeof(SearchContainer<ChangesListItem>))]
[JsonSerializable(typeof(SearchContainer<SearchCollection>))]
[JsonSerializable(typeof(SearchContainer<SearchCompany>))]
[JsonSerializable(typeof(SearchContainer<SearchKeyword>))]
[JsonSerializable(typeof(SearchContainer<SearchMovie>))]
[JsonSerializable(typeof(SearchContainer<SearchMovieWithRating>))]
[JsonSerializable(typeof(SearchContainer<SearchPerson>))]
[JsonSerializable(typeof(SearchContainer<SearchTv>))]
[JsonSerializable(typeof(SearchContainer<SearchTvShowWithRating>))]
[JsonSerializable(typeof(SearchContainer<TmdbEntity>))]
[JsonSerializable(typeof(SearchContainer<TvEpisodeWithRating>))]
[JsonSerializable(typeof(SearchContainerWithDates<SearchMovie>))]
[JsonSerializable(typeof(SearchContainerWithId<ListResult>))]
[JsonSerializable(typeof(SearchContainerWithId<TMDbLib.Objects.People.TaggedImage>))]
[JsonSerializable(typeof(SearchContainerWithId<ReviewBase>))]
[JsonSerializable(typeof(SearchContainerWithId<SearchMovie>))]

// Enums used as discriminators by polymorphic converters
[JsonSerializable(typeof(MediaType))]
[JsonSerializable(typeof(TMDbLib.Objects.Changes.ChangeAction))]

// Polymorphic subtypes resolved by custom converters
[JsonSerializable(typeof(SearchMovie))]
[JsonSerializable(typeof(SearchTv))]
[JsonSerializable(typeof(SearchPerson))]
[JsonSerializable(typeof(SearchTvEpisode))]
[JsonSerializable(typeof(SearchTvSeason))]
[JsonSerializable(typeof(SearchCollection))]
[JsonSerializable(typeof(TmdbMovieSummary))]
[JsonSerializable(typeof(TmdbTvSummary))]
[JsonSerializable(typeof(CombinedCreditsCastMovie))]
[JsonSerializable(typeof(CombinedCreditsCastTv))]
[JsonSerializable(typeof(CombinedCreditsCrewMovie))]
[JsonSerializable(typeof(CombinedCreditsCrewTv))]
[JsonSerializable(typeof(ChangeItemAdded))]
[JsonSerializable(typeof(ChangeItemCreated))]
[JsonSerializable(typeof(ChangeItemUpdated))]
[JsonSerializable(typeof(ChangeItemDeleted))]
[JsonSerializable(typeof(ChangeItemDestroyed))]
[JsonSerializable(typeof(TaggedImage))]

// Disambiguators for types whose simple name collides across namespaces.
[JsonSerializable(typeof(TMDbLib.Objects.Movies.Credits), TypeInfoPropertyName = "MoviesCredits")]
[JsonSerializable(typeof(TMDbLib.Objects.TvShows.Credits), TypeInfoPropertyName = "TvShowsCredits")]
[JsonSerializable(typeof(TMDbLib.Objects.Movies.Cast), TypeInfoPropertyName = "MoviesCast")]
[JsonSerializable(typeof(TMDbLib.Objects.TvShows.Cast), TypeInfoPropertyName = "TvShowsCast")]
[JsonSerializable(typeof(TMDbLib.Objects.Movies.Country), TypeInfoPropertyName = "MoviesCountry")]
[JsonSerializable(typeof(TMDbLib.Objects.Countries.Country), TypeInfoPropertyName = "CountriesCountry")]
[JsonSerializable(typeof(List<TMDbLib.Objects.Movies.Cast>), TypeInfoPropertyName = "ListMoviesCast")]
[JsonSerializable(typeof(List<TMDbLib.Objects.TvShows.Cast>), TypeInfoPropertyName = "ListTvShowsCast")]

// Request body DTOs
[JsonSerializable(typeof(MediaIdRequest))]
[JsonSerializable(typeof(CreateListRequest))]
[JsonSerializable(typeof(RatingRequest))]
[JsonSerializable(typeof(FavoriteRequest))]
[JsonSerializable(typeof(WatchlistRequest))]
[JsonSerializable(typeof(SessionIdRequest))]
[JsonSerializable(typeof(AccessTokenRequest))]
internal partial class TMDbJsonContext : JsonSerializerContext;
