using Flight.Application.DTOs;
using Flight.Application.Validators;
using FluentAssertions;
using Xunit;

namespace Flight.UnitTests.Validators;

/// <summary>
/// Tests unitaires pour le validateur <see cref="CityDtoValidator"/>.
/// </summary>
public class CityDtoValidatorTests
{
    private readonly CityDtoValidator _validator = new();

    private static CityDto ValidDto()
    {
        return new CityDto
        {
            Id = 0,
            Name = "Antananarivo",
            CountryId = 1,
            Latitude = -18.8792,
            Longitude = 47.5079
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
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CityDto.Name));
    }

    [Fact]
    public void Validate_NameTooLong_ShouldFail()
    {
        var dto = ValidDto();
        dto.Name = new string('A', 31);

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CityDto.Name));
    }

    [Fact]
    public void Validate_InvalidCountryId_ShouldFail()
    {
        var dto = ValidDto();
        dto.CountryId = 0;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CityDto.CountryId));
    }

    [Fact]
    public void Validate_InvalidLatitude_ShouldFail()
    {
        var dto = ValidDto();
        dto.Latitude = 100;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CityDto.Latitude));
    }

    [Fact]
    public void Validate_InvalidLongitude_ShouldFail()
    {
        var dto = ValidDto();
        dto.Longitude = 200;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CityDto.Longitude));
    }
}