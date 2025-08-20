namespace etl_backend.Services.Dtos;

public class EditUserRequestDto
{
    public UserDto User { get; set; } = null!;
    public IEnumerable<string>? Roles { get; set; }
}