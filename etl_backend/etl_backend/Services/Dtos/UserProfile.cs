namespace etl_backend.Services.Dtos;

public class UserProfile
{
    public string? Sub { get; set; }                // User ID (UUID)
    public string? Name { get; set; }
    public string? PreferredUsername { get; set; }
    public string? Email { get; set; }
    public bool EmailVerified { get; set; }
    public string? GivenName { get; set; }
    public string? FamilyName { get; set; }
}
