using System.Linq.Expressions;
using Ctoss.Models;

namespace Ctoss.Core;

public interface IFilterProvider
{
    Expression<Func<TModel, bool>>? GetFilterExpression<TModel>(
        IEnumerable<FilterDescriptor>? filterModel);
}