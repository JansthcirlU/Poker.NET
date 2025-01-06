using Poker.NET.Engine.Evaluators.Naive.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Evaluators.Naive.Hands;

public readonly struct FourOfAKind : IHand<FourOfAKind>
{
    public Rank FourOfAKindRank { get; }
    public Rank KickerRank { get; }

    public FourOfAKind()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a FourOfAKind.");
    }
    public FourOfAKind(
        Rank fourOfAKindRank,
        Rank kickerRank)
    {
        FourOfAKindRank = fourOfAKindRank;
        KickerRank = kickerRank;
    }

    public static FourOfAKind FromHand(HoldemHand hand)
    {
        Cards cards = hand.HoleCards | hand.CommunityCards;
        IEnumerable<Cards> matchingFourOfAKinds = HandScoreHelper.GetFourOfAKind()
            .Where(fourOfAKind => (cards & fourOfAKind) == fourOfAKind);
        if (matchingFourOfAKinds.Count() != 1) throw new ArgumentException($"The hold'em hand {cards.ToCardString()} does not contain exactly one four of a kind.");
        
        Cards fourOfAKind = matchingFourOfAKinds.Single();
        Rank fourOfAKindRank = HandScoreHelper.GetFourOfAKindRank(fourOfAKind);
        Rank kickerRank = (cards & ~fourOfAKind).GetIndividualCards()
            .Select(c => c.GetRank())
            .First();
        return new(fourOfAKindRank, kickerRank);
    }

    public static bool TryGetFromHand(HoldemHand hand, out FourOfAKind? result)
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

    public int CompareTo(FourOfAKind other)
    {
        int comparison = FourOfAKindRank.CompareTo(other.FourOfAKindRank);
        if (comparison != 0) return comparison;

        return KickerRank.CompareTo(other.KickerRank);
    }
}
