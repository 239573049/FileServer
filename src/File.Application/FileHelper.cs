using File.Application.Contract;
using File.Shared;

namespace File.Application;

public class FileHelper
{
    public static FileType GetFileType(string path)
    {
        if (System.IO.File.Exists(path))
        {
            return FileType.File;
        }
        else if (Directory.Exists(path))
        {
            return FileType.Directory;
        }

        throw new BusinessException("文件或目录不存在");
    }
}