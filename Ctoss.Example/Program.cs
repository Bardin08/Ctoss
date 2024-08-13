using System.Text.Json;
using Ctoss;
using Ctoss.Configuration.Builders;
using Ctoss.DependencyInjection;
using Ctoss.Example;
using Ctoss.Json;
using Ctoss.Models.AgGrid;
using Microsoft.Extensions.DependencyInjection;

CtossSettingsBuilder.Create()
    .Entity<ExampleNumericEntity>()
    .Property("virtual", x =>
        (x.SubEntity == null ? -1 : x.SubEntity.A) + (x.SubEntity == null ? -1 : x.SubEntity.B))
    .Property("mc", x => int.Parse(x.A.ToString()))
    .Apply()
    .Entity<ExampleTextEntity>()
    .Property(x => x.TextField, settings => { settings.IgnoreCase = true; })
    .Apply();

var serviceCollection = new ServiceCollection();
serviceCollection.AddCtoss();

var sp = serviceCollection.BuildServiceProvider();
var ctossService = sp.GetRequiredService<ICtossService>();

var result = ctossService.Apply(
    GetQueryFromJson(
        JsonExamples.PlainDateRangeFilter),
    ExampleEntityFaker.GetN(100));

Console.WriteLine("Filtered entities:");
foreach (var entity in result.Rows) Console.WriteLine(entity.Property);

return;

AgGridQuery GetQueryFromJson(string json)
{
    return JsonSerializer.Deserialize<AgGridQuery>(json, CtossJsonDefaults.DefaultJsonOptions)!;
}


//
// Console.WriteLine("\nNumeric entities:");
//
// var sortings = new List<Sorting>
// {
//     new()
//     {
//         Property = "virtual",
//         Order = SortingOrder.Asc
//     },
// };
//
// var numericEntities = ExampleNumericEntityFaker.GetN(100).AsQueryable()
//     .WithFilter(JsonExamples.MultipleConditionsNumberFilter)
//     .WithSorting(sortings)
//     .WithPagination(1, 10)
//     .ToList();
//
// foreach (var entity in numericEntities)
//     Console.WriteLine($"A: {entity.A}, B: {entity.B}, SubEntity = ({entity.SubEntity.A + entity.SubEntity.B})");