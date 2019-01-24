using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;
using System.Linq;
using System.Text.RegularExpressions;
using TMDbLib.Objects.People;
using TMDbLibTests.JsonHelpers;
using Xunit;

namespace TMDbLibTests.Helpers
{
    public static class TestImagesHelpers
    {
        private static readonly Regex ImagePathRegex = new Regex(@"^/[a-zA-Z0-9]{26,}\.(?:jpg|png)$", RegexOptions.Compiled);

        public static void TestImages(TestConfig config, ProfileImages images)
        {
            Assert.True(images.Profiles.Count > 0);

            string profileSize = config.Client.Config.Images.ProfileSizes.First();

            TestImagesInternal(config, images.Profiles.Select(s => s.FilePath), profileSize);
        }

        public static void TestImages(TestConfig config, Images images)
        {
            Assert.True(images.Backdrops.Count > 0);
            Assert.True(images.Posters.Count > 0);

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

                Assert.True(TestHelpers.InternetUriExists(url));
                Assert.True(TestHelpers.InternetUriExists(urlSecure));
            }
        }

        public static bool TestImagePath(string path)
        {
            return ImagePathRegex.IsMatch(path);
        }
    }
}
