using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TMDbLibTests.Helpers
{
    public static class TestMethodsHelper
    {
        public static void TestGetExclusive<T, TId, TResult>(Dictionary<T, Func<TResult, object>> methodSelectors, Func<TId, T, TResult> getterMethod, TId id) where T : struct
        {
            Assert.IsTrue(typeof(T).IsEnum);

            // Test each method
            foreach (T method in methodSelectors.Keys)
            {
                // Fetch data
                TResult item = getterMethod(id, method);

                // Ensure we have the piece we're looking for
                Assert.IsNotNull(methodSelectors[method](item));

                // .. And none of the others
                foreach (T nonExpectedMethod in methodSelectors.Keys.Except(new[] { method }))
                    Assert.IsNull(methodSelectors[nonExpectedMethod](item));
            }
        }

        public static void TestAllNotNull<T, TResult>(Dictionary<T, Func<TResult, object>> methodSelectors, TResult item) where T : struct
        {
            Assert.IsTrue(typeof(T).IsEnum);

            // Ensure we have all the pieces
            foreach (T method in methodSelectors.Keys)
            {
                Assert.IsNotNull(methodSelectors[method](item), "Tested item was missing " + method);
            }
        }
    }
}