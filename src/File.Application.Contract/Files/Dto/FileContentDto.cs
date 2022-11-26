using System.Linq;

namespace File.Application.Contract.Files.Dto;

public class FileContentDto
{
    public string Content { get; set; }

    /// <summary>
    /// 语法高亮
    /// </summary>
    public string Language { get; set; }

    public FileContentDto(string content, string fileName)
    {
        Content = content;
        Language = "text";

        // 获取当前后缀语法高亮
        var value = Grammar.Language.FirstOrDefault(x => fileName.EndsWith(x.Key));
        if (value.Key != null)
        {
            Language = value.Value;
        }
    }
}
