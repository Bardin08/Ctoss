using Ctoss.Builders.Filters;
using Ctoss.Builders.Filters.Abstractions;
using Ctoss.Builders.Sorting;
using Microsoft.Extensions.DependencyInjection;

namespace Ctoss.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static void AddCtoss(this IServiceCollection services)
    {
        services.AddSingleton<ITextFilterBuilder, TextFilterBuilder>();
        services.AddSingleton<INumberFilterBuilder, INumberFilterBuilder>();
        services.AddSingleton<IDateFilterBuilder, DateFilterBuilder>();
        services.AddSingleton<ISetFilterBuilder, SetFilterBuilder>();

        services.AddSingleton<IFilterBuilder, FilterBuilder>();
        services.AddSingleton<ISortingBuilder, SortingBuilder>();
    }
}