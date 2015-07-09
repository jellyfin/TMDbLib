using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using Cast = TMDbLib.Objects.TvShows.Cast;
using Credits = TMDbLib.Objects.TvShows.Credits;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientTvShowTests
    {
        private const int BreakingBad = 1396;
        private const int House = 1408;

        private static Dictionary<TvShowMethods, Func<TvShow, object>> _methods;
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        /// <summary>
        /// Run once, on test class initialization
        /// </summary>
        [ClassInitialize]
        public static void InitialInitiator(TestContext context)
        {
            _methods = new Dictionary<TvShowMethods, Func<TvShow, object>>();
            _methods[TvShowMethods.Credits] = tvShow => tvShow.Credits;
            _methods[TvShowMethods.Images] = tvShow => tvShow.Images;
            _methods[TvShowMethods.ExternalIds] = tvShow => tvShow.ExternalIds;
            _methods[TvShowMethods.ContentRatings] = tvShow => tvShow.ContentRatings;
            _methods[TvShowMethods.AlternativeTitles] = tvShow => tvShow.AlternativeTitles;
            _methods[TvShowMethods.Keywords] = tvShow => tvShow.Keywords;
            _methods[TvShowMethods.Changes] = tvShow => tvShow.Changes;
            _methods[TvShowMethods.AccountStates] = tvShow => tvShow.AccountStates;
        }

        [TestMethod]
        public void TestTvShowExtrasNone()
        {
            TvShow tvShow = _config.Client.GetTvShow(BreakingBad);

            TestBreakingBadBaseProperties(tvShow);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvShow, object> selector in _methods.Values)
                Assert.IsNull(selector(tvShow));
        }

        [TestMethod]
        public void TestTvShowExtrasAll()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            _config.Client.TvShowSetRating(BreakingBad, 5);

            TvShowMethods combinedEnum = _methods.Keys.Aggregate((methods, tvShowMethods) => methods | tvShowMethods);
            TvShow tvShow = _config.Client.GetTvShow(BreakingBad, combinedEnum);

            TestBreakingBadBaseProperties(tvShow);

            TestMethodsHelper.TestAllNotNull(_methods, tvShow);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasCredits()
        {
            Credits credits = _config.Client.GetTvShowCredits(BreakingBad);

            Assert.IsNotNull(credits);
            Assert.IsNotNull(credits.Cast);
            Assert.AreEqual(BreakingBad, credits.Id);

            Cast castPerson = credits.Cast[0];
            Assert.AreEqual("Walter White", castPerson.Character);
            Assert.AreEqual("52542282760ee313280017f9", castPerson.CreditId);
            Assert.AreEqual(17419, castPerson.Id);
            Assert.AreEqual("Bryan Cranston", castPerson.Name);
            Assert.IsNotNull(castPerson.ProfilePath);
            Assert.AreEqual(0, castPerson.Order);

            Assert.IsNotNull(credits.Crew);

            Crew crewPerson = credits.Crew.FirstOrDefault(s => s.Id == 66633);
            Assert.IsNotNull(crewPerson);

            Assert.AreEqual(66633, crewPerson.Id);
            Assert.AreEqual("52542287760ee31328001af1", crewPerson.CreditId);
            Assert.AreEqual("Production", crewPerson.Department);
            Assert.AreEqual("Vince Gilligan", crewPerson.Name);
            Assert.AreEqual("Executive Producer", crewPerson.Job);
            Assert.IsNotNull(crewPerson.ProfilePath);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasExternalIds()
        {
            ExternalIds externalIds = _config.Client.GetTvShowExternalIds(BreakingBad);
            Assert.IsNotNull(externalIds);
            Assert.AreEqual(1396, externalIds.Id);
            Assert.AreEqual("/en/breaking_bad", externalIds.FreebaseId);
            Assert.AreEqual("/m/03d34x8", externalIds.FreebaseMid);
            Assert.AreEqual("tt0903747", externalIds.ImdbId);
            Assert.AreEqual(18164, externalIds.TvrageId);
            Assert.AreEqual(81189, externalIds.TvdbId);
        }


        [TestMethod]
        public void TestTvShowSeparateExtrasContentRatings()
        {
            ResultContainer<ContentRating> contentRatings = _config.Client.GetTvShowContentRatings(BreakingBad);
            Assert.IsNotNull(contentRatings);
            Assert.AreEqual(BreakingBad, contentRatings.Id);
            ContentRating contentRating = contentRatings.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("US"));
            Assert.IsNotNull(contentRating);
            Assert.AreEqual("TV-MA", contentRating.Rating);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasAlternativeTitles()
        {
            ResultContainer<AlternativeTitle> alternativeTitles = _config.Client.GetTvShowAlternativeTitles(BreakingBad);
            Assert.IsNotNull(alternativeTitles);
            Assert.AreEqual(BreakingBad, alternativeTitles.Id);
            AlternativeTitle alternativeTitle = alternativeTitles.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("HU"));
            Assert.IsNotNull(alternativeTitle);
            Assert.AreEqual("Totál szívás", alternativeTitle.Title);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasKeywords()
        {
            ResultContainer<Keyword> keywords = _config.Client.GetTvShowKeywords(BreakingBad);
            Assert.IsNotNull(keywords);
            Assert.AreEqual(BreakingBad, keywords.Id);
            Keyword keyword = keywords.Results.FirstOrDefault(r => r.Id == 41525);
            Assert.IsNotNull(keyword);
            Assert.AreEqual("high school teacher", keyword.Name);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasTranslations()
        {
            TranslationsContainer translations = _config.Client.GetTvShowTranslations(BreakingBad);
            Assert.IsNotNull(translations);
            Assert.AreEqual(BreakingBad, translations.Id);

            Translation translation = translations.Translations.FirstOrDefault(r => r.Iso_639_1 == "hr");
            Assert.IsNotNull(translation);
            Assert.AreEqual("Croatian", translation.EnglishName);
            Assert.AreEqual("hr", translation.Iso_639_1);
            Assert.AreEqual("Hrvatski", translation.Name);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasVideos()
        {
            ResultContainer<Video> videos = _config.Client.GetTvShowVideos(BreakingBad);
            Assert.IsNotNull(videos);
            Assert.AreEqual(BreakingBad, videos.Id);

            Video video = videos.Results.FirstOrDefault(r => r.Id == "5335e299c3a368265000001d");
            Assert.IsNotNull(video);

            Assert.AreEqual("5335e299c3a368265000001d", video.Id);
            Assert.AreEqual("en", video.Iso_639_1);
            Assert.AreEqual("6OdIbPMU720", video.Key);
            Assert.AreEqual("Opening Credits (Short)", video.Name);
            Assert.AreEqual("YouTube", video.Site);
            Assert.AreEqual(480, video.Size);
            Assert.AreEqual("Opening Credits", video.Type);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasImages()
        {
            ImagesWithId images = _config.Client.GetTvShowImages(BreakingBad);
            Assert.IsNotNull(images);
            Assert.IsNotNull(images.Backdrops);
            Assert.IsNotNull(images.Posters);
        }

        private void TestBreakingBadBaseProperties(TvShow tvShow)
        {
            Assert.IsNotNull(tvShow);
            Assert.IsNotNull(tvShow.Id);
            Assert.AreEqual("Breaking Bad", tvShow.Name);
            Assert.AreEqual("Breaking Bad", tvShow.OriginalName);
            Assert.IsNotNull(tvShow.Overview);
            Assert.IsNotNull(tvShow.Homepage);
            Assert.AreEqual(new DateTime(2008, 01, 19), tvShow.FirstAirDate);
            Assert.AreEqual(new DateTime(2013, 09, 29), tvShow.LastAirDate);
            Assert.AreEqual(false, tvShow.InProduction);
            Assert.AreEqual("Ended", tvShow.Status);
            Assert.AreEqual("Scripted", tvShow.Type);
            Assert.AreEqual("en", tvShow.OriginalLanguage);

            Assert.IsNotNull(tvShow.ProductionCompanies);
            Assert.AreEqual(3, tvShow.ProductionCompanies.Count);
            Assert.AreEqual(2605, tvShow.ProductionCompanies[0].Id);
            Assert.AreEqual("Gran Via Productions", tvShow.ProductionCompanies[0].Name);

            Assert.IsNotNull(tvShow.CreatedBy);
            Assert.AreEqual(1, tvShow.CreatedBy.Count);
            Assert.AreEqual(66633, tvShow.CreatedBy[0].Id);
            Assert.AreEqual("Vince Gilligan", tvShow.CreatedBy[0].Name);

            Assert.IsNotNull(tvShow.EpisodeRunTime);
            Assert.AreEqual(2, tvShow.EpisodeRunTime.Count);

            Assert.IsNotNull(tvShow.Genres);
            Assert.AreEqual(18, tvShow.Genres[0].Id);
            Assert.AreEqual("Drama", tvShow.Genres[0].Name);

            Assert.IsNotNull(tvShow.Languages);
            Assert.AreEqual("en", tvShow.Languages[0]);

            Assert.IsNotNull(tvShow.Networks);
            Assert.AreEqual(1, tvShow.Networks.Count);
            Assert.AreEqual(174, tvShow.Networks[0].Id);
            Assert.AreEqual("AMC", tvShow.Networks[0].Name);

            Assert.IsNotNull(tvShow.OriginCountry);
            Assert.AreEqual(1, tvShow.OriginCountry.Count);
            Assert.AreEqual("US", tvShow.OriginCountry[0]);

            Assert.IsNotNull(tvShow.Seasons);
            Assert.AreEqual(6, tvShow.Seasons.Count);
            Assert.AreEqual(0, tvShow.Seasons[0].SeasonNumber);
            Assert.AreEqual(1, tvShow.Seasons[1].SeasonNumber);
            Assert.IsNull(tvShow.Seasons[1].Episodes);

            Assert.AreEqual(62, tvShow.NumberOfEpisodes);
            Assert.AreEqual(5, tvShow.NumberOfSeasons);

            Assert.IsNotNull(tvShow.PosterPath);
            Assert.IsNotNull(tvShow.BackdropPath);

            Assert.AreNotEqual(0, tvShow.Popularity);
            Assert.AreNotEqual(0, tvShow.VoteAverage);
            Assert.AreNotEqual(0, tvShow.VoteAverage);
        }

        [TestMethod]
        public void TestTvShowPopular()
        {
            TestHelpers.SearchPages(i => _config.Client.GetTvShowPopular(i));

            SearchContainer<SearchTv> result = _config.Client.GetTvShowPopular();
            Assert.IsNotNull(result.Results[0].Id);
            Assert.IsNotNull(result.Results[0].Name);
            Assert.IsNotNull(result.Results[0].OriginalName);
            Assert.IsNotNull(result.Results[0].FirstAirDate);
            Assert.IsNotNull(result.Results[0].PosterPath);
            Assert.IsNotNull(result.Results[0].BackdropPath);
        }

        [TestMethod]
        public void TestTvShowSeasonCount()
        {
            TvShow tvShow = _config.Client.GetTvShow(1668);
            Assert.AreEqual(tvShow.Seasons[1].EpisodeCount, 24);
        }

        [TestMethod]
        public void TestTvShowVideos()
        {
            TvShow tvShow = _config.Client.GetTvShow(1668, TvShowMethods.Videos);
            Assert.IsNotNull(tvShow.Videos);
            Assert.IsNotNull(tvShow.Videos.Results);
            Assert.IsNotNull(tvShow.Videos.Results[0]);

            Assert.AreEqual("552e1b53c3a3686c4e00207b", tvShow.Videos.Results[0].Id);
            Assert.AreEqual("en", tvShow.Videos.Results[0].Iso_639_1);
            Assert.AreEqual("lGTOru7pwL8", tvShow.Videos.Results[0].Key);
            Assert.AreEqual("Friends - Opening", tvShow.Videos.Results[0].Name);
            Assert.AreEqual("YouTube", tvShow.Videos.Results[0].Site);
            Assert.AreEqual(360, tvShow.Videos.Results[0].Size);
            Assert.AreEqual("Opening Credits", tvShow.Videos.Results[0].Type);
        }

        [TestMethod]
        public void TestTvShowTranslations()
        {
            TranslationsContainer translations = _config.Client.GetTvShowTranslations(1668);

            Assert.AreEqual(1668, translations.Id);
            Translation translation = translations.Translations.SingleOrDefault(s => s.Iso_639_1 == "hr");
            Assert.IsNotNull(translation);

            Assert.AreEqual("Croatian", translation.EnglishName);
            Assert.AreEqual("hr", translation.Iso_639_1);
            Assert.AreEqual("Hrvatski", translation.Name);
        }

        [TestMethod]
        public void TestTvShowSimilars()
        {
            SearchContainer<SearchTv> tvShow = _config.Client.GetTvShowSimilar(1668);

            Assert.IsNotNull(tvShow);
            Assert.IsNotNull(tvShow.Results);

            SearchTv item = tvShow.Results.SingleOrDefault(s => s.Id == 1100);
            Assert.IsNotNull(item);

            Assert.AreEqual("/wfe7Xf7tc0zmnkoNyN3xor0xR8m.jpg", item.BackdropPath);
            Assert.AreEqual(1100, item.Id);
            Assert.AreEqual("How I Met Your Mother", item.OriginalName);
            Assert.AreEqual(new DateTime(2005, 09, 19), item.FirstAirDate);
            Assert.AreEqual("/izncB6dCLV7LBQ5MsOPyMx9mUDa.jpg", item.PosterPath);
            Assert.IsTrue(item.Popularity > 0);
            Assert.AreEqual("How I Met Your Mother", item.Name);
            Assert.IsTrue(item.VoteAverage > 0);
            Assert.IsTrue(item.VoteCount > 0);

            Assert.IsNotNull(item.OriginCountry);
            Assert.AreEqual(1, item.OriginCountry.Count);
            Assert.IsTrue(item.OriginCountry.Contains("US"));
        }

        [TestMethod]
        public void TestTvShowTopRated()
        {
            // This test might fail with inconsistent information from the pages due to a caching problem in the API.
            // Comment from the Developer of the API
            // That would be caused from different pages getting cached at different times. 
            // Since top rated only pulls TV shows with 2 or more votes right now this will be something that happens until we have a lot more ratings. 
            // It's the single biggest missing data right now and there's no way around it until we get more people using the TV data. 
            // And as we get more ratings I increase that limit so we get more accurate results. 
            // With so few ratings for TV shows right now it's set really low.
            TestHelpers.SearchPages(i => _config.Client.GetTvShowTopRated(i));

            SearchContainer<SearchTv> result = _config.Client.GetTvShowTopRated();
            Assert.IsNotNull(result.Results[0].Id);
            Assert.IsNotNull(result.Results[0].Name);
            Assert.IsNotNull(result.Results[0].OriginalName);
            Assert.IsNotNull(result.Results[0].FirstAirDate);
            Assert.IsNotNull(result.Results[0].PosterPath);
            Assert.IsNotNull(result.Results[0].BackdropPath);
        }

        [TestMethod]
        public void TestTvShowLatest()
        {
            TvShow tvShow = _config.Client.GetLatestTvShow();

            Assert.IsNotNull(tvShow);
        }

        [TestMethod]
        public void TestTvShowLists()
        {
            foreach (TvShowListType type in Enum.GetValues(typeof(TvShowListType)).OfType<TvShowListType>())
            {
                TestHelpers.SearchPages(i => _config.Client.GetTvShowList(type, i));
            }
        }

        [TestMethod]
        public void TestTvShowAccountStateFavoriteSet()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            AccountState accountState = _config.Client.GetTvShowAccountState(BreakingBad);

            // Remove the favourite
            if (accountState.Favorite)
                _config.Client.AccountChangeFavoriteStatus(MediaType.TVShow, BreakingBad, false);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT favourited
            accountState = _config.Client.GetTvShowAccountState(BreakingBad);

            Assert.AreEqual(BreakingBad, accountState.Id);
            Assert.IsFalse(accountState.Favorite);

            // Favourite the movie
            _config.Client.AccountChangeFavoriteStatus(MediaType.TVShow, BreakingBad, true);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS favourited
            accountState = _config.Client.GetTvShowAccountState(BreakingBad);
            Assert.AreEqual(BreakingBad, accountState.Id);
            Assert.IsTrue(accountState.Favorite);
        }

        [TestMethod]
        public void TestTvShowAccountStateWatchlistSet()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            AccountState accountState = _config.Client.GetTvShowAccountState(BreakingBad);

            // Remove the watchlist
            if (accountState.Watchlist)
                _config.Client.AccountChangeWatchlistStatus(MediaType.TVShow, BreakingBad, false);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT watchlisted
            accountState = _config.Client.GetTvShowAccountState(BreakingBad);

            Assert.AreEqual(BreakingBad, accountState.Id);
            Assert.IsFalse(accountState.Watchlist);

            // Watchlist the movie
            _config.Client.AccountChangeWatchlistStatus(MediaType.TVShow, BreakingBad, true);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS watchlisted
            accountState = _config.Client.GetTvShowAccountState(BreakingBad);
            Assert.AreEqual(BreakingBad, accountState.Id);
            Assert.IsTrue(accountState.Watchlist);
        }

        [TestMethod]
        public void TestTvShowAccountStateRatingSet()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            AccountState accountState = _config.Client.GetTvShowAccountState(BreakingBad);

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.IsTrue(_config.Client.TvShowRemoveRating(BreakingBad));

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT rated
            accountState = _config.Client.GetTvShowAccountState(BreakingBad);

            Assert.AreEqual(BreakingBad, accountState.Id);
            Assert.IsFalse(accountState.Rating.HasValue);

            // Rate the movie
            _config.Client.TvShowSetRating(BreakingBad, 5);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS rated
            accountState = _config.Client.GetTvShowAccountState(BreakingBad);
            Assert.AreEqual(BreakingBad, accountState.Id);
            Assert.IsTrue(accountState.Rating.HasValue);

            // Remove the rating
            Assert.IsTrue(_config.Client.TvShowRemoveRating(BreakingBad));
        }

        [TestMethod]
        public void TestTvShowSetRatingBadRating()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            Assert.IsFalse(_config.Client.TvShowSetRating(BreakingBad, 7.1));
        }

        [TestMethod]
        public void TestTvShowSetRatingRatingOutOfBounds()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            Assert.IsFalse(_config.Client.TvShowSetRating(BreakingBad, 10.5));
        }

        [TestMethod]
        public void TestTvShowSetRatingRatingLowerBoundsTest()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            Assert.IsFalse(_config.Client.TvShowSetRating(BreakingBad, 0));
        }

        [TestMethod]
        public void TestTvShowSetRatingUserSession()
        {
            _config.Client.SetSessionInformation(_config.UserSessionId, SessionType.UserSession);
            AccountState accountState = _config.Client.GetTvShowAccountState(BreakingBad);

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.IsTrue(_config.Client.TvShowRemoveRating(BreakingBad));

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Test that the movie is NOT rated
            accountState = _config.Client.GetTvShowAccountState(BreakingBad);

            Assert.AreEqual(BreakingBad, accountState.Id);
            Assert.IsFalse(accountState.Rating.HasValue);

            // Rate the movie
            _config.Client.TvShowSetRating(BreakingBad, 5);

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS rated
            accountState = _config.Client.GetTvShowAccountState(BreakingBad);
            Assert.AreEqual(BreakingBad, accountState.Id);
            Assert.IsTrue(accountState.Rating.HasValue);

            // Remove the rating
            Assert.IsTrue(_config.Client.TvShowRemoveRating(BreakingBad));
        }

        [TestMethod]
        public void TestTvShowSetRatingGuestSession()
        {
            // There is no way to validate the change besides the success return of the api call since the guest session doesn't have access to anything else
            _config.Client.SetSessionInformation(_config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.IsTrue(_config.Client.TvShowSetRating(BreakingBad, 7.5));

            // Try changing it back to the previous rating
            Assert.IsTrue(_config.Client.TvShowSetRating(BreakingBad, 8));
        }

        //[TestMethod]
        //public void TestMoviesLanguage()
        //{
        //    Movie movie = _config.Client.GetMovie(AGoodDayToDieHard);
        //    Movie movieItalian = _config.Client.GetMovie(AGoodDayToDieHard, "it");

        //    Assert.IsNotNull(movie);
        //    Assert.IsNotNull(movieItalian);

        //    Assert.AreEqual("A Good Day to Die Hard", movie.Title);
        //    Assert.AreNotEqual(movie.Title, movieItalian.Title);

        //    // Test all extras, ensure none of them exist
        //    foreach (Func<Movie, object> selector in _methods.Values)
        //    {
        //        Assert.IsNull(selector(movie));
        //        Assert.IsNull(selector(movieItalian));
        //    }
        //}
    }
}
