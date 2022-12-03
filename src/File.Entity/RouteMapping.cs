using File.Shared;

namespace File.Entity;

public class RouteMapping
{
    public Guid Id { get; set; }

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
    /// 创建人
    /// </summary>
    public Guid CreateUserInfoId { get; set; }

    /// <summary>
    /// 如果密码不为空则设置了访问密码
    /// </summary>
    public string? Password { get; set; }

    public virtual UserInfo CreateUserInfo { get; set; }

    protected RouteMapping()
    {

    }

    public RouteMapping(string route, string path, FileType type, string? password, Guid createUserInfoId)
    {
        Route = route;
        Path = path;
        Type = type;
        Password = password;
        CreateUserInfoId = createUserInfoId;
    }
}