namespace BraunauMobil.VeloBasar.Tests;

public class AutoVeloDataAttribute
    : AutoDataAttribute
{
    private static IFixture CreateVeloFixture()
        => new VeloFixture();

    public AutoVeloDataAttribute()
        : base(CreateVeloFixture)
    { }
}
