using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using VerifyTests;

namespace TMDbLibTests.Helpers;

/// <summary>
/// Extension methods for Verify test settings.
/// </summary>
internal static partial class VerifyExtensions
{
    /// <summary>
    /// Gets the property name from an expression, using the JSON property name if available.
    /// </summary>
    /// <typeparam name="T">The type containing the property.</typeparam>
    /// <param name="expression">The property expression.</param>
    /// <returns>The JSON property name if available, otherwise the property name.</returns>
    private static string GetPropertyName<T>(Expression<Func<T, object>> expression)
    {
        PropertyInfo prop = PropertyHelpers.GetPropertyInfo(expression);

        JsonPropertyAttribute? jsonPropAttribute = prop.GetCustomAttribute<JsonPropertyAttribute>();
        return jsonPropAttribute?.PropertyName ?? prop.Name;
    }

    /// <summary>
    /// Configures Verify settings to ignore specified properties by name in the verification output.
    /// </summary>
    /// <param name="settings">The Verify settings to configure.</param>
    /// <param name="properties">The property names to ignore.</param>
    /// <returns>The modified Verify settings.</returns>
    public static VerifySettings IgnoreProperty(this VerifySettings settings, params string[] properties)
    {
        foreach (string propName in properties)
        {
            string searchString = $" {propName}: ";
            settings.ScrubLines(x => x.Contains(searchString, StringComparison.OrdinalIgnoreCase));
        }
        return settings;
    }

    /// <summary>
    /// Configures Verify settings to ignore specified properties using expressions in the verification output.
    /// </summary>
    /// <typeparam name="T">The type containing the properties.</typeparam>
    /// <param name="settings">The Verify settings to configure.</param>
    /// <param name="properties">The property expressions to ignore.</param>
    /// <returns>The modified Verify settings.</returns>
    public static VerifySettings IgnoreProperty<T>(this VerifySettings settings, params Expression<Func<T, object>>[] properties)
    {
        string[] propNames = properties.Select(GetPropertyName).ToArray();

        return settings.IgnoreProperty(propNames);
    }

    private static Regex _propRegex = MyRegex();

    /// <summary>
    /// Configures Verify settings to simplify specified properties by replacing non-empty values with "&lt;non-empty&gt;" in the verification output.
    /// </summary>
    /// <param name="settings">The Verify settings to configure.</param>
    /// <param name="properties">The property names to simplify.</param>
    /// <returns>The modified Verify settings.</returns>
    public static VerifySettings SimplifyProperty(this VerifySettings settings, params string[] properties)
    {
        foreach (string propName in properties)
        {
            settings.ScrubLinesWithReplace(original =>
            {
                Match match = _propRegex.Match(original);
                if (!match.Success)
                {
                    return original;
                }
                if (!string.Equals(match.Groups["name"].Value, propName, StringComparison.OrdinalIgnoreCase))
                {
                    return original;
                }
                string newValue = match.Groups["value"].Value;
                if (newValue.Length > 0)
                {
                    newValue = "<non-empty>";
                }
                return match.Groups["pre"].Value + match.Groups["name"].Value + ": " + newValue +
                       match.Groups["post"].Value;
            });
        }
        return settings;
    }

    /// <summary>
    /// Configures Verify settings to simplify specified properties using expressions by replacing non-empty values with "&lt;non-empty&gt;" in the verification output.
    /// </summary>
    /// <typeparam name="T">The type containing the properties.</typeparam>
    /// <param name="settings">The Verify settings to configure.</param>
    /// <param name="properties">The property expressions to simplify.</param>
    /// <returns>The modified Verify settings.</returns>
    public static VerifySettings SimplifyProperty<T>(this VerifySettings settings, params Expression<Func<T, object>>[] properties)
    {
        string[] propNames = properties.Select(GetPropertyName).ToArray();

        return settings.SimplifyProperty(propNames);
    }

    /// <summary>
    /// Generated regex pattern for matching property lines in verification output.
    /// </summary>
    /// <returns>A compiled regex pattern.</returns>
    [GeneratedRegex(@"^(?<pre>[\s]*)(?<name>[^:\s]+): (?<value>.*?)(?<post>,|$)", RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
    private static partial Regex MyRegex();
}
