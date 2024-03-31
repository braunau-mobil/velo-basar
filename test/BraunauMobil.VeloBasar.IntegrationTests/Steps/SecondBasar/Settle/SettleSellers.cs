using BraunauMobil.VeloBasar.IntegrationTests;

namespace BraunauMobil.VeloBasar.IntegrationTests.Steps.SecondBasar.Settle;

public class SettleSellers(TestContext context)
{
    public async Task Run()
    {
        await SettleSeller(ID.Sellers.MeneldorBorondir, "Seller #2 Meneldor Borondir - Velo Basar", "Settlement #1 - Velo Basar", context.SettlementDocument(
            "XYZ - Second Bazaar : Settlement #1",
            "Thal, 6/4/2064",
            "Meneldor Borondir".Line("Heßgasse 10").Line("2828 Bree").Line(),
            "Seller.-ID: 2",
            "Sales commission (10.00% of $120.60):",
            "$120.60",
            "$12.06",
            "$108.54",
            "2 Product",
            "$120.60",
            [
                new ProductTableRowDocumentModel("3", "Additive - Scooter".Line("X45").Line(" blue X290jbgn"), "16", "$51.06", null),
                new ProductTableRowDocumentModel("5", "Draisin - E-bike".Line("DR-F5").Line(" white"), "23", "$69.54", null)
            ],
            "2 Product",
            "$163.23",
            [
                new ProductTableRowDocumentModel("4", "Toxy - Scooter".Line("TY 66-17").Line(" white"), "17", "$45.75", null),
                new ProductTableRowDocumentModel("6", "Cyclecraft - Road bike".Line("No lights, brakes are fine").Line(" yellow"), "28", "$117.48", null)
            ],
            false,
            "Meneldor Borondir",
            "",
            context.BankingQrCode("Meneldor Borondir", "EUR108.54", "XYZ - Revenue Second Bazaar"),
            "Thal on Tuesday, May 6, 2064 at 12:23 PM"
            )
        );

        await SettleSeller(ID.Sellers.AnsenGróin, "Seller #8 Ansen Gróin - Velo Basar", "Settlement #2 - Velo Basar", context.SettlementDocument(
            "XYZ - Second Bazaar : Settlement #2",
            "Thal, 6/4/2064",
            "Ansen Gróin".Line("Liebiggasse 3").Line("4579 Tuckbergen").Line(),
            "Seller.-ID: 8",
            "1 Product",
            "$75.29",
            [
                new ProductTableRowDocumentModel("15", "Nishiki - Scooter".Line("NISHIKI_266634").Line(" peach"), "14", "$75.29", null),
            ],
            false,
            "Ansen A.G. Gróin",
            "AT06 2011 1979 1882 1983",
            context.BankingQrCode("Ansen A.G. Gróin", "AT062011197918821983", "EUR", "XYZ - Revenue Second Bazaar"),
            "Thal on Tuesday, May 6, 2064 at 12:23 PM"
            )
        );

        await SettleSeller(ID.Sellers.AmrothGerstenmann, "Seller #3 Amroth Gerstenmann - Velo Basar", "Settlement #3 - Velo Basar", context.SettlementDocument(
            "XYZ - Second Bazaar : Settlement #3",
            "Thal, 6/4/2064",
            "Amroth Gerstenmann".Line("Concordiaplatz 48").Line("7872 Dwollingen").Line(),
            "Seller.-ID: 3",
            false,
            "Amroth Gerstenmann",
            "",
            context.BankingQrCode("Amroth Gerstenmann", "EUR", "XYZ - Revenue Second Bazaar"),
            "Thal on Tuesday, May 6, 2064 at 12:23 PM"
            )
        );

        await SettleSeller(ID.Sellers.LanghöhlenSiriondil, "Seller #4 Langhöhlen Siriondil - Velo Basar", "Settlement #4 - Velo Basar", context.SettlementDocument(
            "XYZ - Second Bazaar : Settlement #4",
            "Thal, 6/4/2064",
            "Langhöhlen Siriondil".Line("Börseplatz 11").Line("2178 Nargothrond").Line(),
            "Seller.-ID: 4",
            "Sales commission (10.00% of $69.54):",
            "$69.54",
            "$6.95",
            "$62.59",
            "1 Product",
            "$69.54",
            [
                new ProductTableRowDocumentModel("7", "Epple - Woman's city bike".Line("No tires").Line(" white G#%$BIBM#$)"), "22", "$69.54", null),
            ],
            "1 Product",
            "$82.79",
            [
                new ProductTableRowDocumentModel("13", "Subtil Bikes - Unicycle".Line("SUBTIL BIKES_963431").Line(" brown d3b90198"), "24", "$82.79", null),
            ],
            false,
            "Langhöhlen Siriondil",
            "",
            context.BankingQrCode("Langhöhlen Siriondil", "EUR62.59", "XYZ - Revenue Second Bazaar"),
            "Thal on Tuesday, May 6, 2064 at 12:23 PM"
            )
        );

        await SettleSeller(ID.Sellers.FrórBilbo, "Seller #5 Frór Bilbo - Velo Basar", "Settlement #5 - Velo Basar", context.SettlementDocument(
            "XYZ - Second Bazaar : Settlement #5",
            "Thal, 6/4/2064",
            "Frór Bilbo".Line("Helmut-Zilk-Platz 27").Line("8475 Andúnië").Line(),
            "Seller.-ID: 5",
            "Sales commission (10.00% of $221.11):",
            "$221.11",
            "$22.11",
            "$199.00",
            "2 Product",
            "$221.11",
            [
                new ProductTableRowDocumentModel("10", "Seidel & Naumann - Unicycle".Line("SALIKO_52513").Line(" lavender"), "14", "$106.75", null),
                new ProductTableRowDocumentModel("11", "Leiba - Woman's city bike".Line("MIELE_398047").Line(" maroon 1b26-4d44-94fe-027810ef43e7"), "17", "$114.36", null)
            ],
            "2 Product",
            "$334.87",
            [
                new ProductTableRowDocumentModel("8", "Pedalpower - Steel steed".Line("VOSS SPEZIALRAD_465693").Line(" gray 95f7-4ba0-94b6-6c45a1cd0913"), "16", "$151.34", null),
                new ProductTableRowDocumentModel("9", "Egon Rahe - Men's city bike".Line("VELOMOBILES_92370").Line(" maroon"), "20", "$183.53", null)
            ],
            false,
            "Frór F.B. Bilbo",
            "",
            context.BankingQrCode("Frór F.B. Bilbo", "EUR199", "XYZ - Revenue Second Bazaar"),
            "Thal on Tuesday, May 6, 2064 at 12:23 PM"
            )
        );

        await SettleSeller(ID.Sellers.MallorFimbrethil, "Seller #9 Mallor Fimbrethil - Velo Basar", "Settlement #6 - Velo Basar", context.SettlementDocument(
            "XYZ - Second Bazaar : Settlement #6",
            "Thal, 6/4/2064",
            "Mallor Fimbrethil".Line("Schönlaterngasse 16").Line("2758 Edhellond").Line(),
            "Seller.-ID: 9",
            "1 Product",
            "$8.49",
            [
                new ProductTableRowDocumentModel("16", "Idworx - Road bike".Line("IDWORX_768016").Line(" orange 12a2dc85-"), "24", "$8.49", null),
            ],
            false,
            "Mallor M.F. Fimbrethil",
            "",
            context.BankingQrCode("Mallor M.F. Fimbrethil", "EUR", "XYZ - Revenue Second Bazaar"),
            "Thal on Tuesday, May 6, 2064 at 12:23 PM"
            )
        );

        await SettleSeller(ID.Sellers.ChicaCiryatur, "Seller #6 Chica Ciryatur - Velo Basar", "Settlement #7 - Velo Basar", context.SettlementDocument(
            "XYZ - Second Bazaar : Settlement #7",
            "Thal, 6/4/2064",
            "Chica Ciryatur".Line("Tiefer Graben 6").Line("7332 Avallóne").Line(),
            "Seller.-ID: 6",
            "Sales commission (10.00% of $143.64):",
            "$143.64",
            "$14.36",
            "$129.28",
            "1 Product",
            "$143.64",
            [
                new ProductTableRowDocumentModel("12", "Indienrad - Children's bike".Line("INDIENRAD_246011").Line(" brown"), "26", "$143.64", null),
            ],
            true,
            "Chica C.C. Ciryatur",
            "AT38 1400 0163 1454 4716",
            context.BankingQrCode("Chica C.C. Ciryatur", "AT381400016314544716", "EUR129.28", "XYZ - Revenue Second Bazaar"),
            "Thal on Tuesday, May 6, 2064 at 12:23 PM"
            )
        );

        await SettleSeller(ID.Sellers.FolcwineGollum, "Seller #7 Folcwine Gollum - Velo Basar", "Settlement #8 - Velo Basar", context.SettlementDocument(
            "XYZ - Second Bazaar : Settlement #8",
            "Thal, 6/4/2064",
            "Folcwine Gollum".Line("Domgasse 25").Line("7356 Unterharg").Line(),
            "Seller.-ID: 7",
            "1 Product",
            "$149.87",
            [
                new ProductTableRowDocumentModel("14", "Univega - Men's city bike".Line("UNIVEGA_749336").Line(" slate 3eb2377a-"), "26", "$149.87", null)
            ],
            false,
            "Folcwine F.G. Gollum",
            "",
            context.BankingQrCode("Folcwine F.G. Gollum", "EUR", "XYZ - Revenue Second Bazaar"),
            "Thal on Tuesday, May 6, 2064 at 12:23 PM"
            )
        );

        await AssertBasarDetails();
    }

