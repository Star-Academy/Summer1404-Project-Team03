namespace WebApi.Auth.GetProfile;

public class GetProfileResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = new();
}