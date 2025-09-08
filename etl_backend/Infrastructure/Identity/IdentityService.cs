// using Application.Abstractions;
// using Application.ValueObjects;
// using Infrastructure.Identity.Abstractions;
// using Infrastructure.SsoServices.User.Abstractions;
//
// namespace Infrastructure.Identity;
//
// public class IdentityService : IIdentityService
// {
//     private readonly ITokenCookieService _tokenCookieService;
//     private readonly IKeycloakAuthService  _keycloakAuthService;
//     private readonly IKeycloakLogOutUser _keycloakLogOutUser;
//     private readonly ITokenProfileExtractor _tokenProfileExtractor;
//     private readonly IEditUserService _editUserService;
//     public IdentityService(ITokenCookieService tokenCookieService, IKeycloakAuthService keycloakAuthService, IKeycloakLogOutUser keycloakLogOutUser, ITokenProfileExtractor tokenProfileExtractor, IEditUserService editUserService)
//     {
//         _tokenCookieService = tokenCookieService;
//         _keycloakAuthService = keycloakAuthService;
//         _keycloakLogOutUser = keycloakLogOutUser;
//         _tokenProfileExtractor = tokenProfileExtractor;
//         _editUserService = editUserService;
//     }
//     public Task<ProfileDto> GetProfileAsync(string, CancellationToken)
//     {
//         
//     }
//
//     public Task ChangePasswordAsync(ChangePasswordRequest, CancellationToken)
//     {
//         
//     }
//
//     public Task<AuthResult> LoginAsync(LoginRequest, CancellationToken)
//     {
//         // this is where you set the tokens 
//         
//     }
//
//     public Task LogoutAsync(string, CancellationToken)
//     {
//         
//     }
// }