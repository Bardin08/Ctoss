using Ctoss.Builders.Filters;
using Ctoss.Extensions;
using Ctoss.Models;
using Ctoss.Models.AgGrid;
using Ctoss.Models.Enums;
using Ctoss.Tests.Models;

namespace Ctoss.Tests;

public class SortingTest
{
    private readonly FilterBuilder _filterBuilder = new();

    private readonly List<TestEntity> _testEntities =
    [
        new TestEntity(numericProperty: 10, stringProperty: "abc", dateTimeProperty: new DateOnly(2022, 1, 1)),
        new TestEntity(numericProperty: 20, stringProperty: "def", dateTimeProperty: new DateOnly(2023, 2, 2)),
        new TestEntity(numericProperty: 30, stringProperty: "ghi", dateTimeProperty: new DateOnly(2024, 3, 3)),
        new TestEntity(numericProperty: 30, stringProperty: "ghi", dateTimeProperty: new DateOnly(2025, 4, 4))
        {
            ObjectProperty = new NestedEntity() { NumericProperty = 100 }
        },
    ];


    [Fact]
    public void Enumerable_SingleProperty_Success()
    {
        var filter = new AgGridQuery(0, 100, [new Sorting() { Property = "DateTimeProperty", Order = SortingOrder.Desc }], []);
        var result = _testEntities.Apply(filter);
        Assert.True(result.Rows.SequenceEqual(_testEntities.OrderByDescending(x => x.DateTimeProperty)));
    }
    
    [Fact]
    public void Enumerable_NestedPropertyWithNulls_Success()
    {
        var filter = new AgGridQuery(0, 100,
            [new Sorting() { Property = "ObjectProperty.NumericProperty", Order = SortingOrder.Desc }], []);
        var result = _testEntities.Apply(filter);
        Assert.True(result.Rows.SequenceEqual(_testEntities.OrderByDescending(x => x.ObjectProperty?.NumericProperty)));
    }
}