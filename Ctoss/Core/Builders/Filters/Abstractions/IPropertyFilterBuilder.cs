using System.Linq.Expressions;

namespace Ctoss.Core.Builders.Filters.Abstractions;

public interface IPropertyFilterBuilder<in TCondition> : IPropertyBuilder
{
    Expression<Func<T, bool>> GetExpression<T>(string property, TCondition condition);
}
