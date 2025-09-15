using Infrastructure.Dtos;

namespace Infrastructure.SsoServices.User.Abstractions;

public interface IParseTokenResponse
{
    TokenResponseDto ParseTokenResponse(string json);
}