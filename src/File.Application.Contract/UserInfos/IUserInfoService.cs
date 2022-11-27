using System.Threading.Tasks;
using File.Application.Contract.UserInfos.Dto;
using File.Application.Contract.UserInfos.Input;

namespace File.Application.Contract.UserInfos;

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