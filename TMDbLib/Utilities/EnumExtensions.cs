using System;
using System.Reflection;

namespace TMDbLib.Utilities
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();
            TypeInfo typeInfo = type.GetTypeInfo();

            if (!typeInfo.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
            }

            // Tries to find a DisplayAttribute for a potential friendly name for the enum
            MemberInfo[] memberInfo = typeInfo.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                EnumValueAttribute attr = memberInfo[0].GetCustomAttribute<EnumValueAttribute>();
                if (attr != null)
                {
                    // Pull out the Value
                    return attr.Value;
                }
            }

            // If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }
}
