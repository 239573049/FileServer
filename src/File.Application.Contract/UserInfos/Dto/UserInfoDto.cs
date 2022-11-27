using System;

namespace File.Application.Contract.UserInfos.Dto;

public class UserInfoDto
{
    public Guid Id { get; set; }
        
    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string Avatar  { get; set; }
}