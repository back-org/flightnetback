using Flight.Application.DTOs;
using Flight.Application.Validators;
using Flight.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Flight.UnitTests.Validators;

/// <summary>
/// Tests unitaires pour le validateur <see cref="AirlineDtoValidator"/>.
/// </summary>
public class AirlineDtoValidatorTests
{
    private readonly AirlineDtoValidator _validator = new();

    private static AirlineDto ValidDto()
    {
        return new AirlineDto
        {
            Id = 0,
            Name = "Air France",
            State = State.Active,
            DeletedFlag = 0
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
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AirlineDto.Name));
    }

    [Fact]
    public void Validate_NameTooLong_ShouldFail()
    {
        var dto = ValidDto();
        dto.Name = new string('A', 31);

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AirlineDto.Name));
    }

    [Fact]
    public void Validate_InvalidDeletedFlag_ShouldFail()
    {
        var dto = ValidDto();
        dto.DeletedFlag = -1;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AirlineDto.DeletedFlag));
    }

    [Fact]
    public void Validate_InvalidState_ShouldFail()
    {
        var dto = ValidDto();
        dto.State = (State)999;

        var result = _validator.Validate(dto);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(AirlineDto.State));
    }
}