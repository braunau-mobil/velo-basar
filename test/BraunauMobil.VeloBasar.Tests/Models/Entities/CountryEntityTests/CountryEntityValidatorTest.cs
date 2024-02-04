using FluentValidation.Results;

namespace BraunauMobil.VeloBasar.Tests.Models.Entities.CountryEntityTests;

public class CountryEntityValidatorTest
{
    private readonly CountryEntityValidator _sut = new(X.StringLocalizer);

    [Theory]
    [VeloAutoData]
    public void NameIsEmpty_ShouldBeInvalid(CountryEntity country)
    {
        //  Arrange
        country.Name = string.Empty;

        //  Act
        ValidationResult result = _sut.Validate(country);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(country.Name));
    }

    [Theory]
    [VeloAutoData]
    public void Iso3166Alpha3CodeIsEmpty_ShouldBeInvalid(CountryEntity country)
    {
        //  Arrange
        country.Iso3166Alpha3Code = string.Empty;

        //  Act
        ValidationResult result = _sut.Validate(country);

        //  Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(country.Iso3166Alpha3Code));
    }

    [Theory]
    [VeloAutoData]
    public void ShouldBeValid(CountryEntity country)
    {
        //  Arrange
        country.Name = "Austria";
        country.Iso3166Alpha3Code = "AUT";

        //  Act
        ValidationResult result = _sut.Validate(country);

        //  Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }
}
