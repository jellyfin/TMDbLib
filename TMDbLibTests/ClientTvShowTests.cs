using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientTvShowTests
    {
        private const int BreakingBad = 1396;

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
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasCredits()
        {
            Credits credits = _config.Client.GetTvShowCredits(BreakingBad);
            Assert.IsNotNull(credits);
            Assert.IsNotNull(credits.Cast);
            Assert.AreEqual("Walter White", credits.Cast[0].Character);
            Assert.AreEqual("52542282760ee313280017f9", credits.Cast[0].CreditId);
            Assert.AreEqual(17419, credits.Cast[0].Id);
            Assert.AreEqual("Bryan Cranston", credits.Cast[0].Name);
            Assert.IsNotNull(credits.Cast[0].ProfilePath);
            Assert.AreEqual(0, credits.Cast[0].Order);

            Assert.IsNotNull(credits.Crew);

            Crew crewPerson = credits.Crew.FirstOrDefault(s => s.Id == 66633);
            Assert.IsNotNull(crewPerson);

            Assert.AreEqual(66633, crewPerson.Id);
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
            var contentRatings = _config.Client.GetTvShowContentRatings(BreakingBad);
            Assert.IsNotNull(contentRatings);
            Assert.AreEqual(BreakingBad, contentRatings.Id);
            ContentRating contentRating = contentRatings.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("US"));
            Assert.IsNotNull(contentRating);
            Assert.AreEqual("TV-MA", contentRating.Rating);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasAlternativeTitles()
        {
            var alternativeTitles = _config.Client.GetTvShowAlternativeTitles(BreakingBad);
            Assert.IsNotNull(alternativeTitles);
            Assert.AreEqual(BreakingBad, alternativeTitles.Id);
            AlternativeTitle alternativeTitle = alternativeTitles.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("HU"));
            Assert.IsNotNull(alternativeTitle);
            Assert.AreEqual("Totál szívás", alternativeTitle.Title);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasKeywords()
        {
            var keywords = _config.Client.GetTvShowKeywords(BreakingBad);
            Assert.IsNotNull(keywords);
            Assert.AreEqual(BreakingBad, keywords.Id);
            Keyword keyword = keywords.Results.FirstOrDefault(r => r.Id==41525);
            Assert.IsNotNull(keyword);
            Assert.AreEqual("high school teacher", keyword.Name);
        }

        [TestMethod]
        public void TestTvShowSeparateExtrasImages()
        {
            ImagesWithId images = _config.Client.GetTvShowImages(BreakingBad);
            Assert.IsNotNull(images);
            Assert.IsNotNull(images.Backdrops);
            Assert.IsNotNull(images.Posters);
        }

        [TestMethod]
        public void TestTvShowExtrasNone()
        {
            TvShow tvShow = _config.Client.GetTvShow(BreakingBad);

            TestBreakingBadBaseProperties(tvShow);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvShow, object> selector in _methods.Values)
            {
                Assert.IsNull(selector(tvShow));
            }
        }

        [TestMethod]
        public void TestTvShowExtrasAll()
        {
            TvShowMethods combinedEnum = _methods.Keys.Aggregate((methods, tvShowMethods) => methods | tvShowMethods);
            TvShow tvShow = _config.Client.GetTvShow(BreakingBad, combinedEnum);

            TestBreakingBadBaseProperties(tvShow);

            TestMethodsHelper.TestAllNotNull(_methods, tvShow);
        }

        private void TestBreakingBadBaseProperties(TvShow tvShow)
        {
            Assert.IsNotNull(tvShow);
            Assert.IsNotNull(tvShow.Id);
            Assert.AreEqual("Breaking Bad", tvShow.Name);
            Assert.AreEqual("Breaking Bad", tvShow.OriginalName);
            Assert.IsNotNull(tvShow.Overview);
            Assert.IsNotNull(tvShow.Homepage);
            Assert.IsNotNull(tvShow.FirstAirDate);
            Assert.AreEqual(false, tvShow.InProduction);
            Assert.IsNotNull(tvShow.LastAirDate);
            Assert.AreEqual("Ended", tvShow.Status);

            Assert.IsNotNull(tvShow.CreatedBy);
            Assert.AreEqual(1, tvShow.CreatedBy.Count);
            Assert.AreEqual(66633, tvShow.CreatedBy[0].Id);

            Assert.IsNotNull(tvShow.EpisodeRunTime);
            Assert.AreEqual(2, tvShow.EpisodeRunTime.Count);

            Assert.IsNotNull(tvShow.Genres);
            Assert.AreEqual(18, tvShow.Genres[0].Id);

            Assert.IsNotNull(tvShow.Languages);
            Assert.AreEqual("en", tvShow.Languages[0]);

            Assert.IsNotNull(tvShow.Networks);
            Assert.AreEqual(1, tvShow.Networks.Count);
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
            TestHelpers.SearchPages(i => _config.Client.GetTvShowsPopular(i));

            SearchContainer<TvShowBase> result = _config.Client.GetTvShowsPopular();
            Assert.IsNotNull(result.Results[0].Id);
            Assert.IsNotNull(result.Results[0].Name);
            Assert.IsNotNull(result.Results[0].OriginalName);
            Assert.IsNotNull(result.Results[0].FirstAirDate);
            Assert.IsNotNull(result.Results[0].PosterPath);
            Assert.IsNotNull(result.Results[0].BackdropPath);
        }

        [TestMethod]
        public void TestTvShowSeasonCount() {
            TvShow tvShow = _config.Client.GetTvShow(1668);
            Assert.AreEqual(tvShow.Seasons[1].EpisodeCount,  24);
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
            TestHelpers.SearchPages(i => _config.Client.GetTvShowsTopRated(i));

            SearchContainer<TvShowBase> result = _config.Client.GetTvShowsTopRated();
            Assert.IsNotNull(result.Results[0].Id);
            Assert.IsNotNull(result.Results[0].Name);
            Assert.IsNotNull(result.Results[0].OriginalName);
            Assert.IsNotNull(result.Results[0].FirstAirDate);
            Assert.IsNotNull(result.Results[0].PosterPath);
            Assert.IsNotNull(result.Results[0].BackdropPath);
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
