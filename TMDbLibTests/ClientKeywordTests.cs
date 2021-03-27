using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Movies;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientKeywordTests : TestBase
    {
        [Fact]
        public async Task TestGetMovieKeywordsAsync()
        {
            KeywordsContainer keywords = await TMDbClient.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard);

            await Verify(keywords);
        }

        [Fact]
        public async Task TestGetTvShowKeywordsAsync()
        {
            ResultContainer<Keyword> keywords = await TMDbClient.GetTvShowKeywordsAsync(IdHelper.BigBangTheory);

            await Verify(keywords);
        }

        [Fact]
        public async Task TestKeywordGetSingle()
        {
            Keyword keyword = await TMDbClient.GetKeywordAsync(IdHelper.AgentKeyword);

            await Verify(keyword);
        }

        [Fact]
        public async Task TestKeywordsMissing()
        {
            KeywordsContainer keywords = await TMDbClient.GetMovieKeywordsAsync(IdHelper.MissingID);

            Assert.Null(keywords);
        }

        [Fact]
        public async Task TestKeywordMovies()
        {
            SearchContainerWithId<SearchMovie> movies = await TMDbClient.GetKeywordMoviesAsync(IdHelper.AgentKeyword);

            Assert.Equal(IdHelper.AgentKeyword, movies.Id);
            Assert.NotEmpty(movies.Results);

            KeywordsContainer movie = await TMDbClient.GetMovieKeywordsAsync(movies.Results.First().Id);

            Assert.Contains(movie.Keywords, x => IdHelper.AgentKeyword == x.Id);
        }
    }
}