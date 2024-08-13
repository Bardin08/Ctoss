using System.Linq.Expressions;
using Ctoss.Models;

namespace Ctoss.Core;

public interface ISortingProvider
{
    Expression<Func<TModel, object>>? GetSortingExpression<TModel>(Sorting sorting);
}