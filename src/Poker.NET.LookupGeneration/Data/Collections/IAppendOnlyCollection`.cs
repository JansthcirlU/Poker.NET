namespace Poker.NET.LookupGeneration.Data.Collections;

public interface IAppendOnlyComparisonCollection<T> : IEnumerable<T>
{
    /// <summary>
    /// The comparer function to use when adding items to the collection.
    /// </summary>
    Func<T, T, int> Comparer { get; }

    /// <summary>
    /// Adds an item to the collection in the correct order.
    /// </summary>
    void Add(T item);
}