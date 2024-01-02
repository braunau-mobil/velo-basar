using BraunauMobil.VeloBasar.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests.Rendering;

public class RenderingServiceCollectionExtensionsTests
{
    [Fact]
    public void AllServicesShouldbeRegistered()
    {
        //  Arrange
        IServiceCollection services = new ServiceCollection();

        //  Act
        services.AddVeloRendering();

        //  Assert
        services.Should().SatisfyRespectively(
            X.CreateInspector<IVeloHtmlFactory, DefaultVeloHtmlFactory>(ServiceLifetime.Scoped)
            );
    }
}
