using System.Collections.Generic;
using File.Application.Contract.Statistics.Dto;
using System.Threading.Tasks;
using File.Application.Contract.Statistics.Input;

namespace File.Application.Contract.Statistics;

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
}