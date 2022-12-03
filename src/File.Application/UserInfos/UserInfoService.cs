using File.Application.Contract;
using File.Application.Contract.UserInfos.Dto;
using Microsoft.EntityFrameworkCore;

namespace File.Application;

public class UserInfoService : IUserInfoService
{
    private readonly FileDbContext _fileDbContext;
    private readonly CurrentManage _currentManage;
    public UserInfoService(FileDbContext fileDbContext, CurrentManage currentManage)
    {
        _fileDbContext = fileDbContext;
        _currentManage = currentManage;
    }

    /// <inheritdoc />
    public async Task<UserInfoDto> AuthAsync(AuthInput input)
    {
        var userInfo =
            await _fileDbContext.UserInfos.FirstOrDefaultAsync(x =>
                x.Username == input.Username && x.Password == input.Password);

        if (userInfo == null)
        {
            throw new BusinessException("账号密码错误");
        }

        return new UserInfoDto()
        {
            Id = userInfo.Id,
            Avatar = userInfo.Avatar,
            Password = userInfo.Password,
            Username = userInfo.Username
        };
    }

    /// <inheritdoc />
    public async Task<UserInfoDto> GetAsync()
    {
        var userId = _currentManage.GetUserId();

        var userInfo = await _fileDbContext.UserInfos.FirstOrDefaultAsync(x => x.Id == userId);

        if (userInfo == null)
        {
            throw new BusinessException("账号获取错误", 401);
        }

        return new UserInfoDto()
        {
            Avatar = userInfo.Avatar,
            Id = userInfo.Id,
            Username = userInfo.Username
        };
    }
}