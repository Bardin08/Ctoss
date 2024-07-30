using Bogus;

namespace Ctoss.Example;

internal class ExampleEntityFaker : Faker<ExampleEntity>
{
    private static readonly Faker<ExampleEntity> Faker = new ExampleEntityFaker()
        .RuleFor(x => x.Property, f => f.Date.Between(new DateTime(1970, 1, 1), new DateTime(2024, 6, 6)))
        .RuleFor(x => x.Property2, _ => TimeSpan.FromDays(356 * 5));

    public static IQueryable<ExampleEntity> GetN(int n) => Faker.GenerateLazy(n).AsQueryable();
}

internal class ExampleNumericEntityFaker : Faker<ExampleNumericEntity>
{
    private static readonly Faker<ExampleNumericEntity> Faker = new ExampleNumericEntityFaker()
        .RuleFor(x => x.A, f => f.Random.Int(0, 100))
        .RuleFor(x => x.B, f => f.Random.Int(0, 100))
        .RuleFor(x => x.SubEntity, f => new ExampleNumericEntity
        {
            A = f.Random.Int(0, 100),
            B = f.Random.Int(0, 100)
        });

    public static IQueryable<ExampleNumericEntity> GetN(int n) => Faker.GenerateLazy(n).AsQueryable();
}
