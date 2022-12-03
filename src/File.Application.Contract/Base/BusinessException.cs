using System;

namespace File.Application.Contract;

public class BusinessException : Exception
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int Code { get; set; }

    public BusinessException(string? message, int code = 400) : base(message)
    {
        Code = code;
    }

    public BusinessException(string? message, Exception? innerException, int code = 400) : base(message, innerException)
    {
        Code = code;
    }
}