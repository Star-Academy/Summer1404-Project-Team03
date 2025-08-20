namespace etl_backend.Services;

public class EditUserRequestDto
{
    public UserDto User { get; set; } = null!;
    public IEnumerable<string>? Roles { get; set; }
}