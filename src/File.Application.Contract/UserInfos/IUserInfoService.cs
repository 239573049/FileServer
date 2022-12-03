using File.Application.Contract.UserInfos.Dto;
using System.Threading.Tasks;

namespace File.Application.Contract;

public interface IUserInfoService
{
    /// <summary>
    /// 登录
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<UserInfoDto> AuthAsync(AuthInput input);

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns></returns>
    Task<UserInfoDto> GetAsync();
}