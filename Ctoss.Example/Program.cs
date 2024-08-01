using Ctoss.Configuration.Builders;
using Ctoss.Example;
using Ctoss.Extensions;
using Ctoss.Models;
using Ctoss.Models.Enums;

CtossSettingsBuilder.Create()
    .Entity<ExampleEntity>()
    .Property("Property", x => x.Property + x.Property2, p => { p.IgnoreCase = true; })
    .Apply()
    .Entity<ExampleNumericEntity>()
    .Property("virtual", x =>
        (x.SubEntity == null ? -1 : x.SubEntity.A) + (x.SubEntity == null ? -1 : x.SubEntity.B))
    .Property("mc", x => int.Parse(x.A.ToString()))
    .Apply()
    .Entity<ExampleTextEntity>()
    .Property(x => x.TextField, settings => { settings.IgnoreCase = true; })
    .Apply();

var entities = ExampleEntityFaker.GetN(100).AsQueryable()
    .WithFilter(JsonExamples.PlainDateRangeFilter)
    .ToList();

Console.WriteLine("Filtered entities:");
foreach (var entity in entities) Console.WriteLine(entity.Property);

Console.WriteLine("\nNumeric entities:");

var sortings = new List<Sorting>
{
    new()
    {
        Property = "virtual",
        Order = SortingOrder.Asc
    },
};

var numericEntities = ExampleNumericEntityFaker.GetN(100).AsQueryable()
    .WithFilter(JsonExamples.MultipleConditionsNumberFilter)
    .WithSorting(sortings)
    .WithPagination(1, 10)
    .ToList();

foreach (var entity in numericEntities)
    Console.WriteLine($"A: {entity.A}, B: {entity.B}, SubEntity = ({entity.SubEntity.A + entity.SubEntity.B})");
