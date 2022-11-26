using File.Application.Contract.Base;
using File.Application.Contract.Files;
using File.Application.Contract.Files.Dto;
using File.Application.Contract.Files.Input;
using System.Text;

namespace File.Application.Files;

public class FileService : IFileService
{
    /// <inheritdoc />
    public Task<PagedResultDto<FilesListDto>> GetListAsync(GetListInput input)
    {
        var list = new List<FilesListDto>();
        var directories = Directory.GetDirectories(input.Path, input.Name);
        foreach (var directory in directories)
        {
            var directoryInfo = new DirectoryInfo(directory);
            list.Add(new FilesListDto()
            {
                Name = directoryInfo.Name,
                CreatedTime = directoryInfo.CreationTime,
                Type = FileType.Directory,
                FullName = directoryInfo.FullName,
                UpdateTime = directoryInfo.LastWriteTime,
            });
        }

        var files = Directory.GetFiles(input.Path, input.Name);
        foreach (var file in files)
        {
            var fileInfo = new FileInfo(file);
            list.Add(new FilesListDto()
            {
                Name = fileInfo.Name,
                CreatedTime = fileInfo.CreationTime,
                FileType = "文件",
                Length = fileInfo.Length,
                FullName = fileInfo.FullName,
                Type = FileType.File,
                UpdateTime = fileInfo.LastWriteTime,
            });
        }

        return Task.FromResult(new PagedResultDto<FilesListDto>(list.Count, list));
    }

    /// <inheritdoc />
    public async Task<string> GetFileContentAsync(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
        {
            throw new BusinessException("文件不存在");
        }

        try
        {
            await using var file = System.IO.File.OpenRead(filePath);

            if (file.Length > (1024 * 1024) * 4)
            {
                throw new BusinessException("文件大于4MB无法读取");
            }

            using var str = new StreamReader(file);

            return await str.ReadToEndAsync();

        }
        catch (Exception exception)
        {
            throw new BusinessException("读取文件错误", exception);
        }
    }

    /// <inheritdoc />
    public async Task SaveFileContentAsync(SaveFileContentInput input)
    {
        if (!System.IO.File.Exists(input.FilePath))
        {
            throw new BusinessException("文件不存在");
        }

        try
        {
            using var fileStream = System.IO.File.OpenWrite(input.FilePath);
            fileStream.Position = 0;
            await fileStream.WriteAsync(Encoding.UTF8.GetBytes(input.Content));
            await fileStream.FlushAsync();
            fileStream.Close();
        }
        catch (Exception)
        {

            throw;
        }
    }
}