using FileProcessor.Logic.Utilities;
using Microsoft.Extensions.Logging;

namespace FileProcessor.Logic;

public class ProcessingService
{
    private readonly ILogger _logger;
    private readonly string _precessPath;
    
    public ProcessingService(ILogger<ProcessingService> logger)
    {
        _logger = logger;
        _precessPath = ConfigManager.GetPath(logger);
        ProcessFolderManager.CheckIfExists(_precessPath, logger);
    }

    public void Start()
    {
        _logger.LogInformation("Working......");

    }
}