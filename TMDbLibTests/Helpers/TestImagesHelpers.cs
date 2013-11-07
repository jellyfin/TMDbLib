using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TMDbLib.Objects.People;

namespace TMDbLibTests.Helpers
{
    public static class TestImagesHelpers
    {
        public static void TestImages(TestConfig config, ProfileImages images)
        {
            Assert.IsTrue(images.Profiles.Count > 0);

            string profileSize = config.Client.Config.Images.ProfileSizes.First();

            TestImagesInternal(config, images.Profiles.Select(s => s.FilePath), profileSize);
        }

        public static void TestImages(TestConfig config, Images images)
        {
            Assert.IsTrue(images.Backdrops.Count > 0);
            Assert.IsTrue(images.Posters.Count > 0);

            string backdropSize = config.Client.Config.Images.BackdropSizes.First();
            string posterSize = config.Client.Config.Images.PosterSizes.First();

            TestImagesInternal(config, images.Backdrops.Select(s => s.FilePath), backdropSize);

            TestImagesInternal(config, images.Posters.Select(s => s.FilePath), posterSize);
        }

        private static void TestImagesInternal(TestConfig config, IEnumerable<string> images, string posterSize)
        {
            foreach (string imageData in images)
            {
                Uri url = config.Client.GetImageUrl(posterSize, imageData);
                Uri urlSecure = config.Client.GetImageUrl(posterSize, imageData, true);

                Assert.IsTrue(TestHelpers.InternetUriExists(url));
                Assert.IsTrue(TestHelpers.InternetUriExists(urlSecure));
            }
        }
    }
}
