using System.Linq.Expressions;

namespace Ctoss.Core.Builders.Sorting;

public interface ISortingBuilder : IPropertyBuilder
{ 
    Expression<Func<TModel, object>>? BuildSortingExpressionV2<TModel>(Models.Sorting? sorting);
}