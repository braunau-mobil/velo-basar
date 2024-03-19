using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentModelFactoryTests;

public class CreateSettlementModel
    : TestBase
{
    [Fact]
    public void NoSellerThrowsInvalidOperationException()
    {
        //  Arrange
        TransactionEntity settlement = Fixture.BuildTransaction()
            .Without(transaction => transaction.Seller)
            .Create();

        //  Act
        Action act = () => Sut.CreateSettlementModel(settlement);

        //  Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot create Settlement Document without Seller");
    }

    [Theory]
    [InlineData(false, "", "", false)]
    [InlineData(true, "BannerFilePath", "BannerFilePath", true)]
    public void WithPayoutProductsAndPickupProducts(bool useBannerFile, string bannerFilePath, string expectedBannerFilePath, bool expectedAddBanner)
    {
        //  Arrange
        Settings.Settlement.SignatureText = "SignatureText";
        Settings.UseBannerFile = useBannerFile;
        Settings.BannerFilePath = bannerFilePath;
        Settings.Settlement.BankTransactionTextFormat = "BankTransactionTextFormat_{0}";

        BasarEntity basar = Fixture.Create<BasarEntity>();
        basar.Name = "6. Basar";
        basar.Location = "Minas Morgul";
        basar.Date = X.FirstContactDay;
        basar.ProductCommission = 0.2m;

        SellerEntity seller = Fixture.Create<SellerEntity>();
        seller.Id = 55;
        seller.IBAN = "AT395400076812528547";
        seller.City = "Bree";
        seller.FirstName = "Ohtar";
        seller.LastName = "Borthand";
        seller.Street = "Heßgasse 13a";
        seller.ZIP = "X45ZG";

        TransactionEntity settlement = Fixture.BuildTransaction(basar)
            .With(settlement => settlement.Type, TransactionType.Settlement)
            .With(settlement => settlement.Seller, seller)
            .With(settlement => settlement.TimeStamp, X.FirstContactDay.AddHours(2))
            .Create();

        ProductEntity childrensBike = Fixture.Create<ProductEntity>();
        childrensBike.Id = 1;
        childrensBike.Brand = "Puki";
        childrensBike.Color = "Yellow";
        childrensBike.Description = "For ages 2-4";
        childrensBike.DonateIfNotSold = false;
        childrensBike.FrameNumber = "HG679f";
        childrensBike.Price = 64.28m;
        childrensBike.TireSize = "12";
        childrensBike.Type.Name = "Childrens Bike";
        childrensBike.ValueState = ValueState.Settled;
        childrensBike.StorageState = StorageState.Sold;
        settlement.Products.Add(new ProductToTransactionEntity { Product = childrensBike, Transaction = settlement });

        ProductEntity cargoBike = Fixture.Create<ProductEntity>();
        cargoBike.Id = 2;
        cargoBike.Brand = "Cube";
        cargoBike.Color = "Black";
        cargoBike.Description = "Raincover";
        cargoBike.DonateIfNotSold = false;
        cargoBike.FrameNumber = "FM55434123";
        cargoBike.Price = 4312.5m;
        cargoBike.TireSize = "25";
        cargoBike.Type.Name = "Cargo Bike";
        cargoBike.ValueState = ValueState.Settled;
        cargoBike.StorageState = StorageState.Available;
        settlement.Products.Add(new ProductToTransactionEntity { Product = cargoBike, Transaction = settlement });

        ProductEntity cityBike = Fixture.Create<ProductEntity>();
        cityBike.Id = 3;
        cityBike.Brand = "KTM";
        cityBike.Color = "Magenta";
        cityBike.Description = "No saddle";
        cityBike.DonateIfNotSold = true;
        cityBike.FrameNumber = "FM585858";
        cityBike.Price = 73.5m;
        cityBike.TireSize = "26.5";
        cityBike.Type.Name = "City-Bike";
        cityBike.ValueState = ValueState.Settled;
        cityBike.StorageState = StorageState.Lost;
        settlement.Products.Add(new ProductToTransactionEntity { Product = cityBike, Transaction = settlement });

        ProductEntity womanCityBike = Fixture.Create<ProductEntity>();
        womanCityBike.Id = 4;
        womanCityBike.Brand = "Kettler";
        womanCityBike.Color = "Green";
        womanCityBike.Description = "No tires!";
        womanCityBike.DonateIfNotSold = false;
        womanCityBike.FrameNumber = "FMdf0945r";
        womanCityBike.Price = 106.66m;
        womanCityBike.TireSize = "25";
        womanCityBike.Type.Name = "Woman City-Bike";
        womanCityBike.ValueState = ValueState.Settled;
        womanCityBike.StorageState = StorageState.Locked;
        settlement.Products.Add(new ProductToTransactionEntity { Product = womanCityBike, Transaction = settlement });

        //  Act
        SettlementDocumentModel result = Sut.CreateSettlementModel(settlement);

        //  Assert
        SettlementDocumentModel expectedModel = new(
            Settings.Settlement.TitleFormat,
            "Minas Morgul, 05.04.2063",
            "VeloBasar_PageNumberFromOverall",
            "  - powered by https://github.com/braunau-mobil/velo-basar",
            Settings.PageMargins,
            expectedAddBanner,
            expectedBannerFilePath,
            Settings.BannerSubtitle,
            Settings.Website,
            "Ohtar Borthand".Line("Heßgasse 13a").Line("X45ZG Bree").Line(),
            "VeloBasar_SellerIdShort_55",
            new SettlementCommisionSummaryModel(
                "VeloBasar_IncomeFromSoldProducts",
                "VeloBasar_Cost:",
                "VeloBasar_TotalAmount:",
                "VeloBasar_SalesCommision_0,2_137,78",
                "€ 137,78",
                "€ 27,56",
                "€ 110,22"
            ),
            new ProductsTableDocumentModel(
                "VeloBasar_Id",
                "VeloBasar_ProductDescription",
                "VeloBasar_Size",
                "VeloBasar_SellingPrice",
                "VeloBasar_Sum:",
                "VeloBasar_ProductCounter_2",
                "€ 137,78",
                null,
                new[]
                {
                    new ProductTableRowDocumentModel("1", "Puki - Childrens Bike".Line("For ages 2-4").Line(" Yellow HG679f"), "12", "€ 64,28", null),
                    new ProductTableRowDocumentModel("3", "KTM - City-Bike".Line("No saddle").Line(" Magenta FM585858"), "26.5", "€ 73,50", null),
                }
            ),
            Settings.Settlement.SoldTitle,
            new ProductsTableDocumentModel(
                "VeloBasar_Id",
                "VeloBasar_ProductDescription",
                "VeloBasar_Size",
                "VeloBasar_Price",
                "VeloBasar_Sum:",
                "VeloBasar_ProductCounter_2",
                "€ 4.419,16",
                null,
                new[]
                {
                    new ProductTableRowDocumentModel("2", "Cube - Cargo Bike".Line("Raincover").Line(" Black FM55434123"), "25", "€ 4.312,50", null),
                    new ProductTableRowDocumentModel("4", "Kettler - Woman City-Bike".Line("No tires!").Line(" Green FMdf0945r"), "25", "€ 106,66", null),
                }
            ),
            Settings.Settlement.NotSoldTitle,
            Settings.Settlement.ConfirmationText,
            false,
            Settings.QrCodeLengthMillimeters,
            seller.EffectiveBankAccountHolder,
            "AT39 5400 0768 1252 8547",
            "BCD".Line("002").Line("1").Line("SCT").Line().Line(seller.EffectiveBankAccountHolder).Line("AT395400076812528547").Line("EUR110.22").Line().Line().Line("BankTransactionTextFormat_6. Basar").Line().Line(),
            "SignatureText ______________________________",
            "VeloBasar_AtLocationAndDateAndTime_Minas Morgul_05.04.2063 13:22:33"
        );
        result.Should().BeEquivalentTo(expectedModel);
    }

    [Theory]
    [InlineData(false, "", "", false)]
    [InlineData(true, "BannerFilePath", "BannerFilePath", true)]
    public void OnlyPayoutProducts(bool useBannerFile, string bannerFilePath, string expectedBannerFilePath, bool expectedAddBanner)
    {
        //  Arrange
        Settings.Settlement.SignatureText = "SignatureText";
        Settings.UseBannerFile = useBannerFile;
        Settings.BannerFilePath = bannerFilePath;
        Settings.Settlement.BankTransactionTextFormat = "BankTransactionTextFormat_{0}";

        BasarEntity basar = Fixture.Create<BasarEntity>();
        basar.Name = "6. Basar";
        basar.Location = "Minas Morgul";
        basar.Date = X.FirstContactDay;
        basar.ProductCommission = 0.2m;

        SellerEntity seller = Fixture.Create<SellerEntity>();
        seller.Id = 55;
        seller.IBAN = "AT395400076812528547";
        seller.City = "Bree";
        seller.FirstName = "Ohtar";
        seller.LastName = "Borthand";
        seller.Street = "Heßgasse 13a";
        seller.ZIP = "X45ZG";

        TransactionEntity settlement = Fixture.BuildTransaction(basar)
            .With(settlement => settlement.Type, TransactionType.Settlement)
            .With(settlement => settlement.Seller, seller)
            .With(settlement => settlement.TimeStamp, X.FirstContactDay.AddHours(2))
            .Create();

        ProductEntity childrensBike = Fixture.Create<ProductEntity>();
        childrensBike.Id = 1;
        childrensBike.Brand = "Puki";
        childrensBike.Color = "Yellow";
        childrensBike.Description = "For ages 2-4";
        childrensBike.DonateIfNotSold = false;
        childrensBike.FrameNumber = "HG679f";
        childrensBike.Price = 64.28m;
        childrensBike.TireSize = "12";
        childrensBike.Type.Name = "Childrens Bike";
        childrensBike.ValueState = ValueState.Settled;
        childrensBike.StorageState = StorageState.Sold;
        settlement.Products.Add(new ProductToTransactionEntity { Product = childrensBike, Transaction = settlement });

        //  Act
        SettlementDocumentModel result = Sut.CreateSettlementModel(settlement);

        //  Assert
        SettlementDocumentModel expectedModel = new(
            Settings.Settlement.TitleFormat,
            "Minas Morgul, 05.04.2063",
            "VeloBasar_PageNumberFromOverall",
            "  - powered by https://github.com/braunau-mobil/velo-basar",
            Settings.PageMargins,
            expectedAddBanner,
            expectedBannerFilePath,
            Settings.BannerSubtitle,
            Settings.Website,
            "Ohtar Borthand".Line("Heßgasse 13a").Line("X45ZG Bree").Line(),
            "VeloBasar_SellerIdShort_55",
            new SettlementCommisionSummaryModel(
                "VeloBasar_IncomeFromSoldProducts",
                "VeloBasar_Cost:",
                "VeloBasar_TotalAmount:",
                "VeloBasar_SalesCommision_0,2_64,28",
                "€ 64,28",
                "€ 12,86",
                "€ 51,42"
            ),
            new ProductsTableDocumentModel(
                "VeloBasar_Id",
                "VeloBasar_ProductDescription",
                "VeloBasar_Size",
                "VeloBasar_SellingPrice",
                "VeloBasar_Sum:",
                "VeloBasar_ProductCounter_1",
                "€ 64,28",
                null,
                new[]
                {
                    new ProductTableRowDocumentModel("1", "Puki - Childrens Bike".Line("For ages 2-4").Line(" Yellow HG679f"), "12", "€ 64,28", null)
                }
            ),
            Settings.Settlement.SoldTitle,
            null,
            Settings.Settlement.NotSoldTitle,
            Settings.Settlement.ConfirmationText,
            true,
            Settings.QrCodeLengthMillimeters,
            seller.EffectiveBankAccountHolder,
            "AT39 5400 0768 1252 8547",
            "BCD".Line("002").Line("1").Line("SCT").Line().Line(seller.EffectiveBankAccountHolder).Line("AT395400076812528547").Line("EUR51.42").Line().Line().Line("BankTransactionTextFormat_6. Basar").Line().Line(),
            "SignatureText ______________________________",
            "VeloBasar_AtLocationAndDateAndTime_Minas Morgul_05.04.2063 13:22:33"
        );
        result.Should().BeEquivalentTo(expectedModel);
    }

    [Theory]
    [InlineData(false, "", "", false)]
    [InlineData(true, "BannerFilePath", "BannerFilePath", true)]
    public void WithoutProducts(bool useBannerFile, string bannerFilePath, string expectedBannerFilePath, bool expectedAddBanner)
    {
        //  Arrange
        Settings.Settlement.SignatureText = "SignatureText";
        Settings.UseBannerFile = useBannerFile;
        Settings.BannerFilePath = bannerFilePath;
        Settings.Settlement.BankTransactionTextFormat = "BankTransactionTextFormat_{0}";

        BasarEntity basar = Fixture.Create<BasarEntity>();
        basar.Name = "6. Basar";
        basar.Location = "Minas Morgul";
        basar.Date = X.FirstContactDay;
        basar.ProductCommission = 0.2m;

        SellerEntity seller = Fixture.Create<SellerEntity>();
        seller.Id = 55;
        seller.IBAN = "AT395400076812528547";
        seller.City = "Bree";
        seller.FirstName = "Ohtar";
        seller.LastName = "Borthand";
        seller.Street = "Heßgasse 13a";
        seller.ZIP = "X45ZG";

        TransactionEntity settlement = Fixture.BuildTransaction(basar)
            .With(settlement => settlement.Type, TransactionType.Settlement)
            .With(settlement => settlement.Seller, seller)
            .With(settlement => settlement.TimeStamp, X.FirstContactDay.AddHours(2))
            .Create();

        //  Act
        SettlementDocumentModel result = Sut.CreateSettlementModel(settlement);

        //  Assert
        SettlementDocumentModel expectedModel = new(
            Settings.Settlement.TitleFormat,
            "Minas Morgul, 05.04.2063",
            "VeloBasar_PageNumberFromOverall",
            "  - powered by https://github.com/braunau-mobil/velo-basar",
            Settings.PageMargins,
            expectedAddBanner,
            expectedBannerFilePath,
            Settings.BannerSubtitle,
            Settings.Website,
            "Ohtar Borthand".Line("Heßgasse 13a").Line("X45ZG Bree").Line(),
            "VeloBasar_SellerIdShort_55",
            null,
            null,
            Settings.Settlement.SoldTitle,
            null,
            Settings.Settlement.NotSoldTitle,
            Settings.Settlement.ConfirmationText,
            false,
            Settings.QrCodeLengthMillimeters,
            seller.EffectiveBankAccountHolder,
            "AT39 5400 0768 1252 8547",
            "BCD".Line("002").Line("1").Line("SCT").Line().Line(seller.EffectiveBankAccountHolder).Line("AT395400076812528547").Line("EUR").Line().Line().Line("BankTransactionTextFormat_6. Basar").Line().Line(),
            "SignatureText ______________________________",
            "VeloBasar_AtLocationAndDateAndTime_Minas Morgul_05.04.2063 13:22:33"
        );
        result.Should().BeEquivalentTo(expectedModel);
    }
}
