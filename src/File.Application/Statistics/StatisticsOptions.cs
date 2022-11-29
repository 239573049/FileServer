namespace File.Application.Statistics;

public class StatisticsOptions
{
    /// <summary>
    /// 忽略掉的uri
    /// </summary>
    public List<string> IgnoreUri { get; set; } = new();
}
