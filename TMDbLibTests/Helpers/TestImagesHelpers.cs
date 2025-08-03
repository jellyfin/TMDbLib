using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using Xunit;

namespace TMDbLibTests.Helpers;

public static partial class TestImagesHelpers
{

    [GeneratedRegex(@"^/[a-zA-Z0-9]{26,}\.(?:jpg|png|svg)$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    public static void TestImagePaths(Images images)
    {
        if (images.Backdrops is not null)
        {
            TestImagePaths(images.Backdrops);
        }
        if (images.Posters is not null)
        {
            TestImagePaths(images.Posters);
        }
        if (images.Logos is not null)
        {
            TestImagePaths(images.Logos);
        }
    }
    public static void TestImagePaths(IEnumerable<string> imagePaths)
    {
        Assert.All(imagePaths, path => Assert.True(MyRegex().IsMatch(path), "path was not a valid image path, was: " + path));
    }
    public static void TestImagePaths<T>(IEnumerable<T> images) where T : ImageData
    {
        Assert.All(images, x => Assert.True(MyRegex().IsMatch(x.FilePath), "image.FilePath was not a valid image path, was: " + x.FilePath));
    }
    public static void TestImagePaths(IEnumerable<TaggedImage> images)
    {
        Assert.All(images, x => Assert.True(MyRegex().IsMatch(x.FilePath), "image.FilePath was not a valid image path, was: " + x.FilePath));
    }
}
