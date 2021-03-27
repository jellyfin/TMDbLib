using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using VerifyTests;

namespace TMDbLibTests.Helpers
{
    internal static class VerifyExtensions
    {
        private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            PropertyInfo prop = PropertyHelpers.GetPropertyInfo(expression);

            JsonPropertyAttribute jsonPropAttribute = prop.GetCustomAttribute<JsonPropertyAttribute>();
            return jsonPropAttribute?.PropertyName ?? prop.Name;
        }

        public static VerifySettings IgnoreProperty(this VerifySettings settings, params string[] properties)
        {
            foreach (string propName in properties)
            {
                string searchString = $" {propName}: ";
                settings.ScrubLines(x => x.Contains(searchString));
            }

            return settings;
        }

        public static VerifySettings IgnoreProperty<T>(this VerifySettings settings, params Expression<Func<T, object>>[] properties)
        {
            string[] propNames = properties.Select(GetPropertyName).ToArray();

            return settings.IgnoreProperty(propNames);
        }

        private static Regex _propRegex = new Regex(@"^(?<pre>[\s]*)(?<name>[\S]+): (?<value>.*?)(?<post>,|$)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static VerifySettings SimplifyProperty(this VerifySettings settings, params string[] properties)
        {
            foreach (string propName in properties)
            {
                settings.ScrubLinesWithReplace(original =>
                {
                    Match match = _propRegex.Match(original);
                    if (!match.Success)
                        return original;

                    if (match.Groups["name"].Value != propName)
                        return original;

                    string newValue = match.Groups["value"].Value;
                    if (newValue.Length > 0)
                        newValue = "<non-empty>";

                    return match.Groups["pre"].Value + match.Groups["name"].Value + ": " + newValue +
                           match.Groups["post"].Value;
                });
            }

            return settings;
        }

        public static VerifySettings SimplifyProperty<T>(this VerifySettings settings, params Expression<Func<T, object>>[] properties)
        {
            string[] propNames = properties.Select(GetPropertyName).ToArray();

            return settings.SimplifyProperty(propNames);
        }
    }
}