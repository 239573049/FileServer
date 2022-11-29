using File.Application.Contract.Directorys;
using File.Shared;

namespace File.Application.Directorys;

internal class DirectoryService : IDirectoryService
{
    public async Task CreateAsync(string path,string name)
    {
        Directory.CreateDirectory(Path.Combine(path, name));

        await Task.CompletedTask;
    }

    public async Task RenameAsync(string fullName,string path, string name)
    {
        var type = FileHelper.GetFileType(fullName);

        if (type == FileType.File)
        {
            var fileInfo = new FileInfo(fullName);

            fullName = fullName.TrimEnd(name.ToCharArray());
            fileInfo.MoveTo(Path.Combine(fullName, path),false);
        }
        else
        {
            var directoryInfo= new DirectoryInfo(fullName);

            fullName = fullName.TrimEnd(name.ToCharArray());
            directoryInfo.MoveTo(Path.Combine(fullName, path));
        }

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(string path)
    {
        Directory.Delete(path);
     
        await Task.CompletedTask;
    }
}
