
namespace Application.Dtos;

public class UserCreateDto: BaseUserDto
{
    
    public string Username { get; set; } = string.Empty;
    
    public string? Password { get; set; }
    
}