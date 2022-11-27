namespace File.Entity;

public class InterfaceStatistics
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