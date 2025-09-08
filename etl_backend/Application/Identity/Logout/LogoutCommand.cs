using MediatR;

namespace Application.Identity.Logout;

public sealed record LogoutCommand(string UserId) : IRequest<Unit>;
