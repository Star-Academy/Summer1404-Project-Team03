// using Application.Abstractions;
// using Application.ValueObjects;
// using MediatR;
//
// namespace Application.Identity.GetProfile;
//
// public sealed class GetProfileHandler : IRequestHandler<GetProfileQuery, ProfileDto>
// {
//     private readonly IIdentityService _identity;
//
//     public GetProfileHandler(IIdentityService identity)
//     {
//         _identity = identity;
//     }
//
//     public Task<ProfileDto> Handle(GetProfileQuery request, CancellationToken cancellationToken)
//     {
//         return _identity.GetProfileAsync(request.UserId, cancellationToken);
//     }
// }