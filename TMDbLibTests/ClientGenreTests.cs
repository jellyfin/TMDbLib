using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientGenreTests : TestBase
    {
        [Fact]
        public async Task TestGenreTvListAsync()
        {
            // Default language
            List<Genre> genres = await TMDbClient.GetTvGenresAsync();

            Assert.NotNull(genres);
            Assert.True(genres.Count > 0);

            // Another language
            List<Genre> genresDanish = await TMDbClient.GetTvGenresAsync("da");

            Assert.NotNull(genresDanish);
            Assert.True(genresDanish.Count > 0);

            Assert.Equal(genres.Count, genresDanish.Count);

            // At least one should be different
            Assert.Contains(genres, genre => genresDanish.First(danishGenre => danishGenre.Id == genre.Id).Name != genre.Name);
        }

        [Fact]
        public async Task TestGenreMovieListAsync()
        {
            // Default language
            List<Genre> genres = await TMDbClient.GetMovieGenresAsync();

            Assert.NotNull(genres);
            Assert.True(genres.Count > 0);

            // Another language
            List<Genre> genresDanish = await TMDbClient.GetMovieGenresAsync("da");

            Assert.NotNull(genresDanish);
            Assert.True(genresDanish.Count > 0);

            Assert.Equal(genres.Count, genresDanish.Count);

            // At least one should be different
            Assert.Contains(genres, genre => genresDanish.First(danishGenre => danishGenre.Id == genre.Id).Name != genre.Name);
        }

        [Fact]
        public async Task TestGenreMoviesAsync()
        {
            // Get first genre
            Genre genre = (await TMDbClient.GetMovieGenresAsync()).First();

            // Get movies
            SearchContainerWithId<SearchMovie> movies = await TMDbClient.GetGenreMoviesAsync(genre.Id);
            SearchContainerWithId<SearchMovie> moviesPage2 = await TMDbClient.GetGenreMoviesAsync(genre.Id, "it", 2, includeAllMovies: false);
            SearchContainerWithId<SearchMovie> moviesAll = await TMDbClient.GetGenreMoviesAsync(genre.Id, includeAllMovies: true);

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
