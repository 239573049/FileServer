namespace File.Shared;

public enum UploadState
{
    /// <summary>
    /// 正在处理中
    /// </summary>
    BeingProcessed,
    
    /// <summary>
    /// 处理完成并成功
    /// </summary>
    Complete,
    
    /// <summary>
    /// 处理失败并异常
    /// </summary>
    BeDefeated
}