using System.Net;
using FluentAssertions;
using Flight.IntegrationTests.Infrastructure;
using Xunit;

namespace Flight.IntegrationTests.Controllers;

public class FlightsControllerIntegrationTests :
    IClassFixture<FlightWebApplicationFactory>
{
    private readonly HttpClient _client;

    public FlightsControllerIntegrationTests(FlightWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllFlights_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/v1/flights");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
