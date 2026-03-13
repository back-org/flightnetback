using Flight.Application.DTOs;
using Flight.Application.Validators;
using FluentAssertions;
using Xunit;

namespace Flight.UnitTests.Validators;

/// <summary>
/// Tests unitaires pour le validateur <see cref="CountryDtoValidator"/>.
/// </summary>
public class CountryDtoValidatorTests
{
    private readonly CountryDtoValidator _validator = new();

    private static CountryDto ValidDto()
    {
        return new CountryDto
        {
            Id = 0,
            Name = "Madagascar",
            Iso2 = "MG",
            Iso3 = "MDG"
        };
    }

    [Fact]
    public void Validate_ValidDto_ShouldPass()
    {
        var result = _validator.Validate(ValidDto());

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void Validate_EmptyName_ShouldFail(string? name)
    {
        var dto = ValidDto();
        dto.Name = name ?? string.Empty;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CountryDto.Name));
    }

    [Theory]
    [InlineData("")]
    [InlineData("M")]
    [InlineData("MAD")]
    public void Validate_InvalidIso2_ShouldFail(string iso2)
    {
        var dto = ValidDto();
        dto.Iso2 = iso2;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CountryDto.Iso2));
    }

    [Theory]
    [InlineData("")]
    [InlineData("MD")]
    [InlineData("MADA")]
    public void Validate_InvalidIso3_ShouldFail(string iso3)
    {
        var dto = ValidDto();
        dto.Iso3 = iso3;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CountryDto.Iso3));
    }
}