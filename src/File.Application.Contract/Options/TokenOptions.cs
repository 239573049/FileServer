namespace File.Application.Contract.Options;

public class TokenOptions
{
    /// <summary>
    /// 密钥
    /// </summary>
    public string SecretKey { get; set; } = null!;

    /// <summary>
    /// 发行人
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// 拥护者
    /// </summary>
    public string Audience { get; set; } = null!;

    /// <summary>
    /// 过期时间 (分钟)
    /// </summary>
    public int ExpireHours { get; set; }
}