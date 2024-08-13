using System.Linq.Expressions;
using System.Reflection;
using Ctoss.Builders.Filters.Abstractions;
using Ctoss.Models.V2;

namespace Ctoss.Builders.Filters;

public class SetFilterBuilder : ISetFilterBuilder
{
    public Expression<Func<T, bool>> GetExpression<T>(string property, SetCondition condition)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        
        var propertyType = IPropertyBuilder.GetPropertyType<T>(property);
        var propertyExpression = IPropertyBuilder.GetPropertyExpression<T>(property, parameter, propertyType);
        
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
        var set = new HashSet<T>(conditionValues.Select(x => (T)Convert.ChangeType(x, typeof(T))));
        var setExpression = Expression.Constant(set);
        return Expression.Call(setExpression, typeof(HashSet<T>).GetMethod("Contains")!, propertyExpression);
    }
}