namespace Ctoss.Example;

public static class JsonExamples
{
    /// <summary>
    /// This JSON snippet contains an example of the date range filter with
    /// an illustration that field is case-insensitive.
    /// </summary>
    public const string PlainDateRangeFilter =
        """
        {
           "filterModel": {
              "PrOpErTy": {
                 "filterType": "date",
                 "type": "inRange",
                 "dateFrom": "10/10/2002",
                 "dateTo": "10/12/2020"
              }
           }
        }
        """;

    /// <summary>
    /// This illustrates how to define multiple conditions, and use CTOSS configuration to define the virtual field ms
    /// </summary>
    public const string MultipleConditionsNumberFilter =
        """
        {
           "filters":{
              "mc":{
                 "filterType":"number",
                 "operator": "and",
                 "conditions":[
                    {
                       "filterType":"number",
                       "type":"GreaterThan",
                       "filter":"10"
                    },
                    {
                       "filterType":"number",
                       "type":"LessThan",
                       "filter":"15"
                    }
                 ]
              }
           }
        }
        """;
    
    
}
