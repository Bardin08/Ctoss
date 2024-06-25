using Ctoss.Example;
using Ctoss.Extensions;

const string jsonFilter =
    """
    {
        "property": {
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

var entities = ExampleEntityFaker.GetN(10).AsQueryable()
    .WithFilter(jsonFilter) // <-- This is the extension method from the ctoss library
    .ToList();

Console.WriteLine("Filtered entities:");
foreach (var entity in entities)
{
    Console.WriteLine(entity.Property);
}
