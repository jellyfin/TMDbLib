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
        private static Dictionary<CollectionMethods, Func<Collection, object>> _methods;

        public ClientCollectionTests()
        {
            _methods = new Dictionary<CollectionMethods, Func<Collection, object>>
            {
                [CollectionMethods.Images] = collection => collection.Images
            };
        }

        [Fact]
        public async Task TestCollectionsExtrasNone()
        {
            Collection collection = await Config.Client.GetCollectionAsync(IdHelper.JamesBondCollection);

            Assert.NotNull(collection);
            Assert.Equal("James Bond Collection", collection.Name);
            Assert.NotNull(collection.Parts);
            Assert.True(collection.Parts.Count > 0);

            // Test all extras, ensure none of them exist
            foreach (Func<Collection, object> selector in _methods.Values)
            {
                Assert.Null(selector(collection));
            }
        }

        [Fact]
        public async Task TestCollectionMissing()
        {
            Collection collection = await Config.Client.GetCollectionAsync(IdHelper.MissingID);

            Assert.Null(collection);
        }

        [Fact]
        public async Task TestCollectionsParts()
        {
            Collection collection = await Config.Client.GetCollectionAsync(IdHelper.JamesBondCollection);

            Assert.NotNull(collection);
            Assert.Equal("James Bond Collection", collection.Name);

            Assert.NotNull(collection.Parts);
            Assert.True(collection.Parts.Count > 0);

            Assert.Contains(collection.Parts, movie => movie.Title == "Live and Let Die");
            Assert.Contains(collection.Parts, movie => movie.Title == "Dr. No");
        }

        [Fact]
        public async Task TestCollectionsExtrasExclusive()
        {
            await TestMethodsHelper.TestGetExclusive(_methods, extras => Config.Client.GetCollectionAsync(IdHelper.JamesBondCollection, extras));
        }

        [Fact]
        public async Task TestCollectionsExtrasAll()
        {
            await TestMethodsHelper.TestGetAll(_methods, combined => Config.Client.GetCollectionAsync(IdHelper.JamesBondCollection, combined));
        }

        [Fact]
        public async Task TestCollectionsImagesAsync()
        {
            // Get config
            await Config.Client.GetConfigAsync();

            // Test image url generator
            ImagesWithId images = await Config.Client.GetCollectionImagesAsync(IdHelper.JamesBondCollection);

            Assert.Equal(IdHelper.JamesBondCollection, images.Id);
            await TestImagesHelpers.TestImagesAsync(Config, images);
        }
    }
}