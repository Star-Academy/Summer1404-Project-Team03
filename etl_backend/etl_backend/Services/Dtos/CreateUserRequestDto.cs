namespace etl_backend.Services.Dtos;

public class CreateUserRequestDto
{
    public UserDto User { get; set; } = null!;
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}