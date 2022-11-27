using System.Threading.Tasks;

namespace File.Application.Contract.Directorys;

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
}
