using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace TMDbLibTests.Helpers;

/// <summary>
/// Helper methods for testing TMDb API methods.
/// </summary>
public static class TestMethodsHelper
{
    /// <summary>
    /// Tests that a client method will get a specific part of the TMDb api, but not any other methods.
    /// </summary>
    /// <typeparam name="TEnum">The enum type representing different API methods.</typeparam>
    /// <typeparam name="TResult">The result type returned by the getter method.</typeparam>
    /// <param name="methodSelectors">A dictionary mapping enum values to functions that extract the corresponding data from the result.</param>
    /// <param name="getterMethod">A function that fetches data for a given enum value.</param>
    /// <returns>A task representing the asynchronous test operation.</returns>
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
            foreach (TEnum otherMethod in methodSelectors.Keys.Except([method]))
            {
                Assert.Null(methodSelectors[otherMethod](item));
            }
        }
    }

    /// <summary>
    /// Tests that a client method will get all parts of the TMDb api when requested.
    /// </summary>
    /// <typeparam name="TEnum">The enum type representing different API methods.</typeparam>
    /// <typeparam name="TResult">The result type returned by the getter method.</typeparam>
    /// <param name="methodSelectors">A dictionary mapping enum values to functions that extract the corresponding data from the result.</param>
    /// <param name="getterMethod">A function that fetches data for a given enum value.</param>
    /// <param name="extraAction">An optional additional action to perform on the result.</param>
    /// <returns>A task representing the asynchronous test operation.</returns>
    public static async Task TestGetAll<TEnum, TResult>(Dictionary<TEnum, Func<TResult, object>> methodSelectors, Func<TEnum, Task<TResult>> getterMethod, Func<TResult, Task> extraAction = null) where TEnum : Enum
    {
        int combinedEnumInt = 0;
        foreach (TEnum key in methodSelectors.Keys)
        {
            combinedEnumInt |= Convert.ToInt32(key, CultureInfo.InvariantCulture);
        }
        TEnum combinedEnum = (TEnum)Enum.ToObject(typeof(TEnum), combinedEnumInt);

        TResult item = await getterMethod(combinedEnum);

        // Ensure we have all the pieces
        foreach (TEnum method in methodSelectors.Keys)
        {
            Assert.NotNull(methodSelectors[method](item));
        }
        // Execute any additional tests
        if (extraAction is not null)
        {
            await extraAction(item);
        }
    }

    /// <summary>
    /// Tests a set-validate-remove pattern by setting a value, validating it exists, removing it, and validating it's gone.
    /// </summary>
    /// <param name="set">A function that sets a value.</param>
    /// <param name="remove">A function that removes the value.</param>
    /// <param name="validate">A function that validates whether the value exists.</param>
    /// <returns>A task representing the asynchronous test operation.</returns>
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
