using System;
using System.Collections.Generic;
using System.Reflection;

namespace TMDbLib.Utilities
{
    internal static class ObjectHelper
    {
        private static readonly Type StringType = typeof(string);

        public static IEnumerable<KeyValuePair<string, string>> GetStringProperties(this object obj)
        {
            foreach (var prop in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (prop.PropertyType != StringType)
                    continue;

                yield return new KeyValuePair<string, string>(prop.Name, prop.GetValue(obj, null) as string);
            }
        }
    }
}