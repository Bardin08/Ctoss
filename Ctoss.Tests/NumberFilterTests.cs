using Ctoss.Builders.Filters;
using Ctoss.Models.Enums;
using Ctoss.Models.V2;
using Ctoss.Tests.Models;

namespace Ctoss.Tests;

public class FilterTests
{
    private readonly FilterBuilder _filterBuilder = new(
        new TextFilterBuilder(),
        new DateFilterBuilder(),
        new NumberFilterBuilder(),
        new SetFilterBuilder());

    private readonly List<TestEntity> _testEntities =
    [
        new TestEntity
        {
            NumericProperty = 10, StringProperty = "abc", DateTimeProperty = new DateOnly(2022, 1, 1)
        },
        new TestEntity
        {
            NumericProperty = 20, StringProperty = "def", DateTimeProperty = new DateOnly(2023, 2, 2)
        },
        new TestEntity
        {
            NumericProperty = 30, StringProperty = "ghi", DateTimeProperty = new DateOnly(2024, 3, 3)
        }
    ];

    [Fact]
    public void NumericFilter_Equals_Success()
    {
        var filter = new FilterModel
        {
            Filter = "10",
            FilterType = "number",
            Type = "Equals"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(10, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_GreaterThen_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "number",
            Filter = "20",
            Type = "GreaterThan"
        };
        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(30, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_GreaterThenOrEquals_Success()
    {
        var filter = new FilterModel
        {
            Filter = "30",
            FilterType = "number",
            Type = "GreaterThanOrEqual"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(30, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_LessThen_Success()
    {
        var filter = new FilterModel
        {
            Filter = "20",
            FilterType = "number",
            Type = "LessThan"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(10, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_LessThenOrEquals_Success()
    {
        var filter = new FilterModel
        {
            Filter = "10",
            FilterType = "number",
            Type = "LessThanOrEqual"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(10, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_InRange_Success()
    {
        var filter = new FilterModel
        {
            Filter = "0",
            FilterTo = "12",
            FilterType = "number",
            Type = "InRange"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(10, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_NotEquals_Success()
    {
        var condition1 = new NumberCondition
        {
            Filter = "10",
            FilterType = "number",
            Type = NumberFilterOptions.NotEquals
        };
        var condition2 = new NumberCondition
        {
            Filter = "20",
            FilterType = "number",
            Type = NumberFilterOptions.NotEquals
        };

        var filter = new FilterModel
        {
            FilterType = "number",
            Operator = Operator.And,
            Conditions = new List<FilterConditionBase> { condition1, condition2 }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(30, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_NotBlank_Success()
    {
        var filter = new FilterModel
        {
            Filter = "10",
            FilterType = "number",
            Type = "NotBlank"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void NumericFilter_Blank_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "number",
            Type = "Blank"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void NumericFilter_Composed_Success()
    {
        var condition1 = new NumberCondition
        {
            Filter = "25",
            FilterType = "number",
            Type = NumberFilterOptions.LessThan
        };

        var condition2 = new NumberCondition
        {
            Filter = "10",
            FilterType = "number",
            Type = NumberFilterOptions.NotEquals
        };

        var filter = new FilterModel
        {
            Operator = Operator.And,
            FilterType = "number",
            Conditions = new List<FilterConditionBase>
            {
                condition1, condition2
            }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(20, result.First().NumericProperty);
    }
}