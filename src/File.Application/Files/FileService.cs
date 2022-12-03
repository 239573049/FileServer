using File.Application.Contract;
using File.Application.Contract.Eto;
using File.Application.Contract.Files.Dto;
using File.Shared;
using System.IO.Compression;
using System.Text;
using Token.Events;

namespace File.Application;

public class FileService : IFileService
{
    private readonly ILoadEventBus<DeleteFileEto> _loadEventBus;

    public FileService(ILoadEventBus<DeleteFileEto> loadEventBus)
    {
        _loadEventBus = loadEventBus;
    }

    /// <inheritdoc />
    public Task<PagedResultDto<FilesListDto>> GetListAsync(GetListInput input)
    {
        try
        {
            var directories = Directory.GetDirectories(input.Path, input.Name);
            var list = directories.Select(directory => new DirectoryInfo(directory))
                .Select(directoryInfo => new FilesListDto()
                {
                    Name = directoryInfo.Name,
                    CreatedTime = directoryInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    Type = FileType.Directory,
                    FullName = directoryInfo.FullName.Replace("\\", "/"),
                    UpdateTime = directoryInfo.LastWriteTime,
                })
                .ToList();

            var files = Directory.GetFiles(input.Path, input.Name);
            list.AddRange(files.Select(file => new FileInfo(file))
            .Select(fileInfo => new FilesListDto()
            {
                Name = fileInfo.Name,
                CreatedTime = fileInfo.CreationTime.ToString("yyyy-MM-dd HH:mm:ss"),
                FileType = "文件",
                Length = fileInfo.Length,
                FullName = fileInfo.FullName.Replace("\\", "/"),
                Type = FileType.File,
                UpdateTime = fileInfo.LastWriteTime,
            }));

            return Task.FromResult(new PagedResultDto<FilesListDto>(list.Count, list));
        }
        catch (Exception ex)
        {
            throw new BusinessException(ex.Message);
        }
    }

    /// <inheritdoc />
    public async Task<FileContentDto> GetFileContentAsync(string filePath)
    {
        if (!System.IO.File.Exists(filePath))
        {
            throw new BusinessException("文件不存在");
        }

        try
        {
            await using var file = System.IO.File.OpenRead(filePath);

            if (file.Length > 5242880)
            {
                throw new BusinessException("文件大于4MB无法读取");
            }

            using var str = new StreamReader(file);
            return new FileContentDto(await str.ReadToEndAsync(), file.Name);


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
            await using var fileStream = System.IO.File.OpenWrite(input.FilePath);
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

    /// <inheritdoc />
    public async Task DeleteFileAsync(string path)
    {
        if (!System.IO.File.Exists(path))
        {
            throw new BusinessException("文件不存在");
        }

        await _loadEventBus.PushAsync(new DeleteFileEto(path));

        System.IO.File.Delete(path);
        await Task.CompletedTask;
    }

    /// <inheritdoc />
    public async Task CreateAsync(CreateFileInput input)
    {
        if (string.IsNullOrEmpty(input.Name))
        {
            throw new BusinessException("目录名称不能为空");
        }

        var fileName = Path.Combine(input.Path, input.Name);
        if (!Directory.Exists(input.Path))
        {
            throw new BusinessException("文件夹不存在");
        }

        if (System.IO.File.Exists(fileName))
        {
            throw new BusinessException("文件名重复");
        }

        await using var fileStream = System.IO.File.Create(fileName);

        if (!string.IsNullOrEmpty(input.Content))
        {
            await fileStream.WriteAsync(Encoding.UTF8.GetBytes(input.Content));
        }

        await fileStream.FlushAsync();

        fileStream.Close();
    }

    /// <inheritdoc />
    public async Task ExtractToDirectoryAsync(string path, string name)
    {
        if (!Directory.Exists(path))
        {
            throw new BusinessException("文件夹不存在");
        }

        ZipFile.ExtractToDirectory(Path.Combine(path, name), path);

        await Task.CompletedTask;
    }
}