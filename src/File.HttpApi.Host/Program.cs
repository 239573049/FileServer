using File.Application.Contract.Base;
using File.Application.Contract.Files;
using File.Application.Contract.Files.Input;
using File.Application.Extensions;
using File.HttpApi.Host.Filters;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcCore(options =>
{
    options.Filters.Add<ResultFilter>();
});


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

app.Use(async (content, next) =>
{
    try
    {
        await next.Invoke(content);
    }
    catch (BusinessException business)
    {
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto(business.Message, null, business.Code)));
    }
    catch (Exception ex)
    {
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto(ex.Message, null, 500)));
    }

});

app.MapGet("/api/file/list", (IFileService fileService, string? name, string? path, int? page, int? pageSize)
    => fileService.GetListAsync(new GetListInput(name, path, page, pageSize)));

app.MapGet("/api/file/content", (IFileService fileService, string filePath)
    => fileService.GetFileContentAsync(filePath));

app.UseCors("CorsPolicy");

await app.RunAsync();

