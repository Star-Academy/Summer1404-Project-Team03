namespace Application.ValueObjects;

public sealed record AuthResult(string AccessToken, string? RefreshToken, string UserId, string Username);