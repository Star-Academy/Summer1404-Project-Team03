using System.Security.Claims;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return null;

            return user.FindFirst("id")?.Value
                   ?? user.FindFirst(c => c.Type.EndsWith("nameidentifier", StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }

    public string? UserName => _httpContextAccessor.HttpContext?.User.FindFirst("preferred_username")?.Value
                               ?? _httpContextAccessor.HttpContext?.User.Identity?.Name;

    public string[] Roles => _httpContextAccessor.HttpContext?.User.FindAll(ClaimTypes.Role)
        .Select(c => c.Value)
        .ToArray() ?? Array.Empty<string>();

    public bool IsAuthenticated => _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true;

    public Task<bool> IsInRoleAsync(string role)
    {
        return Task.FromResult(Roles.Contains(role, StringComparer.OrdinalIgnoreCase));
    }
}