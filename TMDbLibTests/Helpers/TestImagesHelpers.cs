using System;
using TMDbLib.Objects.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TMDbLibTests.Helpers
{
    public static class TestImagesHelpers
    {
        public static void TestImages(TestConfig config, Images images)
        {
            Assert.IsTrue(images.Backdrops.Count > 0);
            Assert.IsTrue(images.Posters.Count > 0);

            string backdropSizes = config.Client.Config.Images.BackdropSizes.First();
            string posterSizes = config.Client.Config.Images.PosterSizes.First();

            foreach (ImageData imageData in images.Backdrops)
            {
                Uri url = config.Client.GetImageUrl(backdropSizes, imageData.FilePath);
                Uri urlSecure = config.Client.GetImageUrl(backdropSizes, imageData.FilePath, true);

                Assert.IsTrue(TestHelpers.InternetUriExists(url));
                Assert.IsTrue(TestHelpers.InternetUriExists(urlSecure));
            }

            foreach (ImageData imageData in images.Posters)
            {
                Uri url = config.Client.GetImageUrl(posterSizes, imageData.FilePath);
                Uri urlSecure = config.Client.GetImageUrl(posterSizes, imageData.FilePath, true);

                Assert.IsTrue(TestHelpers.InternetUriExists(url));
                Assert.IsTrue(TestHelpers.InternetUriExists(urlSecure));
            }
        }
    }
}
