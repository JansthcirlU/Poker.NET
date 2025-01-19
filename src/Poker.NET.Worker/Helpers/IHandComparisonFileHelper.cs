using Poker.NET.LookupGeneration.Data;

namespace Poker.NET.Worker.Helpers;

public interface IHandComparisonFileHelper
{
    Task<bool> WriteToFileAsync(string directoryPath, HandComparisonTree tree, CancellationToken cancellationToken);
    Task<HandComparisonTree> LoadFromFileAsync(string directoryPath, CancellationToken cancellationToken);
}
