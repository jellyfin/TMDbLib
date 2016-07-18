using System;
using System.Linq;
using Xunit;
using TMDbLib.Objects.Credit;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientCreditTests : TestBase
    {
        private readonly TestConfig _config;

        public ClientCreditTests()
        {
            _config = new TestConfig();
        }

        [Fact]
        public void TestGetCreditBase()
        {
            Credit result = _config.Client.GetCreditsAsync(IdHelper.BruceWillisMiamiVice).Result;

            Assert.NotNull(result);
            Assert.Equal("cast", result.CreditType);
            Assert.Equal("Actors", result.Department);
            Assert.Equal("Actor", result.Job);
            Assert.Equal("tv", result.MediaType);
            Assert.Equal(IdHelper.BruceWillisMiamiVice, result.Id);

            Assert.NotNull(result.Person);
            Assert.Equal("Bruce Willis", result.Person.Name);
            Assert.Equal(62, result.Person.Id);

            Assert.NotNull(result.Media);
            Assert.Equal(1908, result.Media.Id);
            Assert.Equal("Miami Vice", result.Media.Name);
            Assert.Equal("Miami Vice", result.Media.OriginalName);
            Assert.Equal("", result.Media.Character);
        }

        [Fact]
        public void TestGetCreditEpisode()
        {
            Credit result = _config.Client.GetCreditsAsync(IdHelper.BruceWillisMiamiVice).Result;

            Assert.NotNull(result);
            Assert.NotNull(result.Media);
            Assert.NotNull(result.Media.Episodes);

            CreditEpisode item = result.Media.Episodes.SingleOrDefault(s => s.Name == "No Exit");
            Assert.NotNull(item);

            Assert.Equal(new DateTime(1984, 11, 9), item.AirDate);
            Assert.Equal(8, item.EpisodeNumber);
            Assert.Equal("No Exit", item.Name);
            Assert.Equal("Crockett attempts to help an old flame free herself from a racketeer, then is framed for taking bribes. Martin Castillo becomes the squad's new Lieutenant.", item.Overview);
            Assert.Equal(1, item.SeasonNumber);
            Assert.True(TestImagesHelpers.TestImagePath(item.StillPath), "item.StillPath was not a valid image path, was: " + item.StillPath);
        }

        [Fact]
        public void TestGetCreditSeasons()
        {
            Credit result = _config.Client.GetCreditsAsync(IdHelper.HughLaurieHouse).Result;

            Assert.NotNull(result);
            Assert.NotNull(result.Media);
            Assert.NotNull(result.Media.Seasons);

            CreditSeason item = result.Media.Seasons.SingleOrDefault(s => s.SeasonNumber == 1);
            Assert.NotNull(item);

            Assert.Equal(new DateTime(2004, 11, 16), item.AirDate);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.Equal(1, item.SeasonNumber);
        }
    }
}