using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text.Json;
using File.Application.Contract;

namespace File.Application;

public class CurrentManage
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentManage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool? IsAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated;
    }

    public Guid? UserId()
    {
        var id = GetClaimValueByType("Id")?.FirstOrDefault();
        return string.IsNullOrEmpty(id) ? default(Guid?) : Guid.Parse(id);
    }

    public Guid GetUserId()
    {
        var userId = UserId();

        if (userId == null)
        {
            throw new BusinessException("账号未登录", 401);
        }

        return (Guid)userId;
    }

    public T? UserInfo<T>()
    {
        var userInfo = GetClaimValueByType(ClaimTypes.Sid)?.FirstOrDefault();
        return string.IsNullOrEmpty(userInfo) ? default : JsonSerializer.Deserialize<T>(userInfo);
    }

    private IEnumerable<string>? GetClaimValueByType(string claimType)
    {
        return _httpContextAccessor.HttpContext?.User.Claims?.Where(item => item.Type == claimType)
            .Select(item => item.Value);
    }
}