using File.Shared;

namespace File.HttpApi.Host.Module;

public class UploadModule
{
    public string FileName { get; set; }

    /// <summary>
    /// 当前进度
    /// </summary>
    public long UploadingProgress { get; set; }

    /// <summary>
    /// 是否完成
    /// </summary>
    public bool Complete { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public UploadState State { get; set; }
    
    /// <summary>
    /// 如果发生异常传输异常信息
    /// </summary>
    public string Message { get; set; }

    public UploadModule(string fileName, long uploadingProgress, bool complete, UploadState state)
    {
        FileName = fileName;
        UploadingProgress = uploadingProgress;
        Complete = complete;
        State = state;
    }

    public UploadModule(string fileName, long uploadingProgress, bool complete, UploadState state, string message)
    {
        FileName = fileName;
        UploadingProgress = uploadingProgress;
        Complete = complete;
        State = state;
        Message = message;
    }
}