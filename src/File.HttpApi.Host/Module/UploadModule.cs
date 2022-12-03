using File.Shared;

namespace File.HttpApi.Host.Module;

public class UploadModule
{
    public string fileName { get; set; }

    /// <summary>
    /// 当前进度
    /// </summary>
    public long uploadingProgress { get; set; }

    /// <summary>
    /// 是否完成
    /// </summary>
    public bool complete { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public UploadState state { get; set; }

    /// <summary>
    /// 如果发生异常传输异常信息
    /// </summary>
    public string message { get; set; }

    public UploadModule(string fileName, long uploadingProgress, bool complete, UploadState state)
    {
        this.fileName = fileName;
        this.uploadingProgress = uploadingProgress;
        this.complete = complete;
        this.state = state;
    }

    public UploadModule(string fileName, long uploadingProgress, bool complete, UploadState state, string message)
    {
        this.fileName = fileName;
        this.uploadingProgress = uploadingProgress;
        this.complete = complete;
        this.state = state;
        this.message = message;
    }
}