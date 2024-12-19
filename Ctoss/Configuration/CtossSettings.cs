using System.Linq.Expressions;
using System.Reflection;

namespace Ctoss.Configuration;

public static class CtossSettings
{
    public static Dictionary<Type, ITypeSettings> TypeSettings { get; set; } = new();
    
    internal static TypeSettings<T>? GetTypeSettings<T>()
    {
        var hasTypeConfiguration = TypeSettings.ContainsKey(typeof(T));
        return hasTypeConfiguration
            ? TypeSettings[typeof(T)] as TypeSettings<T>
            : null;
    }

    internal static Expression<Func<TEntity, object?>>? GetPropertyMapping<TEntity>(string propertyName)
    {
        var hasTypeConfiguration = TypeSettings.ContainsKey(typeof(TEntity));
        if (!hasTypeConfiguration)
            return null;

        var typeSettings = TypeSettings[typeof(TEntity)] as TypeSettings<TEntity>;
        if (typeSettings == null)
            return null;

        // We can't do case-insensitive search here because mapping is stored in the dictionary
        var hasPropertyConfiguration = typeSettings.PropertyMappings.Properties.ContainsKey(propertyName);
        return hasPropertyConfiguration
            ? typeSettings.PropertyMappings.Properties[propertyName]
            : null;
    }

    internal static Type? GetPropertyType(Type type, string propertyName)
    {
        return (Type?)typeof(CtossSettings).GetMethods(BindingFlags.NonPublic | BindingFlags.Static)!
            .Single(x => x is { IsGenericMethod: true, Name: nameof(GetPropertyType) })
            .MakeGenericMethod(type)
            .Invoke(null, new object[] { propertyName });
    }

    internal static Type? GetPropertyType<TEntity>(string propertyName)
    {
        var propertyMapping = GetPropertyMapping<TEntity>(propertyName);
        if (propertyMapping == null)
            return null;

        var expression = propertyMapping.Body;

        return expression switch
        {
            UnaryExpression unaryExpression => unaryExpression.Operand.Type,
            BinaryExpression binaryExpression => binaryExpression.Type,
            MethodCallExpression methodCallExpression => methodCallExpression.Method.ReturnType,
            MemberExpression memberExpression => memberExpression.Type,
            _ => throw new InvalidOperationException($"Unhandled expression type: {expression.GetType().Name}")
        };
    }

    internal static PropertySettings? GetPropertySettings<TEntity>(string propertyName)
    {
        var hasTypeConfiguration = TypeSettings.ContainsKey(typeof(TEntity));
        if (!hasTypeConfiguration)
            return null;

        var typeSettings = TypeSettings[typeof(TEntity)] as TypeSettings<TEntity>;
        if (typeSettings == null)
            return null;

        var hasPropertyConfiguration = typeSettings.PropertySettings.ContainsKey(propertyName);
        return hasPropertyConfiguration
            ? typeSettings.PropertySettings[propertyName]
            : null;
    }
}
