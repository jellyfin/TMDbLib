using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using TMDbLib.Objects.Account;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Lists;
using TMDbLib.Objects.Search;
using TMDbLibTests.Exceptions;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientAccountTests : TestBase
    {
        public ClientAccountTests() : base()
        {
            if (string.IsNullOrWhiteSpace(Config.UserSessionId))
                throw new ConfigurationErrorsException("To successfully complete the ClientAccountTests you will need to specify a valid 'UserSessionId' in the test config file");
        }

        [Fact]
        public void TestAccountGetDetailsGuestAccount()
        {
            Config.Client.SetSessionInformation(Config.GuestTestSessionId, SessionType.GuestSession);

            Assert.Throws<UserSessionRequiredException>(() => Config.Client.AccountGetDetailsAsync().Sync());
        }

        [Fact]
        public void TestAccountGetDetailsUserAccount()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountDetails account = Config.Client.AccountGetDetailsAsync().Sync();

            // Naturally the specified account must have these values populated for the test to pass
            Assert.NotNull(account);
            Assert.True(account.Id > 1);
            Assert.Equal("Test Name", account.Name);
            Assert.Equal("TMDbTestAccount", account.Username);
            Assert.Equal("BE", account.Iso_3166_1);
            Assert.Equal("en", account.Iso_639_1);

            Assert.NotNull(account.Avatar);
            Assert.NotNull(account.Avatar.Gravatar);
            Assert.Equal("7cf5357dbc2014cbd616257c358ea0a1", account.Avatar.Gravatar.Hash);
        }

        [Fact]
        public void TestAccountAccountGetLists()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => Config.Client.AccountGetListsAsync(i).Result);
            AccountList list = Config.Client.AccountGetListsAsync().Sync().Results[0];

            Assert.NotNull(list.Id);
            Assert.NotNull(list.Name);
            Assert.Null(list.PosterPath);
            Assert.NotNull(list.Description);
            Assert.NotNull(list.ListType);
            Assert.NotNull(list.Iso_639_1);
        }

        [Fact]
        public void TestAccountGetFavoriteMovies()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => Config.Client.AccountGetFavoriteMoviesAsync(i).Result);
            SearchMovie movie = Config.Client.AccountGetFavoriteMoviesAsync().Sync().Results[0];

            // Requires that you have marked at least one movie as favorite else this test will fail
            Assert.True(movie.Id > 0);
            Assert.NotNull(movie.Title);
            Assert.NotNull(movie.PosterPath);
            Assert.NotNull(movie.BackdropPath);
            Assert.NotNull(movie.OriginalTitle);
            Assert.NotNull(movie.Overview);
            Assert.NotNull(movie.OriginalLanguage);
            Assert.NotNull(movie.ReleaseDate);
            Assert.True(movie.VoteCount > 0);
            Assert.True(movie.VoteAverage > 0);
            Assert.True(movie.Popularity > 0);

            Assert.NotNull(movie.GenreIds);
            Assert.True(movie.GenreIds.Any());
        }

        [Fact]
        public void TestAccountGetFavoriteTv()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => Config.Client.AccountGetFavoriteTvAsync(i).Result);
            SearchTv tvShow = Config.Client.AccountGetFavoriteTvAsync().Sync().Results[0];

            // Requires that you have marked at least one movie as favorite else this test will fail
            Assert.True(tvShow.Id > 0);
            Assert.NotNull(tvShow.Name);
            Assert.NotNull(tvShow.PosterPath);
            Assert.NotNull(tvShow.BackdropPath);
            Assert.NotNull(tvShow.OriginalName);
            Assert.NotNull(tvShow.Overview);
            Assert.NotNull(tvShow.OriginalLanguage);
            Assert.NotNull(tvShow.FirstAirDate);
            Assert.True(tvShow.VoteCount > 0);
            Assert.True(tvShow.VoteAverage > 0);
            Assert.True(tvShow.Popularity > 0);

            Assert.NotNull(tvShow.GenreIds);
            Assert.True(tvShow.GenreIds.Any());
        }

        [Fact]
        public void TestAccountGetMovieWatchlist()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => Config.Client.AccountGetFavoriteMoviesAsync(i).Result);
            SearchMovie movie = Config.Client.AccountGetFavoriteMoviesAsync().Sync().Results[0];

            // Requires that you have added at least one movie to your watchlist else this test will fail
            Assert.True(movie.Id > 0);
            Assert.NotNull(movie.Title);
            Assert.NotNull(movie.PosterPath);
            Assert.NotNull(movie.BackdropPath);
            Assert.NotNull(movie.OriginalTitle);
            Assert.NotNull(movie.Overview);
            Assert.NotNull(movie.OriginalLanguage);
            Assert.NotNull(movie.ReleaseDate);
            Assert.True(movie.VoteCount > 0);
            Assert.True(movie.VoteAverage > 0);
            Assert.True(movie.Popularity > 0);

            Assert.NotNull(movie.GenreIds);
            Assert.True(movie.GenreIds.Any());
        }

        [Fact]
        public void TestAccountGetTvWatchlist()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => Config.Client.AccountGetTvWatchlistAsync(i).Result);
            SearchTv tvShow = Config.Client.AccountGetTvWatchlistAsync().Sync().Results[0];

            // Requires that you have added at least one movie to your watchlist else this test will fail
            Assert.True(tvShow.Id > 0);
            Assert.NotNull(tvShow.Name);
            Assert.NotNull(tvShow.PosterPath);
            Assert.NotNull(tvShow.BackdropPath);
            Assert.NotNull(tvShow.OriginalName);
            Assert.NotNull(tvShow.Overview);
            Assert.NotNull(tvShow.OriginalLanguage);
            Assert.NotNull(tvShow.FirstAirDate);
            Assert.True(tvShow.VoteCount > 0);
            Assert.True(tvShow.VoteAverage > 0);
            Assert.True(tvShow.Popularity > 0);

            Assert.NotNull(tvShow.GenreIds);
            Assert.True(tvShow.GenreIds.Any());
        }

        [Fact]
        public void TestAccountGetRatedMovies()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => Config.Client.AccountGetFavoriteMoviesAsync(i).Result);
            SearchMovie movie = Config.Client.AccountGetFavoriteMoviesAsync().Sync().Results[0];

            // Requires that you have rated at least one movie else this test will fail
            Assert.True(movie.Id > 0);
            Assert.NotNull(movie.Title);
            Assert.NotNull(movie.PosterPath);
            Assert.NotNull(movie.BackdropPath);
            Assert.NotNull(movie.OriginalTitle);
            Assert.NotNull(movie.Overview);
            Assert.NotNull(movie.OriginalLanguage);
            Assert.NotNull(movie.ReleaseDate);
            Assert.True(movie.VoteCount > 0);
            Assert.True(movie.VoteAverage > 0);
            Assert.True(movie.Popularity > 0);

            Assert.NotNull(movie.GenreIds);
            Assert.True(movie.GenreIds.Any());
        }

        [Fact]
        public void TestAccountGetRatedTv()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => Config.Client.AccountGetRatedTvShowsAsync(i).Result);
            AccountSearchTv tvShow = Config.Client.AccountGetRatedTvShowsAsync().Sync().Results[0];

            // Requires that you have rated at least one movie else this test will fail
            Assert.True(tvShow.Id > 0);
            Assert.NotNull(tvShow.Name);
            Assert.NotNull(tvShow.PosterPath);
            Assert.NotNull(tvShow.BackdropPath);
            Assert.NotNull(tvShow.OriginalName);
            Assert.NotNull(tvShow.Overview);
            Assert.NotNull(tvShow.OriginalLanguage);
            Assert.NotNull(tvShow.FirstAirDate);
            Assert.True(tvShow.VoteCount > 0);
            Assert.True(tvShow.VoteAverage > 0);
            Assert.True(tvShow.Popularity > 0);

            Assert.NotNull(tvShow.GenreIds);
            Assert.True(tvShow.GenreIds.Any());
        }

        [Fact]
        public void TestAccountGetRatedTvEpisodes()
        {
            IgnoreMissingCSharp("results[array]._id / _id");

            // TODO: Error in TMDb: https://www.themoviedb.org/talk/557f1af49251410a2c002480
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            TestHelpers.SearchPages(i => Config.Client.AccountGetRatedTvShowEpisodesAsync(i).Result);
            AccountSearchTvEpisode tvEpisode = Config.Client.AccountGetRatedTvShowEpisodesAsync().Sync().Results[0];

            // Requires that you have rated at least one movie else this test will fail
            Assert.True(tvEpisode.Id > 0);
            Assert.True(tvEpisode.ShowId > 0);
            Assert.True(tvEpisode.EpisodeNumber > 0);
            Assert.True(tvEpisode.SeasonNumber > 0);
            Assert.NotNull(tvEpisode.Name);
            Assert.NotNull(tvEpisode.AirDate);
            Assert.NotNull(tvEpisode.StillPath);
            Assert.True(tvEpisode.VoteCount > 0);
            Assert.True(tvEpisode.VoteAverage > 0);
            Assert.True(tvEpisode.Rating > 0);
        }

        [Fact]
        public void TestAccountChangeTvFavoriteStatus()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie is not marked as favorite before we start the test
            if (DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho))
                Assert.True(Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false).Result);

            if (DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho))
                throw new Exception($"Test tv show '{IdHelper.DoctorWho}' was already marked as favorite. Unable to perform test correctly");

            // Try to mark is as a favorite
            Assert.True(Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.DoctorWho, true).Result);

            // Check if it worked
            Assert.True(DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho));

            // Try to un-mark is as a favorite
            Assert.True(Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false).Result);

            // Check if it worked
            Assert.False(DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho));
        }

        [Fact]
        public void TestAccountChangeMovieFavoriteStatus()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie is not marked as favorite before we start the test
            if (DoesFavoriteListContainSpecificMovie(IdHelper.Terminator))
                Assert.True(Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.Terminator, false).Result);

            if (DoesFavoriteListContainSpecificMovie(IdHelper.Terminator))
                throw new Exception($"Test movie '{IdHelper.Terminator}' was already marked as favorite. Unable to perform test correctly");

            // Try to mark is as a favorite
            Assert.True(Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.Terminator, true).Result);

            // Check if it worked
            Assert.True(DoesFavoriteListContainSpecificMovie(IdHelper.Terminator));

            // Try to un-mark is as a favorite
            Assert.True(Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.Terminator, false).Result);

            // Check if it worked
            Assert.False(DoesFavoriteListContainSpecificMovie(IdHelper.Terminator));
        }

        [Fact]
        public void TestAccountChangeTvWatchlistStatus()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie is not marked as favorite before we start the test
            if (DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho))
                Assert.True(Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false).Result);

            if (DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho))
                throw new Exception($"Test tv show '{IdHelper.DoctorWho}' was already on watchlist. Unable to perform test correctly");

            // Try to add an item to the watchlist
            Assert.True(Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.DoctorWho, true).Result);

            // Check if it worked
            Assert.True(DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho));

            // Try to remove item from watchlist
            Assert.True(Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false).Result);

            // Check if it worked
            Assert.False(DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho));
        }

        [Fact]
        public void TestAccountChangeMovieWatchlistStatus()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie is not marked as favorite before we start the test
            if (DoesWatchListContainSpecificMovie(IdHelper.Terminator))
                Assert.True(Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.Terminator, false).Result);

            if (DoesWatchListContainSpecificMovie(IdHelper.Terminator))
                throw new Exception($"Test movie '{IdHelper.Terminator}' was already on watchlist. Unable to perform test correctly");

            // Try to add an item to the watchlist
            Assert.True(Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.Terminator, true).Result);

            // Check if it worked
            Assert.True(DoesWatchListContainSpecificMovie(IdHelper.Terminator));

            // Try to remove item from watchlist
            Assert.True(Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.Terminator, false).Result);

            // Check if it worked
            Assert.False(DoesWatchListContainSpecificMovie(IdHelper.Terminator));
        }

        private bool DoesFavoriteListContainSpecificTvShow(int tvId)
        {
            return DoesListContainSpecificMovie(tvId, page => Config.Client.AccountGetFavoriteTvAsync(page).Result.Results.Select(s => s.Id));
        }

        private bool DoesWatchListContainSpecificTvShow(int tvId)
        {
            return DoesListContainSpecificMovie(tvId, page => Config.Client.AccountGetTvWatchlistAsync(page).Result.Results.Select(s => s.Id));
        }

        private bool DoesFavoriteListContainSpecificMovie(int movieId)
        {
            return DoesListContainSpecificMovie(movieId, page => Config.Client.AccountGetFavoriteMoviesAsync(page).Result.Results.Select(s => s.Id));
        }

        private bool DoesWatchListContainSpecificMovie(int movieId)
        {
            return DoesListContainSpecificMovie(movieId, page => Config.Client.AccountGetMovieWatchlistAsync(page).Result.Results.Select(s => s.Id));
        }

        private bool DoesListContainSpecificMovie(int movieId, Func<int, IEnumerable<int>> listGetter)
        {
            int page = 1;
            List<int> originalList = listGetter(1).ToList();
            while (originalList != null && originalList.Any())
            {
                // Check if the current result page contains the relevant movie
                if (originalList.Contains(movieId))
                    return true;

                // See if there is an other page we could try, if not the test passes
                originalList = originalList.Any() ? listGetter(++page).ToList() : null;
            }
            return false;
        }
    }
}
