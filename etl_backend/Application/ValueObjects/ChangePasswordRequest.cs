namespace Application.ValueObjects;

public sealed record ChangePasswordRequest(string UserId, string CurrentPassword, string NewPassword);
