using File.Application.Contract.Files.Dto;
using System.Threading.Tasks;

namespace File.Application.Contract
{
    public interface IFileService
    {
        /// <summary>
        /// 获取文件夹列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<FilesListDto>> GetListAsync(GetListInput input);

        /// <summary>
        /// 指定获取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task<FileContentDto> GetFileContentAsync(string filePath);

        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SaveFileContentAsync(SaveFileContentInput input);

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task DeleteFileAsync(string path);

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAsync(CreateFileInput input);

        /// <summary>
        /// 解压压缩文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        Task ExtractToDirectoryAsync(string path, string name);
    }
}
