using System.Linq.Expressions;
using Ctoss.Models;

namespace Ctoss;

public interface IFilterProvider
{
    Task<Expression<Func<TModel, bool>>?> GetFilterExpressionAsync<TModel>(
        IEnumerable<FilterDescriptor>? filterModel);
}