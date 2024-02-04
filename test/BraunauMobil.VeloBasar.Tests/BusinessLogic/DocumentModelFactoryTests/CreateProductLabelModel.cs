using BraunauMobil.VeloBasar.Models.Documents;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.DocumentModelFactoryTests;

public class CreateProductLabelModel
     : TestBase
{
    [Fact]
    public void NoFrameNumberNoTireSize()
    {
        //  Arrange
        Settings.Label.TitleFormat = "TitleFormat_{0}";
        Settings.Label.MaxDescriptionLength = 30;

        ProductTypeEntity productType = Fixture.BuildProductType()
            .With(productType => productType.Name, "City-Bike")
            .Create();

        ProductEntity product = Fixture.BuildProduct()
            .With(product => product.Id, 666)
            .With(product => product.Brand, "Cube")
            .With(product => product.Description, "This is a really great Bike. I like it a lot, but unfortunatley the description is way to long to fit on the tiny label that gets printed out.")
            .With(product => product.Type, productType)
            .With(product => product.Price, 123.45m)
            .Without(product => product.FrameNumber)
            .Without(product => product.TireSize)
            .Create();
        product.Session.Basar.Name = "1. Basar";

        //  Act
        ProductLabelDocumentModel result = Sut.CreateProductLabelModel(product);

        //  Assert
        ProductLabelDocumentModel expectedModel = new(
            "TitleFormat_1. Basar",
            "Cube - City-Bike",
            "This is a really great Bike. I",
            product.Color,
            null,
            null,
            "666",
            "€ 123,45"
        );

        result.Should().BeEquivalentTo(expectedModel);
    }

    [Fact]
    public void WithFrameNumberNoTireSize()
    {
        //  Arrange
        Settings.Label.TitleFormat = "TitleFormat_{0}";
        Settings.Label.MaxDescriptionLength = 30;

        ProductTypeEntity productType = Fixture.BuildProductType()
            .With(productType => productType.Name, "City-Bike")
            .Create();

        ProductEntity product = Fixture.BuildProduct()
            .With(product => product.Id, 666)
            .With(product => product.Brand, "Cube")
            .With(product => product.Description, "This is a really great Bike. I like it a lot, but unfortunatley the description is way to long to fit on the tiny label that gets printed out.")
            .With(product => product.Type, productType)
            .With(product => product.Price, 123.45m)
            .With(product => product.FrameNumber, "1234567890")
            .Without(product => product.TireSize)
            .Create();
        product.Session.Basar.Name = "1. Basar";

        //  Act
        ProductLabelDocumentModel result = Sut.CreateProductLabelModel(product);

        //  Assert
        ProductLabelDocumentModel expectedModel = new(
            "TitleFormat_1. Basar",
            "Cube - City-Bike",
            "This is a really great Bike. I",
            product.Color,
            "VeloBasar_FrameNumberLabel_1234567890",
            null,
            "666",
            "€ 123,45"
        );

        result.Should().BeEquivalentTo(expectedModel);
    }

    [Fact]
    public void NoFrameNumberWithTireSize()
    {
        //  Arrange
        Settings.Label.TitleFormat = "TitleFormat_{0}";
        Settings.Label.MaxDescriptionLength = 30;

        ProductTypeEntity productType = Fixture.BuildProductType()
            .With(productType => productType.Name, "City-Bike")
            .Create();

        ProductEntity product = Fixture.BuildProduct()
            .With(product => product.Id, 666)
            .With(product => product.Brand, "Cube")
            .With(product => product.Description, "This is a really great Bike. I like it a lot, but unfortunatley the description is way to long to fit on the tiny label that gets printed out.")
            .With(product => product.Type, productType)
            .With(product => product.Price, 123.45m)
            .Without(product => product.FrameNumber)
            .With(product => product.TireSize, "28")
            .Create();
        product.Session.Basar.Name = "1. Basar";

        //  Act
        ProductLabelDocumentModel result = Sut.CreateProductLabelModel(product);

        //  Assert
        ProductLabelDocumentModel expectedModel = new(
            "TitleFormat_1. Basar",
            "Cube - City-Bike",
            "This is a really great Bike. I",
            product.Color,
            null,
            "VeloBasar_TireSizeLabel_28",
            "666",
            "€ 123,45"
        );

        result.Should().BeEquivalentTo(expectedModel);
    }

    [Fact]
    public void WithFrameNumberAndTireSize()
    {
        //  Arrange
        Settings.Label.TitleFormat = "TitleFormat_{0}";
        Settings.Label.MaxDescriptionLength = 30;

        ProductTypeEntity productType = Fixture.BuildProductType()
            .With(productType => productType.Name, "City-Bike")
            .Create();

        ProductEntity product = Fixture.BuildProduct()
            .With(product => product.Id, 666)
            .With(product => product.Brand, "Cube")
            .With(product => product.Description, "This is a really great Bike. I like it a lot, but unfortunatley the description is way to long to fit on the tiny label that gets printed out.")
            .With(product => product.Type, productType)
            .With(product => product.FrameNumber, "ABCD")
            .With(product => product.TireSize, "29")
            .With(product => product.Price, 123.45m)
            .Create();
        product.Session.Basar.Name = "1. Basar";

        //  Act
        ProductLabelDocumentModel result = Sut.CreateProductLabelModel(product);

        //  Assert
        ProductLabelDocumentModel expectedModel = new(
            "TitleFormat_1. Basar",
            "Cube - City-Bike",
            "This is a really great Bike. I",
            product.Color,
            "VeloBasar_FrameNumberLabel_ABCD",
            "VeloBasar_TireSizeLabel_29",
            "666",
            "€ 123,45"
        );

        result.Should().BeEquivalentTo(expectedModel);
    }
}
