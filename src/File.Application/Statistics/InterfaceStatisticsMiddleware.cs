using File.Application.Contract.Eto;
using File.Application.Manage;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Text.Json;
using Token.Events;

namespace File.Application.Statistics;

public class InterfaceStatisticsMiddleware : IMiddleware
{
    private readonly ILoadEventBus<InterfaceStatisticsEto> _loadEventBus;
    private readonly CurrentManage _currentManage;
    private readonly StatisticsOptions _statisticsOptions;
    public InterfaceStatisticsMiddleware(ILoadEventBus<InterfaceStatisticsEto> loadEventBus, CurrentManage currentManage, StatisticsOptions statisticsOptions)
    {
        _loadEventBus = loadEventBus;
        _currentManage = currentManage;
        _statisticsOptions = statisticsOptions;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (_statisticsOptions.IgnoreUri.Any(x => x == context.Request.Path))
        {
            await next.Invoke(context);
            return;
        }
        var sw = Stopwatch.StartNew();
        await next.Invoke(context);
        sw.Stop();
        var data = new InterfaceStatisticsEto()
        {
            CreatedTime = DateTime.Now,
            Path = context.Request.Path,
            Succeed = context.Response.StatusCode == 200,
            Code = context.Response.StatusCode,
            ResponseTime = sw.ElapsedMilliseconds,
            Query = context.Request.QueryString.Value ?? "",
            UserId = _currentManage.UserId()
        };
        await _loadEventBus.PushAsync(data);
        
        Console.WriteLine(JsonSerializer.Serialize(data));
    }
}