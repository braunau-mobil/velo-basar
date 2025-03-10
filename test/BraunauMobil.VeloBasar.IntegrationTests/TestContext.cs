﻿using BraunauMobil.VeloBasar.BusinessLogic;
using BraunauMobil.VeloBasar.Configuration;

namespace BraunauMobil.VeloBasar.IntegrationTests;

public record TestContext(IServiceProvider ServiceProvider, HttpClient HttpClient, PrintSettings PrintSettings)
{
    public AcceptanceDocumentModel AcceptanceDocument(string title, string locationAndDateText, string addressText, string sellerIdText, string statusLink, string sellerToken, string signatureText, string productTableCountText, string productTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel> productTableRows)
        => new(title,
                locationAndDateText,
                "Page {0} of {1}",
                "  - powered by https://github.com/braunau-mobil/velo-basar",
                PrintSettings.PageMargins,
                PrintSettings.Acceptance.SubTitle,
                addressText,
                sellerIdText,
                true,
                statusLink,
                PrintSettings.Acceptance.TokenTitle,
                sellerToken,
                "For XYZ: ______________________________",
                signatureText,
                PrintSettings.QrCodeLengthMillimeters,
                new ProductsTableDocumentModel(
                    "Id",
                    "Procuct description",
                    "Size",
                    "Price",
                    "Sum:",
                    productTableCountText,
                    productTablePriceText,
                    null,
                    productTableRows
                )
            );


    public async Task<IHtmlDocument> AddProductToCart(IHtmlDocument cartDocument, int productId)
    {
        ArgumentNullException.ThrowIfNull(cartDocument);

        IHtmlFormElement form = cartDocument.QueryForm();
        IHtmlButtonElement addButton = cartDocument.QueryButtonByText("Add");
        cartDocument = await HttpClient.SendFormAsync(form, addButton, new Dictionary<string, object>
        {
            { "ProductId", productId }
        });
        return cartDocument;
    }

    public async Task AssertBasarDetails(int basarId, BasarDetailsModel expectedDetails)
    {
        ArgumentNullException.ThrowIfNull(expectedDetails);

        IBasarService basarService = ServiceProvider.GetRequiredService<IBasarService>();
        BasarDetailsModel result = await basarService.GetDetailsAsync(basarId);

        result.Should().BeEquivalentTo(expectedDetails, options =>
        {
            return options.Excluding(details => details.Entity)
                .For(details => details.AcceptedProductTypesByAmount)
                    .Exclude(chartData => chartData.Color)
                .For(details => details.AcceptedProductTypesByCount)
                    .Exclude(chartData => chartData.Color)
                .For(details => details.PriceDistribution)
                    .Exclude(chartData => chartData.Color)
                .For(details => details.SaleDistribution)
                    .Exclude(chartData => chartData.Color)
                .For(details => details.SoldProductTypesByAmount)
                    .Exclude(chartData => chartData.Color)
                .For(details => details.SoldProductTypesByCount)
                    .Exclude(chartData => chartData.Color)
                ;
        });
    }

    public async Task AssertSellerDetails(int basarId, int sellerId, SellerDetailsModel expectedDetails)
    {
        ArgumentNullException.ThrowIfNull(expectedDetails);

        ISellerService sellerService = ServiceProvider.GetRequiredService<ISellerService>();
        SellerDetailsModel result = await sellerService.GetDetailsAsync(basarId, sellerId);

        result.Should().BeEquivalentTo(expectedDetails, options =>
        {
            return options.Excluding(details => details.Entity)
                .Excluding(details => details.BasarId)
                .Excluding(details => details.Products)
                .Excluding(details => details.Transactions);
        });
    }

    public string BankingQrCode(string seller, string amount, string decscription)
        => BankingQrCode(seller, "", amount, decscription);

