using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentModelFactoryTests;

public class CreateAcceptanceModel
    : TestBase
{
#warning @todo NEXT: With status link, and throw exception

    [Fact]
    public void NoStatusLink()
    {
        //  Arrange
        Settings.Acceptance.SignatureText = "Unterschrift";
        Settings.Acceptance.StatusLinkFormat = null;

        BasarEntity basar = Fixture.BuildBasar()
            .With(basar => basar.Name, "4. Basar")
            .With(basar => basar.Location, "Wasserau")
            .With(basar => basar.Date, X.FirstContactDay)
            .Create();

        SellerEntity seller = Fixture.BuildSeller()
            .With(seller => seller.City, "Hobbingen")
            .With(seller => seller.FirstName, "Hamfast")
            .With(seller => seller.LastName, "Gamdschie")
            .With(seller => seller.Street, "Beutelsend 4")
            .With(seller => seller.ZIP, "A457857")
            .Create();

        TransactionEntity acceptance = Fixture.BuildTransaction(basar)
            .With(acceptance => acceptance.Seller, seller)
            .With(acceptance => acceptance.TimeStamp, X.FirstContactDay.AddHours(2))
            .Create();
        
        ProductTypeEntity rennradType = Fixture.BuildProductType()
            .With(type => type.Name, "Rennrad")
            .Create();
        ProductEntity rennrad = Fixture.BuildProduct()
            .With(product => product.Id, 1)
            .With(product => product.Brand, "Harlond Räder")
            .With(product => product.Color, "blau")
            .With(product => product.Description, "Mein tolles Rennrad")
            .With(product => product.DonateIfNotSold, false)
            .With(product => product.FrameNumber, "123456")
            .With(product => product.Price, 12.34m)
            .With(product => product.TireSize, "29")
            .With(product => product.Type, rennradType)
            .Create();
        acceptance.Products.Add(new ProductToTransactionEntity { Product = rennrad, Transaction = acceptance });

        ProductTypeEntity cityBikeType = Fixture.BuildProductType()
            .With(type => type.Name, "City Bike")
            .Create();
        ProductEntity cityBike = Fixture.BuildProduct()
            .With(product => product.Id, 2)
            .With(product => product.Brand, "GCB")
            .With(product => product.Color, "grün")
            .With(product => product.Description, "Das schönste City Bike")
            .With(product => product.DonateIfNotSold, true)
            .With(product => product.FrameNumber, "XYZ")
            .With(product => product.Price, 154.23m)
            .With(product => product.TireSize, "28")
            .With(product => product.Type, cityBikeType)
            .Create();
        acceptance.Products.Add(new ProductToTransactionEntity { Product = cityBike, Transaction = acceptance });

        //  Act
        AcceptanceDocumentModel result = Sut.CreateAcceptanceModel(acceptance);

        //  Assert
        AcceptanceDocumentModel expectedModel = new (
            Settings.Acceptance.TitleFormat,
            "Wasserau, 05.04.2063",
            "VeloBasar_PageNumberFromOverall",
            "  - powered by https://github.com/braunau-mobil/velo-basar",
            Settings.PageMargins,
            Settings.Acceptance.SubTitle,
            "Hamfast Gamdschie\r\nBeutelsend 4\r\nA457857 Hobbingen\r\n",
            "VeloBasar_SellerIdShort_0",
            false,
            string.Empty,
            Settings.Acceptance.TokenTitle,
            seller.Token,
            "Unterschrift ______________________________",
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
                    new ProductTableRowDocumentModel("1", "Harlond Räder - Rennrad\r\nMein tolles Rennrad\r\n blau 123456", "29", "€ 12,34", null),
                    new ProductTableRowDocumentModel("2", "GCB - City Bike\r\nDas schönste City Bike\r\n grün XYZ\r\nVeloBasar_DonateIfNotSoldOnProductTable\r\n", "28", "€ 154,23", null),
                }
            )
        );
        result.Should().BeEquivalentTo(expectedModel);
    }
}
