using MediatR;

namespace Application.Identity.ChangePassword;

public sealed record ChangePasswordCommand(string UserId, string CurrentPassword, string NewPassword) : IRequest<Unit>;
