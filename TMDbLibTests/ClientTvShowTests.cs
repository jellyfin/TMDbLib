using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Cast = TMDbLib.Objects.TvShows.Cast;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLibTests
{
    public class ClientTvShowTests : TestBase
    {
        private static Dictionary<TvShowMethods, Func<TvShow, object>> _methods;

        public ClientTvShowTests()
        {
            _methods = new Dictionary<TvShowMethods, Func<TvShow, object>>
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
                [TvShowMethods.WatchProviders] = tvShow => tvShow.WatchProviders
            };
        }

        [Fact]
        public async Task TestTvShowExtrasNoneAsync()
        {
            TvShow tvShow = await TMDbClient.GetTvShowAsync(IdHelper.BreakingBad);

            TestBreakingBadBaseProperties(tvShow);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvShow, object> selector in _methods.Values)
                Assert.Null(selector(tvShow));
        }

        [Fact]
        public async Task TestTvShowExtrasAllAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 5);

            await TestMethodsHelper.TestGetAll(_methods, combined => TMDbClient.GetTvShowAsync(IdHelper.BreakingBad, combined), TestBreakingBadBaseProperties);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasCreditsAsync()
        {
            Credits credits = await TMDbClient.GetTvShowCreditsAsync(IdHelper.BreakingBad);

            Assert.NotNull(credits);
            Assert.NotNull(credits.Cast);
            Assert.Equal(IdHelper.BreakingBad, credits.Id);

            Cast castPerson = credits.Cast[0];
            Assert.Equal("Walter White", castPerson.Character);
            Assert.Equal("52542282760ee313280017f9", castPerson.CreditId);
            Assert.Equal(17419, castPerson.Id);
            Assert.Equal("Bryan Cranston", castPerson.Name);
            Assert.NotNull(castPerson.ProfilePath);
            Assert.Equal(0, castPerson.Order);
            Assert.True(castPerson.Popularity > 0);
            Assert.Equal("Acting", castPerson.KnownForDepartment);
            Assert.Equal("Bryan Cranston", castPerson.OriginalName);

            Assert.NotNull(credits.Crew);

            Crew crewPerson = credits.Crew.FirstOrDefault(s => s.Id == 66633);
            Assert.NotNull(crewPerson);

            Assert.Equal(66633, crewPerson.Id);
            Assert.Equal("52542287760ee31328001af1", crewPerson.CreditId);
            Assert.Equal("Production", crewPerson.Department);
            Assert.Equal("Vince Gilligan", crewPerson.Name);
            Assert.Equal("Executive Producer", crewPerson.Job);
            Assert.NotNull(crewPerson.ProfilePath);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasExternalIdsAsync()
        {
            ExternalIdsTvShow externalIds = await TMDbClient.GetTvShowExternalIdsAsync(IdHelper.GameOfThrones);

            Assert.NotNull(externalIds);
            Assert.Equal(1399, externalIds.Id);
            Assert.Equal("/en/game_of_thrones", externalIds.FreebaseId);
            Assert.Equal("/m/0524b41", externalIds.FreebaseMid);
            Assert.Equal("tt0944947", externalIds.ImdbId);
            Assert.Equal("24493", externalIds.TvrageId);
            Assert.Equal("121361", externalIds.TvdbId);
            Assert.Equal("GameOfThrones", externalIds.FacebookId);
            Assert.Equal("GameOfThrones", externalIds.TwitterId);
            Assert.Equal("gameofthrones", externalIds.InstagramId);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasContentRatingsAsync()
        {
            ResultContainer<ContentRating> contentRatings = await TMDbClient.GetTvShowContentRatingsAsync(IdHelper.BreakingBad);
            Assert.NotNull(contentRatings);
            Assert.Equal(IdHelper.BreakingBad, contentRatings.Id);

            ContentRating contentRating = contentRatings.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("US"));
            Assert.NotNull(contentRating);
            Assert.Equal("TV-MA", contentRating.Rating);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasAlternativeTitlesAsync()
        {
            ResultContainer<AlternativeTitle> alternativeTitles = await TMDbClient.GetTvShowAlternativeTitlesAsync(IdHelper.BreakingBad);
            Assert.NotNull(alternativeTitles);
            Assert.Equal(IdHelper.BreakingBad, alternativeTitles.Id);

            AlternativeTitle alternativeTitle = alternativeTitles.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("IT"));
            Assert.NotNull(alternativeTitle);
            Assert.Equal("Breaking Bad - Reazioni collaterali", alternativeTitle.Title);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasKeywordsAsync()
        {
            ResultContainer<Keyword> keywords = await TMDbClient.GetTvShowKeywordsAsync(IdHelper.BreakingBad);
            Assert.NotNull(keywords);
            Assert.Equal(IdHelper.BreakingBad, keywords.Id);

            Keyword keyword = keywords.Results.FirstOrDefault(r => r.Id == 41525);
            Assert.NotNull(keyword);
            Assert.Equal("high school teacher", keyword.Name);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasTranslationsAsync()
        {
            TranslationsContainerTv translations = await TMDbClient.GetTvShowTranslationsAsync(IdHelper.BreakingBad);
            Assert.NotNull(translations);
            Assert.Equal(IdHelper.BreakingBad, translations.Id);

            Translation translation = translations.Translations.FirstOrDefault(r => r.Iso_639_1 == "hr");
            Assert.NotNull(translation);
            Assert.Equal("Croatian", translation.EnglishName);
            Assert.Equal("hr", translation.Iso_639_1);
            Assert.Equal("Hrvatski", translation.Name);
        }

        [Fact]
        public async Task TestTvShowSeparateExtrasVideosAsync()
        {
            ResultContainer<Video> videos = await TMDbClient.GetTvShowVideosAsync(IdHelper.BreakingBad);
            Assert.NotNull(videos);
            Assert.Equal(IdHelper.BreakingBad, videos.Id);

            Video video = videos.Results.FirstOrDefault(r => r.Id == "5759db2fc3a3683e7c003df7");
            Assert.NotNull(video);

            Assert.Equal("5759db2fc3a3683e7c003df7", video.Id);
            Assert.Equal("en", video.Iso_639_1);
            Assert.Equal("XZ8daibM3AE", video.Key);
            Assert.Equal("Trailer", video.Name);
            Assert.Equal("YouTube", video.Site);
            Assert.Equal(720, video.Size);
            Assert.Equal("Trailer", video.Type);
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
            Assert.NotNull(images);
            Assert.NotNull(images.Backdrops);
            Assert.NotNull(images.Posters);
        }

        [Fact]
        public async Task TestTvShowGetImagesWithImageLanguageAsync()
        {
            ImagesWithId resp = await TMDbClient.GetTvShowImagesAsync(IdHelper.BreakingBad, language: "en-US", includeImageLanguage: "en");

            Assert.True(resp.Backdrops.Count > 0);
            Assert.True(resp.Posters.Count > 0);
        }

        [Fact]
        public async Task TestTvShowGetTvShowWithImageLanguageAsync()
        {
            TvShow resp = await TMDbClient.GetTvShowAsync(IdHelper.BreakingBad, language: "en-US", includeImageLanguage: "en", extraMethods: TvShowMethods.Images);

            Assert.True(resp.Images.Backdrops.Count > 0);
            Assert.True(resp.Images.Backdrops.All(b => b.Iso_639_1.Equals("en", StringComparison.OrdinalIgnoreCase)));
            Assert.True(resp.Images.Posters.Count > 0);
            Assert.True(resp.Images.Posters.All(p => p.Iso_639_1.Equals("en", StringComparison.OrdinalIgnoreCase)));
        }

        private void TestBreakingBadBaseProperties(TvShow tvShow)
        {
            Assert.NotNull(tvShow);
            Assert.Equal("Breaking Bad", tvShow.Name);
            Assert.Equal("Breaking Bad", tvShow.OriginalName);
            Assert.NotNull(tvShow.Overview);
            Assert.NotNull(tvShow.Homepage);
            Assert.Equal(new DateTime(2008, 01, 19), tvShow.FirstAirDate);
            Assert.Equal(new DateTime(2013, 9, 29), tvShow.LastAirDate);
            Assert.False(tvShow.InProduction);
            Assert.Equal("Ended", tvShow.Status);
            Assert.Equal("Scripted", tvShow.Type);
            Assert.Equal("en", tvShow.OriginalLanguage);

            Assert.NotNull(tvShow.ProductionCompanies);
            Assert.Equal(3, tvShow.ProductionCompanies.Count);
            Assert.Equal(2605, tvShow.ProductionCompanies[0].Id);
            Assert.Equal("Gran Via Productions", tvShow.ProductionCompanies[0].Name);

            Assert.NotNull(tvShow.CreatedBy);
            Assert.Single(tvShow.CreatedBy);
            Assert.Equal(66633, tvShow.CreatedBy[0].Id);
            Assert.Equal("Vince Gilligan", tvShow.CreatedBy[0].Name);

            Assert.NotNull(tvShow.EpisodeRunTime);
            Assert.Equal(2, tvShow.EpisodeRunTime.Count);

            Assert.NotNull(tvShow.Genres);
            Assert.Equal(18, tvShow.Genres[0].Id);
            Assert.Equal("Drama", tvShow.Genres[0].Name);

            Assert.NotNull(tvShow.Languages);
            Assert.Equal("en", tvShow.Languages[0]);

            Assert.Null(tvShow.NextEpisodeToAir);

            Assert.NotNull(tvShow.LastEpisodeToAir);
            Assert.Equal(62161, tvShow.LastEpisodeToAir.Id);

            Assert.NotNull(tvShow.Networks);
            Assert.Single(tvShow.Networks);
            Assert.Equal(174, tvShow.Networks[0].Id);
            Assert.Equal("AMC", tvShow.Networks[0].Name);

            Assert.NotNull(tvShow.OriginCountry);
            Assert.Single(tvShow.OriginCountry);
            Assert.Equal("US", tvShow.OriginCountry[0]);

            Assert.NotNull(tvShow.Seasons);
            Assert.Equal(6, tvShow.Seasons.Count);
            Assert.Equal(0, tvShow.Seasons[0].SeasonNumber);
            Assert.Equal(1, tvShow.Seasons[1].SeasonNumber);

            Assert.Equal(62, tvShow.NumberOfEpisodes);
            Assert.Equal(5, tvShow.NumberOfSeasons);

            Assert.NotNull(tvShow.PosterPath);
            Assert.NotNull(tvShow.BackdropPath);

            Assert.NotEqual(0, tvShow.Popularity);
            Assert.NotEqual(0, tvShow.VoteAverage);
            Assert.NotEqual(0, tvShow.VoteAverage);
        }

        [Fact]
        public async Task TestTvShowPopular()
        {
            await TestHelpers.SearchPagesAsync(i => TMDbClient.GetTvShowPopularAsync(i));

            SearchContainer<SearchTv> result = await TMDbClient.GetTvShowPopularAsync();
            Assert.NotNull(result.Results[0].Name);
            Assert.NotNull(result.Results[0].OriginalName);
            Assert.NotNull(result.Results[0].FirstAirDate);
            Assert.NotNull(result.Results[0].PosterPath);
            Assert.NotNull(result.Results[0].BackdropPath);
        }

        [Fact]
        public async Task TestTvShowSeasonCountAsync()
        {
            TvShow tvShow = await TMDbClient.GetTvShowAsync(1668);
            Assert.Equal(24, tvShow.Seasons[1].EpisodeCount);
        }

        [Fact]
        public async Task TestTvShowVideosAsync()
        {
            TvShow tvShow = await TMDbClient.GetTvShowAsync(1668, TvShowMethods.Videos);
            Assert.NotNull(tvShow.Videos);
            Assert.NotNull(tvShow.Videos.Results);
            Assert.NotNull(tvShow.Videos.Results[0]);

            Assert.Equal("552e1b53c3a3686c4e00207b", tvShow.Videos.Results[0].Id);
            Assert.Equal("en", tvShow.Videos.Results[0].Iso_639_1);
            Assert.Equal("lGTOru7pwL8", tvShow.Videos.Results[0].Key);
            Assert.Equal("Friends - Opening", tvShow.Videos.Results[0].Name);
            Assert.Equal("YouTube", tvShow.Videos.Results[0].Site);
            Assert.Equal(360, tvShow.Videos.Results[0].Size);
            Assert.Equal("Opening Credits", tvShow.Videos.Results[0].Type);
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
            TranslationsContainerTv translations = await TMDbClient.GetTvShowTranslationsAsync(1668);

            Assert.Equal(1668, translations.Id);
            Translation translation = translations.Translations.SingleOrDefault(s => s.Iso_639_1 == "hr");
            Assert.NotNull(translation);

            Assert.Equal("Croatian", translation.EnglishName);
            Assert.Equal("hr", translation.Iso_639_1);
            Assert.Equal("Hrvatski", translation.Name);
        }

        [Fact]
        public async Task TestTvShowSimilarsAsync()
        {
            SearchContainer<SearchTv> tvShow = await TMDbClient.GetTvShowSimilarAsync(1668);

            Assert.NotNull(tvShow);
            Assert.NotNull(tvShow.Results);

            SearchTv item = tvShow.Results.SingleOrDefault(s => s.Id == 1100);
            Assert.NotNull(item);

            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath),
                "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.Equal(1100, item.Id);
            Assert.Equal("How I Met Your Mother", item.OriginalName);
            Assert.Equal(new DateTime(2005, 09, 19), item.FirstAirDate);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath),
                "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.True(item.Popularity > 0);
            Assert.Equal("How I Met Your Mother", item.Name);
            Assert.True(item.VoteAverage > 0);
            Assert.True(item.VoteCount > 0);

            Assert.NotNull(item.OriginCountry);
            Assert.Single(item.OriginCountry);
            Assert.Contains("US", item.OriginCountry);
        }

        [Fact]
        public async Task TestTvShowRecommendationsAsync()
        {
            SearchContainer<SearchTv> tvShow = await TMDbClient.GetTvShowRecommendationsAsync(1668);

            Assert.NotNull(tvShow);
            Assert.NotNull(tvShow.Results);

            SearchTv item = tvShow.Results.SingleOrDefault(s => s.Id == 1100);
            Assert.NotNull(item);

            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.Equal(1100, item.Id);
            Assert.Equal("How I Met Your Mother", item.OriginalName);
            Assert.Equal(new DateTime(2005, 09, 19), item.FirstAirDate);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.Equal("How I Met Your Mother", item.Name);
            Assert.True(item.VoteAverage > 0);
            Assert.True(item.VoteCount > 0);

            Assert.NotNull(item.OriginCountry);
            Assert.Single(item.OriginCountry);
            Assert.Contains("US", item.OriginCountry);
        }

        [Fact]
        public async Task TestTvShowTopRated()
        {
            // This test might fail with inconsistent information from the pages due to a caching problem in the API.
            // Comment from the Developer of the API
            // That would be caused from different pages getting cached at different times.
            // Since top rated only pulls TV shows with 2 or more votes right now this will be something that happens until we have a lot more ratings.
            // It's the single biggest missing data right now and there's no way around it until we get more people using the TV data.
            // And as we get more ratings I increase that limit so we get more accurate results.
            await TestHelpers.SearchPagesAsync(i => TMDbClient.GetTvShowTopRatedAsync(i));

            SearchContainer<SearchTv> result = await TMDbClient.GetTvShowTopRatedAsync();
            Assert.NotNull(result.Results[0].Name);
            Assert.NotNull(result.Results[0].OriginalName);
            Assert.NotNull(result.Results[0].FirstAirDate);
            Assert.NotNull(result.Results[0].PosterPath);
            Assert.NotNull(result.Results[0].BackdropPath);
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
        }

        [Fact]
        public void TestTvShowLists()
        {
            foreach (TvShowListType type in Enum.GetValues(typeof(TvShowListType)).OfType<TvShowListType>())
            {
                TestHelpers.SearchPagesAsync(i => TMDbClient.GetTvShowListAsync(type, i));
            }
        }

        [Fact]
        public async Task TestTvShowAccountStateFavoriteSet()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            AccountState accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            // Remove the favourite
            if (accountState.Favorite)
                await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie is NOT favourited
            accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Favorite);

            // Favourite the movie
            await TMDbClient.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie IS favourited
            accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Favorite);
        }

        [Fact]
        public async Task TestTvShowAccountStateWatchlistSet()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            AccountState accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            // Remove the watchlist
            if (accountState.Watchlist)
                await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie is NOT watchlisted
            accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Watchlist);

            // Watchlist the movie
            await TMDbClient.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie IS watchlisted
            accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Watchlist);
        }

        [Fact]
        public async Task TestTvShowAccountStateRatingSet()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            AccountState accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(await TMDbClient.TvShowRemoveRatingAsync(IdHelper.BreakingBad));

                // Allow TMDb to cache our changes
                await Task.Delay(2000);
            }

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie is NOT rated
            accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the movie
            await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 5);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the movie IS rated
            accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(await TMDbClient.TvShowRemoveRatingAsync(IdHelper.BreakingBad));
        }

        [Fact]
        public async Task TestTvShowSetRatingBadRatingAsync()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            Assert.False(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.1));
        }

        [Fact]
        public async Task TestTvShowSetRatingRatingOutOfBounds()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            Assert.False(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 10.5));
        }

        [Fact]
        public async Task TestTvShowSetRatingRatingLowerBoundsTest()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            Assert.False(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 0));
        }

        [Fact]
        public async Task TestTvShowSetRatingUserSession()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            AccountState accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(await TMDbClient.TvShowRemoveRatingAsync(IdHelper.BreakingBad));

                // Allow TMDb to cache our changes
                await Task.Delay(2000);
            }

            // Test that the episode is NOT rated
            accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the episode
            await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 5);

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Test that the episode IS rated
            accountState = await TMDbClient.GetTvShowAccountStateAsync(IdHelper.BreakingBad);
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(await TMDbClient.TvShowRemoveRatingAsync(IdHelper.BreakingBad));
        }

        [Fact]
        public async Task TestTvShowSetRatingGuestSession()
        {
            // There is no way to validate the change besides the success return of the api call since the guest session doesn't have access to anything else
            await TMDbClient.SetSessionInformationAsync(TestConfig.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(await TMDbClient.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.5));

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