using System.Threading.Tasks;
using File.Application.Contract.RouteMappings.Dto;
using File.Application.Contract.RouteMappings.Input;

namespace File.Application.Contract.RouteMappings;

public interface IRouteMappingService
{
    /// <summary>
    /// 新增路由映射配置
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task CreateAsync(CreateRouteMappingInput input);

    /// <summary>
    /// 删除指定路由映射
    /// </summary>
    /// <param name="route"></param>
    /// <returns></returns>
    Task DeleteAsync(string route);

    /// <summary>
    /// 获取指定
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    Task<RouteMappingDto> GetAsync(string path);
}