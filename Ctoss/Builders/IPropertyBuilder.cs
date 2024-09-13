using System.Linq.Expressions;
using System.Reflection;
using Ctoss.Configuration;
using Ctoss.Expressions;

namespace Ctoss.Builders;

internal interface IPropertyBuilder
{
    private static readonly BindingFlags Flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;
    
    static Expression GetCompletePropertyExpression<T>(string property, ParameterExpression parameter, bool conditionalAccess)
    {
        // NOTE: first of all, we're trying to get a real property name from the given one.
        // If we find it, we can use it to work with an expression. Else the given property name will be used.
        var normalizedProperty = typeof(T).GetProperty(property, Flags);

        var propertyName = normalizedProperty?.Name ?? property;

        var propertyType = GetPropertyType<T>(propertyName);

        var nullablePropertyType = propertyType.IsValueType && Nullable.GetUnderlyingType(propertyType) == null
            ? typeof(Nullable<>).MakeGenericType(propertyType)
            : propertyType;
        
        return GetPropertyExpression<T>(propertyName, parameter, nullablePropertyType, conditionalAccess);
    }
    
    static Expression GetPropertyExpression<T>(
        string property,
        ParameterExpression parameter,
        Type propertyType,
        bool conditionalAccess)
    {
        var customMapping = CtossSettings.GetPropertyMapping<T>(property);
        Expression propertyRawExpression;

        if (customMapping == null)
        {
            var parts = property.Split('.');
            Expression prop = parameter;
            foreach (var p in parts)
            {
                var propExp = Expression.PropertyOrField(prop, p);
                if (!conditionalAccess)
                {
                    prop = propExp;
                }
                else
                {
                    prop = CanBeNull(propExp.Type) ? Expression.Condition(Expression.Equal(prop, Expression.Constant(null)), Expression.Default(propExp.Type), propExp) : propExp;
                }
            }

            propertyRawExpression = prop;
        }
        else
        {
            var visitor = new ReplaceParameterVisitor(customMapping.Parameters[0], parameter);
            propertyRawExpression = visitor.Visit(customMapping.Body);
        }

        var propertyExpression = Expression.Convert(propertyRawExpression, propertyType);
        return propertyExpression;
    }

    static Type GetPropertyType<T>(string property) => GetPropertyType(typeof(T), property);
    
    static Type GetPropertyType(Type entityType, string property)
    {
        var customMappingType = CtossSettings.GetPropertyType(entityType, property);
        if (customMappingType != null)
            return customMappingType;

        var parts = property.Split('.');
        if (parts.Length == 1)
        {
            var propertyType = entityType.GetProperty(property, Flags)?.PropertyType;

            if (propertyType == null)
                throw new ArgumentException($"Property '{property}' not found on type '{entityType.Name}'");

            return propertyType;
        }
        else
        {
            var type = GetPropertyType(entityType, parts[0]);
            return GetPropertyType(type, string.Join(".", parts.Skip(1)));
        }
    }
    
    private static bool CanBeNull(Type type)
    {
        if (type == null)
            throw new ArgumentNullException(nameof(type));

        // Check if it's a reference type
        if (type.IsClass)
            return true;

        // Check if it's a nullable value type
        if (type.IsValueType)
        {
            // Check if it's a nullable type
            if (Nullable.GetUnderlyingType(type) != null)
                return true;
        }

        // Otherwise, the type cannot be null
        return false;
    }
}
