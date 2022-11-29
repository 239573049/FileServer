using System.Collections.Concurrent;
using File.Application.Contract.Base;
using File.Application.Contract.RouteMappings;
using File.Application.Contract.RouteMappings.Dto;
using File.Application.Contract.RouteMappings.Input;
using File.Application.Manage;
using File.Entity;
using File.Shared;
using Microsoft.EntityFrameworkCore;

namespace File.Application.RouteMappings;

public class RouteMappingService : IRouteMappingService
{
    private readonly ConcurrentDictionary<string,RouteMapping> _routeMappings;
    private readonly FileDbContext _fileDbContext;
    private readonly CurrentManage _currentManage;

    public RouteMappingService(ConcurrentDictionary<string, RouteMapping> routeMappings, FileDbContext fileDbContext, CurrentManage currentManage)
    {
        _routeMappings = routeMappings;
        _fileDbContext = fileDbContext;
        _currentManage = currentManage;
    }

    /// <inheritdoc />
    public async Task CreateAsync(CreateRouteMappingInput input)
    {
        if (await _fileDbContext.RouteMappings.AnyAsync(x => x.Route == input.Route || x.Path == input.Path))
        {
            throw new BusinessException("已经存在相同路由映射配置");
        }

        if (!Path.Exists(input.Path))
        {
            throw new BusinessException("路径错误，不存在当前文件或者目录");
        }
        
        var routeMapping = new RouteMapping(input.Route, input.Path, FileHelper.GetFileType(input.Path), input.Visitor,_currentManage.GetUserId());

        await _fileDbContext.RouteMappings.AddAsync(routeMapping);

        await _fileDbContext.SaveChangesAsync();

        // 保存成功以后添加到缓存中
        _routeMappings.TryAdd(routeMapping.Route, routeMapping);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(string route)
    {
        var data = await _fileDbContext.RouteMappings.FirstOrDefaultAsync(x => x.Route == route);
        if (data == null)
        {
            throw new BusinessException("删除失败配置不存在");
        }

        _fileDbContext.RouteMappings.Remove(data);

        await _fileDbContext.SaveChangesAsync();

        // 保存成功以后删除缓存
        _routeMappings.Remove(data.Route,out _);
    }

    /// <inheritdoc />
    public async Task<RouteMappingDto> GetAsync(string path)
    {
        var data = await _fileDbContext.RouteMappings.FirstOrDefaultAsync(x => x.Path == path);

        if (data == null)
        {
            return new RouteMappingDto();
        }

        return new RouteMappingDto()
        {
            CreateUserInfoId = data.CreateUserInfoId,
            Id = data.Id,
            Path = data.Path,
            Route = data.Route,
            Type = data.Type,
            Visitor = data.Visitor
        };
    }
    
}