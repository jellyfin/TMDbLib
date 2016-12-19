using System.Linq;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class AccountStateConverterTest : TestBase
    {
        /// <summary>
        /// Tests the AccountStateConverter on the AccountState type
        /// </summary>
        [Fact]
        public void TestAccountStateConverterAccountState()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            AccountState accountState = Config.Client.GetMovieAccountStateAsync(IdHelper.Avatar).Sync();

            Assert.Equal(IdHelper.Avatar, accountState.Id);
            Assert.True(accountState.Favorite);
            Assert.False(accountState.Watchlist);
            Assert.Equal(2.5d, accountState.Rating);
        }

        /// <summary>
        /// Tests the AccountStateConverter on the TvEpisodeAccountState type
        /// </summary>
        [Fact]
        public void TestAccountStateConverterTvEpisodeAccountState()
        {
            Config.Client.SetSessionInformation(Config.UserSessionId, SessionType.UserSession);
            ResultContainer<TvEpisodeAccountStateWithNumber> season = Config.Client.GetTvSeasonAccountStateAsync(IdHelper.BigBangTheory, 1).Sync();

            // Episode 1 has a rating
            TvEpisodeAccountStateWithNumber episode = season.Results.FirstOrDefault(s => s.EpisodeNumber == 1);
            Assert.NotNull(episode);

            Assert.Equal(IdHelper.BigBangTheorySeason1Episode1Id, episode.Id);
            Assert.Equal(1, episode.EpisodeNumber);
            Assert.Equal(5d, episode.Rating);

            // Episode 2 has no rating
            episode = season.Results.FirstOrDefault(s => s.EpisodeNumber == 2);
            Assert.NotNull(episode);

            Assert.Equal(IdHelper.BigBangTheorySeason1Episode2Id, episode.Id);
            Assert.Equal(2, episode.EpisodeNumber);
            Assert.Null(episode.Rating);
        }
    }
}