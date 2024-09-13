namespace Ctoss.Tests.Models;

public class NestedEntity
{
    public int? NumericProperty { get; set; }
}
public class TestEntity
{
    public TestEntity()
    {
    }

    public TestEntity(int? numericProperty, string? stringProperty, DateOnly dateTimeProperty)
    {
        NumericProperty = numericProperty;
        StringProperty = stringProperty;
        DateTimeProperty = dateTimeProperty;
    }

    public string? StringProperty { get; set; } = null!;
    public DateOnly DateTimeProperty { get; set; }
    public int? NumericProperty { get; set; }
    
    public NestedEntity? ObjectProperty { get; set; }
}
