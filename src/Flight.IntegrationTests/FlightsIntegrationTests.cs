using System.Net;
using System.Net.Http.Json;
using Flight.Api.Models;
using Flight.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Flight.IntegrationTests;

/// <summary>
/// Tests d'intégration pour le endpoint /api/v1/flights.
/// </summary>
public class FlightsIntegrationTests : IClassFixture<FlightWebApplicationFactory>
{
    private readonly HttpClient _client;

    public FlightsIntegrationTests(FlightWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_Flights_ShouldReturn200()
    {
        var response = await _client.GetAsync("/api/v1/flights");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetFlight_NotFound_ShouldReturn404()
    {
        var response = await _client.GetAsync("/api/v1/flights/9999");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateFlight_WithoutAuth_ShouldReturn401()
    {
        var dto = new FlightDto(
            0, "TEST01",
            DateTime.UtcNow.AddHours(2),
            DateTime.UtcNow.AddHours(5),
            20, 150, 500f, 150f, 2, 1);

        var response = await _client.PostAsJsonAsync("/api/v1/flights", dto);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetFlightsPaged_ShouldReturn200WithPagination()
    {
        var response = await _client.GetAsync("/api/v1/flights/paged?pageNumber=1&pageSize=10");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<ApiResponse<PagedResult<FlightDto>>>();
        content.Should().NotBeNull();
        content!.Success.Should().BeTrue();
    }

    [Fact]
    public async Task HealthCheck_ShouldReturn200()
    {
        var response = await _client.GetAsync("/health");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
