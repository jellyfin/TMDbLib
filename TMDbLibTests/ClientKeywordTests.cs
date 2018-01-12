using System.Linq;
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
        public void TestKeywordGet()
        {
            KeywordsContainer keywords = Config.Client.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard).Result;

            Assert.NotNull(keywords);
            Assert.NotNull(keywords.Keywords);
            Assert.True(keywords.Keywords.Count > 0);

            // Try to get all keywords
            foreach (Keyword testKeyword in keywords.Keywords)
            {
                Keyword getKeyword = Config.Client.GetKeywordAsync(testKeyword.Id).Result;

                Assert.NotNull(getKeyword);

                Assert.Equal(testKeyword.Id, getKeyword.Id);
                Assert.Equal(testKeyword.Name, getKeyword.Name);
            }
        }

        [Fact]
        public void TestKeywordMovies()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            KeywordsContainer keywords = Config.Client.GetMovieKeywordsAsync(IdHelper.AGoodDayToDieHard).Result;

            Assert.NotNull(keywords);
            Assert.NotNull(keywords.Keywords);
            Assert.True(keywords.Keywords.Count > 0);

            // Get first keyword
            Keyword testKeyword = keywords.Keywords.First();

            // Get movies
            SearchContainerWithId<SearchMovie> movies = Config.Client.GetKeywordMoviesAsync(testKeyword.Id).Result;
            SearchContainerWithId<SearchMovie> moviesItalian = Config.Client.GetKeywordMoviesAsync(testKeyword.Id, "it").Result;
            SearchContainerWithId<SearchMovie> moviesPage2 = Config.Client.GetKeywordMoviesAsync(testKeyword.Id, 2).Result;

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
                Assert.Equal(0, moviesPage2.Results.Count);

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
