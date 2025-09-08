using Domain.Entities;
using MediatR;

namespace Domain.AccessControl.Events;

public sealed record UserAuthenticated(string UserId, DateTime OccurredAtUtc) : INotification;