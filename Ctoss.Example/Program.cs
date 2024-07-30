using Ctoss.Configuration;
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

const string jsonFilter =
    """
    {
        "PrOpErTy": {
            "filterType": "date",
            "condition1": {
                "filterType": "date",
                "type": "inRange",
                "dateFrom": "10/10/2002",
                "dateTo": "10/12/2020"
            },
            "conditions": [
                {
                    "filterType": "date",
                    "type": "inRange",
                    "date": "10/10/2002",
                    "dateTo": "10/12/2020"
                }
            ]
        }
    }
    """;

const string jsonNumericFilter =
    """
    {
        "mc": {
            "filterType": "number",
            "condition1": {
                "filterType": "number",
                "type": "inRange",
                "filter": "1",
                "filterTo": "20"
            },
            "conditions": [
                {
                    "filterType": "number",
                    "type": "GreaterThan",
                    "filter": "10"
                }
            ]
        }
    }
    """;

const string jsonTextFilter =
    """
    {
        "TextField": {
            "filterType": "text",
            "condition1": {
                "filterType": "text",
                "type": "contains",
                "filter": "a"
            },
            "conditions": [
                {
                    "filterType": "text",
                    "type": "contains",
                    "filter": "a"
                }
            ]
        }
    }
    """;

/*
 * The CTOSS gives you three overloads of the method WithFilter which evaluates a given filter and provides you with
 * a filtering Expression<Func<T, bool>> fully compatible with IQueryable and EF.
 *
 * Overloads:
 * - WithFilter<T>(this IQueryable<T> query, string jsonFilter)
 * - WithFilter<T>(this IQueryable<T> query, string propertyName, Filter filter)
 * - WithFilter<T>(this IQueryable<T> query, Dictionary<string, Filter> filters)
 */
var entities = ExampleEntityFaker.GetN(100).AsQueryable()
    .WithFilter(jsonFilter) // <-- This is the extension method from the ctoss library
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
    .WithFilter(jsonNumericFilter) // <-- This is the extension method from the ctoss library
    .WithSorting(sortings)
    .WithPagination(1, 10)
    .ToList();

foreach (var entity in numericEntities)
    Console.WriteLine($"A: {entity.A}, B: {entity.B}, SubEntity = ({entity.SubEntity.A + entity.SubEntity.B})");

Console.WriteLine("\nText entities:");

var textEntity = new ExampleTextEntity()
{
    TextField = "abc"
};

var textEntities = new List<ExampleTextEntity> { textEntity }.AsQueryable()
    .WithFilter(jsonTextFilter) // <-- This is the extension method from the ctoss library
    .ToList();

foreach (var entity in textEntities) Console.WriteLine(entity.TextField);