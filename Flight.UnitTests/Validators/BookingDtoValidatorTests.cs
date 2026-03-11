using Flight.Application.Validators;
using Flight.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Flight.UnitTests.Validators;

/// <summary>
/// Tests unitaires pour le validateur <see cref="BookingDtoValidator"/>.
/// </summary>
public class BookingDtoValidatorTests
{
    private readonly BookingDtoValidator _validator = new();

    private static BookingDto ValidDto() => new(
        Id: 0,
        FlightType: Confort.Economy,
        FlightId: 1,
        PassengerId: 1,
        Statut: Statut.Pending);

    [Fact]
    public void Validate_ValidDto_ShouldPass()
    {
        var result = _validator.Validate(ValidDto());
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_InvalidFlightId_ShouldFail()
    {
        var dto = ValidDto() with { FlightId = 0 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(BookingDto.FlightId));
    }

    [Fact]
    public void Validate_InvalidPassengerId_ShouldFail()
    {
        var dto = ValidDto() with { PassengerId = -1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(BookingDto.PassengerId));
    }

    [Fact]
    public void Validate_InvalidStatut_ShouldFail()
    {
        var dto = ValidDto() with { Statut = (Statut)999 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
    }
}
