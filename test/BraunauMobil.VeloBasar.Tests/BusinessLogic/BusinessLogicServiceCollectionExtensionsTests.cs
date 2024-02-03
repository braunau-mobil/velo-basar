using BraunauMobil.VeloBasar.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic;

public class BusinessLogicServiceCollectionExtensionsTests
{
    [Fact]
    public void AllServicesShouldbeRegistered()
    {
        //  Arrange
        IServiceCollection services = new ServiceCollection();

        //  Act
        services.AddBusinessLogic();

        //  Assert
        services.Should().SatisfyRespectively(
            X.CreateInspector<IAcceptProductService, AcceptProductService>(ServiceLifetime.Scoped),
            X.CreateInspector<IAcceptSessionService, AcceptSessionService>(ServiceLifetime.Scoped),
            X.CreateInspector<IAdminService, AdminService>(ServiceLifetime.Scoped),
            X.CreateInspector<IBasarService, BasarService>(ServiceLifetime.Scoped),
            X.CreateInspector<IBasarStatsService, BasarStatsService>(ServiceLifetime.Scoped),
            X.CreateInspector<IColorProvider, ColorProvider>(ServiceLifetime.Scoped),
            X.CreateInspector<ICountryService, CountryService>(ServiceLifetime.Scoped),
            X.CreateInspector<IDataGeneratorService, DataGeneratorService>(ServiceLifetime.Scoped),
            X.CreateInspector<IDocumentModelFactory, DocumentModelFactory>(ServiceLifetime.Scoped),
            X.CreateInspector<IDocumentService, DocumentService>(ServiceLifetime.Scoped),
            X.CreateInspector<INumberService, NumberService>(ServiceLifetime.Scoped),
            X.CreateInspector<IProductService, ProductService>(ServiceLifetime.Scoped),
            X.CreateInspector<IProductTypeService, ProductTypeService>(ServiceLifetime.Scoped),
            X.CreateInspector<IStatusPushService, WordPressStatusPushService>(ServiceLifetime.Scoped),
            X.CreateInspector<ISellerService, SellerService>(ServiceLifetime.Scoped),
            X.CreateInspector<ISetupService, SetupService>(ServiceLifetime.Scoped),
            X.CreateInspector<ITokenProvider, SimpleTokenProvider>(ServiceLifetime.Scoped),
            X.CreateInspector<ITransactionService, TransactionService>(ServiceLifetime.Scoped)
            );
    }        
}
