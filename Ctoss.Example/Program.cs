using Ctoss.Example;
using Ctoss.Extensions;

const string jsonFilter =
    """
    {
        "PROPERTY": {
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

/*
 * The CTOSS gives you three overloads of the method WithFilter which evaluates a given filter and provides you with
 * a filtering Expression<Func<T, bool>> fully compatible with IQueryable and EF.
 *
 * Overloads:
 * - WithFilter<T>(this IQueryable<T> query, string jsonFilter)
 * - WithFilter<T>(this IQueryable<T> query, string propertyName, Filter filter)
 * - WithFilter<T>(this IQueryable<T> query, Dictionary<string, Filter> filters)
 */
var entities = ExampleEntityFaker.GetN(10).AsQueryable()
    .WithFilter(jsonFilter) // <-- This is the extension method from the ctoss library
    .ToList();

Console.WriteLine("Filtered entities:");
foreach (var entity in entities)
{
    Console.WriteLine(entity.Property);
}
