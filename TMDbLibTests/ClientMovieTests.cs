using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.Changes;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Cast = TMDbLib.Objects.Movies.Cast;
using Credits = TMDbLib.Objects.Movies.Credits;

namespace TMDbLibTests
{
    public class ClientMovieTests : TestBase
    {
        private static Dictionary<MovieMethods, Func<Movie, object>> _methods;

        public ClientMovieTests()
        {
            _methods = new Dictionary<MovieMethods, Func<Movie, object>>
            {
                [MovieMethods.AlternativeTitles] = movie => movie.AlternativeTitles,
                [MovieMethods.Credits] = movie => movie.Credits,
                [MovieMethods.Images] = movie => movie.Images,
                [MovieMethods.Keywords] = movie => movie.Keywords,
                [MovieMethods.Releases] = movie => movie.Releases,
                [MovieMethods.Videos] = movie => movie.Videos,
                [MovieMethods.Translations] = movie => movie.Translations,
                [MovieMethods.Similar] = movie => movie.Similar,
                [MovieMethods.Reviews] = movie => movie.Reviews,
                [MovieMethods.Lists] = movie => movie.Lists,
                [MovieMethods.Changes] = movie => movie.Changes,
                [MovieMethods.AccountStates] = movie => movie.AccountStates,
                [MovieMethods.ReleaseDates] = movie => movie.ReleaseDates,
                [MovieMethods.Recommendations] = movie => movie.Recommendations,
                [MovieMethods.ExternalIds] = movie => movie.ExternalIds,
                [MovieMethods.WatchProviders] = movie => movie.WatchProviders
            };
        }

        [Fact]
        public async void TestMoviesExtrasNone()
        {
            Movie movie = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard);

            Assert.NotNull(movie);

            // TODO: Test all properties
            Assert.Equal("A Good Day to Die Hard", movie.Title);

            // Test all extras, ensure none of them exist
            foreach (Func<Movie, object> selector in _methods.Values)
            {
                Assert.Null(selector(movie));
            }
        }

