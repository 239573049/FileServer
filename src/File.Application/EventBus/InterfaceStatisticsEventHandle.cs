using File.Application.Contract.Eto;
using File.Entity;
using Microsoft.Extensions.DependencyInjection;
using Token.Handlers;

namespace File.Application;

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
        // TODO: 由于Token.EventBus的消息队列处理器是单例 在单例中创建的服务是无法获取到域的DbContext响应通过 IServiceScopeFactory创建这个域的ServiceProvider才能获取到DbContext
        var fileDbContext = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<FileDbContext>();

        var data = new InterfaceStatistics()
        {
            CreatedTime = eventData.CreatedTime,
            Path = eventData.Path,
            UserId = eventData.UserId,
            Query = eventData.Query,
            Code = eventData.Code,
            ResponseTime = eventData.ResponseTime,
            Succeed = eventData.Succeed
        };

        await fileDbContext!.InterfaceStatistics.AddAsync(data);

        await fileDbContext.SaveChangesAsync();
    }
}