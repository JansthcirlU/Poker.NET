using Poker.NET.Engine.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Hands;

public readonly struct Pair : IHand<Pair>
{
    public Rank PairRank { get; }
    public Rank HighestKickerRank { get; }
    public Rank MiddleKickerRank { get; }
    public Rank LowestKickerRank { get; }

    public Pair()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a Pair.");
    }
    public Pair(
        Rank pairRank,
        Rank highestKickerRank,
        Rank middleKickerRank,
        Rank lowestKickerRank)
    {
        PairRank = pairRank;
        HighestKickerRank = highestKickerRank;
        MiddleKickerRank = middleKickerRank;
        LowestKickerRank = lowestKickerRank;
    }

    public static Pair FromHand(HoldemHand hand)
    {
        Cards cards = hand.HoleCards | hand.CommunityCards;
        IEnumerable<(Cards multiCardHand, Rank pairRank)> multiCardHands = cards
            .ToRankDictionary()
            .Where(r => r.Value.GetCardCount() > 1)
            .Select(r => (r.Value, r.Key));
        if (multiCardHands.Any(t => t.multiCardHand.GetCardCount() > 2)) throw new ArgumentException($"The hold'em hand {hand} does not contain exactly one pair.");
        
        IEnumerable<(Cards pair, Rank pairRank)> pairs = multiCardHands.Where(t => t.multiCardHand.GetCardCount() == 2);
        if (pairs.Count() != 1) throw new ArgumentException($"The hold'em hand {hand} does not contain exactly one pair.");

        (Cards pair, Rank pairRank) = pairs.Single();
        Rank[] kickersByRank = (cards & ~pair)
            .GetIndividualCards()
            .Select(c => c.GetRank())
            .OrderByDescending(r => r)
            .Take(3)
            .ToArray();
        return new(pairRank, kickersByRank[0], kickersByRank[1], kickersByRank[2]);
    }

    public static bool TryGetFromHand(HoldemHand hand, out Pair? result)
    {
        try
        {
            result = FromHand(hand);
            return true;
        }
        catch
        {
            result = null;
            return false;
        }
    }

    public int CompareTo(Pair other)
    {
        Rank[] ranks = [PairRank, HighestKickerRank, MiddleKickerRank, LowestKickerRank];
        Rank[] otherRanks = [other.PairRank, other.HighestKickerRank, other.MiddleKickerRank, other.LowestKickerRank];
        for (int i = 0; i < ranks.Length; i++)
        {
            int comparison = ranks[i].CompareTo(otherRanks[i]);
            if (comparison != 0) return comparison;
        }
        return 0;
    }
}
