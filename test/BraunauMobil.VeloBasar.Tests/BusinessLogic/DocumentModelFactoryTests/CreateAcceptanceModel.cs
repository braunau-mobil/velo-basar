using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentModelFactoryTests;

public class CreateAcceptanceModel
    : TestBase
{
    [Fact]
    public void NoSellerThrowsInvalidOperationException()
    {
        //  Arrange
        TransactionEntity acceptance = Fixture.BuildTransaction()
            .Without(transaction => transaction.Seller)
            .Create();

        //  Act
        Action act = () => Sut.CreateAcceptanceModel(acceptance);

        //  Assert
        act.Should().Throw<InvalidOperationException>().WithMessage("Cannot create Acceptance Document without Seller");
    }

    [Fact]
    public void NoStatusLink()
    {
        //  Arrange
        Settings.Acceptance.SignatureText = "SignatureText";
        Settings.Acceptance.StatusLinkFormat = null;

        BasarEntity basar = Fixture.Create<BasarEntity>();
        basar.Name = "4. Basar";
        basar.Location = "Wasserau";
        basar.Date = X.FirstContactDay;

        SellerEntity seller = Fixture.Create<SellerEntity>();
        seller.City = "Schlucht";
        seller.FirstName = "Hamfast";
        seller.LastName = "Gamdschie";
        seller.Street = "Schwertgasse 4";
        seller.ZIP = "A457857";

        TransactionEntity acceptance = Fixture.BuildTransaction(basar)
            .With(acceptance => acceptance.Seller, seller)
            .With(acceptance => acceptance.TimeStamp, X.FirstContactDay.AddHours(2))
            .Create();

        ProductEntity racingBike = Fixture.Create<ProductEntity>();
        racingBike.Id = 1;
        racingBike.Brand = "Nicolai";
        racingBike.Color = "Blue";
        racingBike.Description = "Is very very old.";
        racingBike.DonateIfNotSold = false;
        racingBike.FrameNumber = "123456";
        racingBike.Price = 12.34m;
        racingBike.TireSize = "29";
        racingBike.Type.Name = "Racing bike";
        acceptance.Products.Add(new ProductToTransactionEntity { Product = racingBike, Transaction = acceptance });

        ProductEntity cityBike = Fixture.Create<ProductEntity>();
        cityBike.Id =2;
        cityBike.Brand = "Wulfhorst";
        cityBike.Color = "Green-Yellow";
        cityBike.Description = "No brakes!";
        cityBike.DonateIfNotSold = true;
        cityBike.FrameNumber = "XYZ";
        cityBike.Price = 154.23m;
        cityBike.TireSize = "28";
        cityBike.Type.Name = "City bike";
        acceptance.Products.Add(new ProductToTransactionEntity { Product = cityBike, Transaction = acceptance });

        //  Act
        AcceptanceDocumentModel result = Sut.CreateAcceptanceModel(acceptance);

        //  Assert
        AcceptanceDocumentModel expectedModel = new(
            Settings.Acceptance.TitleFormat,
            "Wasserau, 05.04.2063",
            "VeloBasar_PageNumberFromOverall",
            "  - powered by https://github.com/braunau-mobil/velo-basar",
            Settings.PageMargins,
            Settings.Acceptance.SubTitle,
            "Hamfast Gamdschie".Line("Schwertgasse 4").Line("A457857 Schlucht").Line(),
            "VeloBasar_SellerIdShort_0",
            false,
            string.Empty,
            Settings.Acceptance.TokenTitle,
            seller.Token,
            "SignatureText ______________________________",
            "VeloBasar_AtLocationAndDateAndTime_Wasserau_05.04.2063 13:22:33",
            Settings.QrCodeLengthMillimeters,
            new ProductsTableDocumentModel(
                "VeloBasar_Id",
                "VeloBasar_ProductDescription",
                "VeloBasar_Size",
                "VeloBasar_Price",
                "VeloBasar_Sum:",
                "VeloBasar_ProductCounter_2",
                "€ 166,57",
                null,
                new[]
                {
                    new ProductTableRowDocumentModel("1", "Nicolai - Racing bike".Line("Is very very old.").Line(" Blue 123456"), "29", "€ 12,34", null),
                    new ProductTableRowDocumentModel("2", "Wulfhorst - City bike".Line("No brakes!").Line(" Green-Yellow XYZ").Line("VeloBasar_DonateIfNotSoldOnProductTable").Line(), "28", "€ 154,23", null),
                }
            )
        );
        result.Should().BeEquivalentTo(expectedModel);
    }

    [Fact]
    public void WithStatusLink()
    {
        //  Arrange
        Settings.Acceptance.SignatureText = "SignatureText";
        Settings.Acceptance.StatusLinkFormat = "StatusLinkFormat_{0}";

        BasarEntity basar = Fixture.Create<BasarEntity>();
        basar.Name = "4. Basar";
        basar.Location = "Wasserau";
        basar.Date = X.FirstContactDay;

        SellerEntity seller = Fixture.Create<SellerEntity>();
        seller.City = "Schlucht";
        seller.FirstName = "Hamfast";
        seller.LastName = "Gamdschie";
        seller.Street = "Schwertgasse 4";
        seller.Token = "X1234";
        seller.ZIP = "A457857";

        TransactionEntity acceptance = Fixture.BuildTransaction(basar)
            .With(acceptance => acceptance.Seller, seller)
            .With(acceptance => acceptance.TimeStamp, X.FirstContactDay.AddHours(2))
            .Create();

        ProductEntity racingBike = Fixture.Create<ProductEntity>();
        racingBike.Id = 1;
        racingBike.Brand = "Nicolai";
        racingBike.Color = "Blue";
        racingBike.Description = "Is very very old.";
        racingBike.DonateIfNotSold = false;
        racingBike.FrameNumber = "123456";
        racingBike.Price = 12.34m;
        racingBike.TireSize = "29";
        racingBike.Type.Name = "Racing bike";
        acceptance.Products.Add(new ProductToTransactionEntity { Product = racingBike, Transaction = acceptance });

        ProductEntity cityBike = Fixture.Create<ProductEntity>();
        cityBike.Id = 2;
        cityBike.Brand = "Wulfhorst";
        cityBike.Color = "Green-Yellow";
        cityBike.Description = "No brakes!";
        cityBike.DonateIfNotSold = true;
        cityBike.FrameNumber = "XYZ";
        cityBike.Price = 154.23m;
        cityBike.TireSize = "28";
        cityBike.Type.Name = "City bike";
        acceptance.Products.Add(new ProductToTransactionEntity { Product = cityBike, Transaction = acceptance });

        //  Act
        AcceptanceDocumentModel result = Sut.CreateAcceptanceModel(acceptance);

        //  Assert
        AcceptanceDocumentModel expectedModel = new(
            Settings.Acceptance.TitleFormat,
            "Wasserau, 05.04.2063",
            "VeloBasar_PageNumberFromOverall",
            "  - powered by https://github.com/braunau-mobil/velo-basar",
            Settings.PageMargins,
            Settings.Acceptance.SubTitle,
            "Hamfast Gamdschie".Line("Schwertgasse 4").Line("A457857 Schlucht").Line(""),
            "VeloBasar_SellerIdShort_0",
            true,
            "StatusLinkFormat_X1234",
            Settings.Acceptance.TokenTitle,
            seller.Token,
            "SignatureText ______________________________",
            "VeloBasar_AtLocationAndDateAndTime_Wasserau_05.04.2063 13:22:33",
            Settings.QrCodeLengthMillimeters,
            new ProductsTableDocumentModel(
                "VeloBasar_Id",
                "VeloBasar_ProductDescription",
                "VeloBasar_Size",
                "VeloBasar_Price",
                "VeloBasar_Sum:",
                "VeloBasar_ProductCounter_2",
                "€ 166,57",
                null,
                new[]
                {
                    new ProductTableRowDocumentModel("1", "Nicolai - Racing bike".Line("Is very very old.").Line(" Blue 123456"), "29", "€ 12,34", null),
                    new ProductTableRowDocumentModel("2", "Wulfhorst - City bike".Line("No brakes!").Line(" Green-Yellow XYZ").Line("VeloBasar_DonateIfNotSoldOnProductTable").Line(), "28", "€ 154,23", null),
                }
            )
        );
        result.Should().BeEquivalentTo(expectedModel);
    }
}
