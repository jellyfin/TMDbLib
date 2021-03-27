using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMDbLib.Objects.Authentication;
using TMDbLib.Objects.General;
using TMDbLib.Objects.TvShows;
using TMDbLib.Utilities.Converters;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.UtilityTests
{
    public class AccountStateConverterTest : TestBase
    {
        [Fact]
        public void AccountStateConverter_WithData()
        {
            // { "rated": { "value": 5 } }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new AccountStateConverter());

            var original = new
            {
                rated = new { value = 5 }
            };

            string json = JsonConvert.SerializeObject(original, settings);
            AccountState result = JsonConvert.DeserializeObject<AccountState>(json, settings);

            Assert.Equal(5, result.Rating);
        }

        [Fact]
        public void AccountStateConverter_WithoutData()
        {
            // { "rated": False }

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new AccountStateConverter());

            var original = new { rated = false };
            string json = JsonConvert.SerializeObject(original, settings);
            AccountState result = JsonConvert.DeserializeObject<AccountState>(json, settings);

            Assert.Null(result.Rating);
        }

        /// <summary>
        /// Tests the AccountStateConverter on the AccountState type
        /// </summary>
        [Fact]
        public async Task TestAccountStateConverterAccountState()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            AccountState accountState = await TMDbClient.GetMovieAccountStateAsync(IdHelper.Avatar);

            await Verify(accountState);
        }

        /// <summary>
        /// Tests the AccountStateConverter on the TvEpisodeAccountState type
        /// </summary>
        [Fact]
        public async Task TestAccountStateConverterTvEpisodeAccountState()
        {
            await TMDbClient.SetSessionInformationAsync(TestConfig.UserSessionId, SessionType.UserSession);
            ResultContainer<TvEpisodeAccountStateWithNumber> season = await TMDbClient.GetTvSeasonAccountStateAsync(IdHelper.BigBangTheory, 1);

            // Episode 1 has a rating
            TvEpisodeAccountStateWithNumber episodeA = season.Results.Single(s => s.EpisodeNumber == 1);

            Assert.NotNull(episodeA.Rating);

            // Episode 2 has no rating
            TvEpisodeAccountStateWithNumber episodeB = season.Results.Single(s => s.EpisodeNumber == 2);
            Assert.Null(episodeB.Rating);

            await Verify(new
            {
                episodeA,
                episodeB
            });
        }
    }
}