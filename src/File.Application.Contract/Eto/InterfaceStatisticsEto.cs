using System;

namespace File.Application.Contract.Eto;

public class InterfaceStatisticsEto
{
    public string Path { get; set; } = null!;

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
    public string Query { get; set; } = null!;
}