    private async Task AssertBasarDetails()
    {
        BasarSettlementStatus basarSettlementStatus = new(true, 0, 0, 0);
        BasarDetailsModel expectedDetails = new(new BasarEntity(), basarSettlementStatus)
        {
            AcceptanceCount = 9,
            AcceptedProductsAmount = 1369.43M,
            AcceptedProductsCount = 14,
            AcceptedProductTypesByAmount = [
                new ChartDataPoint(172.10M, "Scooter", X.AnyColor),
                new ChartDataPoint(69.54M, "E-bike", X.AnyColor),
                new ChartDataPoint(125.97M, "Road bike", X.AnyColor),
                new ChartDataPoint(151.34M, "Steel steed", X.AnyColor),
                new ChartDataPoint(333.40M, "Men's city bike", X.AnyColor),
                new ChartDataPoint(183.90M, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(189.54M, "Unicycle", X.AnyColor),
                new ChartDataPoint(143.64M, "Children's bike", X.AnyColor),
            ],
            AcceptedProductTypesByCount = [
                new ChartDataPoint(3, "Scooter", X.AnyColor),
                new ChartDataPoint(1, "E-bike", X.AnyColor),
                new ChartDataPoint(2, "Road bike", X.AnyColor),
                new ChartDataPoint(1, "Steel steed", X.AnyColor),
                new ChartDataPoint(2, "Men's city bike", X.AnyColor),
                new ChartDataPoint(2, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(2, "Unicycle", X.AnyColor),
                new ChartDataPoint(1, "Children's bike", X.AnyColor),
            ],
            LockedProductsCount = 0,
            LostProductsCount = 0,
            PriceDistribution = context.PriceDistribtion(1, 1, 5, 7, 0),
            SaleCount = 6,
            SaleDistribution = [
                new ChartDataPoint(554.89M, "12:23 PM", X.AnyColor),
            ],
            SellerCount = 7,
            SoldProductsAmount = 554.89M,
            SoldProductsCount = 6,
            SoldProductTypesByAmount = [
                new ChartDataPoint(51.06M, "Scooter", X.AnyColor),
                new ChartDataPoint(69.54M, "E-bike", X.AnyColor),
                new ChartDataPoint(183.90M, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(106.75M, "Unicycle", X.AnyColor),
                new ChartDataPoint(143.64M, "Children's bike", X.AnyColor),
            ],
            SoldProductTypesByCount = [
                new ChartDataPoint(1, "Scooter", X.AnyColor),
                new ChartDataPoint(1, "E-bike", X.AnyColor),
                new ChartDataPoint(2, "Woman's city bike", X.AnyColor),
                new ChartDataPoint(1, "Unicycle", X.AnyColor),
                new ChartDataPoint(1, "Children's bike", X.AnyColor),
            ],
        };

        await context.AssertBasarDetails(ID.SecondBasar, expectedDetails);
    }

    private async Task SettleSeller(int sellerId, string expectedDetailsTitle, string expectedSuccessTitle, SettlementDocumentModel expectedDocument)
    {
        IHtmlDocument sellerDetailsDocument = await GetSellerDetails(sellerId);
        sellerDetailsDocument.Title.Should().Be(expectedDetailsTitle);

        IHtmlAnchorElement settleAnchor = sellerDetailsDocument.QueryAnchorByText("Settle");

        IHtmlDocument successDocument = await context.HttpClient.GetDocumentAsync(settleAnchor.Href);
        successDocument.Title.Should().Be(expectedSuccessTitle);

        IHtmlAnchorElement voucherAnchor = successDocument.QueryAnchorByText("Voucher");
        SettlementDocumentModel document = await context.HttpClient.GetSettlementDocumentAsync(voucherAnchor.Href);
        document.Should().BeEquivalentTo(expectedDocument);
    }

    private async Task<IHtmlDocument> GetSellerDetails(int sellerId)
    {
        IHtmlDocument sellerListDocument = await context.HttpClient.NavigateMenuAsync("Seller");
        IHtmlFormElement form = sellerListDocument.QueryForm();
        IHtmlButtonElement searchButton = sellerListDocument.QueryButtonByText("Search");

        return await context.HttpClient.SendFormAsync(form, searchButton, new Dictionary<string, object>
        {
            { "SearchString", sellerId }
        });
    }
}