    public string BankingQrCode(string seller, string iban, string amount, string decscription)
    {
        ArgumentNullException.ThrowIfNull(seller);
        ArgumentNullException.ThrowIfNull(iban);
        ArgumentNullException.ThrowIfNull(amount);
        ArgumentNullException.ThrowIfNull(decscription);

        return "BCD"
            .Line("002")
            .Line("1")
            .Line("SCT")
            .Line()
            .Line(seller)
            .Line(iban)
            .Line(amount)
            .Line()
            .Line()
            .Line(decscription)
            .Line()
            .Line();
    }

    public async Task DoScoped(Func<IServiceScope, Task> what)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();
        
        await what(scope);
    }

    public async Task<IHtmlDocument> EnterProduct(IHtmlDocument enterProductsDocument, string expectedTitle, IDictionary<string, object> values)
    {
        ArgumentNullException.ThrowIfNull(enterProductsDocument);
        ArgumentNullException.ThrowIfNull(expectedTitle);
        ArgumentNullException.ThrowIfNull(values);

        IHtmlFormElement form = enterProductsDocument.QueryForm();
        IHtmlButtonElement submitButton = enterProductsDocument.QueryButtonByText("Add");

        IHtmlDocument postDocument = await HttpClient.SendFormAsync(form, submitButton, values);
        postDocument.Title.Should().Be(expectedTitle);
        return postDocument;
    }

    public IReadOnlyList<ChartDataPoint> PriceDistribtion(int toNineCommaNineNine, int tenToFourtyNineCommaNineNine, int fiftyToNinteyNineCommaNineNine, int houndredToFourhundredNinetyNineCommaNineNine, int formFiveHundred)
        => [
            new ChartDataPoint(toNineCommaNineNine, "-$9.99", X.AnyColor),
            new ChartDataPoint(tenToFourtyNineCommaNineNine, "$10.00 - $49.99", X.AnyColor),
            new ChartDataPoint(fiftyToNinteyNineCommaNineNine, "$50.00 - $99.99", X.AnyColor),
            new ChartDataPoint(houndredToFourhundredNinetyNineCommaNineNine, "$100.00 - $499.99", X.AnyColor),
            new ChartDataPoint(formFiveHundred, "$500.00+", X.AnyColor)
        ];

    public SaleDocumentModel SaleDocument(string title, string locationAndDateText, string productTableCountText, string productTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel> productTableRows)
        => new(title,
                locationAndDateText,
                "Page {0} of {1}",
                "  - powered by https://github.com/braunau-mobil/velo-basar",
                PrintSettings.PageMargins,
                PrintSettings.Sale.SubTitle,
                true,
                PrintSettings.BannerFilePath!,
                PrintSettings.BannerSubtitle,
                PrintSettings.Website,
                PrintSettings.Sale.HintText,
                PrintSettings.Sale.FooterText,
                new ProductsTableDocumentModel(
                    "Id",
                    "Procuct description",
                    "Size",
                    "Price",
                    "Sum:",
                    productTableCountText,
                    productTablePriceText,
                    PrintSettings.Sale.SellerInfoText,
                    productTableRows
                )
            );

    public SettlementDocumentModel SettlementDocument(string title, string locationAndDateText, string addressText, string sellerIdText, bool addBankingQrCode, string bankAccountHolder, string iban, string bankingQrCodeContent, string signatureText)
        => SettlementDocument(title,
            locationAndDateText,
            addressText,
            sellerIdText,
            null, null, null, null, null, null, null, null, null, null,
            addBankingQrCode,
            bankAccountHolder,
            iban,
            bankingQrCodeContent,
            signatureText
        );

    public SettlementDocumentModel SettlementDocument(string title, string locationAndDateText, string addressText, string sellerIdText, string pickupTableCountText, string pickupTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel> pickupTableRows, bool addBankingQrCode, string bankAccountHolder, string iban, string bankingQrCodeContent, string signatureText)
        => SettlementDocument(title,
            locationAndDateText,
            addressText,
            sellerIdText,
            null, null, null, null, null, null, null,
            pickupTableCountText,
            pickupTablePriceText,
            pickupTableRows,
            addBankingQrCode,
            bankAccountHolder,
            iban,
            bankingQrCodeContent,
            signatureText
        );

    public SettlementDocumentModel SettlementDocument(string title, string locationAndDateText, string addressText, string sellerIdText, string commissionPartText, string payoutAmountInclComissionText, string payoutCommissionAmountText, string payoutAmountText, string payoutTableCountText, string payoutTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel> payoutTableRows, bool addBankingQrCode, string bankAccountHolder, string iban, string bankingQrCodeContent, string signatureText)
        => SettlementDocument(title,
            locationAndDateText,
            addressText,
            sellerIdText,
            commissionPartText,
            payoutAmountInclComissionText,
            payoutCommissionAmountText,
            payoutAmountText,
            payoutTableCountText,
            payoutTablePriceText,
            payoutTableRows,
            null, null, null,
            addBankingQrCode,
            bankAccountHolder,
            iban,
            bankingQrCodeContent,
            signatureText
        );

    public SettlementDocumentModel SettlementDocument(string title, string locationAndDateText, string addressText, string sellerIdText, string? commissionPartText, string? payoutAmountInclComissionText, string? payoutCommissionAmountText, string? payoutAmountText, string? payoutTableCountText, string? payoutTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel>? payoutTableRows, string? pickupTableCountText, string? pickupTablePriceText, IReadOnlyCollection<ProductTableRowDocumentModel>? pickupTableRows, bool addBankingQrCode, string bankAccountHolder, string iban, string bankingQrCodeContent, string signatureText)
    {
        ProductsTableDocumentModel? payoutTable = null;

        if (payoutTableCountText is not null
            && payoutTablePriceText is not null
            && payoutTableRows is not null)
        {
            payoutTable = new ProductsTableDocumentModel(
                "Id",
                "Procuct description",
                "Size",
                "Selling price",
                "Sum:",
                payoutTableCountText,
                payoutTablePriceText,
                null,
                payoutTableRows
            );
        }

        ProductsTableDocumentModel? pickupTable = null;
        if (pickupTableCountText is not null
            && pickupTablePriceText is not null
            && pickupTableRows is not null)
        {
            pickupTable = new ProductsTableDocumentModel(
                "Id",
                "Procuct description",
                "Size",
                "Price",
                "Sum:",
                pickupTableCountText,
                pickupTablePriceText,
                null,
                pickupTableRows
            );
        }

        SettlementCommisionSummaryModel? settlementCommisionSummary = null;
        if (commissionPartText is not null
            && payoutAmountInclComissionText is not null
            && payoutCommissionAmountText is not null
            && payoutAmountText is not null)
        {
            settlementCommisionSummary = new SettlementCommisionSummaryModel(
                   "Revenue from items sold:",
                   "Costs:",
                   "Total amount:",
                   commissionPartText,
                   payoutAmountInclComissionText,
                   payoutCommissionAmountText,
                   payoutAmountText
                );
        }

        return new(title,
                locationAndDateText,
                "Page {0} of {1}",
                "  - powered by https://github.com/braunau-mobil/velo-basar",
                PrintSettings.PageMargins,
                true,
                PrintSettings.BannerFilePath!,
                PrintSettings.BannerSubtitle,
                PrintSettings.Website,
                addressText,
                sellerIdText,
                settlementCommisionSummary,
                payoutTable,
                PrintSettings.Settlement.SoldTitle,
                pickupTable,
                PrintSettings.Settlement.NotSoldTitle,
                PrintSettings.Settlement.ConfirmationText,
                addBankingQrCode,
                PrintSettings.QrCodeLengthMillimeters,
                bankAccountHolder,
                iban,
                bankingQrCodeContent,
                "Signature ______________________________",
                signatureText
            );
    }
        
}
