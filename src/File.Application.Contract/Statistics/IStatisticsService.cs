using File.Application.Contract.Statistics.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace File.Application.Contract;

public interface IStatisticsService
{
    /// <summary>
    /// 获取统计数量
    /// </summary>
    /// <returns></returns>
    Task<StatisticsDto> GetStatisticsAsync();

    /// <summary>
    /// 获取饼图数据
    /// </summary>
    /// <returns></returns>
    Task<List<PieDto>> GetPieAsync(PieInput input);

    /// <summary>
    /// 获取访问列表
    /// </summary>
    /// <returns></returns>
    Task<PagedResultDto<GetStatisticsDto>> GetListAsync(GetStatisticsInput input);
}