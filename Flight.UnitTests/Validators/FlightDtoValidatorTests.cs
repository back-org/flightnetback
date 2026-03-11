using Flight.Application.Validators;
using Flight.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Flight.UnitTests.Validators;

/// <summary>
/// Tests unitaires pour le validateur <see cref="FlightDtoValidator"/>.
/// </summary>
public class FlightDtoValidatorTests
{
    private readonly FlightDtoValidator _validator = new();

    private static FlightDto ValidDto() => new(
        Id: 0,
        Code: "AF1234",
        Departure: DateTime.UtcNow.AddHours(2),
        EstimatedArrival: DateTime.UtcNow.AddHours(5),
        BusinessClassSlots: 20,
        EconomySlots: 150,
        BusinessClassPrice: 500f,
        EconomyPrice: 150f,
        To: 2,
        From: 1);

    [Fact]
    public void Validate_ValidDto_ShouldPass()
    {
        var result = _validator.Validate(ValidDto());
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null!)]
    public void Validate_EmptyCode_ShouldFail(string? code)
    {
        var dto = ValidDto() with { Code = code! };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(FlightDto.Code));
    }

    [Fact]
    public void Validate_CodeTooLong_ShouldFail()
    {
        var dto = ValidDto() with { Code = new string('A', 31) };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(FlightDto.Code));
    }

    [Fact]
    public void Validate_ArrivalBeforeDeparture_ShouldFail()
    {
        var dto = ValidDto() with
        {
            Departure = DateTime.UtcNow.AddHours(5),
            EstimatedArrival = DateTime.UtcNow.AddHours(2)
        };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(FlightDto.EstimatedArrival));
    }

    [Fact]
    public void Validate_SameDepartureAndArrivalAirport_ShouldFail()
    {
        var dto = ValidDto() with { From = 1, To = 1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(FlightDto.From));
    }

    [Fact]
    public void Validate_NegativePrice_ShouldFail()
    {
        var dto = ValidDto() with { EconomyPrice = -10f };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(FlightDto.EconomyPrice));
    }

    [Fact]
    public void Validate_NegativeSlots_ShouldFail()
    {
        var dto = ValidDto() with { BusinessClassSlots = -1 };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(FlightDto.BusinessClassSlots));
    }
}
