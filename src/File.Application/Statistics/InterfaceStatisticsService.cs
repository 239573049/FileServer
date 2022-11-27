using File.Application.Contract.Eto;
using File.Application.Manage;
using Microsoft.AspNetCore.Http;
using Token.Events;

namespace File.Application.Statistics;

public class InterfaceStatisticsMiddleware : IMiddleware
{
    private readonly ILoadEventBus<InterfaceStatisticsEto> _loadEventBus;
    private readonly CurrentManage _currentManage;

    public InterfaceStatisticsMiddleware(ILoadEventBus<InterfaceStatisticsEto> loadEventBus, CurrentManage currentManage)
    {
        _loadEventBus = loadEventBus;
        _currentManage = currentManage;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next.Invoke(context);

        var statistic = new InterfaceStatisticsEto()
        {
            CreatedTime = DateTime.Now,
            Path = context.Request.Path,
            Query = context.Request.QueryString.Value??"",
            UserId = _currentManage.UserId()
        };

        await _loadEventBus.PushAsync(statistic);
    }
}