using Poker.NET.Engine;
using Poker.NET.LookupGeneration;
using Poker.NET.LookupGeneration.Data;
using Poker.NET.LookupGeneration.Extensions;
using Poker.NET.Worker.Helpers;

namespace Poker.NET.Worker;

public class Worker : BackgroundService
{
    private readonly IHandComparisonFileHelper _fileHelper;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Worker> _logger;
    private HandComparisonTree? _lastTree;

    public Worker(
        IHandComparisonFileHelper fileHelper,
        IHostApplicationLifetime lifetime,
        IConfiguration configuration,
        ILogger<Worker> logger)
    {
        _fileHelper = fileHelper;
        _lifetime = lifetime;
        _configuration = configuration;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string checkpointsPath = _configuration["CheckpointsPath"]!;
        if (!Directory.Exists(checkpointsPath))
        {
            Directory.CreateDirectory(checkpointsPath);
            _logger.LogInformation("Created missing checkpoints directory at {CheckpointsPath}.", checkpointsPath);
        }

        _logger.LogInformation("Checking checkpoints directory to load most recent tree.");
        LinkedList<string> checkpointDirectories = [];
        foreach (string? directory in Directory.GetDirectories(checkpointsPath)
                .Where(d => Directory.EnumerateFiles(d, "*.bin").Any())
                .OrderByDescending(d => d))
        {
            if (directory is not null) checkpointDirectories.AddLast(directory);
        }

        if (checkpointDirectories.LastOrDefault() is string lastDirectoryPath)
        {
            _lastTree = await _fileHelper.LoadFromFileAsync(lastDirectoryPath, stoppingToken);
            _logger.LogInformation("Loaded most recent tree from directory {LastDirectoryPath}.", lastDirectoryPath);
        }
        _lastTree ??= []; // In case no tree was loaded
        
        try
        {
            int skippable = (int)_lastTree.Count;
            IEnumerable<HoldemHand> allHands = HandsGenerator.GetAllHands().Skip(skippable);
            int initialChunkSize = 500;
            double growthFactor = 0.05;
            foreach (HoldemHand[] chunk in allHands.ChunkGrowing(initialChunkSize, growthFactor))
            {
                foreach (HoldemHand hand in chunk)
                {
                    _lastTree.Add(hand);
                    stoppingToken.ThrowIfCancellationRequested();
                }
                _logger.LogInformation("Processed chunk of {ChunkLength} hands.", chunk.Length);

                // Save chunk to checkpoint directory
                string currentDateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                string directoryPath = Path.Combine(checkpointsPath, currentDateTime);
                Directory.CreateDirectory(directoryPath);
                bool success = await _fileHelper.WriteToFileAsync(directoryPath, _lastTree, stoppingToken);
                _logger.LogInformation("Wrote chunk of {ChunkLength} hands to checkpoint directory at {DirectoryPath}.", chunk.Length, directoryPath);
                
                // Rotate checkpoint directories and delete oldest if necessary
                if (success)
                {
                    checkpointDirectories.AddLast(directoryPath);
                    while (checkpointDirectories.Count > 3)
                    {
                        string oldestDirectory = checkpointDirectories.First!.Value;
                        Directory.Delete(oldestDirectory, true);
                        checkpointDirectories.RemoveFirst();
                        _logger.LogInformation("Deleted oldest checkpoint directory {OldestDirectory}.", oldestDirectory);
                    }
                }

                stoppingToken.ThrowIfCancellationRequested();
            }
            _logger.LogInformation("Processed all hands.");
        }
        catch (OperationCanceledException opex)
        {
            _logger.LogError(opex, "Worker was cancelled.");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Worker encountered an unhandled exception.");
        }
        finally
        {
            _logger.LogInformation("Worker is stopping.");
            _lifetime.StopApplication();
        }
    }
}
