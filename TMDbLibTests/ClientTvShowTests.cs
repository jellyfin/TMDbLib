using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using Xunit;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
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
                [TvShowMethods.AccountStates] = tvShow => tvShow.AccountStates
            };
        }

        [Fact]
        public void TestTvShowExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / known_for", " / account_states", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / images", " / keywords", " / similar", " / translations", " / videos", " / genre_ids");
            IgnoreMissingJson("seasons[array] / account_states", "seasons[array] / credits", "seasons[array] / episodes", "seasons[array] / external_ids", "seasons[array] / images", "seasons[array] / name", "seasons[array] / overview", "seasons[array] / videos");
            TvShow tvShow = Config.Client.GetTvShowAsync(IdHelper.BreakingBad).Result;

            TestBreakingBadBaseProperties(tvShow);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvShow, object> selector in _methods.Values)
                Assert.Null(selector(tvShow));
        }

        [Fact]
        public void TestTvShowWithSeasonDetailsExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / known_for", " / account_states", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / images", " / keywords", " / similar", " / translations", " / videos", " / genre_ids",
                " / SeasonsWithDetails", "season/0 / account_states", "season/0 / credits", "season/0 / external_ids", "season/0 / id", "season/0 / images", "season/0 / videos", "season/1 / account_states", "season/1 / credits", "season/1 / external_ids",
                "season/1 / id", "season/1 / images", "season/1 / videos", "season/2 / account_states", "season/2 / credits", "season/2 / external_ids", "season/2 / id", "season/2 / images", "season/2 / videos", "season/3 / account_states", "season/3 / credits",
                "season/3 / external_ids", "season/3 / id", "season/3 / images", "season/3 / videos", "season/4 / account_states", "season/4 / credits", "season/4 / external_ids", "season/4 / id", "season/4 / images", "season/4 / videos", "season/5 / account_states",
                "season/5 / credits", "season/5 / external_ids", "season/5 / id", "season/5 / images", "season/5 / videos", "season/0 / episode_count", "season/1 / episode_count", "season/2 / episode_count", "season/3 / episode_count", "season/4 / episode_count",
                "season/5 / episode_count", "seasons[array] / account_states", "seasons[array] / credits", "seasons[array] / episodes", "seasons[array] / external_ids", "seasons[array] / images", "seasons[array] / name", "seasons[array] / overview", "seasons[array] / videos");
            IgnoreMissingCSharp("season/0 / season/0", "season/0._id / _id", "season/1 / season/1", "season/1._id / _id", "season/2 / season/2", "season/2._id / _id", "season/3 / season/3", "season/3._id / _id", "season/4 / season/4", "season/4._id / _id", "season/5 / season/5", "season/5._id / _id");

            TvShow tvShow = Config.Client.GetTvShowAsync(IdHelper.BreakingBad, includeSeasonDetails: true).Result;

            TestBreakingBadBaseProperties(tvShow);

            // Test all extras, ensure none of them are populated
            foreach (Func<TvShow, object> selector in _methods.Values)
                Assert.Null(selector(tvShow));
        }

        [Fact]
        public void TestTvShowWithSeasonsWithMoreThanTwentySeasonsExtrasAll()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / genre_ids", " / known_for", " / similar", " / translations", " / videos", "alternative_titles / id", "content_ratings / id", "credits / id", "external_ids / id", "keywords / id");
            IgnoreMissingJson(" / id", "season/0 / account_states", "season/0 / credits", "season/0 / episode_count", "season/0 / external_ids", "season/0 / id", "season/0 / images", "season/0 / videos", "season/1 / account_states", "season/1 / credits", "season/1 / episode_count", "season/1 / external_ids", "season/1 / id", "season/1 / images", "season/1 / videos", "season/10 / account_states", "season/10 / credits", "season/10 / episode_count", "season/10 / external_ids", "season/10 / id", "season/10 / images", "season/10 / videos", "season/11 / account_states", "season/11 / credits", "season/11 / episode_count", "season/11 / external_ids", "season/11 / id", "season/11 / images", "season/11 / videos", "season/12 / account_states", "season/12 / credits", "season/12 / episode_count", "season/12 / external_ids", "season/12 / id", "season/12 / images", "season/12 / videos", "season/13 / account_states", "season/13 / credits", "season/13 / episode_count", "season/13 / external_ids", "season/13 / id", "season/13 / images", "season/13 / videos", "season/14 / account_states", "season/14 / credits", "season/14 / episode_count", "season/14 / external_ids", "season/14 / id", "season/14 / images", "season/14 / videos", "season/15 / account_states", "season/15 / credits", "season/15 / episode_count", "season/15 / external_ids", "season/15 / id", "season/15 / images", "season/15 / videos", "season/16 / account_states", "season/16 / credits", "season/16 / episode_count", "season/16 / external_ids", "season/16 / id", "season/16 / images", "season/16 / videos", "season/17 / account_states", "season/17 / credits", "season/17 / episode_count", "season/17 / external_ids", "season/17 / id", "season/17 / images", "season/17 / videos", "season/18 / account_states", "season/18 / credits", "season/18 / episode_count", "season/18 / external_ids", "season/18 / id", "season/18 / images", "season/18 / videos", "season/19 / account_states", "season/19 / credits", "season/19 / episode_count", "season/19 / external_ids", "season/19 / id", "season/19 / images", "season/19 / videos", "season/2 / account_states", "season/2 / credits", "season/2 / episode_count", "season/2 / external_ids", "season/2 / id", "season/2 / images", "season/2 / videos", "season/20 / account_states", "season/20 / credits", "season/20 / episode_count", "season/20 / external_ids", "season/20 / id", "season/20 / images", "season/20 / videos", "season/21 / account_states", "season/21 / credits", "season/21 / episode_count", "season/21 / external_ids", "season/21 / id", "season/21 / images", "season/21 / videos", "season/22 / account_states", "season/22 / credits", "season/22 / episode_count", "season/22 / external_ids", "season/22 / id", "season/22 / images", "season/22 / videos", "season/23 / account_states", "season/23 / credits", "season/23 / episode_count", "season/23 / external_ids", "season/23 / id", "season/23 / images", "season/23 / videos", "season/24 / account_states", "season/24 / credits", "season/24 / episode_count", "season/24 / external_ids", "season/24 / id", "season/24 / images", "season/24 / videos", "season/25 / account_states", "season/25 / credits", "season/25 / episode_count", "season/25 / external_ids", "season/25 / id", "season/25 / images", "season/25 / videos", "season/26 / account_states", "season/26 / credits", "season/26 / episode_count", "season/26 / external_ids", "season/26 / id", "season/26 / images", "season/26 / videos", "season/27 / account_states", "season/27 / credits", "season/27 / episode_count", "season/27 / external_ids", "season/27 / id", "season/27 / images", "season/27 / videos", "season/28 / account_states", "season/28 / credits", "season/28 / episode_count", "season/28 / external_ids", "season/28 / id", "season/28 / images", "season/28 / videos", "season/29 / account_states", "season/29 / credits", "season/29 / episode_count", "season/29 / external_ids", "season/29 / id", "season/29 / images", "season/29 / videos", "season/3 / account_states", "season/3 / credits", "season/3 / episode_count", "season/3 / external_ids", "season/3 / id", "season/3 / images", "season/3 / videos", "season/4 / account_states", "season/4 / credits", "season/4 / episode_count", "season/4 / external_ids", "season/4 / id", "season/4 / images", "season/4 / videos", "season/5 / account_states", "season/5 / credits", "season/5 / episode_count", "season/5 / external_ids", "season/5 / id", "season/5 / images", "season/5 / videos", "season/6 / account_states", "season/6 / credits", "season/6 / episode_count", "season/6 / external_ids", "season/6 / id", "season/6 / images", "season/6 / videos", "season/7 / account_states", "season/7 / credits", "season/7 / episode_count", "season/7 / external_ids", "season/7 / id", "season/7 / images", "season/7 / videos", "season/8 / account_states", "season/8 / credits", "season/8 / episode_count", "season/8 / external_ids", "season/8 / id", "season/8 / images", "season/8 / videos", "season/9 / account_states", "season/9 / credits", "season/9 / episode_count", "season/9 / external_ids", "season/9 / id", "season/9 / images", "season/9 / videos", "seasons[array] / account_states", "seasons[array] / credits", "seasons[array] / episodes", "seasons[array] / external_ids", "seasons[array] / images", "seasons[array] / name", "seasons[array] / overview", "seasons[array] / videos");
            IgnoreMissingCSharp("season/0 / season/0", "season/0._id / _id", "season/1 / season/1", "season/1._id / _id", "season/2 / season/2", "season/2._id / _id", "season/3 / season/3", "season/3._id / _id", "season/4 / season/4", "season/4._id / _id", "season/5 / season/5", "season/5._id / _id", "season/10 / season/10", "season/10._id / _id", "season/11 / season/11", "season/11._id / _id", "season/12._id / _id", "season/13._id / _id", "season/14._id / _id", "season/15._id / _id", "season/16._id / _id", "season/17._id / _id", "season/18._id / _id", "season/19._id / _id", "season/20._id / _id", "season/21._id / _id", "season/22._id / _id", "season/23._id / _id", "season/24._id / _id", "season/25._id / _id", "season/26._id / _id", "season/27._id / _id", "season/28._id / _id", "season/29._id / _id", "season/6 / season/6", "season/6._id / _id", "season/7 / season/7", "season/7._id / _id", "season/8 / season/8", "season/8._id / _id", "season/9 / season/9", "season/9._id / _id");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            Config.Client.TvShowSetRatingAsync(IdHelper.Simpsons, 5).Sync();

            TvShowMethods combinedEnum = _methods.Keys.Aggregate((methods, tvShowMethods) => methods | tvShowMethods);
            TvShow tvShow = Config.Client.GetTvShowAsync(IdHelper.Simpsons, combinedEnum, includeSeasonDetails: true).Result;

            TestSimpsonsBaseProperties(tvShow);

            TestMethodsHelper.TestAllNotNull(_methods, tvShow);
        }

        [Fact]
        public void TestTvShowExtrasAll()
        {
            IgnoreMissingJson(" / id");
            IgnoreMissingJson(" / genre_ids", " / known_for", " / similar", " / translations", " / videos", "alternative_titles / id", "content_ratings / id", "credits / id", "external_ids / id", "keywords / id");
            IgnoreMissingJson("seasons[array] / account_states", "seasons[array] / credits", "seasons[array] / episodes", "seasons[array] / external_ids", "seasons[array] / images", "seasons[array] / name", "seasons[array] / overview", "seasons[array] / videos");
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 5).Sync();

            TvShowMethods combinedEnum = _methods.Keys.Aggregate((methods, tvShowMethods) => methods | tvShowMethods);
            TvShow tvShow = Config.Client.GetTvShowAsync(IdHelper.BreakingBad, combinedEnum).Result;

            TestBreakingBadBaseProperties(tvShow);

            TestMethodsHelper.TestAllNotNull(_methods, tvShow);
        }

        [Fact]
        public void TestTvShowSeparateExtrasCredits()
        {
            Credits credits = Config.Client.GetTvShowCreditsAsync(IdHelper.BreakingBad).Result;

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
            ExternalIdsTvShow externalIds = Config.Client.GetTvShowExternalIdsAsync(IdHelper.BreakingBad).Result;

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
            ResultContainer<ContentRating> contentRatings = Config.Client.GetTvShowContentRatingsAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(contentRatings);
            Assert.Equal(IdHelper.BreakingBad, contentRatings.Id);

            ContentRating contentRating = contentRatings.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("US"));
            Assert.NotNull(contentRating);
            Assert.Equal("TV-MA", contentRating.Rating);
        }

        [Fact]
        public void TestTvShowSeparateExtrasAlternativeTitles()
        {
            ResultContainer<AlternativeTitle> alternativeTitles = Config.Client.GetTvShowAlternativeTitlesAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(alternativeTitles);
            Assert.Equal(IdHelper.BreakingBad, alternativeTitles.Id);

            AlternativeTitle alternativeTitle = alternativeTitles.Results.FirstOrDefault(r => r.Iso_3166_1.Equals("IT"));
            Assert.NotNull(alternativeTitle);
            Assert.Equal("Breaking Bad - Reazioni collaterali", alternativeTitle.Title);
        }

        [Fact]
        public void TestTvShowSeparateExtrasKeywords()
        {
            ResultContainer<Keyword> keywords = Config.Client.GetTvShowKeywordsAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(keywords);
            Assert.Equal(IdHelper.BreakingBad, keywords.Id);

            Keyword keyword = keywords.Results.FirstOrDefault(r => r.Id == 41525);
            Assert.NotNull(keyword);
            Assert.Equal("high school teacher", keyword.Name);
        }

        [Fact]
        public void TestTvShowSeparateExtrasTranslations()
        {
            TranslationsContainerTv translations = Config.Client.GetTvShowTranslationsAsync(IdHelper.BreakingBad).Result;
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
            ResultContainer<Video> videos = Config.Client.GetTvShowVideosAsync(IdHelper.BreakingBad).Result;
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
            IgnoreMissingJson(" / id", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / genre_ids", " / images", " / keywords", " / known_for", " / similar", " / translations", " / videos");
            IgnoreMissingJson("seasons[array] / account_states", "seasons[array] / credits", "seasons[array] / episodes", "seasons[array] / external_ids", "seasons[array] / images", "seasons[array] / name", "seasons[array] / overview", "seasons[array] / videos");

            // Test the custom parsing code for Account State rating
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            TvShow show = Config.Client.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates).Result;
            if (show.AccountStates == null || !show.AccountStates.Rating.HasValue)
            {
                Config.Client.TvShowSetRatingAsync(IdHelper.BigBangTheory, 5).Sync();

                // Allow TMDb to update cache
                Thread.Sleep(2000);

                show = Config.Client.GetTvShowAsync(IdHelper.BigBangTheory, TvShowMethods.AccountStates).Result;
            }

            Assert.NotNull(show.AccountStates);
            Assert.True(show.AccountStates.Rating.HasValue);
            Assert.True(Math.Abs(show.AccountStates.Rating.Value - 5) < double.Epsilon);
        }

        [Fact]
        public void TestTvShowSeparateExtrasImages()
        {
            ImagesWithId images = Config.Client.GetTvShowImagesAsync(IdHelper.BreakingBad).Result;
            Assert.NotNull(images);
            Assert.NotNull(images.Backdrops);
            Assert.NotNull(images.Posters);
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

            Assert.Equal(62, tvShow.NumberOfEpisodes);
            Assert.Equal(5, tvShow.NumberOfSeasons);

            Assert.NotNull(tvShow.PosterPath);
            Assert.NotNull(tvShow.BackdropPath);

            Assert.NotEqual(0, tvShow.Popularity);
            Assert.NotEqual(0, tvShow.VoteAverage);
            Assert.NotEqual(0, tvShow.VoteAverage);
        }

        private void TestSimpsonsBaseProperties(TvShow tvShow)
        {
            Assert.NotNull(tvShow);
            Assert.Equal("The Simpsons", tvShow.Name);
            Assert.Equal("The Simpsons", tvShow.OriginalName);
            Assert.NotNull(tvShow.Overview);
            Assert.NotNull(tvShow.Homepage);
            Assert.Equal(new DateTime(1989, 12, 17), tvShow.FirstAirDate);
            Assert.True(new DateTime(2017, 01, 15) >= tvShow.LastAirDate);
            Assert.Equal(true, tvShow.InProduction);
            Assert.Equal("Returning Series", tvShow.Status);
            Assert.Equal("Scripted", tvShow.Type);
            Assert.Equal("en", tvShow.OriginalLanguage);

            Assert.NotNull(tvShow.ProductionCompanies);
            Assert.Equal(3, tvShow.ProductionCompanies.Count);
            Assert.Equal(18, tvShow.ProductionCompanies[0].Id);
            Assert.Equal("Gracie Films", tvShow.ProductionCompanies[0].Name);

            Assert.NotNull(tvShow.CreatedBy);
            Assert.Equal(1, tvShow.CreatedBy.Count);
            Assert.Equal(5741, tvShow.CreatedBy[0].Id);
            Assert.Equal("Matt Groening", tvShow.CreatedBy[0].Name);

            Assert.NotNull(tvShow.EpisodeRunTime);
            Assert.Equal(1, tvShow.EpisodeRunTime.Count);

            Assert.NotNull(tvShow.Genres);
            Assert.Equal(16, tvShow.Genres[0].Id);
            Assert.Equal("Animation", tvShow.Genres[0].Name);

            Assert.NotNull(tvShow.Languages);
            Assert.Equal("en", tvShow.Languages[0]);

            Assert.NotNull(tvShow.Networks);
            Assert.Equal(1, tvShow.Networks.Count);
            Assert.Equal(19, tvShow.Networks[0].Id);
            Assert.Equal("Fox Broadcasting Company", tvShow.Networks[0].Name);

            Assert.NotNull(tvShow.OriginCountry);
            Assert.Equal(1, tvShow.OriginCountry.Count);
            Assert.Equal("US", tvShow.OriginCountry[0]);

            Assert.NotNull(tvShow.Seasons);
            Assert.True(30 >= tvShow.Seasons.Count);
            Assert.Equal(0, tvShow.Seasons[0].SeasonNumber);
            Assert.Equal(1, tvShow.Seasons[1].SeasonNumber);

            Assert.True(614 >= tvShow.NumberOfEpisodes);
            Assert.True(29 >= tvShow.NumberOfSeasons);

            Assert.NotNull(tvShow.PosterPath);
            Assert.NotNull(tvShow.BackdropPath);

            Assert.NotEqual(0, tvShow.Popularity);
            Assert.NotEqual(0, tvShow.VoteAverage);
            Assert.NotEqual(0, tvShow.VoteAverage);
        }

        [Fact]
        public void TestTvShowPopular()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            TestHelpers.SearchPages(i => Config.Client.GetTvShowPopularAsync(i).Result);

            SearchContainer<SearchTv> result = Config.Client.GetTvShowPopularAsync().Sync();
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
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / genre_ids", " / images", " / keywords", " / known_for", " / similar", " / translations", " / videos");
            IgnoreMissingJson("seasons[array] / account_states", "seasons[array] / credits", "seasons[array] / episodes", "seasons[array] / external_ids", "seasons[array] / images", "seasons[array] / name", "seasons[array] / overview", "seasons[array] / videos");
            TvShow tvShow = Config.Client.GetTvShowAsync(1668).Result;
            Assert.Equal(tvShow.Seasons[1].EpisodeCount, 24);
        }

        [Fact]
        public void TestTvShowVideos()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / genre_ids", " / images", " / keywords", " / known_for", " / similar", " / translations", "videos / id");
            IgnoreMissingJson("seasons[array] / account_states", "seasons[array] / credits", "seasons[array] / episodes", "seasons[array] / external_ids", "seasons[array] / images", "seasons[array] / name", "seasons[array] / overview", "seasons[array] / videos");

            TvShow tvShow = Config.Client.GetTvShowAsync(1668, TvShowMethods.Videos).Result;
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
            TranslationsContainerTv translations = Config.Client.GetTvShowTranslationsAsync(1668).Result;

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
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            SearchContainer<SearchTv> tvShow = Config.Client.GetTvShowSimilarAsync(1668).Result;

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
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            // This test might fail with inconsistent information from the pages due to a caching problem in the API.
            // Comment from the Developer of the API
            // That would be caused from different pages getting cached at different times. 
            // Since top rated only pulls TV shows with 2 or more votes right now this will be something that happens until we have a lot more ratings. 
            // It's the single biggest missing data right now and there's no way around it until we get more people using the TV data. 
            // And as we get more ratings I increase that limit so we get more accurate results. 
            TestHelpers.SearchPages(i => Config.Client.GetTvShowTopRatedAsync(i).Result);

            SearchContainer<SearchTv> result = Config.Client.GetTvShowTopRatedAsync().Sync();
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
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / content_ratings", " / credits", " / external_ids", " / genre_ids", " / images", " / keywords", " / similar", " / translations", " / videos");
            IgnoreMissingJson("seasons[array] / account_states", "seasons[array] / credits", "seasons[array] / episodes", "seasons[array] / external_ids", "seasons[array] / images", "seasons[array] / name", "seasons[array] / overview", "seasons[array] / videos");
            TvShow tvShow = Config.Client.GetLatestTvShowAsync().Sync();

            Assert.NotNull(tvShow);
        }

        [Fact]
        public void TestTvShowLists()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / content_ratings", " / created_by", " / credits", " / external_ids", " / genres", " / images", " / in_production", " / keywords", " / languages", " / networks", " / production_companies", " / seasons", " / similar", " / translations", " / videos", "results[array] / media_type");

            foreach (TvShowListType type in Enum.GetValues(typeof(TvShowListType)).OfType<TvShowListType>())
            {
                TestHelpers.SearchPages(i => Config.Client.GetTvShowListAsync(type, i).Result);
            }
        }

        [Fact]
        public void TestTvShowAccountStateFavoriteSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the favourite
            if (accountState.Favorite)
                Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT favourited
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Favorite);

            // Favourite the movie
            Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS favourited
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Favorite);
        }

        [Fact]
        public void TestTvShowAccountStateWatchlistSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the watchlist
            if (accountState.Watchlist)
                Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, false).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT watchlisted
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Watchlist);

            // Watchlist the movie
            Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.BreakingBad, true).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS watchlisted
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Watchlist);
        }

        [Fact]
        public void TestTvShowAccountStateRatingSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(Config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT rated
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the movie
            Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 5).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS rated
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(Config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);
        }

        [Fact]
        public void TestTvShowSetRatingBadRating()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            Assert.False(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.1).Result);
        }

        [Fact]
        public void TestTvShowSetRatingRatingOutOfBounds()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            Assert.False(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 10.5).Result);
        }

        [Fact]
        public void TestTvShowSetRatingRatingLowerBoundsTest()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            Assert.False(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 0).Result);
        }

        [Fact]
        public void TestTvShowSetRatingUserSession()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(Config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Test that the episode is NOT rated
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;

            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the episode
            Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 5).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the episode IS rated
            accountState = Config.Client.GetTvShowAccountStateAsync(IdHelper.BreakingBad).Result;
            Assert.Equal(IdHelper.BreakingBad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(Config.Client.TvShowRemoveRatingAsync(IdHelper.BreakingBad).Result);
        }

        [Fact]
        public void TestTvShowSetRatingGuestSession()
        {
            // There is no way to validate the change besides the success return of the api call since the guest session doesn't have access to anything else
            Config.Client.SetSessionInformation(Config.GuestTestSessionId, SessionType.GuestSession);

            // Try changing the rating
            Assert.True(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 7.5).Result);

            // Try changing it back to the previous rating
            Assert.True(Config.Client.TvShowSetRatingAsync(IdHelper.BreakingBad, 8).Result);
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
