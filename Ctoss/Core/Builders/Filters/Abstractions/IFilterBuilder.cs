using System.Linq.Expressions;
using Ctoss.Models;
using Ctoss.Models.V2;

namespace Ctoss.Core.Builders.Filters.Abstractions;

public interface IFilterBuilder
{
    Expression<Func<T, bool>>? GetExpression<T>(FilterDescriptor? descriptor);
    Expression<Func<T, bool>>? GetExpression<T>(Dictionary<string, FilterModel>? filterSet);
    Expression<Func<T, bool>>? GetExpression<T>(string property, FilterModel filter);
}