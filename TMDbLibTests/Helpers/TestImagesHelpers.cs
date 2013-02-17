using System;
using System.Collections.Generic;
using System.Diagnostics;
using TMDbLib.Objects.General;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMDbLibTests.Helpers
{
    public static class TestImagesHelpers
    {
        public static void TestImages(TestConfig config, Images images)
        {
            Assert.IsTrue(images.Backdrops.Count > 0);
            Assert.IsTrue(images.Posters.Count > 0);

            Debug.WriteLine("Found {0} backdrops, {1} posters.", images.Backdrops.Count, images.Posters.Count);

            List<string> backdropSizes = config.Client.Config.Images.BackdropSizes;
            List<string> posterSizes = config.Client.Config.Images.PosterSizes;

            Debug.WriteLine("Found {0} backdrop sizes, {1} poster sizes.", backdropSizes.Count, posterSizes.Count);
            Debug.WriteLine("Total: {0} backdrops", images.Backdrops.Count * backdropSizes.Count);

            foreach (ImageData imageData in images.Backdrops)
            {
                foreach (string size in backdropSizes)
                {
                    Uri url = config.Client.GetImageUrl(size, imageData.FilePath);
                    Uri urlSecure = config.Client.GetImageUrl(size, imageData.FilePath, true);

                    Assert.IsTrue(TestHelpers.InternetUriExists(url));
                    Assert.IsTrue(TestHelpers.InternetUriExists(urlSecure));
                }
            }

            Debug.WriteLine("Total: {0} posters", images.Posters.Count * posterSizes.Count);

            foreach (ImageData imageData in images.Posters)
            {
                foreach (string size in posterSizes)
                {
                    Uri url = config.Client.GetImageUrl(size, imageData.FilePath);
                    Uri urlSecure = config.Client.GetImageUrl(size, imageData.FilePath, true);

                    Assert.IsTrue(TestHelpers.InternetUriExists(url));
                    Assert.IsTrue(TestHelpers.InternetUriExists(urlSecure));
                }
            }
        }
    }
}
