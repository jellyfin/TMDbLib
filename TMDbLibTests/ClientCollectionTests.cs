using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests;

/// <summary>
/// Contains tests for the TMDb collection functionality.
/// </summary>
public class ClientCollectionTests : TestBase
{
    private static readonly Dictionary<CollectionMethods, Func<Collection, object?>> Methods;

    static ClientCollectionTests()
    {
        Methods = new Dictionary<CollectionMethods, Func<Collection, object?>>
        {
            [CollectionMethods.Images] = collection => collection.Images
        };
    }

    /// <summary>
    /// Tests that retrieving a collection without extras does not include extra data.
    /// </summary>
    [Fact]
    public async Task TestCollectionsExtrasNone()
    {
        var collection = await TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection);
        Assert.NotNull(collection);

        // Test all extras, ensure none of them exist
        foreach (var selector in Methods.Values)
        {
            Assert.Null(selector(collection));
        }
    }

    /// <summary>
    /// Tests that attempting to retrieve a non-existent collection returns null.
    /// </summary>
    [Fact]
    public async Task TestCollectionMissing()
    {
        var collection = await TMDbClient.GetCollectionAsync(IdHelper.MissingID);

        Assert.Null(collection);
    }

    /// <summary>
    /// Tests that retrieving a collection returns the expected collection parts.
    /// </summary>
    [Fact]
    public async Task TestCollectionsParts()
    {
        var collection = await TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection);
        Assert.NotNull(collection);

        await Verify(collection);
    }

    /// <summary>
    /// Tests that requesting specific collection extras returns only those extras.
    /// </summary>
    [Fact]
    public async Task TestCollectionsExtrasExclusive()
    {
        await TestMethodsHelper.TestGetExclusive(Methods, async extras =>
        {
            var result = await TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection, extras);
            Assert.NotNull(result);
            return result;
        });
    }

    /// <summary>
    /// Tests that requesting all collection extras returns all available extra data.
    /// </summary>
    [Fact]
    public async Task TestCollectionsExtrasAll()
    {
        await TestMethodsHelper.TestGetAll(Methods, async combined =>
        {
            var result = await TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection, combined);
            Assert.NotNull(result);
            return result;
        }, async collection => await Verify(collection));
    }

    /// <summary>
    /// Tests that retrieving collection images returns valid image paths.
    /// </summary>
    [Fact]
    public async Task TestCollectionsImagesAsync()
    {
        var images = await TMDbClient.GetCollectionImagesAsync(IdHelper.BackToTheFutureCollection);
        Assert.NotNull(images);

        TestImagesHelpers.TestImagePaths(images);
    }
}
