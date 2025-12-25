using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Argon;
using TMDbLib.Client;
using TMDbLib.Utilities;
using TMDbLib.Utilities.Serializer;
using VerifyTests;
using VerifyXunit;

namespace TMDbLibTests.JsonHelpers;

/// <summary>
/// Base class for all test classes.
/// </summary>
public abstract class TestBase
{
    /// <summary>
    /// Registers necessary module initializers.
    /// </summary>
    [ModuleInitializer]
    public static void Init() => VerifyNewtonsoftJson.Initialize();

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
    protected static ITMDbSerializer Serializer => TMDbJsonSerializer.Instance;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestBase"/> class.
    /// </summary>
    protected TestBase()
    {
        VerifySettings = new VerifySettings();
        VerifySettings.AutoVerify();

        VerifySettings.UseDirectory("../Verification");

        // Note: Dynamic properties (VoteCount, VoteAverage, Popularity, PosterPath, etc.)
        // are excluded via DataSortingContractResolver.IgnoredProperties

        VerifySettings.AddExtraSettings(serializerSettings =>
        {
            // Use custom contract resolver that:
            // 1. Reads Newtonsoft.Json [JsonProperty] attributes for property names
            // 2. Uses camelCase for properties without explicit names
            // 3. Sorts collections for consistent output
            serializerSettings.ContractResolver = new DataSortingContractResolver();

            // Add enum converter that uses TMDbLib's EnumMemberCache for proper string values
            // Insert at beginning to take priority over other converters
            serializerSettings.Converters.Insert(0, new ArgonEnumStringValueConverter());
        });

        WebProxy? proxy = null;

        TestConfig = new TestConfig(proxy: proxy);
    }

    /// <summary>
    /// Verifies an object against a snapshot using Verify.
    /// </summary>
    /// <typeparam name="T">The type of object to verify.</typeparam>
    /// <param name="obj">The object to verify.</param>
    /// <param name="configure">Optional action to configure verify settings.</param>
    /// <returns>A task representing the asynchronous verification operation.</returns>
    protected Task Verify<T>(T obj, Action<VerifySettings>? configure = null)
    {
        var settings = VerifySettings;
        if (configure is not null)
        {
            settings = new VerifySettings(VerifySettings);
            configure(settings);
        }

        return Verifier.Verify(obj, settings);
    }

