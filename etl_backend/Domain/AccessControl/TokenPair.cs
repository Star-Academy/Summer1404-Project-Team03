namespace Domain.AccessControl;

public sealed record TokenPair(string AccessToken, string? RefreshToken, DateTime ExpiresAtUtc);