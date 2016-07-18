using System.Collections.Generic;
using System.Linq;
using Xunit;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientGenreTests : TestBase
    {
        [Fact]
        public void TestGenreTvList()
        {
            // Default language
            List<Genre> genres = Config.Client.GetTvGenresAsync().Sync();

            Assert.NotNull(genres);
            Assert.True(genres.Count > 0);

            // Another language
            List<Genre> genresDanish = Config.Client.GetTvGenresAsync("da").Result;

            Assert.NotNull(genresDanish);
            Assert.True(genresDanish.Count > 0);

            Assert.Equal(genres.Count, genresDanish.Count);

            // At least one should be different
            Assert.True(genres.Any(genre => genresDanish.First(danishGenre => danishGenre.Id == genre.Id).Name != genre.Name));
        }

        [Fact]
        public void TestGenreMovieList()
        {
            // Default language
            List<Genre> genres = Config.Client.GetMovieGenresAsync().Sync();

            Assert.NotNull(genres);
            Assert.True(genres.Count > 0);

            // Another language
            List<Genre> genresDanish = Config.Client.GetMovieGenresAsync("da").Result;

            Assert.NotNull(genresDanish);
            Assert.True(genresDanish.Count > 0);

            Assert.Equal(genres.Count, genresDanish.Count);

            // At least one should be different
            Assert.True(genres.Any(genre => genresDanish.First(danishGenre => danishGenre.Id == genre.Id).Name != genre.Name));
        }

        [Fact]
        public void TestGenreMovies()
        {
            // Get first genre
            Genre genre = Config.Client.GetMovieGenresAsync().Sync().First();

            // Get movies
            SearchContainerWithId<MovieResult> movies = Config.Client.GetGenreMoviesAsync(genre.Id).Result;
            SearchContainerWithId<MovieResult> moviesPage2 = Config.Client.GetGenreMoviesAsync(genre.Id, "it", 2, includeAllMovies: false).Result;
            SearchContainerWithId<MovieResult> moviesAll = Config.Client.GetGenreMoviesAsync(genre.Id, includeAllMovies: true).Result;

            Assert.Equal(1, movies.Page);
            Assert.Equal(2, moviesPage2.Page);
            Assert.Equal(1, moviesAll.Page);

            Assert.True(movies.Results.Count > 0);
            Assert.True(moviesPage2.Results.Count > 0);
            Assert.True(moviesAll.Results.Count > 0);

            Assert.True(movies.Results.All(s => s != null));
            Assert.True(moviesPage2.Results.All(s => s != null));
            Assert.True(moviesAll.Results.All(s => s != null));

            Assert.Equal(movies.TotalResults, moviesPage2.TotalResults);     // Should be the same, despite the use of 'includeAllMovies' and Italian
            Assert.True(moviesAll.TotalResults >= movies.TotalResults);
        }
    }
}
