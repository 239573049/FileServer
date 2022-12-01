using System.Collections.Concurrent;
using File.Application.Contract.Base;
using File.Application.Manage;
using File.Entity;
using Microsoft.AspNetCore.Http;

namespace File.Application.Files;

public class FileMiddleware : IMiddleware
{
    private readonly ConcurrentDictionary<string, RouteMapping> _routeMappings;
    private readonly CurrentManage _currentManage;

    public FileMiddleware(ConcurrentDictionary<string, RouteMapping> routeMappings, CurrentManage currentManage)
    {
        _routeMappings = routeMappings;
        _currentManage = currentManage;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // 获取当前访问的是否为静态文件地址 
        var value = _routeMappings.FirstOrDefault(x => context.Request.Path.Value.StartsWith(x.Key));
        if (value.Key != null)
        {
            if (!value.Value.Visitor && _currentManage.IsAuthenticated() == false)
            {
                throw new BusinessException("没有权限返回静态文件", 403);
            }

            var path = value.Value.Path.Replace(context.Request.Path.Value, value.Value.Path);
            if (System.IO.File.Exists(path))
            {
                context.Response.StatusCode = 200;
                var contentType = FileContentTypes.FirstOrDefault(x => context.Request.Path.Value.EndsWith(x.Key));

                if (contentType.Key != null)
                {
                    context.Response.ContentType = contentType.Value;
                }
                else
                {
                    context.Response.ContentType = "application/octet-stream";
                }

                await using var fileStream = System.IO.File.OpenRead(path);

                await fileStream.CopyToAsync(context.Response.Body);
            }
        }
        else
        {
            await next.Invoke(context);
        }
    }

    public static Dictionary<string, string> FileContentTypes = new();

    static FileMiddleware()
    {
        FileContentTypes.Add(".xml", "application/atom+xml");
        FileContentTypes.Add(".apk", "application/vnd.android.package-archive");
        FileContentTypes.Add(".json", "application/json");
        FileContentTypes.Add(".html", "text/html");
        FileContentTypes.Add(".text", "text/plain");
        FileContentTypes.Add(".ini", "text/plain");
        FileContentTypes.Add(".gif", "image/gif");
        FileContentTypes.Add(".jpg", "image/jpeg");
        FileContentTypes.Add(".png", "image/png");
        FileContentTypes.Add(".pdf", "application/pdf");
    }
}