namespace Infrastructure.Exceptions;

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