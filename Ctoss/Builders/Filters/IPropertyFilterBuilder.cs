using System.Linq.Expressions;

namespace Ctoss.Builders.Filters;

internal interface IPropertyFilterBuilder<in TCondition> : IPropertyBuilder
{
    Expression<Func<T, bool>> GetExpression<T>(string property, TCondition condition, bool conditionalAccess);
}
