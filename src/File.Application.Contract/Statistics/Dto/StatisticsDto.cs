namespace File.Application.Contract.Statistics.Dto;

public class StatisticsDto
{
    /// <summary>
    /// 昨天访问量
    /// </summary>
    public int Yesterday { get; set; }

    /// <summary>
    /// 上星期访问量
    /// </summary>
    public int LastWeek { get; set; }
    
    /// <summary>
    /// 总访问量
    /// </summary>
    public int Total { get; set; }
}