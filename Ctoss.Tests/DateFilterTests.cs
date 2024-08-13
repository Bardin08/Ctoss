using Ctoss.Core.Builders.Filters;
using Ctoss.Models.Enums;
using Ctoss.Models.V2;
using Ctoss.Tests.Models;

namespace Ctoss.Tests;

public class DateFilterTests
{
    private readonly FilterBuilder _filterBuilder = new(
        new TextFilterBuilder(),
        new DateFilterBuilder(),
        new NumberFilterBuilder(),
        new SetFilterBuilder());

    private readonly List<TestEntity> _testEntities =
    [
        new TestEntity(numericProperty: 10, stringProperty: "abc", dateTimeProperty: new DateOnly(2022, 1, 1)),
        new TestEntity(numericProperty: 20, stringProperty: "def", dateTimeProperty: new DateOnly(2023, 2, 2)),
        new TestEntity(numericProperty: 30, stringProperty: "ghi", dateTimeProperty: new DateOnly(2024, 3, 3))
    ];

    [Fact]
    public void DateFilter_Equals_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "date",
            DateFrom = "01/01/2022",
            Type = "equals"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateOnly(2022, 1, 1), result.First().DateTimeProperty);
    }

    [Fact]
    public void DateFilter_GreaterThen_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "date",
            DateFrom = "02/02/2023",
            Type = "GreaterThan"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateOnly(2024, 3, 3), result.First().DateTimeProperty);
    }

    [Fact]
    public void DateFilter_Blank_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "date",
            Type = "Blank"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Empty(result);
    }

    [Fact]
    public void DateFilter_LessThen_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "date",
            DateFrom = "01/01/2023",
            Type = "LessThan"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateOnly(2022, 1, 1), result.First().DateTimeProperty);
    }

    [Fact]
    public void DateFilter_NotBlank_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "date",
            Type = "NotBlank"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void DateFilter_InRange_Success()
    {
        var filter = new FilterModel
        {
            FilterType = "date",
            DateFrom = "06/06/2021",
            DateTo = "09/09/2024",
            Type = "InRange"
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Equal(3, result.Count);
    }

    [Fact]
    public void DateFilter_NotEquals_Success()
    {
        var condition1 = new DateCondition
        {
            DateFrom = "01/01/2022",
            FilterType = "date",
            Type = DateFilterOptions.NotEquals
        };
        var condition2 = new DateCondition
        {
            DateFrom = "03/03/2024",
            FilterType = "date",
            Type = DateFilterOptions.NotEquals
        };

        var filter = new FilterModel
        {
            FilterType = "date",
            Operator = Operator.And,
            Conditions = new List<FilterConditionBase> { condition1, condition2 }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateOnly(2023, 02, 02), result.First().DateTimeProperty);
    }

    [Fact]
    public void DateFilter_Composed_Success()
    {
        var condition1 = new DateCondition
        {
            DateFrom = "01/01/2022",
            FilterType = "date",
            Type = DateFilterOptions.NotEquals
        };

        var condition2 = new DateCondition
        {
            DateFrom = "03/03/2024",
            FilterType = "date",
            Type = DateFilterOptions.LessThan
        };

        var filter = new FilterModel
        {
            FilterType = "date",
            Operator = Operator.And,
            Conditions = new List<FilterConditionBase>
            {
                condition1, condition2
            }
        };

        var expr = _filterBuilder.GetExpression<TestEntity>("DateTimeProperty", filter)!;
        var result = _testEntities.AsQueryable().Where(expr).ToList();

        Assert.Single(result);
        Assert.Equal(new DateOnly(2023, 02, 02), result.First().DateTimeProperty);
    }
}