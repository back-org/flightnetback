using Flight.Api.Controllers;
using Flight.Api.Models;
using Flight.Application.CQRS.Commands.Airlines;
using Flight.Application.CQRS.Queries.Airlines;
using Flight.Application.DTOs;
using Flight.Domain.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Flight.UnitTests.Controllers;

/// <summary>
/// Tests unitaires du contrôleur des compagnies aériennes.
/// </summary>
public class AirlinesControllerTests
{
    private static AirlineDto MakeDto(int id = 1)
        => new(id, "Air France", State.Active, 0);

    [Fact]
    public async Task GetAll_ShouldReturnOk()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAllAirlinesQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<AirlineDto> { MakeDto() });

        var controller = new AirlinesController(mediatorMock.Object);

        var result = await controller.GetAll();

        result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Get_ById_Found_ShouldReturnOk()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAirlineByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(MakeDto());

        var controller = new AirlinesController(mediatorMock.Object);

        var result = await controller.Get(1);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Get_ById_NotFound_ShouldReturnNotFound()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<GetAirlineByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AirlineDto?)null);

        var controller = new AirlinesController(mediatorMock.Object);

        var result = await controller.Get(999);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Create_ValidDto_ShouldReturnCreatedAtAction()
    {
        var dto = MakeDto();
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<CreateAirlineCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var controller = new AirlinesController(mediatorMock.Object);

        var result = await controller.Create(dto);

        result.Result.Should().BeOfType<CreatedAtActionResult>();
    }

    [Fact]
    public async Task Put_Found_ShouldReturnOk()
    {
        var dto = MakeDto(1);
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateAirlineCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        var controller = new AirlinesController(mediatorMock.Object);

        var result = await controller.Put(dto);

        result.Result.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task Put_NotFound_ShouldReturnNotFound()
    {
        var dto = MakeDto(999);
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<UpdateAirlineCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((AirlineDto?)null);

        var controller = new AirlinesController(mediatorMock.Object);

        var result = await controller.Put(dto);

        result.Result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task Delete_Found_ShouldReturnNoContent()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteAirlineCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var controller = new AirlinesController(mediatorMock.Object);

        var result = await controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task Delete_NotFound_ShouldReturnNotFound()
    {
        var mediatorMock = new Mock<IMediator>();
        mediatorMock.Setup(m => m.Send(It.IsAny<DeleteAirlineCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var controller = new AirlinesController(mediatorMock.Object);

        var result = await controller.Delete(999);

        result.Should().BeOfType<NotFoundObjectResult>();
    }
}