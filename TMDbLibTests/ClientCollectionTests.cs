using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TMDbLib.Objects.Collections;
using TMDbLib.Objects.General;
using TMDbLibTests.Helpers;

namespace TMDbLibTests
{
    [TestClass]
    public class ClientCollectionTests
    {
        private const int JamesBondCollection = 645;
        private Dictionary<CollectionMethods, Func<Collection, object>> _methods;
        private TestConfig _config;

        [TestInitialize]
        public void InitTest()
        {
            _config = new TestConfig();

            _methods = new Dictionary<CollectionMethods, Func<Collection, object>>();
            _methods[CollectionMethods.Images] = collection => collection.Images;
        }

        [TestMethod]
        public void TestCollectionsExtrasNone()
        {
            Collection collection = _config.Client.GetCollection(JamesBondCollection);

            // TODO: Test all properties
            Assert.IsNotNull(collection);
            Assert.AreEqual("James Bond Collection", collection.Name);
            Assert.IsNotNull(collection.Parts);
            Assert.IsTrue(collection.Parts.Count > 0);

            // Test all extras, ensure none of them exist
            foreach (Func<Collection, object> selector in _methods.Values)
            {
                Assert.IsNull(selector(collection));
            }
        }

        [TestMethod]
        public void TestCompaniesExtrasExclusive()
        {
            TestMethodsHelper.TestGetExclusive(_methods, (id, extras) => _config.Client.GetCollection(id, extras), JamesBondCollection);
        }

        [TestMethod]
        public void TestCompaniesExtrasAll()
        {
            CollectionMethods combinedEnum = _methods.Keys.Aggregate((methods, movieMethods) => methods | movieMethods);
            Collection item = _config.Client.GetCollection(JamesBondCollection, combinedEnum);

            TestMethodsHelper.TestGetAll(_methods, item);
        }

        [TestMethod]
        public void TestCollectionsImages()
        {
            // Get config
            _config.Client.GetConfig();

            // Test image url generator
            ImagesWithId images = _config.Client.GetCollectionImages(JamesBondCollection);

            Assert.AreEqual(JamesBondCollection, images.Id);
            TestImagesHelpers.TestImages(_config, images);
        }
    }
}
