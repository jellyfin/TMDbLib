using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Reviews;
using TMDbLib.Objects.General;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientReviewTests
    {
        private TestConfig _config;
        private const string TheDarkKnightRisesReviewId = "5010553819c2952d1b000451";

        /// <summary>
        /// Run once, on every test
        /// </summary>
        [TestInitialize]
        public void Initiator()
        {
            _config = new TestConfig();
        }

        [TestMethod]
        public void TestReviewFullDetails()
        {
            Review review = _config.Client.GetReview(TheDarkKnightRisesReviewId).Result;

            Assert.IsNotNull(review);

            Assert.AreEqual(TheDarkKnightRisesReviewId, review.Id);
            Assert.AreEqual(49026, review.MediaId);
            Assert.AreEqual("The Dark Knight Rises", review.MediaTitle);
            Assert.AreEqual("Travis Bell", review.Author);
            Assert.AreEqual("en", review.Iso_639_1);
            Assert.AreEqual("http://j.mp/QSjAK2", review.Url);
            Assert.AreEqual(MediaType.Movie, review.MediaType);
        }
    }
}
