using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLibTests
{
    public class ClientTvShowTests : TestBase
    {
        private static readonly Dictionary<TvShowMethods, Func<TvShow, object>> Methods;

        static ClientTvShowTests()
        {
            Methods = new Dictionary<TvShowMethods, Func<TvShow, object>>
            {
                [TvShowMethods.Credits] = tvShow => tvShow.Credits,
                [TvShowMethods.Images] = tvShow => tvShow.Images,
                [TvShowMethods.ExternalIds] = tvShow => tvShow.ExternalIds,
                [TvShowMethods.ContentRatings] = tvShow => tvShow.ContentRatings,
                [TvShowMethods.AlternativeTitles] = tvShow => tvShow.AlternativeTitles,
                [TvShowMethods.Keywords] = tvShow => tvShow.Keywords,
                [TvShowMethods.Changes] = tvShow => tvShow.Changes,
                [TvShowMethods.AccountStates] = tvShow => tvShow.AccountStates,
                [TvShowMethods.Recommendations] = tvShow => tvShow.Recommendations,
                [TvShowMethods.ExternalIds] = tvShow => tvShow.ExternalIds,
                [TvShowMethods.WatchProviders] = tvShow => tvShow.WatchProviders,
                [TvShowMethods.EpisodeGroups] = tvShow => tvShow.EpisodeGroups,
                [TvShowMethods.CreditsAggregate] = tvShow => tvShow.AggregateCredits
            };
        }

        [Fact]
        public async Task TestTvShowExtrasNoneAsync()
        {
            TvShow tvShow = await TMDbClient.GetTvShowAsync(IdHelper.BreakingBad);

            await Verify(tvShow);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvShow, object> selector in Methods.Values)
                Assert.Null(selector(tvShow));
        }

        [Fact]
        public async Task TestTvShowExtrasAllAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            await TMDbClient.TvShowSetRatingAsync(IdHelper.KeepingUpAppearances, 5);

            await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetTvShowAsync(IdHelper.KeepingUpAppearances, combined), show => Verify(show));
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasCreditsAsync()
        {
            Credits credits = await TMDbClient.GetTvShowCreditsAsync(IdHelper.BreakingBad);

            await Verify(credits);
        }

        [Fact]
        public async Task TestAggregateCreditsExtractAllAsync()
        {
            Credits credits = await TMDbClient.GetAggregateCredits(IdHelper.Lupin);

            await Verify(credits);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasExternalIdsAsync()
        {
            ExternalIdsTvShow externalIds = await TMDbClient.GetTvShowExternalIdsAsync(IdHelper.GameOfThrones);

            await Verify(externalIds);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasContentRatingsAsync()
        {
            ResultContainer<ContentRating> contentRatings = await TMDbClient.GetTvShowContentRatingsAsync(IdHelper.BreakingBad);

            await Verify(contentRatings);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasAlternativeTitlesAsync()
        {
            ResultContainer<AlternativeTitle> alternativeTitles = await TMDbClient.GetTvShowAlternativeTitlesAsync(IdHelper.BreakingBad);

            await Verify(alternativeTitles);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasKeywordsAsync()
        {
            ResultContainer<Keyword> keywords = await TMDbClient.GetTvShowKeywordsAsync(IdHelper.BreakingBad);

            Keyword single = keywords.Results.Single(s => s.Id == 15484);

            await Verify(single);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasTranslationsAsync()
        {
            TranslationsContainerTv translations = await TMDbClient.GetTvShowTranslationsAsync(IdHelper.BreakingBad);

            Translation single = translations.Translations.Single(s => s.Iso_3166_1 == "DK");

            await Verify(single);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasVideosAsync()
        {
            ResultContainer<Video> videos = await TMDbClient.GetTvShowVideosAsync(IdHelper.BreakingBad);

            Video single = videos.Results.Single(s => s.Id == "5759db2fc3a3683e7c003df7");

            await Verify(single);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasAccountStateAsync()
        {
            // Test the custom parsing code for Account State rating
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            TvShow show = await TMDbClient.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates);
            if (show.AccountStates == null || !show.AccountStates.Rating.HasValue)
            {
                await TMDbClient.TvShowSetRatingAsync(IdHelper.BigBangTheory, 5);

                // Allow TMDb to update cache
                await Task.Delay(2000);

                show = await TMDbClient.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates);
            }

            Assert.NotNull(show.AccountStates);
            Assert.True(show.AccountStates.Rating.HasValue);
            Assert.True(Math.Abs(show.AccountStates.Rating.Value - 5) < double.Epsilon);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasImagesAsync()
        {
            ImagesWithId images = await TMDbClient.GetTvShowImagesAsync(IdHelper.BreakingBad);

            TestImagesHelpers.TestImagePaths(images);

            ImageData backdrop = images.Backdrops.Single(s => s.FilePath == "/tsRy63Mu5cu8etL1X7ZLyf7UP1M.jpg");
            ImageData poster = images.Posters.Single(s => s.FilePath == "/ggFHVNu6YYI5L9pCfOacjizRGt.jpg");
            ImageData logo = images.Logos.Single(s => s.FilePath == "/chw44B2VnLha8iiTdyZcIW0ZELC.png");

            await Verify(new
            {
                backdrop,
                poster,
                logo
            });
        }

        [Fact]
        public async Task TestTvShowGetImagesWithImageLanguageAsync()
        {
            ImagesWithId images = await TMDbClient.GetTvShowImagesAsync(IdHelper.BreakingBad, "en-US", "en");

            TestImagesHelpers.TestImagePaths(images);

            ImageData backdrop = images.Backdrops.Single(s => s.FilePath == "/otCnAN1edreRudT5E2OHk8beiDu.jpg");
            ImageData poster = images.Posters.Single(s => s.FilePath == "/ggFHVNu6YYI5L9pCfOacjizRGt.jpg");
            ImageData logo = images.Logos.Single(s => s.FilePath == "/bM2bNffZlZ2UxZqwHaxr5VS3UUI.svg");

            await Verify(new
            {
                backdrop,
                poster,
                logo
            });
        }

        [Fact]
        public async Task TestTvShowGetTvShowWithEpisodeGroups()
        {
            TvShow resp = await TMDbClient.GetTvShowAsync(IdHelper.Seinfeld, TvShowMethods.EpisodeGroups);

            await Verify(resp.EpisodeGroups);
        }

        [Fact]
        public async Task TestTvShowGetTvShowWithImageLanguageAsync()
        {
            TvShow resp = await TMDbClient.GetTvShowAsync(IdHelper.BreakingBad, includeImageLanguage: "pt", extraMethods: TvShowMethods.Images);

            TestImagesHelpers.TestImagePaths(resp.Images);

            ImageData backdrop = null;
            ImageData poster = resp.Images.Posters.Single(s => s.FilePath == "/30erzlzIOtOK3k3T3BAl1GiVMP1.jpg");
            ImageData logo = null;

            await Verify(new
            {
                backdrop,
                poster,
                logo
            });
        }

        [Fact]
        public async Task TestTvShowPopular()
        {
            await TestHelpers.SearchPagesAsync(i => TMDbClient.GetTvShowPopularAsync(i));

            SearchContainer<SearchTv> result = await TMDbClient.GetTvShowPopularAsync();

            Assert.NotEmpty(result.Results);
            Assert.NotNull(result.Results[0].Name);
            Assert.NotNull(result.Results[0].OriginalName);
            Assert.NotNull(result.Results[0].FirstAirDate);
            Assert.NotNull(result.Results[0].PosterPath);
            Assert.NotNull(result.Results[0].BackdropPath);
        }

        [Fact]
        public async Task TestTvShowSeasonCountAsync()
        {
            // TODO: Is this test obsolete?
            TvShow tvShow = await TMDbClient.GetTvShowAsync(1668);

            await Verify(tvShow);
        }

        [Fact]
        public async Task TestTvShowVideosAsync()
        {
            TvShow tvShow = await TMDbClient.GetTvShowAsync(1668, TvShowMethods.Videos);

            await Verify(tvShow);
        }

        [Fact]
        public async Task TestTvShowGetMovieWatchProviders()
        {
            SingleResultContainer<Dictionary<string, WatchProviders>> resp = await TMDbClient.GetTvShowWatchProvidersAsync(IdHelper.GameOfThrones);

            Assert.NotNull(resp);

            Dictionary<string, WatchProviders> watchProvidersByRegion = resp.Results;
            Assert.NotNull(watchProvidersByRegion);

            // Not making further assertions since this data is highly dynamic.
        }

        [Fact]
        public async Task TestTvShowTranslationsAsync()
        {
            TranslationsContainerTv translations = await TMDbClient.GetTvShowTranslationsAsync(IdHelper.BreakingBad);

            await Verify(translations);
        }

        [Fact]
        public async Task TestTvShowSimilarsAsync()
        {
            SearchContainer<SearchTv> tvShows = await TMDbClient.GetTvShowSimilarAsync(IdHelper.BreakingBad);

            Assert.NotEmpty(tvShows.Results);

            SearchTv tvShow = tvShows.Results.Single(s => s.Id == 63351);

            await Verify(tvShow);
        }

        [Fact]
        public async Task TestTvShowRecommendationsAsync()
        {
            SearchContainer<SearchTv> tvShows = await TMDbClient.GetTvShowRecommendationsAsync(IdHelper.BreakingBad);

            Assert.NotEmpty(tvShows.Results);

            SearchTv tvShow = tvShows.Results.Single(s => s.Id == 63351);

            await Verify(tvShow);
        }

        [Fact]
        public async Task TestTvShowTopRated()
        {
            await TestHelpers.SearchPagesAsync(i => TMDbClient.GetTvShowTopRatedAsync(i));

            SearchContainer<SearchTv> result = await TMDbClient.GetTvShowTopRatedAsync();
            Assert.NotNull(result.Results[0].Name);
            Assert.NotNull(result.Results[0].OriginalName);
            Assert.NotNull(result.Results[0].FirstAirDate);
            Assert.NotNull(result.Results[0].PosterPath ?? result.Results[0].BackdropPath);
        }

        [Fact]
        public async Task TestTvShowLatest()
        {
            TvShow tvShow = await TMDbClient.GetLatestTvShowAsync();

            Assert.NotNull(tvShow);
        }

        [Fact]
        public async Task TestTvShowReviews()
        {
            await TestHelpers.SearchPagesAsync<SearchContainerWithId<ReviewBase>, ReviewBase>(i => TMDbClient.GetTvShowReviewsAsync(IdHelper.BreakingBad, page: i));

            SearchContainerWithId<ReviewBase> reviews = await TMDbClient.GetTvShowReviewsAsync(IdHelper.BreakingBad);

            ReviewBase single = reviews.Results.Single(s => s.Id == "5accdbe6c3a3687e2702d058");

            await Verify(single);
        }

        [Fact]
        public async Task TestTvShowLists()
        {
            foreach (TvShowListType type in Enum.GetValues(typeof(TvShowListType)).OfType<TvShowListType>())
            {
                await TestHelpers.SearchPagesAsync(i => TMDbClient.GetTvShowListAsync(type, i));
            }
        }

        [Fact]
        public async Task TestTvShowAccountStateFavoriteSet()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            await TestMethodsHelper.SetValidateRemoveTest(async () =>
            {
                // Favourite the movie
                await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true);
            }, async () =>
            {
                // Un-favorite the movie
                await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false);
            }, async shouldBe =>
            {
                AccountState accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

                Assert.Equal(IdHelper.BreakingBad, accountState.Id);
                Assert.Equal(shouldBe, accountState.Favorite);
            });
        }

        [Fact]
        public async Task TestTvShowAccountStateWatchlistSet()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            await TestMethodsHelper.SetValidateRemoveTest(async () =>
            {
                // Add to watchlist
                await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true);
            }, async () =>
            {
                // Remove from watchlist
                await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false);
            }, async shouldBe =>
            {
                AccountState accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

                Assert.Equal(IdHelper.BreakingBad, accountState.Id);
                Assert.Equal(shouldBe, accountState.Watchlist);
            });
        }

        [Fact]
        public async Task TestTvShowAccountStateRatingSet()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            await TestMethodsHelper.SetValidateRemoveTest(async () =>
            {
                // Rate
                await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 5);
            }, async () =>
            {
                // Un-rate
                Assert.True(await TMDbClient.TvShowRemoveRatingAsync(IdHelper.BreakingBad));
            }, async shouldBe =>
            {
                AccountState accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

                Assert.Equal(IdHelper.BreakingBad, accountState.Id);

                if (shouldBe)
                {
                    Assert.NotNull(accountState.Rating);
                    Assert.Equal(5, accountState.Rating.Value);
                }
                else
                    Assert.Null(accountState.Rating);
            });
        }

        [Fact]
        public async Task TestTvShowSetRating()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            Assert.False(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 10.5));

            Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.0));

            Assert.False(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.1));

            Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.5));

            Assert.False(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 0));
        }

        [Fact]
        public async Task TestTvShowSetRatingGuestSession()
        {
            // There is no way to validate the change besides the success return of the api call since the guest session doesn't have access to anything else
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.5));

            Assert.False(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.1));

            // Try changing it back to the previous rating
            Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 8));
        }

        [Fact]
        public async Task TestTvShowMissingAsync()
        {
            TvShow tvShow = await TMDbClient.GetTvShowAsync(IdHelper.MissingID);

            Assert.Null(tvShow);
        }
    }
}
