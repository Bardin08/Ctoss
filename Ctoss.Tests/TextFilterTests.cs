using Ctoss.Filters;
using Ctoss.Models;
using Ctoss.Models.Conditions;
using Ctoss.Models.Enums;
using Ctoss.Tests.Models;

namespace Ctoss.Tests;

public class TextFilterTests
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
    public void TextFilter_Equals_Success()
    {
        var condition = new TextFilterCondition
        {
            Filter = "abc",
            FilterType = "text",
            Type = TextFilterOptions.Equals
        };

        var filter = new Filter
        {
            FilterType = "text",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_StartsWith_Success()
    {
        var condition = new TextFilterCondition
        {
            Filter = "a",
            FilterType = "text",
            Type = TextFilterOptions.StartsWith
        };

        var filter = new Filter
        {
            FilterType = "text",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_EndsWith_Success()
    {
        var condition = new TextFilterCondition
        {
            Filter = "c",
            FilterType = "text",
            Type = TextFilterOptions.EndsWith
        };

        var filter = new Filter
        {
            FilterType = "text",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_NotBlank_Success()
    {
        var condition = new TextFilterCondition
        {
            FilterType = "text",
            Type = TextFilterOptions.NotBlank
        };

        var filter = new Filter
        {
            FilterType = "text",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter__Success()
    {
        var condition = new TextFilterCondition
        {
            Filter = "ab",
            FilterType = "text",
            Type = TextFilterOptions.Contains
        };

        var filter = new Filter
        {
            FilterType = "text",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_NotEquals_Success()
    {
        var condition1 = new TextFilterCondition
        {
            Filter = "abc",
            FilterType = "text",
            Type = TextFilterOptions.NotEquals
        };
        var condition2 = new TextFilterCondition
        {
            Filter = "ghi",
            FilterType = "text",
            Type = TextFilterOptions.NotEquals
        };

        var filter = new Filter
        {
            FilterType = "text",
            Operator = Operator.And,
            Condition1 = condition1,
            Condition2 = condition2,
            Conditions = new List<FilterCondition> { condition1, condition2 }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("def", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_Composed_Success()
    {
        var condition1 = new TextFilterCondition
        {
            Filter = "def",
            FilterType = "text",
            Type = TextFilterOptions.NotEquals
        };

        var condition2 = new TextFilterCondition
        {
            Filter = "a",
            FilterType = "text",
            Type = TextFilterOptions.StartsWith
        };

        var filter = new Filter
        {
            Operator = Operator.And,
            FilterType = "text",
            Condition1 = condition1,
            Condition2 = condition2,
            Conditions = new List<FilterCondition>
            {
                condition1, condition2
            }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("abc", result.First().StringProperty);
    }
}
