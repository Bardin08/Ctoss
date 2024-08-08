using System.Linq.Expressions;

namespace Ctoss.Builders.Filters.Abstractions;

public interface IPropertyFilterBuilder<in TCondition> : IPropertyBuilder
{
    Expression<Func<T, bool>> GetExpression<T>(string property, TCondition condition);
}
