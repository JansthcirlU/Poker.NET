using Poker.NET.Engine;
using Poker.NET.LookupGeneration;
using Poker.NET.LookupGeneration.Data;
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
        // If more than three checkpoint directories exist, delete the oldest one
        string checkpointsPath = _configuration["CheckpointsPath"]!;
        string[] directories = Directory.Exists(checkpointsPath)
            ? [.. Directory.GetDirectories(checkpointsPath).OrderByDescending(d => d)]
            : [];
        if (directories.Length > 2)
        {
            string oldestDirectory = directories.Last();
            Directory.Delete(oldestDirectory, true);
            _logger.LogInformation("Deleted oldest checkpoint directory {OldestDirectory}.", oldestDirectory);
        }

        // Load latest checkpoint directory if it exists
        string? latestDirectory = directories.FirstOrDefault();
        if (latestDirectory is not null)
        {
            _logger.LogInformation("Loading latest checkpoint directory {LatestDirectory}.", latestDirectory);
            _lastTree = await _fileHelper.LoadFromFileAsync(latestDirectory, stoppingToken);
        }

        // Create checkpoint directory for current run
        string currentDateTime = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
        string directoryPath = Path.Combine(checkpointsPath, currentDateTime);
        Directory.CreateDirectory(directoryPath);
        _logger.LogInformation("Created checkpoint directory {DirectoryPath}.", directoryPath);

        // Generate hand comparison tree
        try
        {
            int skippable = (int)_lastTree!.Count;
            IEnumerable<HoldemHand> allHands = HandsGenerator.GetAllHands().Skip(skippable);
            _logger.LogInformation("Starting to generate hand comparison tree for all hands.");
            int chunksProcessed = 0;
            int chunkSize = 500;
            foreach (IEnumerable<HoldemHand> chunk in allHands.Chunk(chunkSize))
            {
                _logger.LogInformation("Processing hands {ChunkStart} through {ChunkEnd}.", skippable + chunksProcessed * chunkSize, skippable + (chunksProcessed + 1) * chunkSize);
                foreach (HoldemHand hand in chunk)
                {
                    _lastTree.Add(hand);
                    if (stoppingToken.IsCancellationRequested) break;
                }
                _logger.LogInformation("Writing chunk {ChunkNumber} to file.", chunksProcessed);
                await _fileHelper.WriteToFileAsync(directoryPath, _lastTree, stoppingToken);
                
                if (stoppingToken.IsCancellationRequested) break;
                chunksProcessed++;
            }
        }
        catch (Exception)
        {
            _logger.LogCritical("Worker encountered an unhandled exception and will now stop.");
        }
        finally
        {
            // Write hand comparison tree to file
            _lifetime.StopApplication();
        }
    }
}
