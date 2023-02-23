using System.Reflection.Metadata;
using System.Text.Json;
using FileProcessor.Logic.Exceptions;
using FileProcessor.Logic.Models;
using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic.Utilities;

internal static class ConfigManager
{
    private const string ConfigPath = @"../../../../config.json";
    public static string GetPath()
    {
        string processPath = "";
        if (File.Exists(ConfigPath))
        {
            processPath = ReadConfig();
        }
        else
        {
            Logger.LogWarning("Config not found");
            
            CreateConfig();
        }

        return processPath;
    }

    private static void CreateConfig()
    {
        var cfg = new Config { PrecessFolderPath = "" };
        var contents = JsonSerializer.Serialize(cfg);
        
        File.WriteAllText(ConfigPath, contents);

        Logger.LogInformation("New config file was created at {Path}", Path.GetFullPath(ConfigPath));
    }

    private static string ReadConfig()
    {
        var json = File.ReadAllText(ConfigPath);

        Config? cfg;
        try
        {
            cfg = JsonSerializer.Deserialize<Config>(json);
        }
        catch (JsonException)
        {
            Logger.LogError("Invalid config file");
            throw new InvalidConfigException();
        }

        var processPath = cfg?.PrecessFolderPath ?? "";

        if (!Path.IsPathFullyQualified(processPath) || Path.HasExtension(processPath) || processPath.Contains(" "))
        {
            Logger.LogError("Path is invalid. It must be full and not contain special chars or file extensions");
            throw new InvalidConfigException();
        }

        return processPath;
    }
}