using Flight.Api.Controllers;
using Flight.Application.CQRS.Commands.Passengers;
using Flight.Application.CQRS.Queries.Passengers;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Flight.UnitTests.Controllers;

/// <summary>
/// Tests unitaires du contrôleur des passagers.
/// </summary>
public class PassengersControllerTests
{
    private static PassengerDto MakeDto(int id = 1)
        => new(id, "Jean", "", "Dupont", "jean@test.com", "+261340000000", "Tana", Genre.Male);

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllPassengersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<PassengerDto> { MakeDto() });

        var controller = new PassengersController(mediatorMock.Object);
        var result = await controller.GetAll();

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Get_Found_ShouldReturnOk()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetPassengerByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(MakeDto());

        var controller = new PassengersController(mediatorMock.Object);
        var result = await controller.Get(1);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Get_NotFound_ShouldReturnNotFound()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetPassengerByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PassengerDto?)null);

        var controller = new PassengersController(mediatorMock.Object);
        var result = await controller.Get(999);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        var dto = MakeDto();
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<CreatePassengerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var controller = new PassengersController(mediatorMock.Object);
        var result = await controller.Create(dto);

        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task Put_Found_ShouldReturnOk()
    {
        var dto = MakeDto();
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePassengerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var controller = new PassengersController(mediatorMock.Object);
        var result = await controller.Put(dto);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Put_NotFound_ShouldReturnNotFound()
    {
        var dto = MakeDto(999);
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdatePassengerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((PassengerDto?)null);

        var controller = new PassengersController(mediatorMock.Object);
        var result = await controller.Put(dto);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_Found_ShouldReturnNoContent()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<DeletePassengerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new PassengersController(mediatorMock.Object);
        var result = await controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_NotFound_ShouldReturnNotFound()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<DeletePassengerCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var controller = new PassengersController(mediatorMock.Object);
        var result = await controller.Delete(999);

        result.Should().BeOfType<NotFoundObjectResult>();
    }
}