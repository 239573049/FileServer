using File.Shared;

namespace File.Application.Contract;

public class CreateRouteMappingInput
{
    /// <summary>
    /// 路由
    /// </summary>
    public string Route { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public FileType Type { get; set; }

    /// <summary>
    /// 如果密码不为空则设置了访问密码
    /// </summary>
    public string? Password { get; set; }
}