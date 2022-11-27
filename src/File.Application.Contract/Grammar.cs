using System.Collections.Generic;

namespace File.Application.Contract;

public class Grammar
{
    public static Dictionary<string, string> Language { get; } = new();

    static Grammar()
    {
        Language.Add(".md", "markdown");
        Language.Add(".cs", "csharp");
        Language.Add(".java", "java");
        Language.Add(".js", "javascript");
        Language.Add(".scss", "scss");
        Language.Add(".lua", "lua");
        Language.Add(".json", "json");
        Language.Add(".bat", "bat");
        Language.Add("Dockerfile", "dockerfile");
        Language.Add(".go", "go");
        Language.Add(".xml", "xml");
        Language.Add(".yml", "yml");
    }
}
