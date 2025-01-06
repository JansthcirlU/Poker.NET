using Poker.NET.Engine.Evaluators.Naive.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Evaluators.Naive.Hands;

public readonly struct ThreeOfAKind : IHand<ThreeOfAKind>
{
    public Rank ThreeOfAKindRank { get; }
    public Rank HighestKickerRank { get; }
    public Rank LowestKickerRank { get; }

    public ThreeOfAKind()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a ThreeOfAKind.");
    }
    public ThreeOfAKind(
        Rank threeOfAKindRank,
        Rank highestKickerRank,
        Rank lowestKickerRank)
    {
        ThreeOfAKindRank = threeOfAKindRank;
        HighestKickerRank = highestKickerRank;
        LowestKickerRank = lowestKickerRank;
    }

    public static ThreeOfAKind FromHand(HoldemHand hand)
    {
        Cards cards = hand.HoleCards | hand.CommunityCards;
        IEnumerable<(Cards,Rank)> matchingThreeOfAKinds = HandScoreHelper.GetThreeOfAKind()
            .Where(threeOfAKind => (cards & threeOfAKind) == threeOfAKind)
            .Select(threeOfAKind => (Cards: threeOfAKind, Rank: HandScoreHelper.GetThreeOfAKindRank(threeOfAKind)))
            .OrderByDescending(t => t.Rank);
        if (!matchingThreeOfAKinds.Any()) throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain any three of a kind.");
        
        (Cards threeOfAKind, Rank threeOfAKindRank) = matchingThreeOfAKinds.First();
        Rank[] kickersByRank = (cards & ~threeOfAKind).GetIndividualCards()
            .Select(c => c.GetRank())
            .OrderByDescending(r => r)
            .Take(2)
            .ToArray();
        return new(threeOfAKindRank, kickersByRank[0], kickersByRank[1]);
    }

    public static bool TryGetFromHand(HoldemHand hand, out ThreeOfAKind? result)
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

    public int CompareTo(ThreeOfAKind other)
    {
        Rank[] ranks = [ThreeOfAKindRank, HighestKickerRank, LowestKickerRank];
        Rank[] otherRanks = [other.ThreeOfAKindRank, other.HighestKickerRank, other.LowestKickerRank];
        for (int i = 0; i < ranks.Length; i++)
        {
            int comparison = ranks[i].CompareTo(otherRanks[i]);
            if (comparison != 0) return comparison;
        }
        return 0;
    }
}
