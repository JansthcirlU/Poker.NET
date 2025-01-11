using Poker.NET.Engine.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Hands;

public readonly struct HighCard : IHand<HighCard>
{
    public Rank HighestRank { get; }
    public Rank SecondRank { get; }
    public Rank ThirdRank { get; }
    public Rank FourthRank { get; }
    public Rank FifthRank { get; }

    public HighCard()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a HighCard.");
    }
    private HighCard(
        Rank highestRank,
        Rank secondRank,
        Rank thirdRank,
        Rank fourthRank,
        Rank fifthRank)
    {
        HighestRank = highestRank;
        SecondRank = secondRank;
        ThirdRank = thirdRank;
        FourthRank = fourthRank;
        FifthRank = fifthRank;
    }

    public static HighCard FromHand(HoldemHand hand)
    {
        Cards cards = hand.HoleCards | hand.CommunityCards;
        Dictionary<Rank, Cards> cardsByRank = cards.ToRankDictionary();
        if (cardsByRank.Any(kvp => kvp.Value.GetCardCount() > 1)) throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain a high card.");
        
        Dictionary<Suit, Cards> cardsBySuit = cards.ToSuitDictionary();
        if (cardsBySuit.Any(kvp => kvp.Value.GetCardCount() >= 5)) throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain a high card.");

        List<Rank> sortedRanks = cardsByRank.Keys.OrderBy(r => r).ToList();
        if (sortedRanks.Contains(Rank.AceHigh)) sortedRanks.Insert(0, Rank.AceLow);
        for (int i = 0; i <= sortedRanks.Count - 5; i++)
        {
            if (sortedRanks[i] + 1 == sortedRanks[i + 1] &&
                sortedRanks[i] + 2 == sortedRanks[i + 2] &&
                sortedRanks[i] + 3 == sortedRanks[i + 3] &&
                sortedRanks[i] + 4 == sortedRanks[i + 4])
            {
                throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain a high card.");
            }
        }
        
        List<Rank> ranks = cardsByRank
            .Select(c => c.Key)
            .OrderByDescending(r => r)
            .Take(5)
            .ToList();
        return new(ranks[0], ranks[1], ranks[2], ranks[3], ranks[4]);
    }

    public static bool TryGetFromHand(HoldemHand hand, out HighCard? result)
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

    public int CompareTo(HighCard other)
    {
        Rank[] ranks = [HighestRank, SecondRank, ThirdRank, FourthRank, FifthRank];
        Rank[] otherRanks = [other.HighestRank, other.SecondRank, other.ThirdRank, other.FourthRank, other.FifthRank];
        for (int i = 0; i < ranks.Length; i++)
        {
            int comparison = ranks[i].CompareTo(otherRanks[i]);
            if (comparison != 0) return comparison;
        }
        return 0;
    }
}