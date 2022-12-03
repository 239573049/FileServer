using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using File.Application.Contract;

namespace File.Application;

public class CurrentLimitingMiddleware : IMiddleware
{
    /// <summary>
    /// 每多少秒限流的请求上线
    /// </summary>
    public static int Count = 60;
    /// <summary>
    /// 多少秒限流
    /// </summary>
    public static int Seconds = 60;
    private readonly IMemoryCache _memory;
    public CurrentLimitingMiddleware(
        IMemoryCache memory
    )
    {
        this._memory = memory;
    }

    public class Data
    {
        public DateTime Time { get; set; }

        public int Count { get; set; }
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var targetInfo = $"{context.Connection.RemoteIpAddress?.MapToIPv4()}:{context.Request.Path}";//获取客户的ip地址然后进行缓存
        if (!_memory.TryGetValue(targetInfo, out Data? data))
        {
            data = new Data
            {
                Count = 1,
                Time = DateTime.Now.AddSeconds(Seconds)//记录本次添加时间然后叠加配置的秒数
            };
            _memory.Set(targetInfo, data, data.Time);
        }
        else
        {
            if (data?.Count > Count)
            {
                context.Response.StatusCode = 413;
                throw new BusinessException(
                    $"ip：{context.Connection.RemoteIpAddress?.MapToIPv4()}，请求速度过快，超过每{Seconds}秒{Count}次限制，请稍后请求", 413);
            }

            data!.Count++;
            _memory.Set(targetInfo, data, data.Time);//没有超过就继续添加过期时间继续使用第一次添加的
        }

        await next.Invoke(context);
    }
}