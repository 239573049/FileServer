using File.Application.Contract.Eto;
using File.Entity;
using Microsoft.Extensions.DependencyInjection;
using Token.Handlers;

namespace File.Application.EventBus;

/// <summary>
/// 浏览统计事件处理
/// </summary>
public class InterfaceStatisticsEventHandle : ILoadEventHandler<InterfaceStatisticsEto>
{
    private readonly IServiceScopeFactory _serviceProvider;

    public InterfaceStatisticsEventHandle(IServiceScopeFactory serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task HandleEventAsync(InterfaceStatisticsEto eventData)
    {
        var fileDbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FileDbContext>();

        var data = new InterfaceStatistics()
        {
            CreatedTime = eventData.CreatedTime,
            Path = eventData.Path,
            UserId = eventData.UserId,
            Query = eventData.Query,
        };

        await fileDbContext!.InterfaceStatistics.AddAsync(data);

        await fileDbContext.SaveChangesAsync();
    }
}