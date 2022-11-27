using File.Application.Contract.Base;
using File.Application.Contract.Directorys;
using File.Application.Contract.Files;
using File.Application.Contract.Files.Input;
using File.Application.Extensions;
using File.Application.Files;
using File.HttpApi.Host.Filters;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcCore(options =>
{
    options.Filters.Add<ResultFilter>();
});

// 跨域策略
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder =>
    {
        corsBuilder.SetIsOriginAllowed((string _) => true).AllowAnyMethod().AllowAnyHeader()
            .AllowCredentials();
    });
});
builder.Services.AddFileApplication();

var app = builder.Build();

// 异常处理中间件
app.Use(async (content, next) =>
{
    try
    {
        await next.Invoke(content);
    }
    catch (BusinessException business)
    {
        content.Response.StatusCode = business.Code;
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto(business.Message, null, business.Code)));
    }
    catch (UnauthorizedAccessException)
    {
        content.Response.StatusCode = 500;
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto("您没有权限操作", null, 500)));
    }
    catch (NotSupportedException)
    {
        content.Response.StatusCode = 500;
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto("路径格式错误", null, 500)));
    }
    catch (DirectoryNotFoundException)
    {
        content.Response.StatusCode = 500;
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto("路径无效", null, 500)));
    }
    catch (Exception ex)
    {
        content.Response.StatusCode = 500;
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto(ex.Message, null, 500)));
    }

});

#region file

app.MapGet("/api/file/list", (IFileService fileService, string? name, string? path, int? page, int? pageSize)
    => fileService.GetListAsync(new GetListInput(name, path, page, pageSize)));

app.MapGet("/api/file/content", (IFileService fileService, string filePath)
    => fileService.GetFileContentAsync(filePath));

app.MapPost("/api/file/save", (IFileService fileService, SaveFileContentInput input)
    => fileService.SaveFileContentAsync(input));

app.MapDelete("/api/file", (IFileService fileService, string path)
    => fileService.DeleteFileAsync(path));

app.MapPost("/api/file", (IFileService fileService, CreateFileInput input)
    => fileService.CreateAsync(input));

app.MapPost("/api/file/extract-directory", (IFileService fileService, string path, string name)
    => fileService.ExtractToDirectoryAsync(path, name));

#endregion

#region directory

app.MapDelete("/api/directory", (IDirectoryService directoryService, string path)
    => directoryService.DeleteAsync(path));

app.MapPost("/api/directory", (IDirectoryService directoryService, string path, string name)
    => directoryService.CreateAsync(path, name));

#endregion

app.UseCors("CorsPolicy");

await app.RunAsync();

