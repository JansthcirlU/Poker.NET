using System.Collections;
using Poker.NET.Engine;
using Poker.NET.Engine.Evaluators.Naive;
using Poker.NET.LookupGeneration.Data.Collections;

namespace Poker.NET.LookupGeneration.Data;

public class HandComparisonTree : IAppendOnlyComparisonCollection<HoldemHand>
{
    private readonly List<LinkedList<HoldemHand>> _handBins;
    private readonly NaiveHandEvaluator _evaluator;
    public Func<HoldemHand, HoldemHand, int> Comparer { get; }

    private HandComparisonTree(List<LinkedList<HoldemHand>> existing)
    {
        _handBins = existing;
        _evaluator = new();
        Comparer = (first, second) => _evaluator.Compare(first, second);
    }
    public HandComparisonTree()
    {
        _handBins = new(1000);
        _evaluator = new();
        Comparer = (first, second) => _evaluator.Compare(first, second);
    }

    public static HandComparisonTree FromExisting(List<LinkedList<HoldemHand>> existing)
        => new(existing); // Don't validate existing data to speed things up

    public void Add(HoldemHand item)
    {
        if (_handBins.Count == 0)
        {
            LinkedList<HoldemHand> firstBin = [];
            firstBin.AddLast(item);
            _handBins.Add(firstBin);
            return;
        }

        // Binary search to find the correct bin or insertion point
        int left = 0;
        int right = _handBins.Count - 1;

        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            LinkedList<HoldemHand> currentBin = _handBins[mid];
            
            if (currentBin.First is null) throw new InvalidOperationException("Empty bin encountered");

            int comparison = Comparer(currentBin.First.Value, item);

            if (comparison == 0)
            {
                // Same strength, add to this bin
                currentBin.AddLast(item);
                return;
            }

            if (comparison < 0)
            {
                // Current bin is weaker, look in right half
                left = mid + 1;
            }
            else
            {
                // Current bin is stronger, look in left half
                right = mid - 1;
            }
        }

        // Insert new bin at 'left' index
        LinkedList<HoldemHand> newBin = [];
        newBin.AddLast(item);
        _handBins.Insert(left, newBin);
    }

    public IEnumerator<HoldemHand> GetEnumerator()
        => _handBins.SelectMany(l => l).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => GetEnumerator();
}