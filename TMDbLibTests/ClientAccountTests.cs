using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task TestAccountGetDetailsGuestAccount()
        {
            await Config.Client.SetSessionInformationAsync(Config.GuestTestSessionId, SessionType.GuestSession);

            await Assert.ThrowsAsync<UserSessionRequiredException>(() => Config.Client.AccountGetDetailsAsync());
        }

        [Fact]
        public async Task TestAccountGetDetailsUserAccount()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            AccountDetails account = await Config.Client.AccountGetDetailsAsync();

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
        public async Task TestAccountAccountGetLists()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            await TestHelpers.SearchPagesAsync(i => Config.Client.AccountGetListsAsync(i));
            AccountList list = (await Config.Client.AccountGetListsAsync()).Results[0];

            Assert.NotNull(list.Id);
            Assert.NotNull(list.Name);
            Assert.Null(list.PosterPath);
            Assert.NotNull(list.Description);
            Assert.NotNull(list.Iso_639_1);
        }

        [Fact]
        public async Task TestAccountGetFavoriteMovies()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            await TestHelpers.SearchPagesAsync(i => Config.Client.AccountGetFavoriteMoviesAsync(i));
            SearchMovie movie = (await Config.Client.AccountGetFavoriteMoviesAsync()).Results[0];

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
        public async Task TestAccountGetFavoriteTv()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            await TestHelpers.SearchPagesAsync(i => Config.Client.AccountGetFavoriteTvAsync(i));
            SearchTv tvShow = (await Config.Client.AccountGetFavoriteTvAsync()).Results[0];

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
        public async Task TestAccountGetMovieWatchlist()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            await TestHelpers.SearchPagesAsync(i => Config.Client.AccountGetFavoriteMoviesAsync(i));
            SearchMovie movie = (await Config.Client.AccountGetFavoriteMoviesAsync()).Results[0];

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
        public async Task TestAccountGetTvWatchlist()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            await TestHelpers.SearchPagesAsync(i => Config.Client.AccountGetTvWatchlistAsync(i));
            SearchTv tvShow = (await Config.Client.AccountGetTvWatchlistAsync()).Results[0];

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
        public async Task TestAccountGetRatedMovies()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            await TestHelpers.SearchPagesAsync(i => Config.Client.AccountGetFavoriteMoviesAsync(i));
            SearchMovie movie = (await Config.Client.AccountGetFavoriteMoviesAsync()).Results[0];

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
        public async Task TestAccountGetRatedTv()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            await TestHelpers.SearchPagesAsync(i => Config.Client.AccountGetRatedTvShowsAsync(i));
            AccountSearchTv tvShow = (await Config.Client.AccountGetRatedTvShowsAsync()).Results[0];

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
        public async Task TestAccountGetRatedTvEpisodes()
        {
            // TODO: Error in TMDb: https://www.themoviedb.org/talk/557f1af49251410a2c002480
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);
            await TestHelpers.SearchPagesAsync(i => Config.Client.AccountGetRatedTvShowEpisodesAsync(i));
            AccountSearchTvEpisode tvEpisode = (await Config.Client.AccountGetRatedTvShowEpisodesAsync()).Results[0];

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
        public async Task TestAccountChangeTvFavoriteStatusAsync()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie is not marked as favorite before we start the test
            if (await DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho))
                Assert.True(await Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false));

            if (await DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho))
                throw new Exception($"Test tv show '{IdHelper.DoctorWho}' was already marked as favorite. Unable to perform test correctly");

            // Try to mark is as a favorite
            Assert.True(await Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.DoctorWho, true));

            // Check if it worked
            Assert.True(await DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho));

            // Try to un-mark is as a favorite
            Assert.True(await Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false));

            // Check if it worked
            Assert.False(await DoesFavoriteListContainSpecificTvShow(IdHelper.DoctorWho));
        }

        [Fact]
        public async Task TestAccountChangeMovieFavoriteStatusAsync()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie is not marked as favorite before we start the test
            if (await DoesFavoriteListContainSpecificMovie(IdHelper.Terminator))
                Assert.True(await Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.Terminator, false));

            if (await DoesFavoriteListContainSpecificMovie(IdHelper.Terminator))
                throw new Exception($"Test movie '{IdHelper.Terminator}' was already marked as favorite. Unable to perform test correctly");

            // Try to mark is as a favorite
            Assert.True(await Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.Terminator, true));

            // Check if it worked
            Assert.True(await DoesFavoriteListContainSpecificMovie(IdHelper.Terminator));

            // Try to un-mark is as a favorite
            Assert.True(await Config.Client.AccountChangeFavoriteStatusAsync(MediaType.Movie, IdHelper.Terminator, false));

            // Check if it worked
            Assert.False(await DoesFavoriteListContainSpecificMovie(IdHelper.Terminator));
        }

        [Fact]
        public async Task TestAccountChangeTvWatchlistStatusAsync()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie is not marked as favorite before we start the test
            if (await DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho))
                Assert.True(await Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false));

            if (await DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho))
                throw new Exception($"Test tv show '{IdHelper.DoctorWho}' was already on watchlist. Unable to perform test correctly");

            // Try to add an item to the watchlist
            Assert.True(await Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.DoctorWho, true));

            // Check if it worked
            Assert.True(await DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho));

            // Try to remove item from watchlist
            Assert.True(await Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Tv, IdHelper.DoctorWho, false));

            // Check if it worked
            Assert.False(await DoesWatchListContainSpecificTvShow(IdHelper.DoctorWho));
        }

        [Fact]
        public async Task TestAccountChangeMovieWatchlistStatusAsync()
        {
            await Config.Client.SetSessionInformationAsync(Config.UserSessionId, SessionType.UserSession);

            // Ensure that the test movie is not marked as favorite before we start the test
            if (await DoesWatchListContainSpecificMovie(IdHelper.Terminator))
                Assert.True(await Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.Terminator, false));

            if (await DoesWatchListContainSpecificMovie(IdHelper.Terminator))
                throw new Exception($"Test movie '{IdHelper.Terminator}' was already on watchlist. Unable to perform test correctly");

            // Try to add an item to the watchlist
            Assert.True(await Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.Terminator, true));

            // Check if it worked
            Assert.True(await DoesWatchListContainSpecificMovie(IdHelper.Terminator));

            // Try to remove item from watchlist
            Assert.True(await Config.Client.AccountChangeWatchlistStatusAsync(MediaType.Movie, IdHelper.Terminator, false));

            // Check if it worked
            Assert.False(await DoesWatchListContainSpecificMovie(IdHelper.Terminator));
        }

        private async Task<bool> DoesFavoriteListContainSpecificTvShow(int tvId)
        {
            return await DoesListContainSpecificMovie(tvId, async page => (await Config.Client.AccountGetFavoriteTvAsync(page)).Results.Select(s => s.Id));
        }

        private async Task<bool> DoesWatchListContainSpecificTvShow(int tvId)
        {
            return await DoesListContainSpecificMovie(tvId, async page => (await Config.Client.AccountGetTvWatchlistAsync(page)).Results.Select(s => s.Id));
        }

        private async Task<bool> DoesFavoriteListContainSpecificMovie(int movieId)
        {
            return await DoesListContainSpecificMovie(movieId, async page => (await Config.Client.AccountGetFavoriteMoviesAsync(page)).Results.Select(s => s.Id));
        }

        private async Task<bool> DoesWatchListContainSpecificMovie(int movieId)
        {
            return await DoesListContainSpecificMovie(movieId, async page => (await Config.Client.AccountGetMovieWatchlistAsync(page)).Results.Select(s => s.Id));
        }

        private async Task<bool> DoesListContainSpecificMovie(int movieId, Func<int, Task<IEnumerable<int>>> listGetter)
        {
            int page = 1;
            List<int> originalList = (await listGetter(1)).ToList();
            while (originalList != null && originalList.Any())
            {
                // Check if the current result page contains the relevant movie
                if (originalList.Contains(movieId))
                    return true;

                // See if there is an other page we could try, if not the test passes
                originalList = originalList.Any() ? (await listGetter(++page)).ToList() : null;
            }
            return false;
        }
    }
}
