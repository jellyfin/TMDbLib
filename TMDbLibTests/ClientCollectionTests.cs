using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly TestConfig _config;

        public ClientCollectionTests()
        {
            _config = new TestConfig();

            _methods = new Dictionary<CollectionMethods, Func<Collection, object>>
            {
                [CollectionMethods.Images] = collection => collection.Images
            };
        }

        [Fact]
        public void TestCollectionsExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson = true;

            Collection collection = _config.Client.GetCollectionAsync(IdHelper.JamesBondCollection).Result;

            // TODO: Test all properties
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
        public void TestCollectionsExtrasExclusive()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson = true;

            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => _config.Client.GetCollectionAsync(id, extras).Result, IdHelper.JamesBondCollection);
        }

        [Fact]
        public void TestCollectionsExtrasAll()
        {
            CollectionMethods combinedEnum = _methods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Collection item = _config.Client.GetCollectionAsync(IdHelper.JamesBondCollection, combinedEnum).Result;

            TestMethodsHelper.TestAllNotNull(_methods, item);
        }

        [Fact]
        public void TestCollectionsImages()
        {
            // Get config
            _config.Client.GetConfig();

            // Test image url generator
            ImagesWithId images = _config.Client.GetCollectionImagesAsync(IdHelper.JamesBondCollection).Result;

            Assert.Equal(IdHelper.JamesBondCollection, images.Id);
            TestImagesHelpers.TestImages(_config, images);
        }
    }
}
