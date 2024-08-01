using System.Linq.Expressions;
using Ctoss.Models.V2;

namespace Ctoss.Builders.Filters;

internal class SetFilterBuilder : IPropertyFilterBuilder<SetCondition>
{
    public Expression<Func<T, bool>> GetExpression<T>(string property, SetCondition condition)
    {
        throw new NotImplementedException();
    }
}