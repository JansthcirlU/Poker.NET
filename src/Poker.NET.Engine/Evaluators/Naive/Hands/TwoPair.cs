using Poker.NET.Engine.Evaluators.Naive.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Evaluators.Naive.Hands;

public readonly struct TwoPair : IHand<TwoPair>
{
    public Rank HighestPairRank { get; }
    public Rank LowestPairRank { get; }
    public Rank KickerRank { get; }

    public TwoPair()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a TwoPair.");
    }
    public TwoPair(
        Rank highestPairRank,
        Rank lowestPairRank,
        Rank kickerRank)
    {
        HighestPairRank = highestPairRank;
        LowestPairRank = lowestPairRank;
        KickerRank = kickerRank;
    }

    public static TwoPair FromHand(HoldemHand hand)
    {
        Cards cards = hand.HoleCards | hand.CommunityCards;
        IEnumerable<(Cards,(Rank,Rank))> matchingTwoPairs = HandScoreHelper.GetTwoPairs()
            .Where(twoPair => (cards & twoPair) == twoPair)
            .Select(twoPair => (Cards: twoPair, Ranks: HandScoreHelper.GetTwoPairRanks(twoPair)))
            .OrderByDescending(t => t.Ranks.HighestPairRank)
                .ThenByDescending(t => t.Ranks.LowestPairRank);
        if (matchingTwoPairs.Count() != 1) throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain exactly one two pair.");
        
        (Cards twoPair, (Rank highestPairRank, Rank lowestPairRank)) = matchingTwoPairs.First();
        Rank kickerRank = (cards & ~twoPair).GetIndividualCards()
            .Select(c => c.GetRank())
            .OrderByDescending(r => r)
            .First();
        return new(highestPairRank, lowestPairRank, kickerRank);
    }

    public static bool TryGetFromHand(HoldemHand hand, out TwoPair? result)
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

    public int CompareTo(TwoPair other)
    {
        Rank[] ranks = [HighestPairRank, LowestPairRank, KickerRank];
        Rank[] otherRanks = [other.HighestPairRank, other.LowestPairRank, other.KickerRank];
        for (int i = 0; i < ranks.Length; i++)
        {
            int comparison = ranks[i].CompareTo(otherRanks[i]);
            if (comparison != 0) return comparison;
        }
        return 0;
    }
}
