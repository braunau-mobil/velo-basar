using BraunauMobil.VeloBasar.Pdf;
using BraunauMobil.VeloBasar.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace BraunauMobil.VeloBasar.BusinessLogic;

public static class BusinessLogicServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection sc)
    {
        ArgumentNullException.ThrowIfNull(sc);

        return sc
            .AddScoped<IAcceptProductService, AcceptProductService>()
            .AddScoped<IAcceptSessionService, AcceptSessionService>()
            .AddScoped<IAdminService, AdminService>()
            .AddScoped<IBasarService, BasarService>()
            .AddScoped<IBasarStatsService, BasarStatsService>()
            .AddScoped<IColorProvider, ColorProvider>()
            .AddScoped<ICountryService, CountryService>()
            .AddScoped<IDataGeneratorService, DataGeneratorService>()
            .AddScoped<IDocumentModelFactory, DocumentModelFactory>()
            .AddScoped<IDocumentService, DocumentService>()
            .AddScoped<INumberService, NumberService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IProductTypeService, ProductTypeService>()
            .AddScoped<IStatusPushService, WordPressStatusPushService>()
            .AddScoped<ISelectListService, SelectListService>()
            .AddScoped<ISellerService, SellerService>()
            .AddScoped<ISetupService, SetupService>()
            .AddScoped<ITokenProvider, SimpleTokenProvider>()
            .AddScoped<ITransactionDocumentGenerator, ItextTransactionDocumentGenerator>()
            .AddScoped<ITransactionService, TransactionService>()
            ;
    }
}
