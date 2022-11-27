﻿using File.Application.Contract.Directorys;
using File.Application.Contract.Files;
using File.Application.Contract.Options;
using File.Application.Contract.RouteMappings;
using File.Application.Contract.UserInfos;
using File.Application.Directorys;
using File.Application.EventBus;
using File.Application.Files;
using File.Application.Manage;
using File.Application.RouteMappings;
using File.Application.Statistics;
using File.Application.UserInfos;
using File.Entity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Concurrent;
using System.Text;
using File.Application.Contract.Eto;
using Token.Extensions;
using Token.Handlers;

namespace File.Application.Extensions;

public static class FileApplicationExtension
{
    public static async void AddFileApplication(this IServiceCollection services,string connectString)
    {
        services.AddTransient<IFileService, FileService>();
        services.AddTransient<IDirectoryService, DirectoryService>();
        services.AddTransient<IRouteMappingService, RouteMappingService>();
        services.AddTransient<IUserInfoService, UserInfoService>();
        services.AddTransient<CurrentManage>();

        services.AddDbContext<FileDbContext>(options =>
        {
            options.UseSqlite(connectString);
        });

        await services.AddRouteMapping();

        services.AddEventBus();
        services.AddTransient<FileMiddleware>();
        
        services.AddSingleton(typeof(ILoadEventHandler<InterfaceStatisticsEto>),typeof(InterfaceStatisticsEventHandle));

        services.AddTransient<InterfaceStatisticsMiddleware>();
    }

    public static void AddJwt(this IServiceCollection services,TokenOptions tokenOption)
    {
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true, //是否在令牌期间验证签发者
                    ValidateAudience = true, //是否验证接收者
                    ValidateLifetime = true, //是否验证失效时间
                    ValidateIssuerSigningKey = true, //是否验证签名
                    ValidAudience = tokenOption.Audience, //接收者
                    ValidIssuer = tokenOption.Issuer, //签发者，签发的Token的人
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOption.SecretKey!)) // 密钥
                };
            });
    }
    
    /// <summary>
    /// 加载缓存
    /// </summary>
    /// <param name="services"></param>
    private static async Task AddRouteMapping(this IServiceCollection services)
    {
        var dbContext = services.BuildServiceProvider().GetService<FileDbContext>();
        var routes = new List<RouteMapping>();
        try
        {
            routes.AddRange(await dbContext!.RouteMappings.ToListAsync());
        }
        catch (Exception)
        {
        }

        var mappings = new ConcurrentDictionary<string,RouteMapping>();
        routes.ForEach(x =>
        {
            mappings.TryAdd(x.Route, x);
        });

        services.AddSingleton(mappings);

    }

    /// <summary>
    /// 使用FileApplication
    /// </summary>
    /// <param name="app"></param>
    public static void UseFileApplication(this IApplicationBuilder app)
    {
        app.UseMiddleware<InterfaceStatisticsMiddleware>();
        app.UseMiddleware<FileMiddleware>();
        app.UseAuthentication();
        app.UseAuthorization();
    }
}