using File.Application.Contract.Directorys;
using File.Application.Contract.Files;
using File.Application.Directorys;
using File.Application.Files;
using Microsoft.Extensions.DependencyInjection;

namespace File.Application.Extensions;

public static class FileApplicationExtension
{
    public static void AddFileApplication(this IServiceCollection services)
    {
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IDirectoryService, DirectoryService>();

        MessagePack.MessagePackSerializer.DefaultOptions = MessagePack.Resolvers.ContractlessStandardResolver.Options;
    }

}