        [Fact]
        public async void TestMoviesExtrasExclusive()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            await TestMethodsHelper.TestGetExclusive(_methods, extras => TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard, extras));
        }

        [Fact]
        public async Task TestMoviesImdbExtrasAllAsync()
        {
            Dictionary<MovieMethods, Func<Movie, object>> tmpMethods = new Dictionary<MovieMethods, Func<Movie, object>>(_methods);
            tmpMethods.Remove(MovieMethods.Videos);

            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            await TMDbClient.MovieSetRatingAsync(IdHelper.TheDarkKnightRises, 5);

            await TestMethodsHelper.TestGetAll(tmpMethods, combined => TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRisesImdb, combined));
        }

        [Fact]
        public async void TestMoviesExtrasAll()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            await TestMethodsHelper.TestGetAll(_methods, combined => TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard, combined));
        }

        [Fact]
        public async void TestMoviesLanguage()
        {
            Movie movie = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard);
            Movie movieItalian = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard, "it");

            Assert.NotNull(movie);
            Assert.NotNull(movieItalian);

            Assert.Equal("A Good Day to Die Hard", movie.Title);
            Assert.NotEqual(movie.Title, movieItalian.Title);

            // Test all extras, ensure none of them exist
            foreach (Func<Movie, object> selector in _methods.Values)
            {
                Assert.Null(selector(movie));
                Assert.Null(selector(movieItalian));
            }
        }

        [Fact]
        public async void TestMoviesGetMovieAlternativeTitles()
        {
            AlternativeTitles respUs = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "US");
            Assert.NotNull(respUs);

            AlternativeTitles respFrench = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "FR");
            Assert.NotNull(respFrench);

            Assert.DoesNotContain(respUs.Titles, s => s.Title == "Duro de matar 5");
            Assert.Contains(respFrench.Titles, s => s.Title == "Die Hard 5 - Belle Journée Pour mourir");

            Assert.True(respUs.Titles.All(s => s.Iso_3166_1 == "US"));
            Assert.True(respFrench.Titles.All(s => s.Iso_3166_1 == "FR"));
        }

        [Fact]
        public async void TestMoviesGetMovieReleaseDates()
        {
            ResultContainer<ReleaseDatesContainer> resp = await TMDbClient.GetMovieReleaseDatesAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            ReleaseDatesContainer releasesUs = resp.Results.SingleOrDefault(s => s.Iso_3166_1 == "US");
            Assert.NotNull(releasesUs);
            Assert.Single(releasesUs.ReleaseDates);

            ReleaseDateItem singleRelease = releasesUs.ReleaseDates.First();

            Assert.Equal("R", singleRelease.Certification);
            Assert.Equal(string.Empty, singleRelease.Iso_639_1);
            Assert.Equal(string.Empty, singleRelease.Note);
            Assert.Equal(DateTime.Parse("2013-02-14T00:00:00.000Z").ToUniversalTime(), singleRelease.ReleaseDate);
            Assert.Equal(ReleaseDateType.Theatrical, singleRelease.Type);
        }

        [Fact]
        public async void TestMoviesGetMovieAlternativeTitlesCountry()
        {
            AlternativeTitles respUs = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "US");
            Assert.NotNull(respUs);

            TMDbClient.DefaultCountry = "US";

            AlternativeTitles respUs2 = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(respUs2);

            Assert.Equal(respUs.Titles.Count, respUs2.Titles.Count);
        }

        [Fact]
        public async void TestMoviesGetMovieCasts()
        {
            Credits resp = await TMDbClient.GetMovieCreditsAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            Cast cast = resp.Cast.SingleOrDefault(s => s.Name == "Bruce Willis");
            Assert.NotNull(cast);

            Assert.Equal(1, cast.CastId);
            Assert.Equal("John McClane", cast.Character);
            Assert.NotEmpty(cast.CreditId);
            Assert.Equal(62, cast.Id);
            Assert.Equal("Bruce Willis", cast.Name);
            Assert.Equal(0, cast.Order);
            Assert.True(cast.Popularity > 0);
            Assert.Equal("Bruce Willis", cast.OriginalName);
            Assert.Equal("Acting", cast.KnownForDepartment);
            Assert.True(TestImagesHelpers.TestImagePath(cast.ProfilePath), "cast.ProfilePath was not a valid image path, was: " + cast.ProfilePath);

            Crew crew = resp.Crew.SingleOrDefault(s => s.Name == "Marco Beltrami");
            Assert.NotNull(crew);

            Assert.NotEmpty(crew.CreditId);
            Assert.Equal("Sound", crew.Department);
            Assert.Equal(7229, crew.Id);
            Assert.Equal("Original Music Composer", crew.Job);
            Assert.Equal("Marco Beltrami", crew.Name);
            Assert.True(TestImagesHelpers.TestImagePath(crew.ProfilePath), "crew.ProfilePath was not a valid image path, was: " + crew.ProfilePath);
        }

        [Fact]
        public async void TestMoviesGetExternalIds()
        {
            ExternalIdsMovie externalIds = await TMDbClient.GetMovieExternalIdsAsync(IdHelper.BladeRunner2049);

            Assert.NotNull(externalIds);
            Assert.Equal(335984, externalIds.Id);
            Assert.Equal("tt1856101", externalIds.ImdbId);
            Assert.Equal("BladeRunner2049", externalIds.FacebookId);
            Assert.Equal("bladerunner", externalIds.TwitterId);
            Assert.Equal("bladerunnermovie", externalIds.InstagramId);
        }

        [Fact]
        public async void TestMoviesGetMovieImages()
        {
            ImagesWithId resp = await TMDbClient.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            ImageData backdrop = resp.Backdrops.SingleOrDefault(s => s.FilePath == "/17zArExB7ztm6fjUXZwQWgGMC9f.jpg");
            Assert.NotNull(backdrop);

            Assert.True(Math.Abs(1.77777777777778 - backdrop.AspectRatio) < double.Epsilon);
            Assert.True(TestImagesHelpers.TestImagePath(backdrop.FilePath), "backdrop.FilePath was not a valid image path, was: " + backdrop.FilePath);
            Assert.Equal(1080, backdrop.Height);
            Assert.Equal("xx", backdrop.Iso_639_1);
            Assert.True(backdrop.VoteAverage > 0);
            Assert.True(backdrop.VoteCount > 0);
            Assert.Equal(1920, backdrop.Width);

            ImageData poster = resp.Posters.SingleOrDefault(s => s.FilePath == "/c2SQMd00CCGTiDxGXVqA2J9lmzF.jpg");
            Assert.NotNull(poster);

            Assert.True(Math.Abs(0.666666666666667 - poster.AspectRatio) < double.Epsilon);
            Assert.True(TestImagesHelpers.TestImagePath(poster.FilePath), "poster.FilePath was not a valid image path, was: " + poster.FilePath);
            Assert.Equal(1500, poster.Height);
            Assert.Equal("en", poster.Iso_639_1);
            Assert.True(poster.VoteAverage > 0);
            Assert.True(poster.VoteCount > 0);
            Assert.Equal(1000, poster.Width);
        }

        [Fact]
        public async void TestMoviesGetMovieImagesWithImageLanguage()
        {
            ImagesWithId resp = await TMDbClient.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard, language: "en-US", includeImageLanguage: "en");

            Assert.True(resp.Backdrops.Count > 0);
            Assert.True(resp.Posters.Count > 0);
        }

        [Fact]
        public async void TestMoviesGetMovieWithImageLanguage()
        {
            Movie resp = await TMDbClient.GetMovieAsync(IdHelper.Avatar, language: "en-US", includeImageLanguage: "en", extraMethods: MovieMethods.Images);

            Assert.True(resp.Images.Backdrops.Count > 0);
            Assert.True(resp.Images.Backdrops.All(b => b.Iso_639_1.Equals("en", StringComparison.OrdinalIgnoreCase)));
            Assert.True(resp.Images.Posters.Count > 0);
            Assert.True(resp.Images.Posters.All(p => p.Iso_639_1.Equals("en", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public async void TestMoviesGetMovieKeywords()
        {
            KeywordsContainer resp = await TMDbClient.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            Keyword keyword = resp.Keywords.SingleOrDefault(s => s.Id == 186447);
            Assert.NotNull(keyword);

            Assert.Equal(186447, keyword.Id);
            Assert.Equal("rogue", keyword.Name);
        }

        [Fact]
        public async void TestMoviesGetMovieReleases()
        {
            Releases resp = await TMDbClient.GetMovieReleasesAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            Country country = resp.Countries.SingleOrDefault(s => s.Iso_3166_1 == "US");
            Assert.NotNull(country);

            Assert.Equal("R", country.Certification);
            Assert.Equal("US", country.Iso_3166_1);
            Assert.False(country.Primary);
            Assert.Equal(new DateTime(2013, 2, 14), country.ReleaseDate);
        }

        [Fact]
        public async void TestMoviesGetMovieVideos()
        {
            ResultContainer<Video> resp = await TMDbClient.GetMovieVideosAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            Assert.NotNull(resp);
            Assert.NotNull(resp.Results);

            Video video = resp.Results[0];
            Assert.NotNull(video);

            Assert.Equal("533ec6a7c3a368544800556f", video.Id);
            Assert.Equal("en", video.Iso_639_1);
            Assert.Equal("US", video.Iso_3166_1);
            Assert.Equal("7EgVRvG2mM0", video.Key);
            Assert.Equal("A Good Day To Die Hard Official Trailer", video.Name);
            Assert.Equal("YouTube", video.Site);
            Assert.Equal(720, video.Size);
            Assert.Equal("Trailer", video.Type);
        }

        [Fact]
        public async Task TestMoviesGetMovieWatchProviders()
        {
            SingleResultContainer<Dictionary<string, WatchProviders>> resp = await TMDbClient.GetMovieWatchProvidersAsync(IdHelper.AGoodDayToDieHard);

            Assert.NotNull(resp);

            Dictionary<string, WatchProviders> watchProvidersByRegion = resp.Results;
            Assert.NotEmpty(watchProvidersByRegion);

            // Not making further assertions since this data is highly dynamic.
        }

        [Fact]
        public async void TestMoviesGetMovieTranslations()
        {
            TranslationsContainer resp = await TMDbClient.GetMovieTranslationsAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            Translation translation = resp.Translations.SingleOrDefault(s => s.EnglishName == "German");
            Assert.NotNull(translation);

            Assert.Equal("German", translation.EnglishName);
            Assert.Equal("DE", translation.Iso_3166_1);
            Assert.Equal("de", translation.Iso_639_1);
            Assert.Equal("Deutsch", translation.Name);
        }

        [Fact]
        public async void TestMoviesGetMovieSimilarMovies()
        {
            SearchContainer<SearchMovie> resp = await TMDbClient.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            SearchContainer<SearchMovie> respGerman = await TMDbClient.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard, language: "de");
            Assert.NotNull(respGerman);

            Assert.Equal(resp.Results.Count, respGerman.Results.Count);

            int differentTitles = 0;
            for (int i = 0; i < resp.Results.Count; i++)
            {
                Assert.Equal(resp.Results[i].Id, respGerman.Results[i].Id);

                // At least one title should be different, as German is a big language and they dub all their titles.
                differentTitles++;
            }

            Assert.True(differentTitles > 0);
        }

        [Fact]
        public async void TestMoviesGetMovieRecommendationsMovies()
        {
            SearchContainer<SearchMovie> resp = await TMDbClient.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            SearchContainer<SearchMovie> respGerman = await TMDbClient.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard, language: "de");
            Assert.NotNull(respGerman);

            Assert.Equal(resp.Results.Count, respGerman.Results.Count);

            int differentTitles = 0;
            for (int i = 0; i < resp.Results.Count; i++)
            {
                Assert.Equal(resp.Results[i].Id, respGerman.Results[i].Id);

                // At least one title should be different, as German is a big language and they dub all their titles.
                differentTitles++;
            }

            Assert.True(differentTitles > 0);
        }

        [Fact]
        public async void TestMoviesGetMovieReviews()
        {
            SearchContainerWithId<ReviewBase> resp = await TMDbClient.GetMovieReviewsAsync(IdHelper.TheDarkKnightRises);
            Assert.NotNull(resp);

            Assert.NotEqual(0, resp.Results.Count);
            Assert.NotNull(resp.Results[0].Content);
        }

        [Fact]
        public async void TestMoviesGetMovieLists()
        {
            SearchContainerWithId<ListResult> resp = await TMDbClient.GetMovieListsAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);
            Assert.Equal(IdHelper.AGoodDayToDieHard, resp.Id);

            SearchContainerWithId<ListResult> respPage2 = await TMDbClient.GetMovieListsAsync(IdHelper.AGoodDayToDieHard, 2);
            Assert.NotNull(respPage2);

            Assert.Equal(1, resp.Page);
            Assert.Equal(2, respPage2.Page);
            Assert.Equal(resp.TotalResults, resp.TotalResults);
        }

        [Fact]
        public async Task TestMoviesGetMovieChangesAsync()
        {
            //GetMovieChangesAsync(int id, DateTime? startDate = null, DateTime? endDate = null)
            // FindAsync latest changed title
            Movie lastestMovie = await TMDbClient.GetMovieLatestAsync();
            int latestChanged = lastestMovie.Id;

            // Fetch changelog
            DateTime lower = DateTime.UtcNow.AddDays(-13);
            DateTime higher = DateTime.UtcNow.AddDays(1);
            List<Change> respRange = await TMDbClient.GetMovieChangesAsync(latestChanged, lower, higher);

            Assert.NotNull(respRange);
            Assert.True(respRange.Count > 0);

            // As TMDb works in days, we need to adjust our values also
            lower = lower.AddDays(-1);
            higher = higher.AddDays(1);

            foreach (Change change in respRange)
                foreach (ChangeItemBase changeItem in change.Items)
                {
                    DateTime date = changeItem.Time;
                    Assert.True(lower <= date);
                    Assert.True(date <= higher);
                }
        }

        [Fact]
        public async void TestMoviesMissing()
        {
            Movie movie1 = await TMDbClient.GetMovieAsync(IdHelper.MissingID);
            Assert.Null(movie1);

            Movie movie2 = await TMDbClient.GetMovieAsync(IdHelper.MissingMovie);
            Assert.Null(movie2);
        }

        [Fact]
        public async Task TestMoviesImagesAsync()
        {
            // Get config
            await TMDbClient.GetConfigAsync();

            // Test image url generator
            ImagesWithId images = await TMDbClient.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard);

            Assert.Equal(IdHelper.AGoodDayToDieHard, images.Id);
            await TestImagesHelpers.TestImagesAsync(TestConfig, images);
        }

        [Fact]
        public async void TestMoviesPopularList()
        {
            SearchContainer<SearchMovie> list = await TMDbClient.GetMoviePopularListAsync();

            Assert.NotNull(list);
            Assert.True(list.Results.Count > 0);
            Assert.Equal(1, list.Page);

            SearchContainer<SearchMovie> listPage2 = await TMDbClient.GetMoviePopularListAsync(page: 2);

            Assert.NotNull(listPage2);
            Assert.True(listPage2.Results.Count > 0);
            Assert.Equal(2, listPage2.Page);

            SearchContainer<SearchMovie> listDe = await TMDbClient.GetMoviePopularListAsync("de");

            Assert.NotNull(listDe);
            Assert.True(listDe.Results.Count > 0);
            Assert.Equal(1, listDe.Page);

            // At least one title should differ
            Assert.Contains(list.Results, s => listDe.Results.Any(x => x.Title != s.Title));

            SearchContainer<SearchMovie> listRegion = await TMDbClient.GetMoviePopularListAsync(region: "de");

            Assert.NotNull(listRegion);
            Assert.True(listRegion.Results.Count > 0);
            Assert.Equal(1, listRegion.Page);

            // At least one title should differ
            Assert.Contains(listDe.Results, s => listRegion.Results.Any(x => x.Title != s.Title));
        }

        [Fact]
        public async void TestMoviesTopRatedList()
        {
            SearchContainer<SearchMovie> list = await TMDbClient.GetMovieTopRatedListAsync();

            Assert.NotNull(list);
            Assert.True(list.Results.Count > 0);
            Assert.Equal(1, list.Page);

            SearchContainer<SearchMovie> listPage2 = await TMDbClient.GetMovieTopRatedListAsync(page: 2);

            Assert.NotNull(listPage2);
            Assert.True(listPage2.Results.Count > 0);
            Assert.Equal(2, listPage2.Page);

            SearchContainer<SearchMovie> listDe = await TMDbClient.GetMovieTopRatedListAsync("de");

            Assert.NotNull(listDe);
            Assert.True(listDe.Results.Count > 0);
            Assert.Equal(1, listDe.Page);

            // At least one title should differ
            Assert.Contains(list.Results, s => listDe.Results.Any(x => x.Title != s.Title));

            SearchContainer<SearchMovie> listRegion = await TMDbClient.GetMovieTopRatedListAsync(region: "de");

            Assert.NotNull(listRegion);
            Assert.True(listRegion.Results.Count > 0);
            Assert.Equal(1, listRegion.Page);

            // At least one title should differ
            Assert.Contains(listDe.Results, s => listRegion.Results.Any(x => x.Title != s.Title));
        }

        [Fact]
        public async void TestMoviesNowPlayingList()
        {
            SearchContainerWithDates<SearchMovie> list = await TMDbClient.GetMovieNowPlayingListAsync();

            Assert.NotNull(list);
            Assert.True(list.Results.Count > 0);
            Assert.Equal(1, list.Page);

            SearchContainerWithDates<SearchMovie> listPage2 = await TMDbClient.GetMovieNowPlayingListAsync(page: 2);

            Assert.NotNull(listPage2);
            Assert.True(listPage2.Results.Count > 0);
            Assert.Equal(2, listPage2.Page);

            SearchContainerWithDates<SearchMovie> listDe = await TMDbClient.GetMovieNowPlayingListAsync("de");

            Assert.NotNull(listDe);
            Assert.True(listDe.Results.Count > 0);
            Assert.Equal(1, listDe.Page);

            // At least one title should differ
            Assert.Contains(list.Results, s => listDe.Results.Any(x => x.Title != s.Title));

            SearchContainerWithDates<SearchMovie> listRegion = await TMDbClient.GetMovieNowPlayingListAsync(region: "de");

            Assert.NotNull(listRegion);
            Assert.True(listRegion.Results.Count > 0);
            Assert.Equal(1, listRegion.Page);

            // At least one title should differ
            Assert.Contains(listDe.Results, s => listRegion.Results.Any(x => x.Title != s.Title));
        }

        [Fact]
        public async void TestMoviesUpcomingList()
        {
            SearchContainerWithDates<SearchMovie> list = await TMDbClient.GetMovieUpcomingListAsync();

            Assert.NotNull(list);
            Assert.True(list.Results.Count > 0);
            Assert.Equal(1, list.Page);

            SearchContainerWithDates<SearchMovie> listPage2 = await TMDbClient.GetMovieUpcomingListAsync(page: 2);

            Assert.NotNull(listPage2);
            Assert.True(listPage2.Results.Count > 0);
            Assert.Equal(2, listPage2.Page);

            SearchContainerWithDates<SearchMovie> listDe = await TMDbClient.GetMovieUpcomingListAsync(language: "de");

            Assert.NotNull(listDe);
            Assert.True(listDe.Results.Count > 0);
            Assert.Equal(1, listDe.Page);

            // At least one title should differ
            Assert.Contains(list.Results, s => listDe.Results.Any(x => x.Title != s.Title));

            SearchContainerWithDates<SearchMovie> listDeRegion = await TMDbClient.GetMovieUpcomingListAsync(region: "de");

            Assert.NotNull(listDeRegion);
            Assert.True(listDeRegion.Results.Count > 0);
            Assert.Equal(1, listDeRegion.Page);

            // At least one title should differ
            Assert.Contains(listDe.Results, s => listDeRegion.Results.Any(x => x.Title != s.Title));
        }

        [Fact]
        public async Task TestMoviesAccountStateFavoriteSetAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

            // Remove the favourite
            if (accountState.Favorite)
                await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie is NOT favourited
            accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.False(accountState.Favorite);

            // Favourite the movie
            await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie IS favourited
            accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);
            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.True(accountState.Favorite);
        }

        [Fact]
        public async Task TestMoviesAccountStateWatchlistSetAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

            // Remove the watchlist
            if (accountState.Watchlist)
                await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie is NOT watchlisted
            accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.False(accountState.Watchlist);

            // Watchlist the movie
            await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie IS watchlisted
            accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);
            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.True(accountState.Watchlist);
        }

        [Fact]
        public async Task TestMoviesAccountStateRatingSetAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(await TMDbClient.MovieRemoveRatingAsync(IdHelper.MadMaxFuryRoad));

                // Allow TMDb to cache our changes
                await Task.Delay(2000);
            }

            // Test that the movie is NOT rated
            accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the movie
            await TMDbClient.MovieSetRatingAsync(IdHelper.MadMaxFuryRoad, 5);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie IS rated
            accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);
            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(await TMDbClient.MovieRemoveRatingAsync(IdHelper.MadMaxFuryRoad));
        }

        [Fact]
        public async Task TestMoviesSetRatingBadRating()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            Assert.False(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 7.1));
        }

        [Fact]
        public async Task TestMoviesSetRatingRatingOutOfBounds()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            Assert.False(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 10.5));
        }

        [Fact]
        public async Task TestMoviesSetRatingRatingLowerBoundsTest()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            Assert.False(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 0));
        }

        [Fact]
        public async Task TestMoviesSetRatingUserSession()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie has a different rating than our test rating
            double? rating = (await TMDbClient.GetMovieAccountStateAsync(IdHelper.Avatar)).Rating;
            Assert.NotNull(rating);

            double originalRating = rating.Value;
            double newRating = Math.Abs(originalRating - 7.5) < double.Epsilon ? 2.5 : 7.5;

            // Try changing the rating
            Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, newRating));

            // Allow TMDb to not cache our data
            await Task.Delay(2000);

            // Check if it worked
            Assert.Equal(newRating, (await TMDbClient.GetMovieAccountStateAsync(IdHelper.Avatar)).Rating);

            // Try changing it back to the previous rating
            Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, originalRating));

            // Allow TMDb to not cache our data
            await Task.Delay(2000);

            // Check if it worked
            Assert.Equal(originalRating, (await TMDbClient.GetMovieAccountStateAsync(IdHelper.Avatar)).Rating);
        }

        [Fact]
        public async void TestMoviesGetHtmlEncodedText()
        {
            Movie item = await TMDbClient.GetMovieAsync(IdHelper.Furious7, "de");

            Assert.NotNull(item);

            Assert.DoesNotContain("&amp;", item.Overview);
        }

        [Fact]
        public async void TestMoviesGet()
        {
            Movie item = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard);

            Assert.NotNull(item);
            Assert.Equal(IdHelper.AGoodDayToDieHard, item.Id);
            Assert.Equal(IdHelper.AGoodDayToDieHardImdb, item.ImdbId);

            // Check all properties
            Assert.Equal("A Good Day to Die Hard", item.Title);
            Assert.Equal("A Good Day to Die Hard", item.OriginalTitle);
            Assert.Equal("en", item.OriginalLanguage);

            Assert.Equal("Released", item.Status);
            Assert.Equal("Yippee Ki-Yay Mother Russia", item.Tagline);
            Assert.Equal("Iconoclastic, take-no-prisoners cop John McClane, finds himself for the first time on foreign soil after traveling to Moscow to help his wayward son Jack - unaware that Jack is really a highly-trained CIA operative out to stop a nuclear weapons heist. With the Russian underworld in pursuit, and battling a countdown to war, the two McClanes discover that their opposing methods make them unstoppable heroes.", item.Overview);
            Assert.Equal("http://www.diehardmovie.com/", item.Homepage);

            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);

            Assert.False(item.Adult);
            Assert.False(item.Video);

            Assert.NotNull(item.BelongsToCollection);
            Assert.Equal(1570, item.BelongsToCollection.Id);
            Assert.Equal("Die Hard Collection", item.BelongsToCollection.Name);
            Assert.True(TestImagesHelpers.TestImagePath(item.BelongsToCollection.BackdropPath), "item.BelongsToCollection.BackdropPath was not a valid image path, was: " + item.BelongsToCollection.BackdropPath);
            Assert.True(TestImagesHelpers.TestImagePath(item.BelongsToCollection.PosterPath), "item.BelongsToCollection.PosterPath was not a valid image path, was: " + item.BelongsToCollection.PosterPath);

            //Assert.Equal(1, item.BelongsToCollection.Count);
            //Assert.Equal(1570, item.BelongsToCollection[0].Id);
            //Assert.Equal("Die Hard Collection", item.BelongsToCollection[0].Name);
            //Assert.True(TestImagesHelpers.TestImagePath(item.BelongsToCollection[0].BackdropPath), "item.BelongsToCollection[0].BackdropPath was not a valid image path, was: " + item.BelongsToCollection[0].BackdropPath);
            //Assert.True(TestImagesHelpers.TestImagePath(item.BelongsToCollection[0].PosterPath), "item.BelongsToCollection[0].PosterPath was not a valid image path, was: " + item.BelongsToCollection[0].PosterPath);

            Assert.Equal(2, item.Genres.Count);
            Assert.Equal(28, item.Genres[0].Id);
            Assert.Equal("Action", item.Genres[0].Name);
            Assert.Equal(53, item.Genres[1].Id);
            Assert.Equal("Thriller", item.Genres[1].Name);

            Assert.True(item.ReleaseDate > new DateTime(2013, 01, 01));
            Assert.Equal(304654182, item.Revenue);
            Assert.Equal(92000000, item.Budget);
            Assert.Equal(98, item.Runtime);
        }

        [Fact]
        public async Task TestMoviesExtrasAccountStateAsync()
        {
            // Test the custom parsing code for Account State rating
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            Movie movie = await TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRises, MovieMethods.AccountStates);
            if (movie.AccountStates == null || !movie.AccountStates.Rating.HasValue)
            {
                await TMDbClient.MovieSetRatingAsync(IdHelper.TheDarkKnightRises, 5);

                // Allow TMDb to update cache
                await Task.Delay(2000);

                movie = await TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRises, MovieMethods.AccountStates);
            }

            Assert.NotNull(movie.AccountStates);
            Assert.True(movie.AccountStates.Rating.HasValue);
            Assert.True(Math.Abs(movie.AccountStates.Rating.Value - 5) < double.Epsilon);
        }
    }
}