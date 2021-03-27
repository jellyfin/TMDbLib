using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TMDbLibTests.Helpers
{
    public static class TestMethodsHelper
    {
        /// <summary>
        /// Tests that a client method will get a specific part of the TMDb api, but not any other methods
        /// </summary>
        public static async Task TestGetExclusive<TEnum, TResult>(Dictionary<TEnum, Func<TResult, object>> methodSelectors, Func<TEnum, Task<TResult>> getterMethod) where TEnum : Enum
        {
            // Test each method
            foreach (TEnum method in methodSelectors.Keys)
            {
                // Fetch data
                TResult item = await getterMethod(method);

                // Ensure we have the piece we're looking for
                Assert.NotNull(methodSelectors[method](item));

                // .. And none of the others
                foreach (TEnum otherMethod in methodSelectors.Keys.Except(new[] { method }))
                    Assert.Null(methodSelectors[otherMethod](item));
            }
        }

        /// <summary>
        /// Tests that a client method will get all parts of the TMDb api, when requested
        /// </summary>
        public static async Task TestGetAll<TEnum, TResult>(Dictionary<TEnum, Func<TResult, object>> methodSelectors, Func<TEnum, Task<TResult>> getterMethod, Func<TResult, Task> extraAction = null) where TEnum : Enum
        {
            int combinedEnumInt = 0;
            foreach (TEnum key in methodSelectors.Keys)
                combinedEnumInt |= Convert.ToInt32(key);

            TEnum combinedEnum = (TEnum)Enum.ToObject(typeof(TEnum), combinedEnumInt);

            TResult item = await getterMethod(combinedEnum);

            // Ensure we have all the pieces
            foreach (TEnum method in methodSelectors.Keys)
                Assert.NotNull(methodSelectors[method](item));

            // Execute any additional tests
            if (extraAction != null)
                await extraAction(item);
        }

        public static async Task SetValidateRemoveTest(Func<Task> set, Func<Task> remove, Func<bool, Task> validate)
        {
            // Act
            await set();

            // Allow TMDb to cache our changes
            await Task.Delay(2000);
            
            // Validate
            await validate(true);
            
            // Act
            await remove();

            // Allow TMDb to cache our changes
            await Task.Delay(2000);

            // Validate
            await validate(false);
        }
    }
}