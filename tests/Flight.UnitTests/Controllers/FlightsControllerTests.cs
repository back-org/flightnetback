using Flight.Api.Controllers;
using Flight.Application.CQRS.Commands.Flights;
using Flight.Application.CQRS.Queries.Flights;
using Flight.Application.DTOs;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace Flight.UnitTests.Controllers;

/// <summary>
/// Tests unitaires du contrôleur des vols.
/// </summary>
public class FlightsControllerTests
{
    private static FlightDto MakeDto(int id = 1)
        => new(id, "AF001", DateTime.UtcNow.AddHours(2), DateTime.UtcNow.AddHours(5), 20, 150, 500f, 150f, 2, 1);

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllFlightsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<FlightDto> { MakeDto() });

        var controller = new FlightsController(mediatorMock.Object);
        var result = await controller.GetAll();

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Get_Found_ShouldReturnOk()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetFlightByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(MakeDto());

        var controller = new FlightsController(mediatorMock.Object);
        var result = await controller.Get(1);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Get_NotFound_ShouldReturnNotFound()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetFlightByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((FlightDto?)null);

        var controller = new FlightsController(mediatorMock.Object);
        var result = await controller.Get(999);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        var dto = MakeDto();
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateFlightCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var controller = new FlightsController(mediatorMock.Object);
        var result = await controller.Create(dto);

        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task Put_Found_ShouldReturnOk()
    {
        var dto = MakeDto();
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateFlightCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var controller = new FlightsController(mediatorMock.Object);
        var result = await controller.Put(dto);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Put_NotFound_ShouldReturnNotFound()
    {
        var dto = MakeDto(999);
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateFlightCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((FlightDto?)null);

        var controller = new FlightsController(mediatorMock.Object);
        var result = await controller.Put(dto);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_Found_ShouldReturnNoContent()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteFlightCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new FlightsController(mediatorMock.Object);
        var result = await controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_NotFound_ShouldReturnNotFound()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteFlightCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var controller = new FlightsController(mediatorMock.Object);
        var result = await controller.Delete(999);

        result.Should().BeOfType<NotFoundObjectResult>();
    }
}