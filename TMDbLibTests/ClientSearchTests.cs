using System;
using Xunit;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Linq;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
    public class ClientSearchTests : TestBase
    {
        [Fact]
        public void TestSearchMovie()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            TestHelpers.SearchPages(i => Config.Client.SearchMovieAsync("007", i).Result);

            // Search pr. Year
            // 1962: First James Bond movie, "Dr. No"
            SearchContainer<SearchMovie> result = Config.Client.SearchMovieAsync("007", year: 1962).Result;

            Assert.True(result.Results.Any());
            SearchMovie item = result.Results.SingleOrDefault(s => s.Id == 646);

            Assert.NotNull(item);
            Assert.Equal(646, item.Id);
            Assert.Equal(false, item.Adult);
            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.Equal("en", item.OriginalLanguage);
            Assert.Equal("Dr. No", item.OriginalTitle);
            Assert.Equal("In the film that launched the James Bond saga, Agent 007 (Sean Connery) battles mysterious Dr. No, a scientific genius bent on destroying the U.S. space program. As the countdown to disaster begins, Bond must go to Jamaica, where he encounters beautiful Honey Ryder (Ursula Andress), to confront a megalomaniacal villain in his massive island headquarters.", item.Overview);
            Assert.Equal(false, item.Video);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.Equal(new DateTime(1962, 10, 4), item.ReleaseDate);
            Assert.Equal("Dr. No", item.Title);
            Assert.True(item.Popularity > 0);
            Assert.True(item.VoteAverage > 0);
            Assert.True(item.VoteCount > 0);

            Assert.NotNull(item.GenreIds);
            Assert.True(item.GenreIds.Contains(12));
            Assert.True(item.GenreIds.Contains(28));
            Assert.True(item.GenreIds.Contains(53));
        }

        [Fact]
        public void TestSearchCollection()
        {
            TestHelpers.SearchPages(i => Config.Client.SearchCollectionAsync("007", i).Result);

            SearchContainer<SearchCollection> result = Config.Client.SearchCollectionAsync("James Bond").Result;

            Assert.True(result.Results.Any());
            SearchCollection item = result.Results.SingleOrDefault(s => s.Id == 645);

            Assert.NotNull(item);
            Assert.Equal(645, item.Id);
            Assert.Equal("James Bond Collection", item.Name);
            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
        }

        [Fact]
        public void TestSearchPerson()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            TestHelpers.SearchPages(i => Config.Client.SearchPersonAsync("Willis", i).Result);

            SearchContainer<SearchPerson> result = Config.Client.SearchPersonAsync("Willis").Result;

            Assert.True(result.Results.Any());
            SearchPerson item = result.Results.SingleOrDefault(s => s.Id == 62);

            Assert.NotNull(item);
            Assert.Equal(62, item.Id);
            Assert.Equal("Bruce Willis", item.Name);
            Assert.True(TestImagesHelpers.TestImagePath(item.ProfilePath), "item.ProfilePath was not a valid image path, was: " + item.ProfilePath);
            Assert.Equal(false, item.Adult);
            Assert.True(item.Popularity > 0);

            Assert.NotNull(item.KnownFor);
            Assert.True(item.KnownFor.Any(s => s.Id == 680));
        }

        [Fact]
        public void TestSearchList()
        {
            TestHelpers.SearchPages(i => Config.Client.SearchListAsync("to watch", i).Result);

            SearchContainer<SearchList> result = Config.Client.SearchListAsync("to watch").Result;

            Assert.True(result.Results.Any());
            SearchList item = result.Results.SingleOrDefault(s => s.Id == "54a5c0ceaed56c28c300013a");

            Assert.NotNull(item);
            Assert.Equal("to watch", item.Description);
            Assert.Equal("54a5c0ceaed56c28c300013a", item.Id);
            Assert.Equal("en", item.Iso_639_1);
            Assert.Equal("movie", item.ListType);
            Assert.Equal("Movies", item.Name);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.True(item.FavoriteCount > 0);
            Assert.True(item.ItemCount > 0);
        }

        [Fact]
        public void TestSearchCompany()
        {
            TestHelpers.SearchPages(i => Config.Client.SearchCompanyAsync("20th", i).Result);

            SearchContainer<SearchCompany> result = Config.Client.SearchCompanyAsync("20th").Result;

            Assert.True(result.Results.Any());
            SearchCompany item = result.Results.SingleOrDefault(s => s.Id == 25);

            Assert.NotNull(item);
            Assert.Equal(25, item.Id);
            Assert.True(TestImagesHelpers.TestImagePath(item.LogoPath), "item.LogoPath was not a valid image path, was: " + item.LogoPath);
            Assert.Equal("20th Century Fox", item.Name);
        }

        [Fact]
        public void TestSearchKeyword()
        {
            TestHelpers.SearchPages(i => Config.Client.SearchKeywordAsync("plot", i).Result);

            SearchContainer<SearchKeyword> result = Config.Client.SearchKeywordAsync("plot").Result;

            Assert.True(result.Results.Any());
            SearchKeyword item = result.Results.SingleOrDefault(s => s.Id == 11121);

            Assert.NotNull(item);
            Assert.Equal(11121, item.Id);
            Assert.Equal("plot", item.Name);
        }

        [Fact]
        public void TestSearchTvShow()
        {
            // Ignore missing json
            IgnoreMissingJson("results[array] / media_type");

            TestHelpers.SearchPages(i => Config.Client.SearchTvShowAsync("Breaking Bad", i).Result);

            SearchContainer<SearchTv> result = Config.Client.SearchTvShowAsync("Breaking Bad").Result;

            Assert.True(result.Results.Any());
            SearchTv item = result.Results.SingleOrDefault(s => s.Id == 1396);

            Assert.NotNull(item);
            Assert.Equal(1396, item.Id);
            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.Equal(new DateTime(2008, 1, 19), item.FirstAirDate);
            Assert.Equal("Breaking Bad", item.Name);
            Assert.Equal("Breaking Bad", item.OriginalName);
            Assert.Equal("en", item.OriginalLanguage);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.Equal("Breaking Bad is an American crime drama television series created and produced by Vince Gilligan. Set and produced in Albuquerque, New Mexico, Breaking Bad is the story of Walter White, a struggling high school chemistry teacher who is diagnosed with inoperable lung cancer at the beginning of the series. He turns to a life of crime, producing and selling methamphetamine, in order to secure his family's financial future before he dies, teaming with his former student, Jesse Pinkman. Heavily serialized, the series is known for positioning its characters in seemingly inextricable corners and has been labeled a contemporary western by its creator.", item.Overview);
            Assert.True(item.Popularity > 0);
            Assert.True(item.VoteAverage > 0);
            Assert.True(item.VoteCount > 0);

            Assert.NotNull(item.GenreIds);
            Assert.Equal(1, item.GenreIds.Count);
            Assert.Equal(18, item.GenreIds[0]);

            Assert.NotNull(item.OriginCountry);
            Assert.Equal(1, item.OriginCountry.Count);
            Assert.Equal("US", item.OriginCountry[0]);
        }

        [Fact]
        public void TestSearchMulti()
        {
            TestHelpers.SearchPages(i => Config.Client.SearchMultiAsync("Arrow", i).Result);

            SearchContainer<SearchBase> result = Config.Client.SearchMultiAsync("Arrow").Result;

            Assert.True(result.Results.Any());
            SearchTv item = result.Results.OfType<SearchTv>().SingleOrDefault(s => s.Id == 1412);

            Assert.NotNull(item);
            Assert.Equal(1412, item.Id);
            Assert.True(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.Equal(new DateTime(2012, 10, 10), item.FirstAirDate);
            Assert.Equal(MediaType.Tv, item.MediaType);
            Assert.Equal("Arrow", item.Name);
            Assert.Equal("Arrow", item.OriginalName);
            Assert.True(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.True(item.Popularity > 0);
            Assert.True(item.VoteAverage > 0);
            Assert.True(item.VoteCount > 0);

            Assert.NotNull(item.OriginCountry);
            Assert.Equal(2, item.OriginCountry.Count);
            Assert.True(item.OriginCountry.Contains("US"));
            Assert.True(item.OriginCountry.Contains("CA"));
        }
    }
}
