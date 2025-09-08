// using Application.Abstractions;
// using Application.ValueObjects;
// using MediatR;
//
// namespace Application.Identity.Login;
//
// public sealed class LoginHandler : IRequestHandler<LoginCommand, AuthResult>
// {
//     private readonly IIdentityService _identity;
//
//     public LoginHandler(IIdentityService identity)
//     {
//         _identity = identity;
//     }
//
//     public Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
//     {
//         return _identity.LoginAsync(new LoginRequest(request.Username, request.Password), cancellationToken);
//     }
// }