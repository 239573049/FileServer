using File.Application.Contract.Statistics;
using File.Application.Contract.Statistics.Dto;
using File.Application.Contract.Statistics.Input;
using Microsoft.EntityFrameworkCore;

namespace File.Application.Statistics;

public class StatisticsService : IStatisticsService
{
    private readonly FileDbContext _fileDbContext;

    public StatisticsService(FileDbContext fileDbContext)
    {
        _fileDbContext = fileDbContext;
    }

    /// <inheritdoc />
    public async Task<StatisticsDto> GetStatisticsAsync()
    {
        var statistics = new StatisticsDto();
        var currentTime = DateTime.Now;

        // 获取昨天时间
        var yesterdayStartTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")).AddDays(-1);
        var yesterdayEndTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")).AddDays(-1);
        statistics.Yesterday = await _fileDbContext
            .InterfaceStatistics
            .CountAsync(x => x.CreatedTime >= yesterdayStartTime && x.CreatedTime <= yesterdayEndTime);

        int week = Convert.ToInt32(currentTime.DayOfWeek);
        week = week == 0 ? 7 : week;
        var lastWeekStartTime = currentTime.AddDays(1 - week - 7);//上周星期一
        var lastWeekEndTime = currentTime.AddDays(7 - week - 7);//上周星期天

        statistics.LastWeek = await _fileDbContext
            .InterfaceStatistics
            .CountAsync(x => x.CreatedTime >= lastWeekStartTime && x.CreatedTime <= lastWeekEndTime);

        statistics.Total = await _fileDbContext.InterfaceStatistics.CountAsync();

        return statistics;
    }

    /// <inheritdoc />
    public async Task<List<PieDto>> GetPieAsync(PieInput input)
    {
        DateTime startTime;
        DateTime endTime;
        switch (input.Type)
        {
            case PieType.Today:
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00"));
                endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                break;
            case PieType.Yesterday:
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 00:00:00")).AddDays(-1);
                endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59")).AddDays(-1);
                break;
            case PieType.Month:
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-01 00:00:00"));
                endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                break;
            case PieType.Total:
                startTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-01-01 00:00:00"));
                endTime = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd 23:59:59"));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var data = _fileDbContext
            .InterfaceStatistics
            .Where(x =>x.CreatedTime >= startTime && x.CreatedTime <= endTime)
            .GroupBy(x=>x.Path)
            .Select(x=>new PieDto
            {
                Type = x.Key,
                Value = x.Count()
            })
            .OrderByDescending(x=>x.Value)
            .Skip(0).Take(10);

        return await data.ToListAsync();
    }
}