using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace BookStoreApp.API.Endpoints;

public static class TestEndpoints
{
    public static IEndpointRouteBuilder MapTestEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/api/test/{value}", (string value, ILogger<TestEndpointMarker> logger) =>
            {
                logger.LogInformation("Echoing test value {Value}", value);
                return Results.Ok(value);
            })
            .WithName("TestEndpoint")
            .WithOpenApi();

        return endpoints;
    }

    private sealed class TestEndpointMarker;
}
