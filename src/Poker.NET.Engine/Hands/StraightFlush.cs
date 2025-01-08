using Poker.NET.Engine.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Hands;

public readonly struct StraightFlush : IHand<StraightFlush>
{
    public Rank HighestRank { get; }

    public StraightFlush()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a StraightFlush.");
    }
    public StraightFlush(Rank highestRank)
    {
        HighestRank = highestRank;
    }

    public static StraightFlush FromHand(HoldemHand hand)
    {
        Cards cards = hand.HoleCards | hand.CommunityCards;
        IEnumerable<Rank> matchingStraightFlushRanks = HandScoreHelper.GetStraightFlush()
            .Where(straightFlush => (cards & straightFlush) == straightFlush)
            .Select(HandScoreHelper.GetStraightFlushRank)
            .OrderByDescending(r => r);
        if (!matchingStraightFlushRanks.Any()) throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain a straight flush.");

        Rank highestRank = matchingStraightFlushRanks.First();
        return new(highestRank);
    }

    public static bool TryGetFromHand(HoldemHand hand, out StraightFlush? result)
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

    public int CompareTo(StraightFlush other)
        => HighestRank.CompareTo(other.HighestRank);
}