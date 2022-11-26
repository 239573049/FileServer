namespace File.Application.Contract.Files.Input;

public class SaveFileContentInput
{
    public SaveFileContentInput(string filePath, string content)
    {
        FilePath = filePath;
        Content = content;
    }

    /// <summary>
    /// 文件地址
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// 文件内容
    /// </summary>
    public string Content { get; set; }
}
