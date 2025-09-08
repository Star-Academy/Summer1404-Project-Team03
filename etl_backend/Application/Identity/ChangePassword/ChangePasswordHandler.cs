// using Application.Abstractions;
// using Application.ValueObjects;
// using MediatR;
//
// namespace Application.Identity.ChangePassword;
//
// public sealed class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, Unit>
// {
//     private readonly IIdentityService _identity;
//
//     public ChangePasswordHandler(IIdentityService identity)
//     {
//         _identity = identity;
//     }
//
//     public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
//     {
//         await _identity.ChangePasswordAsync(
//             new ChangePasswordRequest(request.UserId, request.CurrentPassword, request.NewPassword),
//             cancellationToken);
//         return Unit.Value;
//     }
// }