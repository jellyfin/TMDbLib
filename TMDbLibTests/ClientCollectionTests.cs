using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;
using TMDbLibTests.JsonHelpers;

namespace TMDbLibTests
{
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

        [Fact]
        public async Task TestCollectionsExtrasNone()
        {
            Collection collection = await TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection);

            // Test all extras, ensure none of them exist
            foreach (Func<Collection, object> selector in Methods.Values)
                Assert.Null(selector(collection));
        }

        [Fact]
        public async Task TestCollectionMissing()
        {
            Collection collection = await TMDbClient.GetCollectionAsync(IdHelper.MissingID);

            Assert.Null(collection);
        }

        [Fact]
        public async Task TestCollectionsParts()
        {
            Collection collection = await TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection);

            await Verify(collection);
        }

        [Fact]
        public async Task TestCollectionsExtrasExclusive()
        {
            await TestMethodsHelper.TestGetExclusive(Methods, extras => TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection, extras));
        }

        [Fact]
        public async Task TestCollectionsExtrasAll()
        {
            await TestMethodsHelper.TestGetAll(Methods, combined => TMDbClient.GetCollectionAsync(IdHelper.BackToTheFutureCollection, combined), async collection => await Verify(collection));
        }

        [Fact]
        public async Task TestCollectionsImagesAsync()
        {
            ImagesWithId images = await TMDbClient.GetCollectionImagesAsync(IdHelper.BackToTheFutureCollection);

            TestImagesHelpers.TestImagePaths(images);
        }
    }
}