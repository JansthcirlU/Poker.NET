using Poker.NET.Engine;
using Poker.NET.LookupGeneration;
using Poker.NET.LookupGeneration.Data;

namespace Poker.NET.Worker.Helpers;

public class HandComparisonFileHelper : IHandComparisonFileHelper
{
    private readonly ILogger<HandComparisonFileHelper> _logger;

    public HandComparisonFileHelper(ILogger<HandComparisonFileHelper> logger)
    {
        _logger = logger;
    }

    public async Task<bool> WriteToFileAsync(string directoryPath, HandComparisonTree tree, CancellationToken cancellationToken)
    {
        uint i = 0;
        ulong saved = 0;
        string binPath = Path.Combine(directoryPath, $"{i}.bin");
        
        try
        {
            Directory.CreateDirectory(directoryPath);

            foreach (IEnumerable<HoldemHand> bin in tree.Bins)
            {
                binPath = Path.Combine(directoryPath, $"{i}.bin");
                using FileStream stream = new(binPath, FileMode.Create, FileAccess.Write);
                IEnumerable<byte[]> binBytes = bin.Select(h => BinaryHandHelper.Pack(h, i));
                foreach (byte[] bytes in binBytes)
                {
                    await stream.WriteAsync(bytes, cancellationToken);
                    saved++;
                }
                i++;
            }

            _logger.LogInformation("Wrote {BinCount} bin(s) and {SavedCount} entries to directory {DirectoryPath}.", i, saved, directoryPath);
            return true;
        }
        catch (Exception ex)
        {
            Directory.Delete(directoryPath, true);
            _logger.LogCritical(ex, "Failed to write whole hand comparison tree to file, removing incomplete checkpoint directory.");
            return false;
        }
    }

    public async Task<HandComparisonTree> LoadFromFileAsync(string directoryPath, CancellationToken cancellationToken)
    {
        List<LinkedList<HoldemHand>> existing = [];
        try
        {
            _logger.LogInformation("Loading whole hand comparison tree from directory {DirectoryPath}.", directoryPath);
            string[] files = [..
                Directory.EnumerateFiles(directoryPath, "*.bin")
                    .Select(f => new
                    {
                        Path = f,
                        HasValidIndex = int.TryParse(Path.GetFileNameWithoutExtension(f), out int index),
                        Index = index
                    })
                    .Where(f => f.HasValidIndex)
                    .OrderBy(f => f.Index)
                    .Select(f => f.Path)
                ];
            foreach (string file in files)
            {
                LinkedList<HoldemHand> bin = [];
                using FileStream stream = new(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[stream.Length];
                await stream.ReadExactlyAsync(buffer, cancellationToken);
                for (int i = 0; i < buffer.Length; i += 9)
                {
                    byte[] bytes = buffer[i..(i + 9)];
                    (HoldemHand hand, _) = BinaryHandHelper.Unpack(bytes);
                    bin.AddLast(hand);
                }
                existing.Add(bin);
                _logger.LogInformation("Loaded bin {BinNumber} from file {BinPath}.", existing.Count, file);
            }
            return HandComparisonTree.FromExisting(existing);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Failed to load whole hand comparison tree from file.");
            throw;
        }
    }
}