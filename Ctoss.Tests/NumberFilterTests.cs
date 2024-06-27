using Ctoss.Filters;
using Ctoss.Models;
using Ctoss.Models.Conditions;
using Ctoss.Models.Enums;
using Ctoss.Tests.Models;

namespace Ctoss.Tests;

public class FilterTests
{
    private readonly FilterBuilder _filterBuilder = new();

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
        var condition = new NumberFilterCondition
        {
            Filter = 10,
            FilterType = "number",
            Type = NumberFilterOptions.Equals
        };

        var filter = new Filter
        {
            FilterType = "number",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(10, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_GreaterThen_Success()
    {
        var condition = new NumberFilterCondition
        {
            Filter = 20,
            FilterType = "number",
            Type = NumberFilterOptions.GreaterThan
        };

        var filter = new Filter
        {
            FilterType = "number",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(30, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_GreaterThenOrEquals_Success()
    {
        var condition = new NumberFilterCondition
        {
            Filter = 30,
            FilterType = "number",
            Type = NumberFilterOptions.GreaterThanOrEqual
        };

        var filter = new Filter
        {
            FilterType = "number",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(30, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_LessThen_Success()
    {
        var condition = new NumberFilterCondition
        {
            Filter = 20,
            FilterType = "number",
            Type = NumberFilterOptions.LessThan
        };

        var filter = new Filter
        {
            FilterType = "number",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(10, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_LessThenOrEquals_Success()
    {
        var condition = new NumberFilterCondition
        {
            Filter = 10,
            FilterType = "number",
            Type = NumberFilterOptions.LessThanOrEqual
        };

        var filter = new Filter
        {
            FilterType = "number",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(10, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_InRange_Success()
    {
        var condition = new NumberFilterCondition
        {
            Filter = 0,
            FilterTo = 12,
            FilterType = "number",
            Type = NumberFilterOptions.InRange
        };

        var filter = new Filter
        {
            FilterType = "number",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(10, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_NotEquals_Success()
    {
        var condition1 = new NumberFilterCondition
        {
            Filter = 10,
            FilterType = "number",
            Type = NumberFilterOptions.NotEquals
        };
        var condition2 = new NumberFilterCondition
        {
            Filter = 20,
            FilterType = "number",
            Type = NumberFilterOptions.NotEquals
        };

        var filter = new Filter
        {
            FilterType = "number",
            Operator = Operator.And,
            Condition1 = condition1,
            Condition2 = condition2,
            Conditions = new List<FilterCondition> { condition1, condition2 }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("NumericProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(30, result.First().NumericProperty);
    }

    [Fact]
    public void NumericFilter_Composed_Success()
    {
        var condition1 = new NumberFilterCondition
        {
            Filter = 25,
            FilterType = "number",
            Type = NumberFilterOptions.LessThan
        };

        var condition2 = new NumberFilterCondition
        {
            Filter = 10,
            FilterType = "number",
            Type = NumberFilterOptions.NotEquals
        };

        var filter = new Filter
        {
            Operator = Operator.And,
            FilterType = "number",
            Condition1 = condition1,
            Condition2 = condition2,
            Conditions = new List<FilterCondition>
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
