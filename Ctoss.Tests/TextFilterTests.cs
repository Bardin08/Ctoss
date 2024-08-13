using Ctoss.Core.Builders.Filters;
using Ctoss.Models.Enums;
using Ctoss.Models.V2;
using Ctoss.Tests.Models;

namespace Ctoss.Tests;

public class TextFilterTests
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
    public void TextFilter_Equals_Success()
    {
        var filter = new FilterModel
        {
            Filter = "abc",
            FilterType = "text",
            Type = "Equals"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_StartsWith_Success()
    {
        var filter = new FilterModel
        {
            Filter = "a",
            FilterType = "text",
            Type = "StartsWith"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_EndsWith_Success()
    {
        var filter = new FilterModel
        {
            Filter = "c",
            FilterType = "text",
            Type = "EndsWith"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_NotBlank_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "text",
            Type = "NotBlank"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_Contains_Success()
    {
        var filter = new FilterModel
        {
            Filter = "ab",
            FilterType = "text",
            Type = "Contains"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("abc", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_NotEquals_Success()
    {
        var condition1 = new TextCondition
        {
            Filter = "abc",
            FilterType = "text",
            Type = TextFilterOptions.NotEquals
        };
        var condition2 = new TextCondition
        {
            Filter = "ghi",
            FilterType = "text",
            Type = TextFilterOptions.NotEquals
        };

        var filter = new FilterModel
        {
            FilterType = "text",
            Operator = Operator.And,
            Conditions = new List<FilterConditionBase> { condition1, condition2 }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("StringProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal("def", result.First().StringProperty);
    }

    [Fact]
    public void TextFilter_Composed_Success()
    {
        var condition1 = new TextCondition
        {
            Filter = "def",
            FilterType = "text",
            Type = TextFilterOptions.NotEquals
        };

        var condition2 = new TextCondition
        {
            Filter = "a",
            FilterType = "text",
            Type = TextFilterOptions.StartsWith
        };

        var filter = new FilterModel
        {
            Operator = Operator.And,
            FilterType = "text",
            Conditions = new List<FilterConditionBase>
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
