using FileProcessor.Logic.Exceptions;
using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic.Utilities;

internal static class ProcessFolderManager
{
    public static (DirectoryInfo sources, DirectoryInfo results) GetFolders(string path)
    {
        var dir = new DirectoryInfo(path);
        
        try
        {
            dir.Create();
        }
        catch (IOException)
        {
            Logger.LogError("The path is invalid: {Path}", path);
            throw new InvalidConfigException();
        }
        
        var sources = dir.CreateSubdirectory("sources");
        var results = dir.CreateSubdirectory("results");

        return (sources, results);
    }
}