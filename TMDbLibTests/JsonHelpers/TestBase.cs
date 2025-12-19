using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLib.Utilities.Serializer;
using TMDbLibTests.Helpers;
using VerifyTests;
using VerifyXunit;

namespace TMDbLibTests.JsonHelpers;

/// <summary>
/// Base class for all test classes.
/// </summary>
[UsesVerify]
public abstract class TestBase
{
    /// <summary>
    /// Gets the Verify settings used for all tests in this class.
    /// </summary>
    private VerifySettings VerifySettings { get; }

    /// <summary>
    /// Gets the test configuration containing API credentials and client settings.
    /// </summary>
    protected readonly TestConfig TestConfig;

    /// <summary>
    /// Gets the configured TMDbClient instance for API calls.
    /// </summary>
    protected TMDbClient TMDbClient => TestConfig.Client;

    /// <summary>
    /// Gets the JSON serializer instance used for TMDb objects.
    /// </summary>
    protected ITMDbSerializer Serializer => TMDbJsonSerializer.Instance;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestBase"/> class.
    /// </summary>
    protected TestBase()
    {
        VerifySettings = new VerifySettings();
        //VerifySettings.AutoVerify();

        VerifySettings.UseDirectory("../Verification");

        // Ignore and simplify many dynamic properties
        VerifySettings.IgnoreProperty<SearchMovie>(x => x.VoteCount, x => x.Popularity, x => x.VoteAverage);
        VerifySettings.SimplifyProperty<SearchMovie>(x => x.BackdropPath, x => x.PosterPath);
        VerifySettings.SimplifyProperty<SearchPerson>(x => x.ProfilePath);
        VerifySettings.SimplifyProperty<SearchTvEpisode>(x => x.StillPath);
        VerifySettings.SimplifyProperty<ImageData>(x => x.FilePath);
        VerifySettings.SimplifyProperty<SearchCompany>(x => x.LogoPath);

        VerifySettings.AddExtraSettings(serializerSettings =>
        {
            serializerSettings.ContractResolver = new DataSortingContractResolver(serializerSettings.ContractResolver);
        });

        WebProxy proxy = null;
        //proxy = new WebProxy("http://127.0.0.1:8888");

        TestConfig = new TestConfig(serializer: null, proxy: proxy);
    }

    /// <summary>
    /// Verifies an object against a snapshot using Verify.
    /// </summary>
    /// <typeparam name="T">The type of object to verify.</typeparam>
    /// <param name="obj">The object to verify.</param>
    /// <param name="configure">Optional action to configure verify settings.</param>
    /// <returns>A task representing the asynchronous verification operation.</returns>
    protected Task Verify<T>(T obj, Action<VerifySettings> configure = null)
    {
        VerifySettings settings = VerifySettings;

        if (configure != null)
        {
            settings = new VerifySettings(VerifySettings);
            configure(settings);
        }
        return Verifier.Verify(obj, settings);
    }

    /// <summary>
    /// Custom contract resolver that sorts collections during serialization to ensure consistent test output.
    /// </summary>
    class DataSortingContractResolver : IContractResolver
    {
        private readonly IContractResolver _innerResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSortingContractResolver"/> class.
        /// </summary>
        /// <param name="innerResolver">The inner contract resolver to wrap.</param>
        public DataSortingContractResolver(IContractResolver innerResolver)
        {
            _innerResolver = innerResolver;
        }

        /// <summary>
        /// Resolves the contract for a given type and adds serialization callbacks for sorting.
        /// </summary>
        /// <param name="type">The type to resolve the contract for.</param>
        /// <returns>The JSON contract for the type.</returns>
        public JsonContract ResolveContract(Type type)
        {
            JsonContract contract = _innerResolver.ResolveContract(type);

            // Add a callback that is invoked on each serialization of an object
            // We do this to be able to sort lists
            contract.OnSerializingCallbacks.Add(SerializingCallback);

            return contract;
        }

        private static string[] _sortFieldsInOrder = { "CreditId", "Id", "Iso_3166_1", "EpisodeNumber", "SeasonNumber" };

        /// <summary>
        /// Callback invoked during serialization to sort lists based on specific properties.
        /// </summary>
        /// <param name="obj">The object being serialized.</param>
        /// <param name="context">The streaming context.</param>
        private void SerializingCallback(object obj, StreamingContext context)
        {
            if (!(obj is IEnumerable) || obj is IDictionary)
                return;

            Type objType = obj.GetType();
            if (obj is IList objAsList)
            {
                Debug.Assert(objType.IsGenericType);

                Type innerType = objType.GetGenericArguments().First();

                // Determine which comparer to use
                IComparer comparer = null;
                if (innerType.IsValueType)
                    comparer = Comparer.Default;
                else
                {
                    foreach (string fieldName in _sortFieldsInOrder)
                    {
                        PropertyInfo prop = innerType.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                        if (prop == null)
                            continue;

                        comparer = new CompareObjectOnProperty(prop);
                        break;
                    }
                }
                if (comparer != null)
                {
                    // Is sorted?
                    bool isSorted = IsSorted(objAsList, comparer);

                    if (!isSorted)
                    {
                        // Sort the list using our comparer
                        List<object> sortList = objAsList.Cast<object>().ToList();
                        sortList.Sort((x, y) => comparer.Compare(x, y));

                        // Transfer values
                        for (int i = 0; i < objAsList.Count; i++)
                            objAsList[i] = sortList[i];
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether a list is sorted according to the given comparer.
        /// </summary>
        /// <param name="list">The list to check.</param>
        /// <param name="comparer">The comparer to use.</param>
        /// <returns>True if the list is sorted; otherwise, false.</returns>
        private static bool IsSorted(IList list, IComparer comparer)
        {
            for (int i = 1; i < list.Count; i++)
            {
                object a = list[i - 1];
                object b = list[i];

                if (comparer.Compare(a, b) > 0)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Comparer that compares objects based on a specific property value.
        /// </summary>
        class CompareObjectOnProperty : IComparer
        {
            private readonly PropertyInfo _property;

            /// <summary>
            /// Initializes a new instance of the <see cref="CompareObjectOnProperty"/> class.
            /// </summary>
            /// <param name="property">The property to compare on.</param>
            public CompareObjectOnProperty(PropertyInfo property)
            {
                _property = property;
            }

            /// <summary>
            /// Compares two objects based on the specified property.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A value indicating the relative order of the objects.</returns>
            public int Compare(object x, object y)
            {
                object valX = _property.GetValue(x);
                object valY = _property.GetValue(y);

                return Comparer.Default.Compare(valX, valY);
            }
        }
    }
}
