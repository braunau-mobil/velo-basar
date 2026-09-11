using BraunauMobil.VeloBasar.BusinessLogic;
using Xan.AspNetCore.Models;

namespace BraunauMobil.VeloBasar.Tests.BusinessLogic.QrImportServiceTests;

public class TryApplyToProductAsync
    : TestBase
{
    [Theory]
    [VeloInlineAutoData(null!)]
    [VeloInlineAutoData("")]
    [VeloInlineAutoData("   ")]
    public async Task NullOrBlankText_ReturnsEmpty(string? qrText, ProductEntity product)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(qrText, product);

        //  Assert
        result.Status.Should().Be(QrImportStatus.Empty);
    }

    [Theory]
    [VeloInlineAutoData("this is not a velo-basar payload")]
    [VeloInlineAutoData("2\tproduct\tKTM\tMountainbike\tTest\t\t\t\t100\ttrue")]
    [VeloInlineAutoData("VB")]
    [VeloInlineAutoData("VBXP0123")]
    [VeloInlineAutoData("VB3")]
    [VeloInlineAutoData("VB3X9DA4Q2ADDTQMST31D5N64QBBCK4L8PBJEG4GI28964O302BKE9QMB0K6")]
    [VeloInlineAutoData("VB3P")]
    [VeloInlineAutoData("VB3P0")]
    [VeloInlineAutoData("VB3P9DA4Q2ADDTQMST31D5N64QBBCK4L8PBJEG4GI28964O302BKE9QMB0K=")]
    [VeloInlineAutoData("VB3P9DA4Q2ADDTQMST31D5N64QBBCK4L8PBJEG4GI28964O302BKE9QMB0KZ")]
    [VeloInlineAutoData(QrPayload.ProductWithTooFewFields)]
    public async Task InvalidFormat_ReturnsInvalidFormat(string qrText, ProductEntity product)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(qrText, product);

        //  Assert
        result.Status.Should().Be(QrImportStatus.InvalidFormat);
    }

    [Theory]
    [VeloAutoData]
    public async Task InvalidUtf8WithValidChecksum_ReturnsInvalidFormat(ProductEntity product)
    {
        //  Arrange
        string qrText = QrPayload.EncodeBytes(3, 'P', [0x4B, 0xC3, 0x28, 0x09, 0xFF]);

        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(qrText, product);

        //  Assert
        result.Status.Should().Be(QrImportStatus.InvalidFormat);
    }

    [Theory]
    [VeloAutoData]
    public async Task WrongKind_ReturnsWrongKind(ProductEntity product)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(QrPayload.Seller, product);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.WrongKind);
            result.ActualKind.Should().Be("seller");
        }
    }

    [Theory]
    [VeloInlineAutoData(2)]
    [VeloInlineAutoData(4)]
    [VeloInlineAutoData(31)]
    public async Task UnsupportedSchemaVersion_ReturnsUnsupportedSchemaVersion(int schemaVersion, ProductEntity product)
    {
        //  Arrange
        string qrText = QrPayload.Encode(schemaVersion, 'P', "KTM", "Mountainbike", "Test", "", "", "", "100", "true");

        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(qrText, product);

        //  Assert
        result.Status.Should().Be(QrImportStatus.UnsupportedSchemaVersion);
    }

    [Theory]
    [VeloAutoData]
    public async Task DroppedCharacter_ReturnsChecksumMismatch(ProductEntity product)
    {
        //  Arrange
        string qrText = QrPayload.DropCharacter(QrPayload.Product, 20);

        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(qrText, product);

        //  Assert
        result.Status.Should().Be(QrImportStatus.ChecksumMismatch);
    }

    [Theory]
    [VeloAutoData]
    public async Task WrongCharacter_ReturnsChecksumMismatch(ProductEntity product)
    {
        //  Arrange
        string qrText = QrPayload.ReplaceCharacter(QrPayload.Product, 30);

        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(qrText, product);

        //  Assert
        result.Status.Should().Be(QrImportStatus.ChecksumMismatch);
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_KnownType_MapsFieldsAndReturnsSuccess(ProductEntity product)
    {
        //  Arrange
        ProductTypeEntity type = Fixture.BuildProductType()
            .With(t => t.Name, "Mountainbike")
            .With(t => t.State, ObjectState.Enabled)
            .Create();
        Db.ProductTypes.Add(type);
        await Db.SaveChangesAsync();

        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(QrPayload.Product, product);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            result.UnmatchedProductType.Should().BeNull();

            product.Brand.Should().Be("KTM");
            product.TypeId.Should().Be(type.Id);
            product.Description.Should().Be("28 Zoll, guter Zustand");
            product.Color.Should().Be("Schwarz/Rot");
            product.FrameNumber.Should().Be("ABC123");
            product.TireSize.Should().Be("28\"");
            product.Price.Should().Be(249.5m);
            product.DonateIfNotSold.Should().BeFalse();
        }
    }

    [Theory]
    [VeloInlineAutoData("lowercase")]
    [VeloInlineAutoData("surrounding whitespace")]
    public async Task ValidPayload_CaseOrWhitespaceChanged_ReturnsSuccess(string variant, ProductEntity product)
    {
        //  Arrange
        string qrText = variant == "lowercase"
            ? QrPayload.Product.ToLowerInvariant()
            : $"  {QrPayload.Product}\r\n";

        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(qrText, product);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            product.Brand.Should().Be("KTM");
            product.Description.Should().Be("28 Zoll, guter Zustand");
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_UnknownType_LeavesTypeIdZeroAndReturnsUnmatchedType(ProductEntity product)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(QrPayload.Product, product);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            result.UnmatchedProductType.Should().Be("Mountainbike");
            product.TypeId.Should().Be(0);
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_OmittedOptionalFields_ClearsExistingValues(ProductEntity product)
    {
        //  Arrange
        product.Color = "Old Color";
        product.FrameNumber = "OLD123";
        product.TireSize = "26\"";

        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(QrPayload.ProductWithoutOptionalFields, product);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            product.Color.Should().BeNull();
            product.FrameNumber.Should().BeNull();
            product.TireSize.Should().BeNull();
            product.Price.Should().Be(100m);
            product.DonateIfNotSold.Should().BeTrue();
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_EmptyPrice_SetsPriceToZero(ProductEntity product)
    {
        //  Arrange
        string qrText = QrPayload.Encode(3, 'P', "KTM", "Mountainbike", "Test", "", "", "", "", "false");

        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(qrText, product);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            product.Price.Should().Be(0m);
        }
    }

    [Theory]
    [VeloAutoData]
    public async Task ValidPayload_NonAscii_DecodesFields(ProductEntity product)
    {
        //  Act
        QrImportResult result = await Sut.TryApplyToProductAsync(QrPayload.ProductWithNonAscii, product);

        //  Assert
        using (new AssertionScope())
        {
            result.Status.Should().Be(QrImportStatus.Success);
            product.Description.Should().Be("Schöner Zustand, Größe M");
            product.Color.Should().Be("Grün");
        }
    }
}
