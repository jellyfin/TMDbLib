using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
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
        private readonly TestConfig _config;
        
        public ClientTvShowTests()
        {
            _config = new TestConfig();

            _methods = new Dictionary<TvShowMethods, Func<TvShow, object>>
            {
                [TvShowMethods.Credits] = tvShow => tvShow.Credits,
                [TvShowMethods.Images] = tvShow => tvShow.Images,
                [TvShowMethods.ExternalIds] = tvShow => tvShow.ExternalIds,
                [TvShowMethods.ContentRatings] = tvShow => tvShow.ContentRatings,
                [TvShowMethods.AlternativeTitles] = tvShow => tvShow.AlternativeTitles,
                [TvShowMethods.Keywords] = tvShow => tvShow.Keywords,
                [TvShowMethods.Changes] = tvShow => tvShow.Changes,
                [TvShowMethods.AccountStates] = tvShow => tvShow.AccountStates
            };
        }

        [Fact]
        public void TestTvShowExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson = true;

            TvShow tvShow = _config.Client.GetTvShowAsync(IdHelper.BreakingBad).Result;

            TestBreakingBadBaseProperties(tvShow);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvShow, object> selector in _methods.Values)
                Assert.Null(selector(tvShow));
        }

        [Fact]
        public void TestTvShowExtrasAll()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            _config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 5).Sync();

            TvShowMethods combinedEnum = _methods.Keys.Aggregate((methods, tvShowMethods) => methods | tvShowMethods);
            TvShow tvShow = _config.Client.GetTvShowAsync(IdHelper.BreakingBad, combinedEnum).Result;

            TestBreakingBadBaseProperties(tvShow);

            TestMethodsHelper.TestAllNotNull(_methods, tvShow);
        }

        [Fact]
        public void TestTvShowSeparateExtrasCredits()
        {
            Credits credits = _config.Client.GetTvShowCreditsAsync(IdHelper.BreakingBad).Result;

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
        public void TestTvShowSeparateExtrasExternalIds()
        {
            ExternalIds externalIds = _config.Client.GetTvShowExternalIdsAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(externalIds);
            Assert.Equal(1396, externalIds.Id);
            Assert.Equal("/en/breaking_bad", externalIds.FreebaseId);
            Assert.Equal("/m/03d34x8", externalIds.FreebaseMid);
            Assert.Equal("tt0903747", externalIds.ImdbId);
            Assert.Equal("18164", externalIds.TvrageId);
            Assert.Equal("81189", externalIds.TvdbId);
        }


        [Fact]
        public void TestTvShowSeparateExtrasContentRatings()
        {
            ResultContainer<ContentRating> contentRatings = _config.Client.GetTvShowContentRatingsAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(contentRatings);
            Assert.Equal(IdHelper.BreakingBad, contentRatings.Id);

            ContentRating contentRating = contentRatings.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("US"));
            Assert.NotNull(contentRating);
            Assert.Equal("TV-MA", contentRating.Rating);
        }

        [Fact]
        public void TestTvShowSeparateExtrasAlternativeTitles()
        {
            ResultContainer<AlternativeTitle> alternativeTitles = _config.Client.GetTvShowAlternativeTitlesAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(alternativeTitles);
            Assert.Equal(IdHelper.BreakingBad, alternativeTitles.Id);

            AlternativeTitle alternativeTitle = alternativeTitles.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("IT"));
            Assert.NotNull(alternativeTitle);
            Assert.Equal("Breaking Bad - Reazioni collaterali", alternativeTitle.Title);
        }

        [Fact]
        public void TestTvShowSeparateExtrasKeywords()
        {
            ResultContainer<Keyword> keywords = _config.Client.GetTvShowKeywordsAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(keywords);
            Assert.Equal(IdHelper.BreakingBad, keywords.Id);

            Keyword keyword = keywords.Results.FirstOrDefault(r => r.Id == 41525);
            Assert.NotNull(keyword);
            Assert.Equal("high school teacher", keyword.Name);
        }

        [Fact]
        public void TestTvShowSeparateExtrasTranslations()
        {
            TranslationsContainer translations = _config.Client.GetTvShowTranslationsAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(translations);
            Assert.Equal(IdHelper.BreakingBad, translations.Id);

            Translation translation = translations.Translations.FirstOrDefault(r => r.Iso_639_1 == "hr");
            Assert.NotNull(translation);
            Assert.Equal("Croatian", translation.EnglishName);
            Assert.Equal("hr", translation.Iso_639_1);
            Assert.Equal("Hrvatski", translation.Name);
        }

        [Fact]
        public void TestTvShowSeparateExtrasVideos()
        {
            ResultContainer<Video> videos = _config.Client.GetTvShowVideosAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(videos);
            Assert.Equal(IdHelper.BreakingBad, videos.Id);

            Video video = videos.Results.FirstOrDefault(r => r.Id == "5335e299c3a368265000001d");
            Assert.NotNull(video);

            Assert.Equal("5335e299c3a368265000001d", video.Id);
            Assert.Equal("en", video.Iso_639_1);
            Assert.Equal("6OdIbPMU720", video.Key);
            Assert.Equal("Opening Credits (Short)", video.Name);
            Assert.Equal("YouTube", video.Site);
            Assert.Equal(480, video.Size);
            Assert.Equal("Opening Credits", video.Type);
        }

        [Fact]
        public void TestTvShowSeparateExtrasAccountState()
        {
            // Test the custom parsing code for Account State rating
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            TvShow show = _config.Client.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates).Result;
            if (show.AccountStates == null || !show.AccountStates.Rating.HasValue)
            {
                _config.Client.TvShowSetRatingAsync(IdHelper.BigBangTheory, 5).Sync();

                // Allow TMDb to update cache
                Thread.Sleep(2000);

                show = _config.Client.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates).Result;
            }

            Assert.NotNull(show.AccountStates);
            Assert.True(show.AccountStates.Rating.HasValue);
            Assert.True(Math.Abs(show.AccountStates.Rating.Value - 5) < double.Epsilon);
        }

        [Fact]
        public void TestTvShowSeparateExtrasImages()
        {
            ImagesWithId images = _config.Client.GetTvShowImagesAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(images);
            Assert.NotNull(images.Backdrops);
            Assert.NotNull(images.Posters);
        }

        private void TestBreakingBadBaseProperties(TvShow tvShow)
        {
            Assert.NotNull(tvShow);
            Assert.NotNull(tvShow.Id);
            Assert.Equal("Breaking Bad", tvShow.Name);
            Assert.Equal("Breaking Bad", tvShow.OriginalName);
            Assert.NotNull(tvShow.Overview);
            Assert.NotNull(tvShow.Homepage);
            Assert.Equal(new DateTime(2008, 01, 19), tvShow.FirstAirDate);
            Assert.Equal(new DateTime(2013, 9, 29), tvShow.LastAirDate);
            Assert.Equal(false, tvShow.InProduction);
            Assert.Equal("Ended", tvShow.Status);
            Assert.Equal("Scripted", tvShow.Type);
            Assert.Equal("en", tvShow.OriginalLanguage);

            Assert.NotNull(tvShow.ProductionCompanies);
            Assert.Equal(3, tvShow.ProductionCompanies.Count);
            Assert.Equal(2605, tvShow.ProductionCompanies[0].Id);
            Assert.Equal("Gran Via Productions", tvShow.ProductionCompanies[0].Name);

            Assert.NotNull(tvShow.CreatedBy);
            Assert.Equal(1, tvShow.CreatedBy.Count);
            Assert.Equal(66633, tvShow.CreatedBy[0].Id);
            Assert.Equal("Vince Gilligan", tvShow.CreatedBy[0].Name);

            Assert.NotNull(tvShow.EpisodeRunTime);
            Assert.Equal(2, tvShow.EpisodeRunTime.Count);

            Assert.NotNull(tvShow.Genres);
            Assert.Equal(18, tvShow.Genres[0].Id);
            Assert.Equal("Drama", tvShow.Genres[0].Name);

            Assert.NotNull(tvShow.Languages);
            Assert.Equal("en", tvShow.Languages[0]);

            Assert.NotNull(tvShow.Networks);
            Assert.Equal(1, tvShow.Networks.Count);
            Assert.Equal(174, tvShow.Networks[0].Id);
            Assert.Equal("AMC", tvShow.Networks[0].Name);

            Assert.NotNull(tvShow.OriginCountry);
            Assert.Equal(1, tvShow.OriginCountry.Count);
            Assert.Equal("US", tvShow.OriginCountry[0]);

            Assert.NotNull(tvShow.Seasons);
            Assert.Equal(6, tvShow.Seasons.Count);
            Assert.Equal(0, tvShow.Seasons[0].SeasonNumber);
            Assert.Equal(1, tvShow.Seasons[1].SeasonNumber);
            Assert.Null(tvShow.Seasons[1].Episodes);

            Assert.Equal(62, tvShow.NumberOfEpisodes);
            Assert.Equal(5, tvShow.NumberOfSeasons);

            Assert.NotNull(tvShow.PosterPath);
            Assert.NotNull(tvShow.BackdropPath);

            Assert.NotEqual(0, tvShow.Popularity);
            Assert.NotEqual(0, tvShow.VoteAverage);
            Assert.NotEqual(0, tvShow.VoteAverage);
        }

        [Fact]
        public void TestTvShowPopular()
        {
            TestHelpers.SearchPages(i => _config.Client.GetTvShowPopularAsync(i).Result);

            SearchContainer<SearchTv> result = _config.Client.GetTvShowPopularAsync().Sync();
            Assert.NotNull(result.Results[0].Id);
            Assert.NotNull(result.Results[0].Name);
            Assert.NotNull(result.Results[0].OriginalName);
            Assert.NotNull(result.Results[0].FirstAirDate);
            Assert.NotNull(result.Results[0].PosterPath);
            Assert.NotNull(result.Results[0].BackdropPath);
        }

        [Fact]
        public void TestTvShowSeasonCount()
        {
            TvShow tvShow = _config.Client.GetTvShowAsync(1668).Result;
            Assert.Equal(tvShow.Seasons[1].EpisodeCount, 24);
        }

        [Fact]
        public void TestTvShowVideos()
        {
            TvShow tvShow = _config.Client.GetTvShowAsync(1668, TvShowMethods.Videos).Result;
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
        public void TestTvShowTranslations()
        {
            TranslationsContainer translations = _config.Client.GetTvShowTranslationsAsync(1668).Result;

            Assert.Equal(1668, translations.Id);
            Translation translation = translations.Translations.SingleOrDefault(s => s.Iso_639_1 == "hr");
            Assert.NotNull(translation);

            Assert.Equal("Croatian", translation.EnglishName);
            Assert.Equal("hr", translation.Iso_639_1);
            Assert.Equal("Hrvatski", translation.Name);
        }

        [Fact]
        public void TestTvShowSimilars()
        {
            SearchContainer<SearchTv> tvShow = _config.Client.GetTvShowSimilarAsync(1668).Result;

            Assert.NotNull(tvShow);
            Assert.NotNull(tvShow.Results);

            SearchTv item = tvShow.Results.SingleOrDefault(s => s.Id == 1100);
            Assert.NotNull(item);

            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.Equal(1100, item.Id);
            Assert.Equal("How I Met Your Mother", item.OriginalName);
            Assert.Equal(new DateTime(2005, 09, 19), item.FirstAirDate);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.True(item.Popularity > 0);
            Assert.Equal("How I Met Your Mother", item.Name);
            Assert.True(item.VoteAverage > 0);
            Assert.True(item.VoteCount > 0);

            Assert.NotNull(item.OriginCountry);
            Assert.Equal(1, item.OriginCountry.Count);
            Assert.True(item.OriginCountry.Contains("US"));
        }

        [Fact]
        public void TestTvShowTopRated()
        {
            // This test might fail with inconsistent information from the pages due to a caching problem in the API.
            // Comment from the Developer of the API
            // That would be caused from different pages getting cached at different times. 
            // Since top rated only pulls TV shows with 2 or more votes right now this will be something that happens until we have a lot more ratings. 
            // It's the single biggest missing data right now and there's no way around it until we get more people using the TV data. 
            // And as we get more ratings I increase that limit so we get more accurate results. 
            TestHelpers.SearchPages(i => _config.Client.GetTvShowTopRatedAsync(i).Result);

            SearchContainer<SearchTv> result = _config.Client.GetTvShowTopRatedAsync().Sync();
            Assert.NotNull(result.Results[0].Id);
            Assert.NotNull(result.Results[0].Name);
            Assert.NotNull(result.Results[0].OriginalName);
            Assert.NotNull(result.Results[0].FirstAirDate);
            Assert.NotNull(result.Results[0].PosterPath);
            Assert.NotNull(result.Results[0].BackdropPath);
        }

        [Fact]
        public void TestTvShowLatest()
        {
            TvShow tvShow = _config.Client.GetLatestTvShowAsync().Sync();

            Assert.NotNull(tvShow);
        }

        [Fact]
        public void TestTvShowLists()
        {
            foreach (TvShowListType type in Enum.GetValues(typeof(TvShowListType)).OfType<TvShowListType>())
            {
                TestHelpers.SearchPages(i => _config.Client.GetTvShowListAsync(type, i).Result);
            }
        }

        [Fact]
        public void TestTvShowAccountStateFavoriteSet()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            AccountState accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the favourite
            if (accountState.Favorite)
                _config.Client.AccountChangeFavoriteStatusAsync(MediaType.TVShow, IdHelper.BreakingBad, false).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT favourited
            accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Favorite);

            // Favourite the movie
            _config.Client.AccountChangeFavoriteStatusAsync(MediaType.TVShow, IdHelper.BreakingBad, true).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS favourited
            accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Favorite);
        }

        [Fact]
        public void TestTvShowAccountStateWatchlistSet()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            AccountState accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the watchlist
            if (accountState.Watchlist)
                _config.Client.AccountChangeWatchlistStatusAsync(MediaType.TVShow, IdHelper.BreakingBad, false).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT watchlisted
            accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Watchlist);

            // Watchlist the movie
            _config.Client.AccountChangeWatchlistStatusAsync(MediaType.TVShow, IdHelper.BreakingBad, true).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS watchlisted
            accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Watchlist);
        }

        [Fact]
        public void TestTvShowAccountStateRatingSet()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            AccountState accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(_config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT rated
            accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the movie
            _config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 5).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS rated
            accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(_config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);
        }

        [Fact]
        public void TestTvShowSetRatingBadRating()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            Assert.False(_config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.1).Result);
        }

        [Fact]
        public void TestTvShowSetRatingRatingOutOfBounds()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            Assert.False(_config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 10.5).Result);
        }

        [Fact]
        public void TestTvShowSetRatingRatingLowerBoundsTest()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            Assert.False(_config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 0).Result);
        }

        [Fact]
        public void TestTvShowSetRatingUserSession()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            AccountState accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(_config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Test that the episode is NOT rated
            accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the episode
            _config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 5).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the episode IS rated
            accountState = _config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(_config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);
        }

        [Fact]
        public void TestTvShowSetRatingGuestSession()
        {
            // There is no way to validate the change besides the success return of the api call since the guest session doesn't have access to anything else
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(_config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.5).Result);

            // Try changing it back to the previous rating
            Assert.True(_config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 8).Result);
        }

        //[Fact]
        //public void TestMoviesLanguage()
        //{
        //    Movie movie = _config.Client.GetMovieAsync(AGoodDayToDieHard);
        //    Movie movieItalian = _config.Client.GetMovieAsync(AGoodDayToDieHard, "it");

        //    Assert.NotNull(movie);
        //    Assert.NotNull(movieItalian);

        //    Assert.Equal("A Good Day to Die Hard", movie.Title);
        //    Assert.NotEqual(movie.Title, movieItalian.Title);

        //    // Test all extras, ensure none of them exist
        //    foreach (Func<Movie, object> selector in _methods.Values)
        //    {
        //        Assert.Null(selector(movie));
        //        Assert.Null(selector(movieItalian));
        //    }
        //}
    }
}
