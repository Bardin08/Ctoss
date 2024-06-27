using System.Linq.Expressions;

namespace Ctoss.Filters;

public interface IPropertyFilterBuilder<in TCondition>
{
    Expression<Func<T, bool>> GetExpression<T>(string property, TCondition condition);
}