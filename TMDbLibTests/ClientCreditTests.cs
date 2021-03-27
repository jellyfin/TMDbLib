using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Credit;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientCreditTests : TestBase
    {
        [Fact]
        public async Task TestGetCreditTv()
        {
            Credit result = await TMDbClient.GetCreditsAsync(IdHelper.BruceWillisMiamiVice);

            // Episode must exist
            Assert.Contains(result.Media.Episodes, s => s.Name == "No Exit");

            await Verify(result);
        }

        [Fact]
        public async Task TestMissingCredit()
        {
            Credit result = await TMDbClient.GetCreditsAsync(IdHelper.MissingID.ToString());

            Assert.Null(result);
        }

        [Fact]
        public async Task TestGetCreditSeasons()
        {
            Credit result = await TMDbClient.GetCreditsAsync(IdHelper.HughLaurieHouse);

            // Season must exist
            Assert.Contains(result.Media.Seasons, s => s.SeasonNumber == 1);

            await Verify(result);
        }
    }
}