using System.Security.Claims;
using FastEndpoints;

namespace WebApi;

public class DebugRolesEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("/debug-roles");
        AllowAnonymous(); // temporary
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var roles = User.Claims
            .Where(c => c.Type == ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        await SendAsync(new { roles });
    }
}