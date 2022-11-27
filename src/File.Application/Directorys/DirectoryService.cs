using File.Application.Contract.Base;
using File.Application.Contract.Directorys;

namespace File.Application.Directorys;

internal class DirectoryService : IDirectoryService
{
    public async Task CreateAsync(string path,string name)
    {
        Directory.CreateDirectory(Path.Combine(path, name));

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(string path)
    {
        Directory.Delete(path,true);
     
        await Task.CompletedTask;
    }
}
