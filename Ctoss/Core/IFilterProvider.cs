using System.Linq.Expressions;
using Ctoss.Models;

namespace Ctoss.Core;

public interface IFilterProvider
{
    Task<Expression<Func<TModel, bool>>?> GetFilterExpressionAsync<TModel>(
        IEnumerable<FilterDescriptor>? filterModel);
}