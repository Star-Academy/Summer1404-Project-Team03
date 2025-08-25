namespace etl_backend.Services.SsoServices.Exceptions;

public class ApiException : Exception
{
    public int StatusCode { get; }
    public string? ResponseContent { get; }

    public ApiException(string message, int statusCode, string? responseContent = null) 
        : base(message)
    {
        StatusCode = statusCode;
        ResponseContent = responseContent;
    }
}
