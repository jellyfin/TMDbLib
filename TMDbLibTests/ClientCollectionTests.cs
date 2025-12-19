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
    private static readonly Dictionary<CollectionMethods, Func<Collection, object>> Methods;

    static ClientCollectionTests()
    {
        Methods = new Dictionary<CollectionMethods, Func<Collection, object>>
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
        Collection collection = await TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection);

        // Test all extras, ensure none of them exist
        foreach (Func<Collection, object> selector in Methods.Values)
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
        Collection collection = await TMDbClient.GetCollectionAsync(IdHelper.MissingID);

        Assert.Null(collection);
    }

    /// <summary>
    /// Tests that retrieving a collection returns the expected collection parts.
    /// </summary>
    [Fact]
    public async Task TestCollectionsParts()
    {
        Collection collection = await TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection);

        await Verify(collection);
    }

    /// <summary>
    /// Tests that requesting specific collection extras returns only those extras.
    /// </summary>
    [Fact]
    public async Task TestCollectionsExtrasExclusive()
    {
        await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection, extras));
    }

    /// <summary>
    /// Tests that requesting all collection extras returns all available extra data.
    /// </summary>
    [Fact]
    public async Task TestCollectionsExtrasAll()
    {
        await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection, combined), async collection => await Verify(collection));
    }

    /// <summary>
    /// Tests that retrieving collection images returns valid image paths.
    /// </summary>
    [Fact]
    public async Task TestCollectionsImagesAsync()
    {
        ImagesWithId images = await TMDbClient.GetCollectionImagesAsync(IdHelper.BackToTheFutureCollection);

        TestImagesHelpers.TestImagePaths(images);
    }
}
