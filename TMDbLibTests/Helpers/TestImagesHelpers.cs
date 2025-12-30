using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMDbLib.Objects.General;
using TMDbLib.Objects.People;
using Xunit;

namespace TMDbLibTests.Helpers;

/// <summary>
/// Helper methods for testing image paths and data.
/// </summary>
public static partial class TestImagesHelpers
{

    [GeneratedRegex(@"^/[a-zA-Z0-9]{26,}\.(?:jpg|png|svg)$", RegexOptions.Compiled)]
    private static partial Regex MyRegex();

    /// <summary>
    /// Tests that all image paths in an Images object are valid.
    /// </summary>
    /// <param name="images">The images object containing backdrops, posters, and logos to test.</param>
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

    /// <summary>
    /// Tests that all provided image paths match the expected format.
    /// </summary>
    /// <param name="imagePaths">The collection of image paths to validate.</param>
    public static void TestImagePaths(IEnumerable<string> imagePaths)
    {
        Assert.NotNull(imagePaths);
        Assert.All(imagePaths, path => Assert.True(MyRegex().IsMatch(path), "path was not a valid image path, was: " + path));
    }

    /// <summary>
    /// Tests that all image data objects have valid file paths.
    /// </summary>
    /// <typeparam name="T">The type of image data.</typeparam>
    /// <param name="images">The collection of image data objects to validate.</param>
    public static void TestImagePaths<T>(IEnumerable<T> images) where T : ImageData
    {
        Assert.NotNull(images);
        Assert.All(images, x =>
        {
            Assert.NotNull(x.FilePath);
            Assert.True(MyRegex().IsMatch(x.FilePath), "image.FilePath was not a valid image path, was: " + x.FilePath);
        });
    }

    /// <summary>
    /// Tests that all tagged images have valid file paths.
    /// </summary>
    /// <param name="images">The collection of tagged images to validate.</param>
    public static void TestImagePaths(IEnumerable<TaggedImage> images)
    {
        Assert.NotNull(images);
        Assert.All(images, x =>
        {
            Assert.NotNull(x.FilePath);
            Assert.True(MyRegex().IsMatch(x.FilePath), "image.FilePath was not a valid image path, was: " + x.FilePath);
        });
    }
}
