using System;

namespace File.Application.Contract.Statistics.Dto;

public class GetStatisticsDto
{
    public Guid Id { get; set; }

    /// <summary>
    /// 访问响应状态码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool Succeed { get; set; }

    /// <summary>
    /// 响应耗时（ms）
    /// </summary>
    public long ResponseTime { get; set; }

    /// <summary>
    /// 访问接口
    /// </summary>
    public string Path { get; set; } 

    /// <summary>
    /// 访问时间
    /// </summary>
    public DateTime CreatedTime { get; set; }

    /// <summary>
    /// 访问人
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// 访问携带的参数
    /// </summary>
    public string Query { get; set; } 

    public GetStatisticsDto(Guid id, int code, bool succeed, long responseTime, string path, DateTime createdTime, Guid? userId, string query)
    {
        Id = id;
        Code = code;
        Succeed = succeed;
        ResponseTime = responseTime;
        Path = path;
        CreatedTime = createdTime;
        UserId = userId;
        Query = query;
    }
}