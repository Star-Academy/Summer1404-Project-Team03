using Application.ValueObjects;
using MediatR;

namespace Application.Identity.GetProfile;

public sealed record GetProfileQuery(string UserId) : IRequest<ProfileDto>;
