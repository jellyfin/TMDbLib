using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using Xunit;

namespace TMDbLibTests.Helpers
{
    public static class TestImagesHelpers
    {
        private static readonly Regex ImagePathRegex = new Regex(@"^/[a-zA-Z0-9]{26,}\.(?:jpg|png|svg)$", RegexOptions.Compiled);

        public static void TestImagePaths(Images images)
        {
            TestImagePaths(images.Backdrops);
            TestImagePaths(images.Posters);
            TestImagePaths(images.Logos);
        }

        public static void TestImagePaths(IEnumerable<string> imagePaths)
        {
            Assert.All(imagePaths, path => Assert.True(ImagePathRegex.IsMatch(path), "path was not a valid image path, was: " + path));
        }

        public static void TestImagePaths<T>(IEnumerable<T> images) where T : ImageData
        {
            Assert.All(images, x => Assert.True(ImagePathRegex.IsMatch(x.FilePath), "image.FilePath was not a valid image path, was: " + x.FilePath));
        }

        public static void TestImagePaths(IEnumerable<TaggedImage> images)
        {
            Assert.All(images, x => Assert.True(ImagePathRegex.IsMatch(x.FilePath), "image.FilePath was not a valid image path, was: " + x.FilePath));
        }
    }
}
