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
            .AddScoped<IDataGeneratorService, DataGeneratorService>()
            .AddScoped<INumberService, NumberService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IStatusPushService, WordPressStatusPushService>()
            .AddScoped<ISelectListService, SelectListService>()
            .AddScoped<ISellerService, SellerService>()
            .AddScoped<ISetupService, SetupService>()
            .AddScoped<IProductLabelService, ProductLabelService>()
            .AddScoped<ITokenProvider, SimpleTokenProvider>()
            .AddScoped<ITransactionDocumentService, TransactionDocumentService>()
            .AddScoped<ITransactionService, TransactionService>()
            ;
    }
}
