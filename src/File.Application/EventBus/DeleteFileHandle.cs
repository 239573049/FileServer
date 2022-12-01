using System.Collections.Concurrent;
using File.Application.Contract.Eto;
using File.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Token.Handlers;

namespace File.Application.EventBus;

public class DeleteFileHandle : ILoadEventHandler<DeleteFileEto>
{
    private readonly ConcurrentDictionary<string,RouteMapping> _routeMappings;
    private readonly IServiceScopeFactory _serviceProvider;

    public DeleteFileHandle(ConcurrentDictionary<string, RouteMapping> routeMappings, IServiceScopeFactory serviceProvider)
    {
        _routeMappings = routeMappings;
        _serviceProvider = serviceProvider;
    }

    public async Task HandleEventAsync(DeleteFileEto eventData)
    {
        var fileDbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FileDbContext>();
        
        var data = await fileDbContext.RouteMappings.FirstOrDefaultAsync(x => x.Path == eventData.Path);
        if (data == null)
        {
            return;
        }

        fileDbContext.RouteMappings.Remove(data);

        await fileDbContext.SaveChangesAsync();

        // 保存成功以后删除缓存
        _routeMappings.Remove(data.Route,out _);
    }
}