using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests;

public static class X
{
    public static DateTime FirstContactDay { get; } = new DateTime(2063, 04, 05, 11, 22, 33);

    public static Action<ServiceDescriptor> CreateInspector<TServiceType, TImplementationType>(ServiceLifetime lifetime)
        => item =>
        {
            item.ServiceType.Should().Be(typeof(TServiceType));
            item.ImplementationType.Should().Be(typeof(TImplementationType));
            item.Lifetime.Should().Be(lifetime);
        };

    public static T StrictFake<T>()
        where T : class
        => A.Fake<T>(options => options.Strict());
}
