﻿using File.Shared;

namespace File.Application.Contract.RouteMappings.Input;

public class CreateRouteMappingInput
{
    /// <summary>
    /// 路由
    /// </summary>
    public string Route { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public FileType Type { get; set; }

    /// <summary>
    /// 是否允许其他人访问
    /// </summary>
    public bool Visitor { get; set; }
}