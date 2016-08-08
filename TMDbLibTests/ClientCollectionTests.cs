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
        
        public ClientCollectionTests()
        {
            _methods = new Dictionary<CollectionMethods, Func<Collection, object>>
            {
                [CollectionMethods.Images] = collection => collection.Images
            };
        }

        [Fact]
        public void TestCollectionsExtrasNone()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / images");

            Collection collection = Config.Client.GetCollectionAsync(IdHelper.JamesBondCollection).Result;

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
        public void TestCollectionsParts()
        {
            // We will intentionally ignore errors reg. missing JSON as we do not request it
            IgnoreMissingJson(" / images");

            Collection collection = Config.Client.GetCollectionAsync(IdHelper.JamesBondCollection).Result;

            Assert.NotNull(collection);
            Assert.Equal("James Bond Collection", collection.Name);

            Assert.NotNull(collection.Parts);
            Assert.True(collection.Parts.Count > 0);

            Assert.Contains(collection.Parts, movie => movie.Title == "Live and Let Die");
            Assert.Contains(collection.Parts, movie => movie.Title == "Dr. No");
        }

        [Fact]
        public void TestCollectionsExtrasExclusive()
        {
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => Config.Client.GetCollectionAsync(id, extras).Result, IdHelper.JamesBondCollection);
        }

        [Fact]
        public void TestCollectionsExtrasAll()
        {
            CollectionMethods combinedEnum = _methods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Collection item = Config.Client.GetCollectionAsync(IdHelper.JamesBondCollection, combinedEnum).Result;

            TestMethodsHelper.TestAllNotNull(_methods, item);
        }

        [Fact]
        public void TestCollectionsImages()
        {
            // Get config
            Config.Client.GetConfig();

            // Test image url generator
            ImagesWithId images = Config.Client.GetCollectionImagesAsync(IdHelper.JamesBondCollection).Result;

            Assert.Equal(IdHelper.JamesBondCollection, images.Id);
            TestImagesHelpers.TestImages(Config, images);
        }
    }
}
