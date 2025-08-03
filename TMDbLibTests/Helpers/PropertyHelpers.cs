using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TMDbLibTests.Helpers;

internal static class PropertyHelpers
{
    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
    {
        Type type = typeof(TSource);

        MemberExpression member;
        if (propertyLambda.Body is MemberExpression asMember)
        {
            member = asMember;
        }
        else if (propertyLambda.Body is UnaryExpression asUnary && asUnary.Operand is MemberExpression asMember2)
        {
            member = asMember2;
        }
        else
        {
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a method, not a property.");
        }
        if (member.Member is not PropertyInfo propInfo)
        {
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a field, not a property.");
        }
        if (type != propInfo.ReflectedType &&
            !type.IsSubclassOf(propInfo.ReflectedType))
        {
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a property that is not from type {type}.");
        }
        return propInfo;
    }
}
