using File.Application.Contract.Eto;
using File.Application.Manage;
using File.Entity;
using Microsoft.Extensions.DependencyInjection;
using Token.Handlers;

namespace File.Application.EventBus;

/// <summary>
/// 浏览统计事件处理
/// </summary>
public class InterfaceStatisticsEventHandle : ILoadEventHandler<InterfaceStatisticsEto>
{
    private readonly IServiceProvider _serviceProvider;

    public InterfaceStatisticsEventHandle(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task HandleEventAsync(InterfaceStatisticsEto eventData)
    {
        // var fileDbContext = _serviceProvider.GetService<FileDbContext>();
        // var data = new InterfaceStatistics()
        // {
        //     CreatedTime = eventData.CreatedTime,
        //     Path = eventData.Path,
        //     UserId = eventData.UserId,
        //     Query = eventData.Query,
        // };
        //
        // await fileDbContext.InterfaceStatistics.AddAsync(data);
        //
        // await fileDbContext.SaveChangesAsync();
    }
}