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
        public async Task TestKeywordGet()
        {
            KeywordsContainer keywords = await Config.Client.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard);

            Assert.NotNull(keywords);
            Assert.NotNull(keywords.Keywords);
            Assert.True(keywords.Keywords.Count > 0);

            // Try to get all keywords
            foreach (Keyword testKeyword in keywords.Keywords)
            {
                Keyword getKeyword = await Config.Client.GetKeywordAsync(testKeyword.Id);

                Assert.NotNull(getKeyword);

                Assert.Equal(testKeyword.Id, getKeyword.Id);
                Assert.Equal(testKeyword.Name, getKeyword.Name);
            }
        }

        [Fact]
        public async Task TestKeywordsMissing()
        {
            KeywordsContainer keywords = await Config.Client.GetMovieKeywordsAsync(IdHelper.MissingID);

            Assert.Null(keywords);
        }

        [Fact]
        public async Task TestKeywordMovies()
        {
            KeywordsContainer keywords = await Config.Client.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard);

            Assert.NotNull(keywords);
            Assert.NotNull(keywords.Keywords);
            Assert.True(keywords.Keywords.Count > 0);

            // Get first keyword
            Keyword testKeyword = keywords.Keywords.First();

            // Get movies
            SearchContainerWithId<SearchMovie> movies = await Config.Client.GetKeywordMoviesAsync(testKeyword.Id);
            SearchContainerWithId<SearchMovie> moviesItalian = await Config.Client.GetKeywordMoviesAsync(testKeyword.Id, "it");
            SearchContainerWithId<SearchMovie> moviesPage2 = await Config.Client.GetKeywordMoviesAsync(testKeyword.Id, 2);

            Assert.NotNull(movies);
            Assert.NotNull(moviesItalian);
            Assert.NotNull(moviesPage2);

            Assert.Equal(testKeyword.Id, movies.Id);
            Assert.Equal(testKeyword.Id, moviesItalian.Id);
            Assert.Equal(testKeyword.Id, moviesPage2.Id);

            Assert.True(movies.Results.Count > 0);
            Assert.True(moviesItalian.Results.Count > 0);

            if (movies.TotalResults > movies.Results.Count)
                Assert.True(moviesPage2.Results.Count > 0);
            else
                Assert.Empty(moviesPage2.Results);

            Assert.Equal(1, movies.Page);
            Assert.Equal(1, moviesItalian.Page);
            Assert.Equal(2, moviesPage2.Page);

            // All titles on page 1 must be the same
            bool allTitlesIdentical = true;
            for (int index = 0; index < movies.Results.Count; index++)
            {
                Assert.Equal(movies.Results[index].Id, moviesItalian.Results[index].Id);

                // At least one title must differ in title
                if (movies.Results[index].Title != moviesItalian.Results[index].Title)
                    allTitlesIdentical = false;
            }

            Assert.False(allTitlesIdentical);
        }
    }
}