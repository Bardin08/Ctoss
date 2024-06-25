using Ctoss.Models;
using Ctoss.Models.Conditions;
using Ctoss.Models.Enums;
using Ctoss.Tests.Models;

namespace Ctoss.Tests;

public class DateFilterTests
{
    private readonly FilterBuilder _filterBuilder = new();

    private readonly List<TestEntity> _testEntities =
    [
        new TestEntity
        {
            NumericProperty = 10, StringProperty = "abc", DateTimeProperty = new DateTime(2022, 1, 1)
        },
        new TestEntity
        {
            NumericProperty = 20, StringProperty = "def", DateTimeProperty = new DateTime(2023, 2, 2)
        },
        new TestEntity
        {
            NumericProperty = 30, StringProperty = "ghi", DateTimeProperty = new DateTime(2024, 3, 3)
        }
    ];

    [Fact]
    public void DateFilter_Equals_Success()
    {
        var condition = new DateFilterCondition
        {
            DateFrom = "01/01/2022",
            FilterType = "date",
            Type = DateFilterOptions.Equals
        };

        var filter = new NumberFilter
        {
            FilterType = "date",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateTime(2022, 1, 1), result.First().DateTimeProperty);
    }

    [Fact]
    public void DateFilter_GreaterThen_Success()
    {
        var condition = new DateFilterCondition
        {
            DateFrom = "02/02/2023",
            FilterType = "date",
            Type = DateFilterOptions.GreaterThen
        };

        var filter = new NumberFilter
        {
            FilterType = "date",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateTime(2024, 3, 3), result.First().DateTimeProperty);
    }

    [Fact]
    public void DateFilter_Blank_Success()
    {
        var condition = new DateFilterCondition
        {
            FilterType = "date",
            Type = DateFilterOptions.Blank
        };

        var filter = new NumberFilter
        {
            FilterType = "date",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void DateFilter_LessThen_Success()
    {
        var condition = new DateFilterCondition
        {
            DateFrom = "01/01/2023",
            FilterType = "date",
            Type = DateFilterOptions.LessThen
        };

        var filter = new NumberFilter
        {
            FilterType = "date",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateTime(2022, 1, 1), result.First().DateTimeProperty);
    }

    [Fact]
    public void DateFilter_NotBlank_Success()
    {
        var condition = new DateFilterCondition
        {
            FilterType = "date",
            Type = DateFilterOptions.NotBlank
        };

        var filter = new NumberFilter
        {
            FilterType = "date",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void DateFilter_InRange_Success()
    {
        var condition = new DateFilterCondition
        {
            DateFrom = "06/06/2021",
            DateTo = "09/09/2024",
            FilterType = "date",
            Type = DateFilterOptions.InRange
        };

        var filter = new NumberFilter
        {
            FilterType = "date",
            Condition1 = condition,
            Conditions = new List<FilterCondition> { condition }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void DateFilter_NotEquals_Success()
    {
        var condition1 = new DateFilterCondition
        {
            DateFrom = "01/01/2022",
            FilterType = "date",
            Type = DateFilterOptions.NotEquals
        };
        var condition2 = new DateFilterCondition
        {
            DateFrom = "03/03/2024",
            FilterType = "date",
            Type = DateFilterOptions.NotEquals
        };

        var filter = new NumberFilter
        {
            FilterType = "date",
            Operator = Operator.And,
            Condition1 = condition1,
            Condition2 = condition2,
            Conditions = new List<FilterCondition> { condition1, condition2 }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateTime(2023, 02, 02), result.First().DateTimeProperty);
    }

    [Fact]
    public void DateFilter_Composed_Success()
    {
        var condition1 = new DateFilterCondition
        {
            DateFrom = "01/01/2022",
            FilterType = "date",
            Type = DateFilterOptions.NotEquals
        };

        var condition2 = new DateFilterCondition
        {
            DateFrom = "03/03/2024",
            FilterType = "date",
            Type = DateFilterOptions.LessThen
        };

        var filter = new NumberFilter
        {
            Operator = Operator.And,
            FilterType = "date",
            Condition1 = condition1,
            Condition2 = condition2,
            Conditions = new List<FilterCondition>
            {
                condition1, condition2
            }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateTime(2023, 02, 02), result.First().DateTimeProperty);
    }
}
