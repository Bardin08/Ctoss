using Ctoss;
using Ctoss.Example;

const string jsonString = """
                          {
                              "tin": {
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

var filterBuilder = new FilterBuilder();
var expr = filterBuilder.GetExpression<Entity>(jsonString);
Console.WriteLine(expr);

namespace Ctoss.Example
{
    class Entity
    {
        public DateTime Tin { get; set; }
    }
}
