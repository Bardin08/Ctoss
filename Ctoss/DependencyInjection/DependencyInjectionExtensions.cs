using Ctoss.Core;
using Ctoss.Core.Builders.Filters;
using Ctoss.Core.Builders.Filters.Abstractions;
using Ctoss.Core.Builders.Sorting;
using Microsoft.Extensions.DependencyInjection;

namespace Ctoss.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static void AddCtoss(this IServiceCollection services)
    {
        services.AddSingleton<ITextFilterBuilder, TextFilterBuilder>();
        services.AddSingleton<INumberFilterBuilder, NumberFilterBuilder>();
        services.AddSingleton<IDateFilterBuilder, DateFilterBuilder>();
        services.AddSingleton<ISetFilterBuilder, SetFilterBuilder>();

        services.AddSingleton<IFilterBuilder, FilterBuilder>();
        services.AddSingleton<ISortingBuilder, SortingBuilder>();
        
        services.AddSingleton<IFilterProvider, FilterProvider>();
        services.AddSingleton<IFilterExpressionCache, FilterExpressionCache>();
                
        services.AddSingleton<ISortingProvider, SortingProvider>();
        services.AddSingleton<ISortingExpressionCache, SortingExpressionCache>();
        
        services.AddSingleton<ICtossService, CtossService>();
    }
}