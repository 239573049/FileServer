namespace File.Application.Contract.Files.Input;
public class CreateFileInput
{
    public string Path { get; set; }

    public string Name { get; set; }

    public string Content { get; set; }

    public CreateFileInput(string path, string name, string content)
    {
        Path = path;
        Name = name;
        Content = content;
    }
}
