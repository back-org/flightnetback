using Flight.Application.Validators;
using Flight.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Flight.UnitTests.Validators;

/// <summary>
/// Tests unitaires pour le validateur <see cref="PassengerDtoValidator"/>.
/// </summary>
public class PassengerDtoValidatorTests
{
    private readonly PassengerDtoValidator _validator = new();

    private static PassengerDto ValidDto() => new(
        Id: 0,
        Name: "Jean",
        MiddleName: "",
        LastName: "Dupont",
        Email: "jean.dupont@example.com",
        Contact: "+261341234567",
        Address: "123 rue de la Paix, Antananarivo",
        Sex: Genre.Male);

    [Fact]
    public void Validate_ValidDto_ShouldPass()
    {
        var result = _validator.Validate(ValidDto());
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null!)]
    public void Validate_EmptyName_ShouldFail(string? name)
    {
        var dto = ValidDto() with { Name = name! };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(PassengerDto.Name));
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("missing@")]
    [InlineData("@nodomain.com")]
    public void Validate_InvalidEmail_ShouldFail(string email)
    {
        var dto = ValidDto() with { Email = email };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(PassengerDto.Email));
    }

    [Fact]
    public void Validate_EmptyContact_ShouldFail()
    {
        var dto = ValidDto() with { Contact = "" };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(PassengerDto.Contact));
    }

    [Fact]
    public void Validate_EmptyAddress_ShouldFail()
    {
        var dto = ValidDto() with { Address = "" };
        var result = _validator.Validate(dto);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(PassengerDto.Address));
    }
}
