using Bogus;

namespace Ctoss.Example;

internal class ExampleEntityFaker : Faker<ExampleEntity>
{
    private static readonly Faker<ExampleEntity> Faker = new ExampleEntityFaker()
        .RuleFor(x => x.Property, f => f.Date.Between(new DateTime(1970, 1, 1), new DateTime(2024, 6, 6)));

    public static IQueryable<ExampleEntity> GetN(int n) => Faker.GenerateLazy(n).AsQueryable();
}
