// using Application.Abstractions;
// using MediatR;
//
// namespace Application.Identity.Logout;
//
// public sealed class LogoutHandler : IRequestHandler<LogoutCommand, Unit>
// {
//     private readonly IIdentityService _identity;
//
//     public LogoutHandler(IIdentityService identity)
//     {
//         _identity = identity;
//     }
//
//     public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
//     {
//         await _identity.LogoutAsync(request.UserId, cancellationToken);
//         return Unit.Value;
//     }
// }