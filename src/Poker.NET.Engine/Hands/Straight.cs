using Poker.NET.Engine.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Hands;

public readonly struct Straight : IHand<Straight>
{
    public Rank HighestRank { get; }

    public Straight()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a Straight.");
    }
    private Straight(Rank highestRank)
    {
        HighestRank = highestRank;
    }

    public static Straight FromHand(HoldemHand hand)
    {
        Cards cards = hand.HoleCards | hand.CommunityCards;
        IEnumerable<Rank> matchingStraightRanks = HandScoreHelper.GetStraight()
            .Where(straight => (cards & straight) == straight)
            .Select(HandScoreHelper.GetStraightRank)
            .OrderByDescending(r => r);
        if (!matchingStraightRanks.Any()) throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain any straights.");

        Rank highestRank = matchingStraightRanks.First();
        return new(highestRank);
    }

    public static bool TryGetFromHand(HoldemHand hand, out Straight? result)
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

    public int CompareTo(Straight other)
        => HighestRank.CompareTo(other.HighestRank);
}
