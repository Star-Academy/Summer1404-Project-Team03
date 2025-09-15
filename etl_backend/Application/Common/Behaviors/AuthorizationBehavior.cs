using System.Reflection;
using Application.Common.Authorization;
using Application.Common.Exceptions;
using Application.Services.Abstractions;
using MediatR;

namespace Application.Common.Behaviors;


public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ICurrentUserService _currentUser;

    public AuthorizationBehavior(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var authorizeAttribute = request.GetType().GetCustomAttribute<RequireRoleAttribute>();
        if (authorizeAttribute == null)
        {
            return await next();
        }

        if (!_currentUser.IsAuthenticated)
        {
            throw new ForbiddenException("User is not authenticated.");
        }

        var userHasRequiredRole = authorizeAttribute.Roles
            .Any(requiredRole => _currentUser.Roles.Contains(requiredRole));

        if (!userHasRequiredRole)
        {
            throw new ForbiddenException($"User is not authorized. Required roles: {string.Join(", ", authorizeAttribute.Roles)}");
        }

        return await next();
    }
}