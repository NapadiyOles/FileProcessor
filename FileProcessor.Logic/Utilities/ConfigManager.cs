using System.Reflection.Metadata;
using System.Text.Json;
using FileProcessor.Logic.Exceptions;
using FileProcessor.Logic.Models;
using Microsoft.Extensions.Logging;

namespace FileProcessor.Logic.Utilities;

public static class ConfigManager
{
    private const string ConfigPath = @"../../../../config.json";
    public static string GetPath(ILogger logger)
    {
        string processPath = "";
        if (File.Exists(ConfigPath))
        {
            processPath = ReadConfig(logger);
        }
        else
        {
            logger.LogWarning("Config not found");
            
            CreateConfig(logger);
        }

        return processPath;
    }

    private static void CreateConfig(ILogger logger)
    {
        var cfg = new Config { PrecessFolderPath = "" };
        var contents = JsonSerializer.Serialize(cfg);
        
        File.WriteAllText(ConfigPath, contents);

        logger.LogInformation("New config file was created at {CfgPath}", Path.GetFullPath(ConfigPath));
    }

    private static string ReadConfig(ILogger logger)
    {
        var json = File.ReadAllText(ConfigPath);

        Config? cfg;
        try
        {
            cfg = JsonSerializer.Deserialize<Config>(json);
        }
        catch (JsonException)
        {
            logger.LogError("Invalid config file");
            throw new InvalidConfigException();
        }

        var processPath = cfg?.PrecessFolderPath ?? "";

        if (!Path.IsPathFullyQualified(processPath) && Path.HasExtension(processPath))
        {
            logger.LogError("Path is invalid or empty");
            throw new InvalidConfigException();
        }

        return processPath;
    }
}