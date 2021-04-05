using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using TMDbLib.Client;
using TMDbLib.Objects.General;
using TMDbLib.Objects.Search;
using TMDbLibTests.Helpers;
using VerifyTests;
using VerifyXunit;

namespace TMDbLibTests.JsonHelpers
{
    [UsesVerify]
    public abstract class TestBase
    {
        private VerifySettings VerifySettings { get; }

        protected readonly TestConfig TestConfig;

        protected TMDbClient TMDbClient => TestConfig.Client;

        protected TestBase()
        {
            VerifySettings = new VerifySettings();
            //VerifySettings.UseExtension("json");
            //VerifySettings.AutoVerify();

            VerifySettings.UseDirectory("..\\Verification");

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

            JsonSerializerSettings sett = new JsonSerializerSettings();

            WebProxy proxy = null;
            //WebProxy proxy = new WebProxy("http://127.0.0.1:8888");

            TestConfig = new TestConfig(serializer: JsonSerializer.Create(sett), proxy: proxy);
        }

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

        class DataSortingContractResolver : IContractResolver
        {
            private readonly IContractResolver _innerResolver;

            public DataSortingContractResolver(IContractResolver innerResolver)
            {
                _innerResolver = innerResolver;
            }

            public JsonContract ResolveContract(Type type)
            {
                JsonContract contract = _innerResolver.ResolveContract(type);

                // Add a callback that is invoked on each serialization of an object
                // We do this to be able to sort lists
                contract.OnSerializingCallbacks.Add(SerializingCallback);

                return contract;
            }

            private static string[] _sortFieldsInOrder = { "CreditId", "Id", "Iso_3166_1", "EpisodeNumber", "SeasonNumber" };

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

            private static bool IsSorted(IList list, IComparer comparer)
            {
                for (var i = 1; i < list.Count; i++)
                {
                    var a = list[i - 1];
                    var b = list[i];

                    if (comparer.Compare(a, b) > 0)
                        return false;
                }

                return true;
            }

            class CompareObjectOnProperty : IComparer
            {
                private readonly PropertyInfo _property;

                public CompareObjectOnProperty(PropertyInfo property)
                {
                    _property = property;
                }

                public int Compare(object x, object y)
                {
                    object? valX = _property.GetValue(x);
                    object? valY = _property.GetValue(y);

                    return Comparer.Default.Compare(valX, valY);
                }
            }
        }
    }
}