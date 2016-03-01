using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using System.Linq;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientSearchTests
    {
        private TestConfig _config;

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestSearchMovie()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchMovieAsync("007", i).Result);

            // Search pr. Year
            // 1962: First James Bond movie, "Dr. No"
            SearchContainer<SearchMovie> result = _config.Client.SearchMovieAsync("007", year: 1962).Result;

            Assert.IsTrue(result.Results.Any());
            SearchMovie item = result.Results.SingleOrDefault(s => s.Id == 646);

            Assert.IsNotNull(item);
            Assert.AreEqual(646, item.Id);
            Assert.AreEqual(false, item.Adult);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.AreEqual("en", item.OriginalLanguage);
            Assert.AreEqual("Dr. No", item.OriginalTitle);
            Assert.AreEqual("When Strangways, the British SIS Station Chief in Jamaica goes missing, MI6 send James Bond - Agent 007 to investigate. His investigation leads him to the mysterious Crab Key; the secret base of Dr No who he suspects is trying to sabotage the American space program using a radio beam. With the assistance of local fisherman Quarrel, who had been helping Strangways, Bond sneaks onto Crab Key where he meets the beautiful Honey Ryder. Can the three of them defeat an army of henchmen and a \"fire breathing dragon\" in order to stop Dr No, save the space program and get revenge for Strangways? Dr. No is the first film of legendary James Bond series starring Sean Connery in the role of Fleming's British super agent.", item.Overview);
            Assert.AreEqual(false, item.Video);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.AreEqual(new DateTime(1962, 10, 4), item.ReleaseDate);
            Assert.AreEqual("Dr. No", item.Title);
            Assert.IsTrue(item.Popularity > 0);
            Assert.IsTrue(item.VoteAverage > 0);
            Assert.IsTrue(item.VoteCount > 0);

            Assert.IsNotNull(item.GenreIds);
            Assert.IsTrue(item.GenreIds.Contains(12));
            Assert.IsTrue(item.GenreIds.Contains(28));
            Assert.IsTrue(item.GenreIds.Contains(53));
        }

        [TestMethod]
        public void TestSearchCollection()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchCollectionAsync("007", i).Result);

            SearchContainer<SearchResultCollection> result = _config.Client.SearchCollectionAsync("James Bond").Result;

            Assert.IsTrue(result.Results.Any());
            SearchResultCollection item = result.Results.SingleOrDefault(s => s.Id == 645);

            Assert.IsNotNull(item);
            Assert.AreEqual(645, item.Id);
            Assert.AreEqual("James Bond Collection", item.Name);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
        }

        [TestMethod]
        public void TestSearchPerson()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchPersonAsync("Willis", i).Result);

            SearchContainer<SearchPerson> result = _config.Client.SearchPersonAsync("Willis").Result;

            Assert.IsTrue(result.Results.Any());
            SearchPerson item = result.Results.SingleOrDefault(s => s.Id == 62);

            Assert.IsNotNull(item);
            Assert.AreEqual(62, item.Id);
            Assert.AreEqual("Bruce Willis", item.Name);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.ProfilePath), "item.ProfilePath was not a valid image path, was: " + item.ProfilePath);
            Assert.AreEqual(false, item.Adult);
            Assert.IsTrue(item.Popularity > 0);

            Assert.IsNotNull(item.KnownFor);
            Assert.IsTrue(item.KnownFor.Any(s => s.Id == 680));
        }

        [TestMethod]
        public void TestSearchList()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchListAsync("to watch", i).Result);

            SearchContainer<SearchList> result = _config.Client.SearchListAsync("to watch").Result;

            Assert.IsTrue(result.Results.Any());
            SearchList item = result.Results.SingleOrDefault(s => s.Id == "54a5c0ceaed56c28c300013a");

            Assert.IsNotNull(item);
            Assert.AreEqual("to watch", item.Description);
            Assert.AreEqual("54a5c0ceaed56c28c300013a", item.Id);
            Assert.AreEqual("en", item.Iso_639_1);
            Assert.AreEqual("movie", item.ListType);
            Assert.AreEqual("Movies", item.Name);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.IsTrue(item.FavoriteCount > 0);
            Assert.IsTrue(item.ItemCount > 0);
        }

        [TestMethod]
        public void TestSearchCompany()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchCompanyAsync("20th", i).Result);

            SearchContainer<SearchCompany> result = _config.Client.SearchCompanyAsync("20th").Result;

            Assert.IsTrue(result.Results.Any());
            SearchCompany item = result.Results.SingleOrDefault(s => s.Id == 25);

            Assert.IsNotNull(item);
            Assert.AreEqual(25, item.Id);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.LogoPath), "item.LogoPath was not a valid image path, was: " + item.LogoPath);
            Assert.AreEqual("20th Century Fox", item.Name);
        }

        [TestMethod]
        public void TestSearchKeyword()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchKeywordAsync("plot", i).Result);

            SearchContainer<SearchKeyword> result = _config.Client.SearchKeywordAsync("plot").Result;

            Assert.IsTrue(result.Results.Any());
            SearchKeyword item = result.Results.SingleOrDefault(s => s.Id == 11121);

            Assert.IsNotNull(item);
            Assert.AreEqual(11121, item.Id);
            Assert.AreEqual("plot", item.Name);
        }

        [TestMethod]
        public void TestSearchTvShow()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchTvShowAsync("Breaking Bad", i).Result);

            SearchContainer<SearchTv> result = _config.Client.SearchTvShowAsync("Breaking Bad").Result;

            Assert.IsTrue(result.Results.Any());
            SearchTv item = result.Results.SingleOrDefault(s => s.Id == 1396);

            Assert.IsNotNull(item);
            Assert.AreEqual(1396, item.Id);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.AreEqual(new DateTime(2008, 1, 19), item.FirstAirDate);
            Assert.AreEqual("Breaking Bad", item.Name);
            Assert.AreEqual("Breaking Bad", item.OriginalName);
            Assert.AreEqual("en", item.OriginalLanguage);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.AreEqual("Breaking Bad is an American crime drama television series created and produced by Vince Gilligan. Set and produced in Albuquerque, New Mexico, Breaking Bad is the story of Walter White, a struggling high school chemistry teacher who is diagnosed with inoperable lung cancer at the beginning of the series. He turns to a life of crime, producing and selling methamphetamine, in order to secure his family's financial future before he dies, teaming with his former student, Jesse Pinkman. Heavily serialized, the series is known for positioning its characters in seemingly inextricable corners and has been labeled a contemporary western by its creator.", item.Overview);
            Assert.IsTrue(item.Popularity > 0);
            Assert.IsTrue(item.VoteAverage > 0);
            Assert.IsTrue(item.VoteCount > 0);

            Assert.IsNotNull(item.GenreIds);
            Assert.AreEqual(1, item.GenreIds.Count);
            Assert.AreEqual(18, item.GenreIds[0]);

            Assert.IsNotNull(item.OriginCountry);
            Assert.AreEqual(1, item.OriginCountry.Count);
            Assert.AreEqual("US", item.OriginCountry[0]);
        }

        [TestMethod]
        public void TestSearchMulti()
        {
            TestHelpers.SearchPages(i => _config.Client.SearchMultiAsync("Arrow", i).Result);

            SearchContainer<SearchMulti> result = _config.Client.SearchMultiAsync("Arrow").Result;

            Assert.IsTrue(result.Results.Any());
            SearchMulti item = result.Results.SingleOrDefault(s => s.Id == 1412);

            Assert.IsNotNull(item);
            Assert.AreEqual(1412, item.Id);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.BackdropPath), "item.BackdropPath was not a valid image path, was: " + item.BackdropPath);
            Assert.AreEqual(new DateTime(2012, 10, 10), item.FirstAirDate);
            Assert.AreEqual(MediaType.TVShow, item.Type);
            Assert.AreEqual("Arrow", item.Name);
            Assert.AreEqual("Arrow", item.OriginalName);
            Assert.IsTrue(TestImagesHelpers.TestImagePath(item.PosterPath), "item.PosterPath was not a valid image path, was: " + item.PosterPath);
            Assert.IsTrue(item.Popularity > 0);
            Assert.IsTrue(item.VoteAverage > 0);
            Assert.IsTrue(item.VoteCount > 0);

            Assert.IsNotNull(item.OriginCountry);
            Assert.AreEqual(2, item.OriginCountry.Count);
            Assert.IsTrue(item.OriginCountry.Contains("US"));
            Assert.IsTrue(item.OriginCountry.Contains("CA"));
        }
    }
}
