using Poker.NET.Engine.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Hands;

public readonly struct Flush : IHand<Flush>
{
    public Rank HighestRank { get; }
    public Rank SecondRank { get; }
    public Rank ThirdRank { get; }
    public Rank FourthRank { get; }
    public Rank FifthRank { get; }

    public Flush()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a Flush.");
    }
    private Flush(
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

    public static Flush FromHand(HoldemHand hand)
    {
        Cards cards = hand.HoleCards | hand.CommunityCards;
        IEnumerable<(Rank, Rank, Rank, Rank, Rank)> matchingFlushRanks = HandScoreHelper.GetFlush()
            .Where(flush => (cards & flush) == flush)
            .Select(HandScoreHelper.GetFlushRanks)
            .OrderByDescending(rs => rs.Highest)
                .ThenByDescending(rs => rs.Second)
                    .ThenByDescending(rs => rs.Third)
                        .ThenByDescending(rs => rs.Fourth)
                            .ThenByDescending(rs => rs.Fifth);
        if (!matchingFlushRanks.Any()) throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain any flushes.");

        (Rank highestRank, Rank secondRank, Rank thirdRank, Rank fourthRank, Rank fifthRank) = matchingFlushRanks.First();
        return new(highestRank, secondRank, thirdRank, fourthRank, fifthRank);
    }

    public static bool TryGetFromHand(HoldemHand hand, out Flush? result)
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

    public int CompareTo(Flush other)
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
