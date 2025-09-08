using Infrastructure.Dtos;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Identity.Abstractions;

public interface ITokenCookieService
{
    void SetTokens(HttpResponse response, TokenResponseDto tokenResponse);
    void RemoveTokens(HttpResponse response);   
}
