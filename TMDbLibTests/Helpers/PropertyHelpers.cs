using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TMDbLibTests.Helpers;

/// <summary>
/// Helper methods for working with property expressions.
/// </summary>
internal static class PropertyHelpers
{
    /// <summary>
    /// Extracts PropertyInfo from a property lambda expression.
    /// </summary>
    /// <typeparam name="TSource">The source type containing the property.</typeparam>
    /// <typeparam name="TProperty">The property type.</typeparam>
    /// <param name="propertyLambda">The property expression to extract from.</param>
    /// <returns>The PropertyInfo for the specified property.</returns>
    /// <exception cref="ArgumentException">Thrown when the expression does not represent a valid property access.</exception>
    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(Expression<Func<TSource, TProperty>> propertyLambda)
    {
        var type = typeof(TSource);

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

        if (propInfo.ReflectedType is not null &&
            type != propInfo.ReflectedType &&
            !type.IsSubclassOf(propInfo.ReflectedType))
        {
            throw new ArgumentException($"Expression '{propertyLambda}' refers to a property that is not from type {type}.");
        }

        return propInfo;
    }
}
