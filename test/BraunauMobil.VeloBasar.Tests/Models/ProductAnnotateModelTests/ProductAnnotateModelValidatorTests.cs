using BraunauMobil.VeloBasar.Tests;
using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.InitializationConfigurationTests;

public class ProductAnnotateModelValidatorTests
{
    private readonly ProductAnnotateModelValidator _sut = new(new StringLocalizerMock<SharedResources>());

    [Theory]
    [VeloAutoData]
    public void NotesEmpty_ShouldBeInvalid(ProductAnnotateModel model)
    {
        //  Arrange
        model.Notes = "";

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(error => error.PropertyName == nameof(model.Notes));
        }
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(ProductAnnotateModel model)
    {
        //  Arrange

        //  Act
        ValidationResult result = _sut.Validate(model);

        //  Assert
        using (new AssertionScope())
        {
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }
    }
}
