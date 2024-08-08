using System.Linq.Expressions;
using System.Reflection;
using Ctoss.Configuration;
using Ctoss.Expressions;

namespace Ctoss.Builders;

public interface IPropertyBuilder
{
    static Expression GetCompletePropertyExpression<T>(string property, ParameterExpression parameter)
    {
        // NOTE: first of all, we're trying to get a real property name from the given one.
        // If we find it, we can use it to work with an expression. Else the given property name will be used.
        var normalizedProperty = typeof(T)
            .GetProperty(property, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

        var propertyName = normalizedProperty?.Name ?? property;

        var propertyType = GetPropertyType<T>(propertyName);

        var nullablePropertyType = propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null
            ? typeof(Nullable<>).MakeGenericType(propertyType)
            : propertyType;
        
        return GetPropertyExpression<T>(propertyName, parameter, nullablePropertyType);
    }
    
    static Expression GetPropertyExpression<T>(
        string property,
        ParameterExpression parameter,
        Type propertyType)
    {
        var customMapping = CtossSettings.GetPropertyMapping<T>(property);
        Expression propertyRawExpression;

        if (customMapping == null)
        {
            propertyRawExpression = Expression.Property(parameter, property);
        }
        else
        {
            var visitor = new ReplaceParameterVisitor(customMapping.Parameters[0], parameter);
            propertyRawExpression = visitor.Visit(customMapping.Body);
        }

        var propertyExpression = Expression.Convert(propertyRawExpression, propertyType);
        return propertyExpression;
    }

    static Type GetPropertyType<T>(string property)
    {
        var customMappingType = CtossSettings.GetPropertyType<T>(property);
        if (customMappingType != null)
            return customMappingType;

        var propertyType = typeof(T).GetProperty(property)?.PropertyType;
        if (propertyType == null)
            throw new ArgumentException($"Property '{property}' not found on type '{typeof(T).Name}'");

        return propertyType;
    }
}
