using Poker.NET.Engine.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Hands;

public readonly struct FullHouse : IHand<FullHouse>
{
    public Rank ThreeOfAKindRank { get; }
    public Rank PairRank { get; }

    public FullHouse()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a FullHouse.");
    }
    public FullHouse(
        Rank threeOfAKindRank,
        Rank pairRank)
    {
        ThreeOfAKindRank = threeOfAKindRank;
        PairRank = pairRank;
    }

    public static FullHouse FromHand(HoldemHand hand)
    {
        Cards cards = hand.HoleCards | hand.CommunityCards;
        IEnumerable<(Rank, Rank)> matchingFullHouseRanks = HandScoreHelper.GetFullHouse()
            .Where(fullHouse => (cards & fullHouse) == fullHouse)
            .Select(HandScoreHelper.GetFullHouseRanks)
            .OrderByDescending(rs => rs.ThreeOfAKindRank)
                .ThenByDescending(rs => rs.PairRank);
        if (!matchingFullHouseRanks.Any()) throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain any full houses.");

        (Rank threeOfAKindRank, Rank pairRank) = matchingFullHouseRanks.First();
        return new(threeOfAKindRank, pairRank);
    }

    public static bool TryGetFromHand(HoldemHand hand, out FullHouse? result)
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

    public int CompareTo(FullHouse other)
    {
        int comparison = ThreeOfAKindRank.CompareTo(other.ThreeOfAKindRank);
        if (comparison != 0) return comparison;

        return PairRank.CompareTo(other.PairRank);
    }
}
