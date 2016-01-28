using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace TMDbLib.Utilities
{
    public static class EnumExtensions
    {
        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
            }

            // Tries to find a DisplayAttribute for a potential friendly name for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);

                if (attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DisplayAttribute)attrs[0]).Description;
                }
            }

            // If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }
    }
}
