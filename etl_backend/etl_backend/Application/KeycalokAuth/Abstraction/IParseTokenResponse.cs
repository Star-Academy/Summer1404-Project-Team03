using etl_backend.Application.KeycalokAuth.Dtos;

namespace etl_backend.Application.KeycalokAuth.Abstraction;

public interface IParseTokenResponse
{
    TokenResponseDto ParseTokenResponse(string json);
}