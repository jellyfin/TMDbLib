using System.Collections.Generic;
using System.Linq;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.People;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests
{
    public class JsonMagicTests : TestBase
    {
        /// <summary>
        /// Tests the KnownForConverter
        /// </summary>
        [Fact]
        public void TestJsonKnownForConverter()
        {
            SearchContainer<SearchPerson> result = Config.Client.SearchPersonAsync("Willis").Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);

            List<KnownForBase> knownForList = result.Results.SelectMany(s => s.KnownFor).ToList();
            Assert.True(knownForList.Any());

            Assert.Contains(knownForList, item => item.MediaType == MediaType.Tv && item is KnownForTv);
            Assert.Contains(knownForList, item => item.MediaType == MediaType.Movie && item is KnownForMovie);
        }

        /// <summary>
        /// Tests the TaggedImageConverter
        /// </summary>
        [Fact]
        public void TestJsonTaggedImageConverter()
        {
            // Get images
            SearchContainerWithId<TaggedImage> result = Config.Client.GetPersonTaggedImagesAsync(IdHelper.HughLaurie, 1).Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);
            Assert.Equal(IdHelper.HughLaurie, result.Id);

            Assert.Contains(result.Results, item => item.MediaType == MediaType.Tv && item.Media is SearchTv);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Movie && item.Media is SearchMovie);
        }

        /// <summary>
        /// Tests the SearchBaseConverter
        /// </summary>
        [Fact]
        public void TestSearchBaseConverter()
        {
            TestHelpers.SearchPages(i => Config.Client.SearchMultiAsync("Rock", i).Sync());
            SearchContainer<SearchBase> result = Config.Client.SearchMultiAsync("Rock").Sync();

            Assert.NotNull(result);
            Assert.NotNull(result.Results);

            Assert.Contains(result.Results, item => item.MediaType == MediaType.Tv && item is SearchTv);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Movie && item is SearchMovie);
            Assert.Contains(result.Results, item => item.MediaType == MediaType.Person && item is SearchPerson);
        }

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
            ResultContainer<TvEpisodeAccountState> season = Config.Client.GetTvSeasonAccountStateAsync(IdHelper.BigBangTheory, 1).Sync();

            // Episode 1 has a rating
            TvEpisodeAccountState episode = season.Results.FirstOrDefault(s => s.EpisodeNumber == 1);
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