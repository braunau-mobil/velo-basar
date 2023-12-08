using BraunauMobil.VeloBasar.Controllers;
using BraunauMobil.VeloBasar.Rendering;
using Microsoft.AspNetCore.Routing;
using Xan.AspNetCore.Mvc;
using Xan.AspNetCore.Mvc.Routing;

namespace BraunauMobil.VeloBasar.Routing;

public sealed partial class VeloRouter
    : LinkRouter
    , IVeloRouter
{
    public VeloRouter(LinkGenerator linkGenerator
        , IAcceptProductRouter acceptProduct, IAcceptSessionRouter acceptSession, IAdminRouter admin, ICrudRouter<BasarEntity> basar, ICancelRouter cancel, ICartRouter cart, ICrudRouter<CountryEntity> country, IDevRouter dev, IAcceptanceLabelsRouter labels, IProductRouter products, ICrudRouter<ProductTypeEntity> productType, ISellerRouter seller, ISetupRouter setup, ITransactionRouter transactions)
        : base(linkGenerator)
    {
        AcceptProduct = acceptProduct ?? throw new ArgumentNullException(nameof(acceptProduct));
        AcceptSession = acceptSession ?? throw new ArgumentNullException(nameof(acceptSession));
        Admin = admin ?? throw new ArgumentNullException(nameof(admin));
        Basar = basar ?? throw new ArgumentNullException(nameof(basar)); 
        Cancel = cancel ?? throw new ArgumentNullException(nameof(cancel));
        Cart = cart ?? throw new ArgumentNullException(nameof(cart));
        Country = country ?? throw new ArgumentNullException(nameof(country));
        Dev = dev ?? throw new ArgumentNullException(nameof(dev));
        AcceptanceLabels = labels ?? throw new ArgumentNullException(nameof(labels));
        Product = products ?? throw new ArgumentNullException(nameof(products));
        ProductType = productType ?? throw new ArgumentNullException(nameof(productType));
        Seller = seller ?? throw new ArgumentNullException(nameof(seller));
        Setup = setup ?? throw new ArgumentNullException(nameof(setup));
        Transaction = transactions ?? throw new ArgumentNullException(nameof(transactions));
    }

    public IAcceptProductRouter AcceptProduct { get; }

    public IAcceptSessionRouter AcceptSession { get; }

    public IAdminRouter Admin { get; }

    public ICrudRouter<BasarEntity> Basar { get; }

    public ICancelRouter Cancel { get; }

    public ICartRouter Cart { get; }

    public ICrudRouter<CountryEntity> Country { get; }

    public IDevRouter Dev { get; }

    public IAcceptanceLabelsRouter AcceptanceLabels { get; }

    public IProductRouter Product { get; }

    public ICrudRouter<ProductTypeEntity> ProductType { get; }

    public ISellerRouter Seller { get; }

    public ISetupRouter Setup { get; }

    public ITransactionRouter Transaction { get; }

    public string ToHome()
        => GetUriByAction(MvcHelper.ControllerName<HomeController>(), nameof(HomeController.Index));

    public string ToLogin()
        => GetUriByAction(MvcHelper.ControllerName<SecurityController>(), nameof(SecurityController.Login));

    public string ToLogout()
        => GetUriByAction(MvcHelper.ControllerName<SecurityController>(), nameof(SecurityController.Logout));

    public string ToSetTheme(Theme theme)
        => GetUriByAction(MvcHelper.ControllerName<HomeController>(), nameof(HomeController.SetTheme), new { theme });
}
