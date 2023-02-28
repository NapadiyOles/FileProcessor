using FileProcessor.Logic.Exceptions;
using FileProcessor.Logic.Models;
using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic.Utilities;

internal static class ProcessFolderManager
{
    public static (DirectoryInfo sources, DirectoryInfo results) GetFolders(Config paths)
    {
        var sources = CreateDir(paths.SourcePath);
        var results = CreateDir(paths.ResultPath);

        return (sources, results);
    }

    private static DirectoryInfo CreateDir(string path)
    {
        try
        {
            return Directory.CreateDirectory(path);
        }
        catch (IOException)
        {
            Logger.LogError("The path is invalid: {Path}", path);
            throw new InvalidConfigException();
        }
    }
}