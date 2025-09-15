using System.Text.Json;
using Application.Dtos;

namespace Infrastructure.SsoServices.Admin.Mappers;

internal static class RoleMapper
{
    public static RoleDto FromJsonElement(JsonElement element)
    {
        return new RoleDto
        {
            Id = element.GetProperty("id").GetString() ?? string.Empty,
            Name = element.GetProperty("name").GetString() ?? string.Empty
        };
    }

    public static IEnumerable<RoleDto> FromJsonArray(JsonElement array)
        => array.EnumerateArray().Select(FromJsonElement);
}