namespace BraunauMobil.VeloBasar.Tests;

public static class X
{
    public static T StrictFake<T>()
        where T : class
        => A.Fake<T>(options => options.Strict());
}
