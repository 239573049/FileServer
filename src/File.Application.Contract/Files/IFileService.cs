using File.Application.Contract.Base;
using File.Application.Contract.Files.Dto;
using File.Application.Contract.Files.Input;
using System.Threading.Tasks;

namespace File.Application.Contract.Files
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
        /// <param name="filePath">文件地址</param>
        /// <param name="content">文件内容</param>
        /// <returns></returns>
        Task SaveFileContentAsync(SaveFileContentInput input);
    }
}
