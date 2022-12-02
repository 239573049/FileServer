using File.Application.Contract.Base;
using File.Application.Contract.Directorys;
using File.Application.Contract.Files;
using File.Application.Contract.Files.Input;
using File.Application.Contract.Options;
using File.Application.Contract.RouteMappings;
using File.Application.Contract.RouteMappings.Input;
using File.Application.Contract.UserInfos;
using File.Application.Contract.UserInfos.Input;
using File.Application.Extensions;
using File.HttpApi.Host.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using File.Application.Contract.Statistics;
using File.Application.Contract.Statistics.Input;
using File.HttpApi.Host.Hubs;
using Microsoft.AspNetCore.Mvc;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvcCore(options => { options.Filters.Add<ResultFilter>(); });

// �������
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", corsBuilder =>
    {
        corsBuilder.SetIsOriginAllowed((string _) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddFileApplication(builder.Configuration.GetConnectionString("Default")!);

var configurationSection = builder.Configuration.GetSection(nameof(TokenOptions));

builder.Services.Configure<TokenOptions>(configurationSection);

builder.Services.AddHttpContextAccessor();

builder.Services.AddJwt(configurationSection.Get<TokenOptions>()!);
builder.Services.AddSignalR()
    .AddHubOptions<UploadingHub>(options =>
    {
        options.MaximumReceiveMessageSize = 1024 * 36;
    })
    .AddJsonProtocol()
    .AddMessagePackProtocol();

var app = builder.Build();

// �쳣�����м��
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
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto("��û��Ȩ�޲���", null, 500)));
    }
    catch (NotSupportedException)
    {
        content.Response.StatusCode = 500;
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto("·����ʽ����", null, 500)));
    }
    catch (DirectoryNotFoundException)
    {
        content.Response.StatusCode = 500;
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto("·����Ч", null, 500)));
    }
    catch (Exception ex)
    {
        content.Response.StatusCode = 500;
        await content.Response.Body.WriteAsync(
            JsonSerializer.SerializeToUtf8Bytes(new HttpResultDto(ex.Message, null, 500)));
    }
});

// ע��ʹ��˳���ֹ����
app.UseFileApplication();
app.UseStaticFiles();

#region file

app.MapGet("/api/file/list", (IFileService fileService, string? name, string? path, int? page, int? pageSize)
        => fileService.GetListAsync(new GetListInput(name, path, page, pageSize)))
    .RequireAuthorization();

app.MapGet("/api/file/content", (IFileService fileService, string filePath)
        => fileService.GetFileContentAsync(filePath))
    .RequireAuthorization();

app.MapPost("/api/file/save", (IFileService fileService, SaveFileContentInput input)
        => fileService.SaveFileContentAsync(input))
    .RequireAuthorization();

app.MapDelete("/api/file", (IFileService fileService, string path)
        => fileService.DeleteFileAsync(path))
    .RequireAuthorization();

app.MapPost("/api/file", (IFileService fileService, CreateFileInput input)
        => fileService.CreateAsync(input))
    .RequireAuthorization();

app.MapPost("/api/file/extract-directory", (IFileService fileService, string path, string name)
        => fileService.ExtractToDirectoryAsync(path, name))
    .RequireAuthorization();

app.MapPost("/api/file/uploading", async (string path, string name, [FromForm] IFormFile file) =>
{
    try
    {
        var p = Path.Combine(path, name.TrimEnd(file.FileName.ToCharArray()));
        if (!Directory.Exists(p))
        {
            Directory.CreateDirectory(p);
        }

        await using var fileStream =
            System.IO.File.Open(Path.Combine(p, file.FileName), FileMode.Create, FileAccess.Write);
        await file.CopyToAsync(fileStream);
        fileStream.Close();
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}).RequireAuthorization();

#endregion

#region directory

app.MapDelete("/api/directory", (IDirectoryService directoryService, string path)
        => directoryService.DeleteAsync(path))
    .RequireAuthorization()
    .WithDescription("ɾ���ļ���");

app.MapPost("/api/directory", (IDirectoryService directoryService, string path, string name)
        => directoryService.CreateAsync(path, name))
    .RequireAuthorization()
    .WithDescription("����ļ�������");

app.MapPut("/api/directory/rename", (IDirectoryService directoryService, string fullName, string path, string name)
        => directoryService.RenameAsync(fullName, path, name))
    .RequireAuthorization()
    .WithDescription("�޸��ļ�������");


app.MapGet("/api/directory/drives", (IDirectoryService directoryService)
    => directoryService.GetDrives())
    .RequireAuthorization()
    .WithDescription("��ȡӲ���б�");

#endregion

#region routeMapping

app.MapPost("/api/route-mapping", (IRouteMappingService routeMappingService, CreateRouteMappingInput input)
        => routeMappingService.CreateAsync(input))
    .RequireAuthorization();

app.MapDelete("/api/route-mapping", (IRouteMappingService routeMappingService, string route)
        => routeMappingService.DeleteAsync(route))
    .RequireAuthorization();

app.MapGet("/api/route-mapping", (IRouteMappingService routeMappingService, string path)
        => routeMappingService.GetAsync(path))
    .RequireAuthorization();

#endregion

#region auth

app.MapPost("/api/auth",
    async (IUserInfoService userInfoService, AuthInput input, IOptions<TokenOptions> tokenOptions)
        =>
    {
        var userInfo = await userInfoService.AuthAsync(input);

        var claims = new[]
        {
            new Claim("userInfo", JsonSerializer.Serialize(userInfo)),
            new Claim("Id", userInfo.Id.ToString())
        };

        var cred = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.Value.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            tokenOptions.Value.Issuer, // ǩ����
            tokenOptions.Value.Audience, // ������
            claims, // payload
            expires: DateTime.Now.AddHours(tokenOptions.Value.ExpireHours), // ����ʱ��
            signingCredentials: cred); // ����
        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    });
app.MapGet("/api/user-info", (IUserInfoService userInfoService)
        => userInfoService.GetAsync())
    .RequireAuthorization();

#endregion

#region statistics

app.MapGet("/api/statistics/statistics", (IStatisticsService statisticsService)
        => statisticsService.GetStatisticsAsync())
    .RequireAuthorization()
    .WithDescription("��ȡͳ��");


app.MapGet("/api/statistics/pie", (IStatisticsService statisticsService, PieType type)
        => statisticsService.GetPieAsync(new PieInput() { Type = type }))
    .RequireAuthorization()
    .WithDescription("��ȡͳ�Ʊ�ͼ");

app.MapGet("/api/statistics/list", (IStatisticsService statisticsService, string keywords, int page, int pageSize)
    => statisticsService.GetListAsync(new GetStatisticsInput(keywords, page, pageSize)))
    .RequireAuthorization()
    .WithDescription("��ȡ�����б�");

#endregion

app.UseCors("CorsPolicy");

app.MapHub<UploadingHub>("/uploading");

await app.RunAsync();