using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace File.Application.Contract;

public interface IDirectoryService
{
    /// <summary>
    /// 删除文件夹
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    Task DeleteAsync(string path);

    /// <summary>
    /// 创建文件夹
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    Task CreateAsync(string path, string name);

    /// <summary>
    /// 重命名
    /// </summary>
    /// <param name="fullName">原地址</param>
    /// <param name="path">新目录</param>
    /// <param name="name">原名称</param>
    /// <returns></returns>
    Task RenameAsync(string fullName, string path, string name);

    Task<IEnumerable<DriveInfo>> GetDrives();
}
