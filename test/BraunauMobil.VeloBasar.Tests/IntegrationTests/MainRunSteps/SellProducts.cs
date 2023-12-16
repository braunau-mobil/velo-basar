using BraunauMobil.VeloBasar.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace BraunauMobil.VeloBasar.Tests.IntegrationTests.MainRunSteps;

public class SellProducts
    : TestStepBase
{
    public SellProducts(TestContext testContext)
        : base(testContext)
    { }

    public override async Task Run()
    {
        CartModel cart = await Do<CartController, CartModel>(async controller =>
        {
            IActionResult result = await controller.Index();

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            return view.ViewData.Model.Should().BeOfType<CartModel>().Subject;
        });

        cart.ActiveBasarId.Should().Be(V.FirstBasar.Id);
        cart.HasProducts.Should().BeFalse();
        cart.ProductId.Should().Be(0);
        cart.Products.Should().BeEmpty();

        cart.ProductId = V.Products.FirstBasar.Frodo.Stahlross.Id;

        await Do<CartController>(async controller =>
        {
            IActionResult result = await controller.Add(cart);

            ViewResult view = result.Should().BeOfType<ViewResult>().Subject;
            view.ViewName.Should().BeNull();
            view.ViewData.ModelState.IsValid.Should().BeTrue();
            view.ViewData.Model.Should().Be(cart);
        });

        cart.ActiveBasarId.Should().Be(V.FirstBasar.Id);
        cart.HasProducts.Should().BeTrue();
        cart.ProductId.Should().Be(0);
        cart.Products.Should().HaveCount(1);
        cart.Products.Should().Contain(p => p.Id == V.Products.FirstBasar.Frodo.Stahlross.Id);

        //  @todo continue
    }
}
