using System.Linq.Expressions;
using System.Reflection;
using Ctoss.Models.V2;

namespace Ctoss.Builders.Filters;

internal class SetFilterBuilder : IPropertyFilterBuilder<SetCondition>
{
    public Expression<Func<T, bool>> GetExpression<T>(string property, SetCondition condition, bool conditionalAccess)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        // Build: x => x.Property.Contains(condition.Filter)
        var propertyType = IPropertyBuilder.GetPropertyType<T>(property);
        var propertyExpression = IPropertyBuilder.GetPropertyExpression<T>(property, parameter, propertyType, conditionalAccess);
        
        var valueExpression = GetContainsExpression(condition.Values, propertyExpression, propertyType);

        return Expression.Lambda<Func<T, bool>>(valueExpression, parameter);
    }

    private Expression GetContainsExpression(List<string> conditionValues, Expression propertyExpression, Type propertyType)
    {
        return (Expression)GetType().GetMethod(nameof(GetContainsExpressionGeneric), BindingFlags.Instance | BindingFlags.NonPublic)
            !.MakeGenericMethod(propertyType)
            .Invoke(this, new object[] {propertyExpression, conditionValues})!;
    }

    private Expression GetContainsExpressionGeneric<T>(Expression propertyExpression, List<string> conditionValues)
    {
        var set = new HashSet<T>(conditionValues.Select(ConvertTo<T>));
        var setExpression = Expression.Constant(set);
        return Expression.Call(setExpression, typeof(HashSet<T>).GetMethod("Contains")!, propertyExpression);
    }

    private T ConvertTo<T>(string s)
    {
        if (typeof(T).IsEnum)
        {
            return (T)Enum.Parse(typeof(T), s);
        }

        return (T)Convert.ChangeType(s, typeof(T));
    }
}