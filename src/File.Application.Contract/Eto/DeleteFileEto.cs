namespace File.Application.Contract.Eto;

public class DeleteFileEto
{
    /// <summary>
    /// 地址
    /// </summary>
    public string Path { get; set; } 

    public DeleteFileEto(string path)
    {
        Path = path;
    }
}