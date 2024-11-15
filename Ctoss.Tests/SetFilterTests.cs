using Ctoss.Builders.Filters;
using Ctoss.Models.V2;

namespace Ctoss.Tests;

public class SetFilterTests
{
    private enum Status
    {
        Error, Warning, Info
    }
    
    private record Value(string Name, Status Status);
    
    private readonly Value[] _values =
    {
        new ("Error", Status.Error),
        new ("Warning", Status.Warning),
        new ("Info", Status.Info)
    };
    
    private readonly FilterBuilder _filterBuilder = new();
    
    [Fact]
    public void SetFilter_MultipleEnum_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "set",
            Values = ["Error", "Warning"], 
        };

        var expr = _filterBuilder.GetExpression<Value>("Status", filter, true)!;
        var result = _values.AsQueryable().Where(expr).ToList();

        Assert.True(result.SequenceEqual(_values.Where(p => p.Status == Status.Error || p.Status == Status.Warning)));
    }
}