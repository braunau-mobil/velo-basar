using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentModelFactoryTests;

public class CreateSaleModel
    : TestBase
{
    [Theory]
    [InlineData(false, "", "", false)]
    [InlineData(true, "MyBannerFilePath", "MyBannerFilePath", true)]
    public void CheckModel(bool useBannerFile, string bannerFilePath, string expectedBannerFilePath, bool expectedAddBanner)
    {
        //  Arrange
        Settings.UseBannerFile = useBannerFile;
        Settings.BannerFilePath = bannerFilePath;

        BasarEntity basar = Fixture.Create<BasarEntity>();
        basar.Name = "3. Basar";
        basar.Location = "Minas Tirith";
        basar.Date = X.FirstContactDay;

        SellerEntity seller = Fixture.Create<SellerEntity>();
        seller.City = "Hobbingen";
        seller.FirstName = "Hamfast";
        seller.LastName = "Gamdschie";
        seller.Street = "Beutelsend 4";
        seller.ZIP = "A457857";

        TransactionEntity acceptance = Fixture.BuildTransaction(basar)
            .With(acceptance => acceptance.Seller, seller)
            .With(acceptance => acceptance.TimeStamp, X.FirstContactDay.AddHours(2))
            .Create();

        ProductEntity cargoBike = Fixture.Create<ProductEntity>();
        cargoBike.Id = 1;
        cargoBike.Brand = "Riese und Müller";
        cargoBike.Color = "Black/Green";
        cargoBike.Description = "My great cargo bike";
        cargoBike.DonateIfNotSold = false;
        cargoBike.FrameNumber = "123456";
        cargoBike.Price = 18.68m;
        cargoBike.TireSize = "29";
        cargoBike.Type.Name = "Cargo Bike";
        cargoBike.Session.Seller.City = "Gondolin";
        cargoBike.Session.Seller.FirstName = "Asfaloth";
        cargoBike.Session.Seller.LastName = "Glaurung";
        cargoBike.Session.Seller.Street = "Morzinplatz 44";
        cargoBike.Session.Seller.ZIP = "555AA";
        acceptance.Products.Add(new ProductToTransactionEntity { Product = cargoBike, Transaction = acceptance });

        ProductEntity cityBike = Fixture.Create<ProductEntity>();
        cityBike.Id = 2;
        cityBike.Brand = "Pepper";
        cityBike.Color = "Red";
        cityBike.Description = "No lights";
        cityBike.DonateIfNotSold = true;
        cityBike.FrameNumber = "XYZ";
        cityBike.Price = 89.32m;
        cityBike.TireSize = "28";
        cityBike.Type.Name = "City-Bike";
        cityBike.Session.Seller.City = "Ost-in-Edhil";
        cityBike.Session.Seller.FirstName = "Gilfanon";
        cityBike.Session.Seller.LastName = "Tareg";
        cityBike.Session.Seller.Street = "Essiggasse 6";
        cityBike.Session.Seller.ZIP = "XYF4";
        acceptance.Products.Add(new ProductToTransactionEntity { Product = cityBike, Transaction = acceptance });

        //  Act
        SaleDocumentModel result = Sut.CreateSaleModel(acceptance);

        //  Assert
        SaleDocumentModel expectedModel = new(
            Settings.Sale.TitleFormat,
            "Minas Tirith, 05.04.2063",
            "VeloBasar_PageNumberFromOverall",
            "  - powered by https://github.com/braunau-mobil/velo-basar",
            Settings.PageMargins,
            Settings.Sale.SubTitle,
            expectedAddBanner,
            expectedBannerFilePath,
            Settings.BannerSubtitle,
            Settings.Website,
            Settings.Sale.HintText,
            Settings.Sale.FooterText,
            new ProductsTableDocumentModel(
                "VeloBasar_Id",
                "VeloBasar_ProductDescription",
                "VeloBasar_Size",
                "VeloBasar_Price",
                "VeloBasar_Sum:",
                "VeloBasar_ProductCounter_2",
                "€ 108,00",
                Settings.Sale.SellerInfoText,
                new[]
                {
                    new ProductTableRowDocumentModel("1", "Riese und Müller - Cargo Bike".Line("My great cargo bike").Line(" Black/Green 123456"), "29", "€ 18,68", "* Asfaloth Glaurung, Morzinplatz 44, 555AA Gondolin"),
                    new ProductTableRowDocumentModel("2", "Pepper - City-Bike".Line("No lights").Line(" Red XYZ"), "28", "€ 89,32", "* Gilfanon Tareg, Essiggasse 6, XYF4 Ost-in-Edhil")
                }
            )
        );
        result.Should().BeEquivalentTo(expectedModel);
    }
}
