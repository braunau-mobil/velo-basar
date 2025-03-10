﻿using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.FirstBasar;

public class Sell(TestContext context)
{
    public async Task Run()
    {
        IHtmlDocument cartDocument = await context.HttpClient.NavigateMenuAsync("Cart");
        cartDocument.Title.Should().Be("New Sale - Velo Basar");

        IHtmlFormElement form = cartDocument.QueryForm();
        IHtmlButtonElement submitButton = cartDocument.QueryButtonByText("Add");

        cartDocument = await context.HttpClient.SendFormAsync(form, submitButton, new Dictionary<string, object>
        {
            { "ProductId", ID.Products.FirstBasar.SchattenfellMagsame.Votec }
        });
        cartDocument.Title.Should().Be("New Sale - Velo Basar");

        form = cartDocument.QueryForm();
        IHtmlButtonElement sellButton = cartDocument.QueryButtonByText("Sell");
        IHtmlDocument successDocument = await context.HttpClient.SendFormAsync(form, sellButton);
        successDocument.Title.Should().Be("Sale #1 - Velo Basar");

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SaleDocumentModel document = await context.HttpClient.GetSaleDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(context.SaleDocument(
            "XYZ - First Bazaar : Sale receipt #1",
            "Edoras, 5/4/2063",
            "1 Product",
            "$92.99",
            [
                new ProductTableRowDocumentModel("1", "Votec - Children's bike".Line("Votec VRC Comp").Line(" green 1067425379"), "24", "$92.99", "* Schattenfell Magsame, Heidenschuss 41, 6295 Hobbingen")
            ])
        );

        await AssertBasarDetails();
    }

    private async Task AssertBasarDetails()
    {
        BasarSettlementStatus basarSettlementStatus = new(false, 0, 0, 0);
        BasarDetailsModel expectedDetails = new(new BasarEntity(), basarSettlementStatus)
        {
            AcceptanceCount = 1,
            AcceptedProductsAmount = 191.88M,
            AcceptedProductsCount = 2,
            AcceptedProductTypesByAmount = [
                new ChartDataPoint(92.99m, "Children's bike", X.AnyColor),
                new ChartDataPoint(98.89m, "Steel steed", X.AnyColor),
            ],
            AcceptedProductTypesByCount = [
                new ChartDataPoint(1, "Children's bike", X.AnyColor),
                new ChartDataPoint(1, "Steel steed", X.AnyColor)
            ],
            LockedProductsCount = 0,
            LostProductsCount = 0,
            PriceDistribution = context.PriceDistribtion(0, 0, 2, 0, 0),
            SaleCount = 1,
            SaleDistribution = [
                new ChartDataPoint(92.99M, "11:22 AM", X.AnyColor),
            ],
            SellerCount = 1,
            SoldProductsAmount = 92.99m,
            SoldProductsCount = 1,
            SoldProductTypesByCount = [
                new ChartDataPoint(1, "Children's bike", X.AnyColor),
            ],
            SoldProductTypesByAmount = [
                new ChartDataPoint(92.99m, "Children's bike", X.AnyColor),
            ],
        };

        await context.AssertBasarDetails(ID.FirstBasar, expectedDetails);
    }
}
