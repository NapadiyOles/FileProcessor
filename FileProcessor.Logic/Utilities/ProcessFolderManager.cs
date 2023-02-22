using Microsoft.Extensions.Logging;

namespace FileProcessor.Logic.Utilities;

public static class ProcessFolderManager
{
    public static void CheckIfExists(string path, ILogger logger)
    {
        if (Directory.Exists(path))
        {
            
        }
        else
        {
            Directory.CreateDirectory(path);
        }
    }
}