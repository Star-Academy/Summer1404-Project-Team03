using Application.ValueObjects;
using MediatR;
namespace Application.Identity.Login;

public sealed record LoginCommand(string Username, string Password) : IRequest<AuthResult>;
