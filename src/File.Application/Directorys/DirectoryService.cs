using File.Application.Contract.Base;
using File.Application.Contract.Directorys;
using File.Shared;
using Microsoft.IdentityModel.Tokens;

namespace File.Application.Directorys;

internal class DirectoryService : IDirectoryService
{
    public async Task CreateAsync(string path,string name)
    {
        if (name.IsNullOrEmpty())
        {
            throw new BusinessException("目录名称不能为空");
        }
        
        Directory.CreateDirectory(Path.Combine(path, name));

        await Task.CompletedTask;
    }

    public async Task RenameAsync(string fullName,string path, string name)
    {
        if (name.IsNullOrEmpty())
        {
            throw new BusinessException("目录名称不能为空");
        }
        
        var type = FileHelper.GetFileType(fullName);

        if (type == FileType.File)
        {
            var fileInfo = new FileInfo(fullName);

            fullName = fullName.TrimEnd(name.ToCharArray());
            if (Path.Exists(Path.Combine(fullName, path)))
            {
                throw new BusinessException("已经存在相同文件");
            }
            fileInfo.MoveTo(Path.Combine(fullName, path),false);
        }
        else
        {
            var directoryInfo= new DirectoryInfo(fullName);

            fullName = fullName.TrimEnd(name.ToCharArray());
            if (Path.Exists(Path.Combine(fullName, path)))
            {
                throw new BusinessException("已经存在相同文件夹");
            }
            directoryInfo.MoveTo(Path.Combine(fullName, path));
        }

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(string path)
    {
        Directory.Delete(path,true);
     
        await Task.CompletedTask;
    }
}
