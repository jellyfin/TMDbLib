using System;
using System.Collections.Generic;
using TMDbLib.Objects.General;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMDbLib.Objects.People;
using Xunit;

namespace TMDbLibTests.Helpers
{
    public static class TestImagesHelpers
    {
        private static readonly Regex ImagePathRegex = new Regex(@"^/[a-zA-Z0-9]{26,}\.(?:jpg|png)$", RegexOptions.Compiled);

        public static async Task TestImagesAsync(TestConfig config, ProfileImages images)
        {
            Assert.True(images.Profiles.Count > 0);

            string profileSize = config.Client.Config.Images.ProfileSizes.First();

            await TestImagesInternal(config, images.Profiles.Select(s => s.FilePath), profileSize);
        }

        public static async Task TestImagesAsync(TestConfig config, Images images)
        {
            Assert.True(images.Backdrops.Count > 0);
            Assert.True(images.Posters.Count > 0);

            string backdropSize = config.Client.Config.Images.BackdropSizes.First();
            string posterSize = config.Client.Config.Images.PosterSizes.First();

            await TestImagesInternal(config, images.Backdrops.Select(s => s.FilePath), backdropSize);

            await TestImagesInternal(config, images.Posters.Select(s => s.FilePath), posterSize);
        }

        private static async Task TestImagesInternal(TestConfig config, IEnumerable<string> images, string posterSize)
        {
            foreach (string imageData in images)
            {
                Uri url = config.Client.GetImageUrl(posterSize, imageData);
                Uri urlSecure = config.Client.GetImageUrl(posterSize, imageData, true);

                Assert.True(await TestHelpers.InternetUriExistsAsync(url));
                Assert.True(await TestHelpers.InternetUriExistsAsync(urlSecure));
            }
        }

        public static bool TestImagePath(string path)
        {
            return ImagePathRegex.IsMatch(path);
        }
    }
}
