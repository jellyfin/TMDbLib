using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
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
        public void TestMoviesExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / credits", " / images", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos", " / recommendations", " / external_ids");

            Movie movie = Config.Client.GetMovieAsync(IdHelper.AGoodDayToDieHard).Result;

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
        public void TestMoviesExtrasExclusive()
        {
            // Ignore missing json
            IgnoreMissingJson("similar.results[array] / media_type");

            // We ignore the 'notes' field, as TMDb sometimes leaves it out
            IgnoreMissingJson("release_dates.results[array].release_dates[array] / note");
            IgnoreMissingJson(" / id");

            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / credits", " / images", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos", "alternative_titles / id", "credits / id", "keywords / id", "release_dates / id", "releases / id", "translations / id", "videos / id", " / recommendations");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => Config.Client.GetMovieAsync(id, extras).Result, IdHelper.AGoodDayToDieHard);
        }

        [Fact]
        public void TestMoviesImdbExtrasAll()
        {
            // Ignore missing json
            IgnoreMissingJson(" / id", " / videos", "alternative_titles / id", "credits / id", "keywords / id", "release_dates / id", "releases / id", "reviews.results[array] / media_type", "translations / id", "similar.results[array] / media_type", " / recommendations");

            Dictionary<MovieMethods, Func<Movie, object>> tmpMethods = new Dictionary<MovieMethods, Func<Movie, object>>(_methods);
            tmpMethods.Remove(MovieMethods.Videos);

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Account states will only show up if we've done something
            Config.Client.MovieSetRatingAsync(IdHelper.TheDarkKnightRises, 5).Sync();

            MovieMethods combinedEnum = tmpMethods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Movie item = Config.Client.GetMovieAsync(IdHelper.TheDarkKnightRisesImdb, combinedEnum).Result;

            TestMethodsHelper.TestAllNotNull(tmpMethods, item);
        }

        [Fact]
        public void TestMoviesExtrasAll()
        {
            // We ignore the 'notes' field, as TMDb sometimes leaves it out
            IgnoreMissingJson("release_dates.results[array].release_dates[array] / note");

            IgnoreMissingJson("similar.results[array] / media_type");
            IgnoreMissingJson(" / id", "alternative_titles / id", "credits / id", "keywords / id", "release_dates / id", "releases / id", "translations / id", "videos / id", " / recommendations");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            MovieMethods combinedEnum = _methods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Movie item = Config.Client.GetMovieAsync(IdHelper.AGoodDayToDieHard, combinedEnum).Result;

            TestMethodsHelper.TestAllNotNull(_methods, item);
        }

        [Fact]
        public void TestMoviesLanguage()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / credits", " / images", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos", " / recommendations");

            Movie movie = Config.Client.GetMovieAsync(IdHelper.AGoodDayToDieHard).Result;
            Movie movieItalian = Config.Client.GetMovieAsync(IdHelper.AGoodDayToDieHard, "it").Result;

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
        public void TestMoviesGetMovieAlternativeTitles()
        {
            AlternativeTitles respUs = Config.Client.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "US").Result;
            Assert.NotNull(respUs);

            AlternativeTitles respFrench = Config.Client.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "FR").Result;
            Assert.NotNull(respFrench);

            Assert.False(respUs.Titles.Any(s => s.Title == "Duro de matar 5"));
            Assert.True(respFrench.Titles.Any(s => s.Title == "Die Hard 5 - Belle Journée Pour mourir"));

            Assert.True(respUs.Titles.All(s => s.Iso_3166_1 == "US"));
            Assert.True(respFrench.Titles.All(s => s.Iso_3166_1 == "FR"));
        }

        [Fact]
        public void TestMoviesGetMovieReleaseDates()
        {
            // We ignore the 'notes' field, as TMDb sometimes leaves it out
            IgnoreMissingJson("results[array].release_dates[array] / note");

            ResultContainer<ReleaseDatesContainer> resp = Config.Client.GetMovieReleaseDatesAsync(IdHelper.AGoodDayToDieHard).Result;
            Assert.NotNull(resp);

            ReleaseDatesContainer releasesUs = resp.Results.SingleOrDefault(s => s.Iso_3166_1 == "US");
            Assert.NotNull(releasesUs);
            Assert.Equal(1, releasesUs.ReleaseDates.Count);

            ReleaseDateItem singleRelease = releasesUs.ReleaseDates.First();

            Assert.Equal("R", singleRelease.Certification);
            Assert.Equal(string.Empty, singleRelease.Iso_639_1);
            Assert.Equal(string.Empty, singleRelease.Note);
            Assert.Equal(DateTime.Parse("2013-02-14T00:00:00.000Z").ToUniversalTime(), singleRelease.ReleaseDate);
            Assert.Equal(ReleaseDateType.Theatrical, singleRelease.Type);
        }

        [Fact]
        public void TestMoviesGetMovieAlternativeTitlesCountry()
        {
            AlternativeTitles respUs = Config.Client.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard, "US").Result;
            Assert.NotNull(respUs);

            Config.Client.DefaultCountry = "US";

            AlternativeTitles respUs2 = Config.Client.GetMovieAlternativeTitlesAsync(IdHelper.AGoodDayToDieHard).Result;
            Assert.NotNull(respUs2);

            Assert.Equal(respUs.Titles.Count, respUs2.Titles.Count);
        }

        [Fact]
        public void TestMoviesGetMovieCasts()
        {
            Credits resp = Config.Client.GetMovieCreditsAsync(IdHelper.AGoodDayToDieHard).Result;
            Assert.NotNull(resp);

            Cast cast = resp.Cast.SingleOrDefault(s => s.Name == "Bruce Willis");
            Assert.NotNull(cast);

            Assert.Equal(1, cast.CastId);
            Assert.Equal("John McClane", cast.Character);
            Assert.Equal("52fe4751c3a36847f812f049", cast.CreditId);
            Assert.Equal(62, cast.Id);
            Assert.Equal("Bruce Willis", cast.Name);
            Assert.Equal(0, cast.Order);
            Assert.True(TestImagesHelpers.TestImagePath(cast.ProfilePath), "cast.ProfilePath was not a valid image path, was: " + cast.ProfilePath);

            Crew crew = resp.Crew.SingleOrDefault(s => s.Name == "Marco Beltrami");
            Assert.NotNull(crew);

            Assert.Equal("5336b0e09251417d9b000cc7", crew.CreditId);
            Assert.Equal("Sound", crew.Department);
            Assert.Equal(7229, crew.Id);
            Assert.Equal("Music", crew.Job);
            Assert.Equal("Marco Beltrami", crew.Name);
            Assert.True(TestImagesHelpers.TestImagePath(crew.ProfilePath), "crew.ProfilePath was not a valid image path, was: " + crew.ProfilePath);
        }

        [Fact]
        public void TestMoviesGetExternalIds()
        {
            ExternalIdsMovie externalIds = Config.Client.GetMovieExternalIdsAsync(IdHelper.BladeRunner2049).Result;

            Assert.NotNull(externalIds);
            Assert.Equal(335984, externalIds.Id);
            Assert.Equal("tt1856101", externalIds.ImdbId);
            Assert.Equal("BladeRunner2049", externalIds.FacebookId);
            Assert.Equal("bladerunner", externalIds.TwitterId);
            Assert.Equal("bladerunnermovie", externalIds.InstagramId);
        }

        [Fact]
        public void TestMoviesGetMovieImages()
        {
            ImagesWithId resp = Config.Client.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard).Result;
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
        public void TestMoviesGetMovieImagesWithImageLanguage()
        {
            ImagesWithId resp = Config.Client.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard, language: "en-US", includeImageLanguage: "en").Result;

            Assert.True(resp.Backdrops.Count > 0);
            Assert.True(resp.Posters.Count > 0);
        }

        [Fact]
        public void TestMoviesGetMovieWithImageLanguage()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / credits", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos", " / recommendations", " / external_ids");

            Movie resp = Config.Client.GetMovieAsync(IdHelper.Avatar, language: "en-US", includeImageLanguage: "en", extraMethods: MovieMethods.Images).Result;

            Assert.True(resp.Images.Backdrops.Count > 0);
            Assert.True(resp.Images.Backdrops.All(b => b.Iso_639_1.Equals("en", StringComparison.OrdinalIgnoreCase)));
            Assert.True(resp.Images.Posters.Count > 0);
            Assert.True(resp.Images.Posters.All(p => p.Iso_639_1.Equals("en", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void TestMoviesGetMovieKeywords()
        {
            KeywordsContainer resp = Config.Client.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard).Result;
            Assert.NotNull(resp);

            Keyword keyword = resp.Keywords.SingleOrDefault(s => s.Id == 186447);
            Assert.NotNull(keyword);

            Assert.Equal(186447, keyword.Id);
            Assert.Equal("rogue", keyword.Name);
        }

        [Fact]
        public void TestMoviesGetMovieReleases()
        {
            Releases resp = Config.Client.GetMovieReleasesAsync(IdHelper.AGoodDayToDieHard).Result;
            Assert.NotNull(resp);

            Country country = resp.Countries.SingleOrDefault(s => s.Iso_3166_1 == "US");
            Assert.NotNull(country);

            Assert.Equal("R", country.Certification);
            Assert.Equal("US", country.Iso_3166_1);
            Assert.Equal(false, country.Primary);
            Assert.Equal(new DateTime(2013, 2, 14), country.ReleaseDate);
        }

        [Fact]
        public void TestMoviesGetMovieVideos()
        {
            ResultContainer<Video> resp = Config.Client.GetMovieVideosAsync(IdHelper.AGoodDayToDieHard).Result;

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
        public void TestMoviesGetMovieWatchProviders()
        {
            SingleResultContainer<WatchProvidersByRegion> resp = Config.Client.GetMovieWatchProvidersAsync(IdHelper.AGoodDayToDieHard).Result;

            Assert.NotNull(resp);

            WatchProvidersByRegion watchProvidersByRegion = resp.Results;
            Assert.NotNull(watchProvidersByRegion);

            // Not making further assertions since this data is highly dynamic.
        }

        [Fact]
        public void TestMoviesGetMovieTranslations()
        {
            TranslationsContainer resp = Config.Client.GetMovieTranslationsAsync(IdHelper.AGoodDayToDieHard).Result;
            Assert.NotNull(resp);

            Translation translation = resp.Translations.SingleOrDefault(s => s.EnglishName == "German");
            Assert.NotNull(translation);

            Assert.Equal("German", translation.EnglishName);
            Assert.Equal("DE", translation.Iso_3166_1);
            Assert.Equal("de", translation.Iso_639_1);
            Assert.Equal("Deutsch", translation.Name);
        }

        [Fact]
        public void TestMoviesGetMovieSimilarMovies()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            SearchContainer<SearchMovie> resp = Config.Client.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard).Result;
            Assert.NotNull(resp);

            SearchContainer<SearchMovie> respGerman = Config.Client.GetMovieSimilarAsync(IdHelper.AGoodDayToDieHard, language: "de").Result;
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
        public void TestMoviesGetMovieRecommendationsMovies()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type", "results[array] / popularity");

            SearchContainer<SearchMovie> resp = Config.Client.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard).Result;
            Assert.NotNull(resp);

            SearchContainer<SearchMovie> respGerman = Config.Client.GetMovieRecommendationsAsync(IdHelper.AGoodDayToDieHard, language: "de").Result;
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
        public void TestMoviesGetMovieReviews()
        {
            SearchContainerWithId<ReviewBase> resp = Config.Client.GetMovieReviewsAsync(IdHelper.TheDarkKnightRises).Result;
            Assert.NotNull(resp);

            Assert.NotEqual(0, resp.Results.Count);
            Assert.NotNull(resp.Results[0].Content);
        }

        [Fact]
        public void TestMoviesGetMovieLists()
        {
            SearchContainerWithId<ListResult> resp = Config.Client.GetMovieListsAsync(IdHelper.AGoodDayToDieHard).Result;
            Assert.NotNull(resp);
            Assert.Equal(IdHelper.AGoodDayToDieHard, resp.Id);

            SearchContainerWithId<ListResult> respPage2 = Config.Client.GetMovieListsAsync(IdHelper.AGoodDayToDieHard, 2).Result;
            Assert.NotNull(respPage2);

            Assert.Equal(1, resp.Page);
            Assert.Equal(2, respPage2.Page);
            Assert.Equal(resp.TotalResults, resp.TotalResults);
        }

        [Fact]
        public void TestMoviesGetMovieChanges()
        {
            // Not all ChangeItem's have an iso_639_1
            IgnoreMissingJson(" / iso_639_1");

            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / credits", " / images", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos", " / recommendations");

            //GetMovieChangesAsync(int id, DateTime? startDate = null, DateTime? endDate = null)
            // FindAsync latest changed title
            Movie lastestMovie = Config.Client.GetMovieLatestAsync().Sync();
            int latestChanged = lastestMovie.Id;

            // Fetch changelog
            DateTime lower = DateTime.UtcNow.AddDays(-13);
            DateTime higher = DateTime.UtcNow.AddDays(1);
            List<Change> respRange = Config.Client.GetMovieChangesAsync(latestChanged, lower, higher).Result;

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
        public void TestMoviesMissing()
        {
            Movie movie1 = Config.Client.GetMovieAsync(IdHelper.MissingID).Result;
            Assert.Null(movie1);

            Movie movie2 = Config.Client.GetMovieAsync(IdHelper.MissingMovie).Result;
            Assert.Null(movie2);
        }

        [Fact]
        public void TestMoviesImages()
        {
            // Get config
            Config.Client.GetConfigAsync().Sync();

            // Test image url generator
            ImagesWithId images = Config.Client.GetMovieImagesAsync(IdHelper.AGoodDayToDieHard).Result;

            Assert.Equal(IdHelper.AGoodDayToDieHard, images.Id);
            TestImagesHelpers.TestImages(Config, images);
        }

        [Fact]
        public void TestMoviesPopularList()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            SearchContainer<SearchMovie> list = Config.Client.GetMoviePopularListAsync().Result;

            Assert.NotNull(list);
            Assert.True(list.Results.Count > 0);
            Assert.Equal(1, list.Page);

            SearchContainer<SearchMovie> listPage2 = Config.Client.GetMoviePopularListAsync(page: 2).Result;

            Assert.NotNull(listPage2);
            Assert.True(listPage2.Results.Count > 0);
            Assert.Equal(2, listPage2.Page);

            SearchContainer<SearchMovie> listDe = Config.Client.GetMoviePopularListAsync("de").Result;

            Assert.NotNull(listDe);
            Assert.True(listDe.Results.Count > 0);
            Assert.Equal(1, listDe.Page);

            // At least one title should differ
            Assert.True(list.Results.Any(s => listDe.Results.Any(x => x.Title != s.Title)));

            SearchContainer<SearchMovie> listRegion = Config.Client.GetMoviePopularListAsync(region: "de").Result;

            Assert.NotNull(listRegion);
            Assert.True(listRegion.Results.Count > 0);
            Assert.Equal(1, listRegion.Page);

            // At least one title should differ
            Assert.True(listDe.Results.Any(s => listRegion.Results.Any(x => x.Title != s.Title)));
        }

        [Fact]
        public void TestMoviesTopRatedList()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            SearchContainer<SearchMovie> list = Config.Client.GetMovieTopRatedListAsync().Result;

            Assert.NotNull(list);
            Assert.True(list.Results.Count > 0);
            Assert.Equal(1, list.Page);

            SearchContainer<SearchMovie> listPage2 = Config.Client.GetMovieTopRatedListAsync(page: 2).Result;

            Assert.NotNull(listPage2);
            Assert.True(listPage2.Results.Count > 0);
            Assert.Equal(2, listPage2.Page);

            SearchContainer<SearchMovie> listDe = Config.Client.GetMovieTopRatedListAsync("de").Result;

            Assert.NotNull(listDe);
            Assert.True(listDe.Results.Count > 0);
            Assert.Equal(1, listDe.Page);

            // At least one title should differ
            Assert.True(list.Results.Any(s => listDe.Results.Any(x => x.Title != s.Title)));

            SearchContainer<SearchMovie> listRegion = Config.Client.GetMovieTopRatedListAsync(region: "de").Result;

            Assert.NotNull(listRegion);
            Assert.True(listRegion.Results.Count > 0);
            Assert.Equal(1, listRegion.Page);

            // At least one title should differ
            Assert.True(listDe.Results.Any(s => listRegion.Results.Any(x => x.Title != s.Title)));
        }

        [Fact]
        public void TestMoviesNowPlayingList()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            SearchContainerWithDates<SearchMovie> list = Config.Client.GetMovieNowPlayingListAsync().Result;

            Assert.NotNull(list);
            Assert.True(list.Results.Count > 0);
            Assert.Equal(1, list.Page);

            SearchContainerWithDates<SearchMovie> listPage2 = Config.Client.GetMovieNowPlayingListAsync(page: 2).Result;

            Assert.NotNull(listPage2);
            Assert.True(listPage2.Results.Count > 0);
            Assert.Equal(2, listPage2.Page);

            SearchContainerWithDates<SearchMovie> listDe = Config.Client.GetMovieNowPlayingListAsync("de").Result;

            Assert.NotNull(listDe);
            Assert.True(listDe.Results.Count > 0);
            Assert.Equal(1, listDe.Page);

            // At least one title should differ
            Assert.True(list.Results.Any(s => listDe.Results.Any(x => x.Title != s.Title)));

            SearchContainerWithDates<SearchMovie> listRegion = Config.Client.GetMovieNowPlayingListAsync(region: "de").Result;

            Assert.NotNull(listRegion);
            Assert.True(listRegion.Results.Count > 0);
            Assert.Equal(1, listRegion.Page);

            // At least one title should differ
            Assert.True(listDe.Results.Any(s => listRegion.Results.Any(x => x.Title != s.Title)));
        }

        [Fact]
        public void TestMoviesUpcomingList()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            SearchContainerWithDates<SearchMovie> list = Config.Client.GetMovieUpcomingListAsync().Result;

            Assert.NotNull(list);
            Assert.True(list.Results.Count > 0);
            Assert.Equal(1, list.Page);

            SearchContainerWithDates<SearchMovie> listPage2 = Config.Client.GetMovieUpcomingListAsync(page: 2).Result;

            Assert.NotNull(listPage2);
            Assert.True(listPage2.Results.Count > 0);
            Assert.Equal(2, listPage2.Page);

            SearchContainerWithDates<SearchMovie> listDe = Config.Client.GetMovieUpcomingListAsync(language: "de").Result;

            Assert.NotNull(listDe);
            Assert.True(listDe.Results.Count > 0);
            Assert.Equal(1, listDe.Page);

            // At least one title should differ
            Assert.True(list.Results.Any(s => listDe.Results.Any(x => x.Title != s.Title)));

            SearchContainerWithDates<SearchMovie> listDeRegion = Config.Client.GetMovieUpcomingListAsync(region: "de").Result;

            Assert.NotNull(listDeRegion);
            Assert.True(listDeRegion.Results.Count > 0);
            Assert.Equal(1, listDeRegion.Page);

            // At least one title should differ
            Assert.True(listDe.Results.Any(s => listDeRegion.Results.Any(x => x.Title != s.Title)));
        }

        [Fact]
        public void TestMoviesAccountStateFavoriteSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad).Result;

            // Remove the favourite
            if (accountState.Favorite)
                Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT favourited
            accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad).Result;

            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.False(accountState.Favorite);

            // Favourite the movie
            Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS favourited
            accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad).Result;
            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.True(accountState.Favorite);
        }

        [Fact]
        public void TestMoviesAccountStateWatchlistSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad).Result;

            // Remove the watchlist
            if (accountState.Watchlist)
                Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, false).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie is NOT watchlisted
            accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad).Result;

            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.False(accountState.Watchlist);

            // Watchlist the movie
            Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.MadMaxFuryRoad, true).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS watchlisted
            accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad).Result;
            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.True(accountState.Watchlist);
        }

        [Fact]
        public void TestMoviesAccountStateRatingSet()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad).Result;

            // Remove the rating
            if (accountState.Rating.HasValue)
            {
                Assert.True(Config.Client.MovieRemoveRatingAsync(IdHelper.MadMaxFuryRoad).Result);

                // Allow TMDb to cache our changes
                Thread.Sleep(2000);
            }

            // Test that the movie is NOT rated
            accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad).Result;

            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.False(accountState.Rating.HasValue);

            // Rate the movie
            Config.Client.MovieSetRatingAsync(IdHelper.MadMaxFuryRoad, 5).Sync();

            // Allow TMDb to cache our changes
            Thread.Sleep(2000);

            // Test that the movie IS rated
            accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.MadMaxFuryRoad).Result;
            Assert.Equal(IdHelper.MadMaxFuryRoad, accountState.Id);
            Assert.True(accountState.Rating.HasValue);

            // Remove the rating
            Assert.True(Config.Client.MovieRemoveRatingAsync(IdHelper.MadMaxFuryRoad).Result);
        }

        [Fact]
        public void TestMoviesSetRatingBadRating()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            Assert.False(Config.Client.MovieSetRatingAsync(IdHelper.Avatar, 7.1).Result);
        }

        [Fact]
        public void TestMoviesSetRatingRatingOutOfBounds()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            Assert.False(Config.Client.MovieSetRatingAsync(IdHelper.Avatar, 10.5).Result);
        }

        [Fact]
        public void TestMoviesSetRatingRatingLowerBoundsTest()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            Assert.False(Config.Client.MovieSetRatingAsync(IdHelper.Avatar, 0).Result);
        }

        [Fact]
        public void TestMoviesSetRatingUserSession()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie has a different rating than our test rating
            var rating = Config.Client.GetMovieAccountStateAsync(IdHelper.Avatar).Result.Rating;
            Assert.NotNull(rating);

            double originalRating = rating.Value;
            double newRating = Math.Abs(originalRating - 7.5) < double.Epsilon ? 2.5 : 7.5;

            // Try changing the rating
            Assert.True(Config.Client.MovieSetRatingAsync(IdHelper.Avatar, newRating).Result);

            // Allow TMDb to not cache our data
            Thread.Sleep(2000);

            // Check if it worked
            Assert.Equal(newRating, Config.Client.GetMovieAccountStateAsync(IdHelper.Avatar).Result.Rating);

            // Try changing it back to the previous rating
            Assert.True(Config.Client.MovieSetRatingAsync(IdHelper.Avatar, originalRating).Result);

            // Allow TMDb to not cache our data
            Thread.Sleep(2000);

            // Check if it worked
            Assert.Equal(originalRating, Config.Client.GetMovieAccountStateAsync(IdHelper.Avatar).Result.Rating);
        }

        [Fact]
        public void TestMoviesGetHtmlEncodedText()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / credits", " / images", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos", " / recommendations", " / external_ids");

            Movie item = Config.Client.GetMovieAsync(IdHelper.Furious7, "de").Result;

            Assert.NotNull(item);

            Assert.False(item.Overview.Contains("&amp;"));
        }

        [Fact]
        public void TestMoviesGet()
        {
            IgnoreMissingJson(" / account_states", " / alternative_titles", " / changes", " / credits", " / images", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos", " / recommendations", " / external_ids");

            Movie item = Config.Client.GetMovieAsync(IdHelper.AGoodDayToDieHard).Result;

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

            Assert.Equal(false, item.Adult);
            Assert.Equal(false, item.Video);

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
        public void TestMoviesExtrasAccountState()
        {
            // Ignore certain properties
            IgnoreMissingJson(" / id", " / alternative_titles", " / changes", " / credits", " / images", " / keywords", " / lists", " / release_dates", " / releases", " / reviews", " / similar", " / translations", " / videos", " / recommendations", " / external_ids");

            // Test the custom parsing code for Account State rating
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            Movie movie = Config.Client.GetMovieAsync(IdHelper.TheDarkKnightRises, MovieMethods.AccountStates).Result;
            if (movie.AccountStates == null || !movie.AccountStates.Rating.HasValue)
            {
                Config.Client.MovieSetRatingAsync(IdHelper.TheDarkKnightRises, 5).Sync();

                // Allow TMDb to update cache
                Thread.Sleep(2000);

                movie = Config.Client.GetMovieAsync(IdHelper.TheDarkKnightRises, MovieMethods.AccountStates).Result;
            }

            Assert.NotNull(movie.AccountStates);
            Assert.True(movie.AccountStates.Rating.HasValue);
            Assert.True(Math.Abs(movie.AccountStates.Rating.Value - 5) < double.Epsilon);
        }
    }
}