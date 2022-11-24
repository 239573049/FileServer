namespace File.Application.Contract.Base;

public class HttpResultDto
{
    public int Code { get; set; }

    public string? Message { get; set; }

    public object? Data { get; set; }

    public HttpResultDto()
    {
    }

    public HttpResultDto(int code, object? data = null, string? message = null)
    {
        Code = code;
        Message = message;
        Data = data;
    }

    public HttpResultDto(string? message, object? data = null, int code = 200)
    {
        Code = code;
        Message = message;
        Data = data;
    }
}