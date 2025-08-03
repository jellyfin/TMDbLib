using System;
using System.Collections.Generic;
using System.Linq;
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
        private static readonly Dictionary<MovieMethods, Func<Movie, object>> Methods;

        static ClientMovieTests()
        {
            Methods = new Dictionary<MovieMethods, Func<Movie, object>>
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
        public async Task TestMoviesExtrasNone()
        {
            Movie movie = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard);

            await Verify(movie);

            // Test all extras, ensure none of them exist
            foreach (Func<Movie, object> selector in Methods.Values)
            {
                Assert.Null(selector(movie));
            }
        }

        [Fact]
        public async Task TestMoviesExtrasExclusive()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard, extras));
        }

        [Fact]
        public async Task TestMoviesImdbExtrasAllAsync()
        {
            Dictionary<MovieMethods, Func<Movie, object>> tmpMethods = new Dictionary<MovieMethods, Func<Movie, object>>(Methods);
            tmpMethods.Remove(MovieMethods.Videos);

            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            await TMDbClient.MovieSetRatingAsync(IdHelper.TheDarkKnightRises, 5);

            await TestMethodsHelper.TestGetAll(tmpMethods, combined => TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRisesImdb, combined), movie => Verify(movie));
        }

        [Fact]
        public async Task TestMoviesLanguage()
        {
            Movie movie = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard);
            Movie movieItalian = await TMDbClient.GetMovieAsync(IdHelper.AGoodDayToDieHard, "it");

            Assert.NotNull(movie);
            Assert.NotNull(movieItalian);

            Assert.Equal("A Good Day to Die Hard", movie.Title);
            Assert.NotEqual(movie.Title, movieItalian.Title);
        }

        [Fact]
        public async Task TestMoviesGetMovieAlternativeTitles()
        {
            AlternativeTitles respUs = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "US");
            AlternativeTitles respFrench = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "FR");

            TMDbClient.DefaultCountry = "CA";

            AlternativeTitles respCaDefault = await TMDbClient.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard);

            await Verify(new
            {
                respUs,
                respFrench,
                respCaDefault
            });
        }

        [Fact]
        public async Task TestMoviesGetMovieReleaseDates()
        {
            ResultContainer<ReleaseDatesContainer> resp = await TMDbClient.GetMovieReleaseDatesAsync(IdHelper.AGoodDayToDieHard);

            await Verify(resp);
        }

        [Fact]
        public async Task TestMoviesGetMovieCasts()
        {
            Credits resp = await TMDbClient.GetMovieCreditsAsync(IdHelper.AGoodDayToDieHard);
            Assert.NotNull(resp);

            Cast cast = resp.Cast.Single(s => s.CreditId == "52fe4751c3a36847f812f049");
            Crew crew = resp.Crew.Single(s => s.CreditId == "5336b04a9251417db4000c80");

            await Verify(new
            {
                cast,
                crew
            });

            TestImagesHelpers.TestImagePaths(resp.Cast.Select(s => s.ProfilePath).Where(s => s != null));
            TestImagesHelpers.TestImagePaths(resp.Crew.Select(s => s.ProfilePath).Where(s => s != null));
        }

        [Fact]
        public async Task TestMoviesGetExternalIds()
        {
            ExternalIdsMovie externalIds = await TMDbClient.GetMovieExternalIdsAsync(IdHelper.BladeRunner2049);

            await Verify(externalIds);
        }

        [Fact]
        public async Task TestMoviesGetMovieImages()
        {
            ImagesWithId resp = await TMDbClient.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard);

            TestImagesHelpers.TestImagePaths(resp);

            ImageData backdrop = resp.Backdrops.Single(s => s.FilePath == "/js3J4SBiRfLvmRzaHoTA2tpKROw.jpg");
            ImageData poster = resp.Posters.Single(s => s.FilePath == "/c4G6lW5hAWmwveThfLSqs52yHB1.jpg");
            ImageData logo = resp.Logos.Single(s => s.FilePath == "/sZcHIwp0UD7aqOKzPkOqtd63F9r.png");

            await Verify(new
            {
                backdrop,
                poster,
                logo
            });
        }

        [Fact]
        public async Task TestMoviesGetMovieImagesWithImageLanguage()
        {
            ImagesWithId images = await TMDbClient.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard, "en-US", "en");

            TestImagesHelpers.TestImagePaths(images);

            ImageData backdrop = images.Backdrops.Single(s => s.FilePath == "/js3J4SBiRfLvmRzaHoTA2tpKROw.jpg");
            ImageData poster = images.Posters.Single(s => s.FilePath == "/9Zq88w35f1PpM22TIbf2amtjHD6.jpg");
            ImageData logo = images.Logos.Single(s => s.FilePath == "/sZcHIwp0UD7aqOKzPkOqtd63F9r.png");

            await Verify(new
            {
                backdrop,
                poster,
                logo
            });
        }

        [Fact]
        public async Task TestMoviesGetMovieWithImageLanguage()
        {
            Movie resp = await TMDbClient.GetMovieAsync(IdHelper.Avatar, "de-DE", "de", MovieMethods.Images);
            Images images = resp.Images;

            TestImagesHelpers.TestImagePaths(images);

            ImageData backdrop = images.Backdrops.Single(s => s.FilePath == "/4U9fN2jsQ94GQfDGeLEe8UaReRO.jpg");
            ImageData poster = images.Posters.Single(s => s.FilePath == "/8VV4YUwOGxgolFZTo2SgNwsfznR.jpg");
            ImageData logo = images.Logos.Single(s => s.FilePath == "/jIWzq9B4KPH9hyUISlma02ijTFb.png");

            await Verify(new
            {
                backdrop,
                poster,
                logo
            });
        }

        [Fact]
        public async Task TestMoviesGetMovieKeywords()
        {
            KeywordsContainer resp = await TMDbClient.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard);

            await Verify(resp);
        }

        [Fact]
        public async Task TestMoviesGetMovieReleases()
        {
            Releases resp = await TMDbClient.GetMovieReleasesAsync(IdHelper.AGoodDayToDieHard);

            await Verify(resp);
        }

        [Fact]
        public async Task TestMoviesGetMovieVideos()
        {
            ResultContainer<Video> resp = await TMDbClient.GetMovieVideosAsync(IdHelper.AGoodDayToDieHard);

            await Verify(resp);
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
        public async Task TestMoviesGetMovieTranslations()
        {
            TranslationsContainer resp = await TMDbClient.GetMovieTranslationsAsync(IdHelper.AGoodDayToDieHard);

            await Verify(resp);
        }

        [Fact]
        public async Task TestMoviesGetMovieSimilarMovies()
        {
            SearchContainer<SearchMovie> resp = await TMDbClient.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard);
            SearchContainer<SearchMovie> respGerman = await TMDbClient.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard, "de");

            SearchMovie single = resp.Results.Single(s => s.Id == 708);
            SearchMovie singleGerman = respGerman.Results.Single(s => s.Id == 708);

            await Verify(new
            {
                single,
                singleGerman
            });
        }

        [Fact]
        public async Task TestMoviesGetMovieRecommendationsMovies()
        {
            SearchContainer<SearchMovie> resp = await TMDbClient.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard);
            SearchContainer<SearchMovie> respGerman = await TMDbClient.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard, "de");

            SearchMovie single = resp.Results.Single(s => s.Id == 1571);
            SearchMovie singleGerman = respGerman.Results.Single(s => s.Id == 1571);

            await Verify(new
            {
                single,
                singleGerman
            });
        }

        [Fact]
        public async Task TestMoviesGetMovieReviews()
        {
            SearchContainerWithId<ReviewBase> resp = await TMDbClient.GetMovieReviewsAsync(IdHelper.AGoodDayToDieHard);

            ReviewBase single = resp.Results.Single(s => s.Id == "5ae9d7ae0e0a26394e008aeb");

            await Verify(single);
        }

        [Fact]
        public async Task TestMoviesGetMovieLists()
        {
            await TestHelpers.SearchPagesAsync<SearchContainerWithId<ListResult>, ListResult>(page => TMDbClient.GetMovieListsAsync(IdHelper.AGoodDayToDieHard, page));

            SearchContainerWithId<ListResult> resp = await TMDbClient.GetMovieListsAsync(IdHelper.AGoodDayToDieHard);

            Assert.Equal(IdHelper.AGoodDayToDieHard, resp.Id);
            Assert.NotEmpty(resp.Results);
        }

        [Fact]
        public async Task TestMoviesGetMovieChangesAsync()
        {
            IList<Change> changes = await TMDbClient.GetMovieChangesAsync(IdHelper.Avatar, startDate: DateTime.UtcNow.AddYears(-1));

            Assert.NotEmpty(changes);
        }

        [Fact]
        public async Task TestMoviesMissing()
        {
            Movie movie = await TMDbClient.GetMovieAsync(IdHelper.MissingID);
            Assert.Null(movie);
        }

        [Fact]
        public async Task TestMoviesPopularList()
        {
            await TestHelpers.SearchPagesAsync(page => TMDbClient.GetMoviePopularListAsync(page: page));

            SearchContainer<SearchMovie> list = await TMDbClient.GetMoviePopularListAsync("de");
            Assert.NotEmpty(list.Results);
        }

        [Fact]
        public async Task TestMoviesTopRatedList()
        {
            await TestHelpers.SearchPagesAsync(page => TMDbClient.GetMovieTopRatedListAsync(page: page));

            SearchContainer<SearchMovie> list = await TMDbClient.GetMovieTopRatedListAsync("de");
            Assert.NotEmpty(list.Results);
        }

        [Fact]
        public async Task TestMoviesNowPlayingList()
        {
            await TestHelpers.SearchPagesAsync<SearchContainerWithDates<SearchMovie>, SearchMovie>(page => TMDbClient.GetMovieNowPlayingListAsync(page: page));

            SearchContainer<SearchMovie> list = await TMDbClient.GetMovieNowPlayingListAsync("de");
            Assert.NotEmpty(list.Results);
        }

        [Fact]
        public async Task TestMoviesUpcomingList()
        {
            await TestHelpers.SearchPagesAsync<SearchContainerWithDates<SearchMovie>, SearchMovie>(page => TMDbClient.GetMovieUpcomingListAsync(page: page));

            SearchContainer<SearchMovie> list = await TMDbClient.GetMovieUpcomingListAsync("de");
            Assert.NotEmpty(list.Results);
        }

        [Fact]
        public async Task TestMoviesAccountStateFavoriteSetAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            await TestMethodsHelper.SetValidateRemoveTest(
                () => TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true),
                () => TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false),
                async shouldBeSet =>
                {
                    AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

                    Assert.Equal(shouldBeSet, accountState.Favorite);
                });
        }

        [Fact]
        public async Task TestMoviesAccountStateWatchlistSetAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            await TestMethodsHelper.SetValidateRemoveTest(
                () => TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true),
                () => TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false),
                async shouldBeSet =>
                {
                    AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

                    Assert.Equal(shouldBeSet, accountState.Watchlist);
                });
        }

        [Fact]
        public async Task TestMoviesAccountStateRatingSetAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            await TestMethodsHelper.SetValidateRemoveTest(
                () => TMDbClient.MovieSetRatingAsync(IdHelper.MadMaxFuryRoad, 7.5),
                () => TMDbClient.MovieRemoveRatingAsync(IdHelper.MadMaxFuryRoad),
                async shouldBeSet =>
                {
                    AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad);

                    Assert.Equal(shouldBeSet, accountState.Rating.HasValue);
                });
        }

        [Fact]
        public async Task TestMoviesSetRatingBadRating()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            Assert.False(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 7.1));

            Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 8.0));

            Assert.False(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 10.5));

            Assert.True(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 1.5));

            Assert.False(await TMDbClient.MovieSetRatingAsync(IdHelper.Avatar, 0));

            Assert.False(await TMDbClient.MovieRemoveRatingAsync(IdHelper.Avatar));
        }

        [Fact]
        public async Task TestMoviesGetHtmlEncodedText()
        {
            Movie item = await TMDbClient.GetMovieAsync(IdHelper.Furious7, "de");

            Assert.NotNull(item);

            Assert.DoesNotContain("&amp;", item.Overview, StringComparison.Ordinal);
        }

        [Fact]
        public async Task TestMoviesExtrasAccountStateAsync()
        {
            // Test the custom parsing code for Account State rating
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            Movie movie = await TMDbClient.GetMovieAsync(IdHelper.TheDarkKnightRises, MovieMethods.AccountStates);

            if (movie.AccountStates?.Rating == null)
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
