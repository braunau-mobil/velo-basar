using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Cookies;
using BraunauMobil.VeloBasar.Routing;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Controllers;

public sealed class CartController
    : AbstractVeloController
{
    private readonly IProductService _productService;
    private readonly ITransactionService _transactionService;
    private readonly IVeloRouter _router;
    private readonly IValidator<CartModel> _validator;
    private readonly ICartCookie _cookie;

    public CartController(IProductService productService, ITransactionService transactionService, IVeloRouter router, IValidator<CartModel> validator, ICartCookie cookie)
    {
        _productService = productService ?? throw new ArgumentNullException(nameof(productService));
        _transactionService = transactionService ?? throw new ArgumentNullException(nameof(transactionService));
        _router = router ?? throw new ArgumentNullException(nameof(router));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
        _cookie = cookie ?? throw new ArgumentNullException(nameof(cookie));
    }

    [HttpPost]
    public async Task<IActionResult> Add(CartModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        IList<int> cart = _cookie.GetCart();

        SetValidationResult(await _validator.ValidateAsync(model));

        if (ModelState.IsValid)
        {
            cart.Add(model.ProductId);
            _cookie.SetCart(cart);

            model.ProductId = 0;
        }
            
        model.Products = await _productService.GetManyAsync(cart);
        return View(nameof(Index), model);        
    }

    public async Task<IActionResult> Checkout(int activeBasarId)
    {
        ArgumentNullException.ThrowIfNull(activeBasarId);

        IList<int> cart = _cookie.GetCart();
        if (cart.Count <= 0)
        {
            return StatusCode(StatusCodes.Status405MethodNotAllowed);
        }

        int saleId = await _transactionService.CheckoutAsync(activeBasarId, cart);
        _cookie.ClearCart();
        return Redirect(_router.Transaction.ToSucess(saleId));
    }

    public IActionResult Clear()
    {
        _cookie.ClearCart();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int productId)
    {
        IList<int> cart = _cookie.GetCart();
        cart.Remove(productId);
        _cookie.SetCart(cart);

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Index()
    {
        IList<int> cart = _cookie.GetCart();
        CartModel model = new()
        {
            Products = await _productService.GetManyAsync(cart)
        };
        return View(model);
    }
}
