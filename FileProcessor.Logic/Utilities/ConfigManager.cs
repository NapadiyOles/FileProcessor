using System.Text.Json;
using FileProcessor.Logic.Exceptions;
using FileProcessor.Logic.Models;
using Microsoft.Extensions.Logging;
using static FileProcessor.Logic.Utilities.LoggingManager;

namespace FileProcessor.Logic.Utilities;

internal static class ConfigManager
{
    private const string ConfigPath = @"../../../../config.json";
    public static Config GetConfig()
    {
        if (!File.Exists(ConfigPath))
        {
            Logger.LogWarning("Config not found");
            CreateConfig();
            throw new InvalidConfigException();
        }

        return ReadConfig();
    }

    private static void CreateConfig()
    {
        var cfg = new Config();
        var contents = JsonSerializer.Serialize(cfg);
        
        File.WriteAllText(ConfigPath, contents);

        Logger.LogInformation("New config file was created at {Path}", Path.GetFullPath(ConfigPath));
    }

    private static Config ReadConfig()
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

        if (cfg is null)
        {
            Logger.LogError("Invalid config file");
            throw new InvalidConfigException();
        }
        
        PathIsValid(cfg.SourcePath, "Source");
        PathIsValid(cfg.ResultPath, "Result");
        
        return cfg;
    }

    private static void PathIsValid(string path, string name)
    {
        if (!Path.IsPathFullyQualified(path) || Path.HasExtension(path) || path.Contains(" "))
        {
            Logger.LogError("{Name} path is invalid. It must be full and not contain special chars or file extensions", name);
            throw new InvalidConfigException();
        }
    }
}