    /// <summary>
    /// Argon-compatible enum converter that uses TMDbLib's EnumMemberCache for proper string values.
    /// </summary>
    class ArgonEnumStringValueConverter : Argon.JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var str = EnumMemberCache.GetString(value);
            writer.WriteValue(str);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var val = EnumMemberCache.GetValue(reader.Value as string, objectType);
            return val;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType.IsEnum;
        }
    }

    /// <summary>
    /// Custom contract resolver that sorts collections and reads Newtonsoft.Json attributes.
    /// </summary>
    class DataSortingContractResolver : DefaultContractResolver
    {
        // Properties to completely ignore during serialization (dynamic values that change over time)
        private static readonly HashSet<string> IgnoredProperties = new(StringComparer.OrdinalIgnoreCase)
        {
            "VoteCount", "vote_count",
            "VoteAverage", "vote_average",
            "Popularity", "popularity",
            "Rating", "rating"
        };

        // Properties to show with <non-empty> placeholder (verify non-null but not exact value)
        private static readonly HashSet<string> NonEmptyProperties = new(StringComparer.OrdinalIgnoreCase)
        {
            "PosterPath", "poster_path",
            "BackdropPath", "backdrop_path",
            "ProfilePath", "profile_path",
            "StillPath", "still_path",
            "FilePath", "file_path",
            "LogoPath", "logo_path",
            "AspectRatio", "aspect_ratio",
            "Height", "height",
            "Width", "width",
            "Key", "key"
        };

        /// <summary>
        /// Determines if a property name represents an ID field.
        /// </summary>
        private static bool IsIdProperty(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }

            var lowerName = name.ToLowerInvariant();

            // Exact match for "id"
            if (lowerName == "id")
            {
                return true;
            }

            // Ends with "_id" (credit_id, cast_id, episode_id, imdb_id, etc.)
            if (lowerName.EndsWith("_id", StringComparison.Ordinal))
            {
                return true;
            }

            // Also check CamelCase pattern like "CreditId", "CastId"
            if (name.Length > 2 && name.EndsWith("Id", StringComparison.Ordinal) && char.IsUpper(name[^2]))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converter that writes any value as a string placeholder.
        /// </summary>
        class PlaceholderConverter : Argon.JsonConverter
        {
            private readonly string _placeholder;

            public PlaceholderConverter(string placeholder) => _placeholder = placeholder;

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is null)
                {
                    writer.WriteNull();
                }
                else
                {
                    writer.WriteValue(_placeholder);
                }
            }

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                return reader.Value;
            }

            public override bool CanConvert(Type objectType) => true;
        }

        /// <summary>
        /// Converter that writes non-empty values as "&lt;non-empty&gt;".
        /// </summary>
        class NonEmptyConverter : Argon.JsonConverter
        {
            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is null)
                {
                    writer.WriteNull();
                }
                else
                {
                    var str = value.ToString();
                    writer.WriteValue(!string.IsNullOrEmpty(str) ? "<non-empty>" : str);
                }
            }

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                return reader.Value;
            }

            public override bool CanConvert(Type objectType) => true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSortingContractResolver"/> class.
        /// </summary>
        public DataSortingContractResolver()
        {
            // Use camel case for properties that don't have explicit names
            NamingStrategy = new CamelCaseNamingStrategy
            {
                ProcessDictionaryKeys = false,
                OverrideSpecifiedNames = false
            };
        }

        /// <summary>
        /// Creates a property for the given member.
        /// </summary>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            // Check for Newtonsoft.Json JsonPropertyAttribute and use its name
            var jsonPropertyAttr = member.GetCustomAttributes(true)
                .FirstOrDefault(a => a.GetType().FullName == "Newtonsoft.Json.JsonPropertyAttribute");

            if (jsonPropertyAttr is not null)
            {
                // Get the PropertyName from the attribute using reflection
                var propertyNameProp = jsonPropertyAttr.GetType().GetProperty("PropertyName");
                if (propertyNameProp is not null)
                {
                    var name = propertyNameProp.GetValue(jsonPropertyAttr) as string;
                    if (!string.IsNullOrEmpty(name))
                    {
                        property.PropertyName = name;
                    }
                }
            }

            // Skip ignored properties (dynamic values that change over time)
            if (IgnoredProperties.Contains(member.Name) || (property.PropertyName is not null && IgnoredProperties.Contains(property.PropertyName)))
            {
                property.Ignored = true;
                return property;
            }

            // Apply <non-empty> transformation for image paths and dimensions
            if (NonEmptyProperties.Contains(member.Name) || (property.PropertyName is not null && NonEmptyProperties.Contains(property.PropertyName)))
            {
                property.Converter = new NonEmptyConverter();
                return property;
            }

            // Apply {Scrubbed} transformation for ID properties
            if (IsIdProperty(member.Name) || (property.PropertyName is not null && IsIdProperty(property.PropertyName)))
            {
                property.Converter = new PlaceholderConverter("{Scrubbed}");
            }

            return property;
        }

        /// <summary>
        /// Resolves the contract for a given type and adds sorting for arrays.
        /// </summary>
        public override JsonContract ResolveContract(Type type)
        {
            var contract = base.ResolveContract(type);

            // For array contracts, wrap with sorting converter
            if (contract is JsonArrayContract arrayContract)
            {
                arrayContract.Converter = new SortingListConverter(arrayContract.Converter);
            }

            return contract;
        }

        private static string[] _sortFieldsInOrder = { "CreditId", "Id", "Iso_3166_1", "EpisodeNumber", "SeasonNumber" };

        /// <summary>
        /// Converter that sorts lists during serialization.
        /// </summary>
        class SortingListConverter : Argon.JsonConverter
        {
            public SortingListConverter(Argon.JsonConverter? innerConverter)
            {
            }

            public override bool CanConvert(Type objectType)
            {
                return typeof(IEnumerable).IsAssignableFrom(objectType) && !typeof(IDictionary).IsAssignableFrom(objectType);
            }

            public override bool CanRead => false;

            public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException("SortingListConverter should only be used for serialization");
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                var items = value as IEnumerable;
                if (items is null)
                {
                    writer.WriteNull();
                    return;
                }

                // Convert to list for sorting
                var itemList = items.Cast<object>().ToList();

                if (itemList.Count > 0)
                {
                    var objType = value.GetType();
                    if (objType.IsGenericType)
                    {
                        var innerType = objType.GetGenericArguments().First();

                        // Determine which comparer to use
                        IComparer? comparer = null;
                        if (innerType.IsValueType)
                        {
                            comparer = Comparer.Default;
                        }
                        else
                        {
                            foreach (var fieldName in _sortFieldsInOrder)
                            {
                                var prop = innerType.GetProperty(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
                                if (prop is not null)
                                {
                                    comparer = new CompareObjectOnProperty(prop);
                                    break;
                                }
                            }
                        }

                        if (comparer is not null && !IsSorted(itemList, comparer))
                        {
                            itemList.Sort((x, y) => comparer.Compare(x, y));
                        }
                    }
                }

                // Write array directly to avoid recursive converter issues
                writer.WriteStartArray();
                foreach (var item in itemList)
                {
                    serializer.Serialize(writer, item);
                }

                writer.WriteEndArray();
            }

            private static bool IsSorted(IList list, IComparer comparer)
            {
                for (var i = 1; i < list.Count; i++)
                {
                    var a = list[i - 1];
                    var b = list[i];

                    if (comparer.Compare(a, b) > 0)
                    {
                        return false;
                    }
                }

                return true;
            }
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
            public int Compare(object? x, object? y)
            {
                var valX = _property.GetValue(x);
                var valY = _property.GetValue(y);

                if (valX is null && valY is null)
                {
                    return 0;
                }

                if (valX is null)
                {
                    return -1;
                }

                if (valY is null)
                {
                    return 1;
                }

                return Comparer.Default.Compare(valX, valY);
            }
        }
    }
}
