using Poker.NET.Engine.Evaluators.Base;
using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Hands.Base;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Evaluators.Naive;

public class NaiveHandEvaluator : IHandEvaluator
{
    public int Compare(HoldemHand first, HoldemHand second)
    {
        IHand firstHand = ToHand(first);
        IHand secondHand = ToHand(second);
        if (firstHand is HighCard highCard)
        {
            if (secondHand is HighCard otherHighCard) return highCard.CompareTo(otherHighCard);
            return -1;
        }
        if (firstHand is Pair pair)
        {
            if (secondHand is HighCard) return 1;
            if (secondHand is Pair otherPair) return pair.CompareTo(otherPair);
            return -1;
        }
        if (firstHand is TwoPair twoPair)
        {
            if (secondHand is HighCard) return 1;
            if (secondHand is Pair) return 1;
            if (secondHand is TwoPair otherTwoPair) return twoPair.CompareTo(otherTwoPair);
            return -1;
        }
        if (firstHand is ThreeOfAKind threeOfAKind)
        {
            if (secondHand is HighCard) return 1;
            if (secondHand is Pair) return 1;
            if (secondHand is TwoPair) return 1;
            if (secondHand is ThreeOfAKind otherThreeOfAKind) return threeOfAKind.CompareTo(otherThreeOfAKind);
            return -1;
        }
        if (firstHand is Straight straight)
        {
            if (secondHand is HighCard) return 1;
            if (secondHand is Pair) return 1;
            if (secondHand is TwoPair) return 1;
            if (secondHand is ThreeOfAKind) return 1;
            if (secondHand is Straight otherStraight) return straight.CompareTo(otherStraight);
            return -1;
        }
        if (firstHand is Flush flush)
        {
            if (secondHand is HighCard) return 1;
            if (secondHand is Pair) return 1;
            if (secondHand is TwoPair) return 1;
            if (secondHand is ThreeOfAKind) return 1;
            if (secondHand is Straight) return 1;
            if (secondHand is Flush otherFlush) return flush.CompareTo(otherFlush);
            return -1;
        }
        if (firstHand is FullHouse fullHouse)
        {
            if (secondHand is HighCard) return 1;
            if (secondHand is Pair) return 1;
            if (secondHand is TwoPair) return 1;
            if (secondHand is ThreeOfAKind) return 1;
            if (secondHand is Straight) return 1;
            if (secondHand is Flush) return 1;
            if (secondHand is FullHouse otherFullHouse) return fullHouse.CompareTo(otherFullHouse);
            return -1;
        }
        if (firstHand is FourOfAKind fourOfAKind)
        {
            if (secondHand is HighCard) return 1;
            if (secondHand is Pair) return 1;
            if (secondHand is TwoPair) return 1;
            if (secondHand is ThreeOfAKind) return 1;
            if (secondHand is Straight) return 1;
            if (secondHand is Flush) return 1;
            if (secondHand is FullHouse) return 1;
            if (secondHand is FourOfAKind otherFourOfAKind) return fourOfAKind.CompareTo(otherFourOfAKind);
            return -1;
        }
        if (firstHand is StraightFlush straightFlush)
        {
            if (secondHand is HighCard) return 1;
            if (secondHand is Pair) return 1;
            if (secondHand is TwoPair) return 1;
            if (secondHand is ThreeOfAKind) return 1;
            if (secondHand is Straight) return 1;
            if (secondHand is Flush) return 1;
            if (secondHand is FullHouse) return 1;
            if (secondHand is FourOfAKind) return 1;
            if (secondHand is StraightFlush otherStraightFlush) return straightFlush.CompareTo(otherStraightFlush);
        }
        Cards firstCards = first.HoleCards | first.CommunityCards;
        Cards secondCards = second.HoleCards | second.CommunityCards;
        throw new InvalidOperationException($"Cannot compare the hold'em hands {firstCards.ToCardString()} and {secondCards.ToCardString()}.");
    }
    
    internal static IHand ToHand(HoldemHand hand)
    {
        // Whichever matches first is the strongest hand
        if (StraightFlush.TryGetFromHand(hand, out StraightFlush? straightFlush)) return straightFlush!;
        if (FourOfAKind.TryGetFromHand(hand, out FourOfAKind? fourOfAKind)) return fourOfAKind!;
        if (FullHouse.TryGetFromHand(hand, out FullHouse? fullHouse)) return fullHouse!;
        if (Flush.TryGetFromHand(hand, out Flush? flush)) return flush!;
        if (Straight.TryGetFromHand(hand, out Straight? straight)) return straight!;
        if (ThreeOfAKind.TryGetFromHand(hand, out ThreeOfAKind? threeOfAKind)) return threeOfAKind!;
        if (TwoPair.TryGetFromHand(hand, out TwoPair? twoPair)) return twoPair!;
        if (Pair.TryGetFromHand(hand, out Pair? pair)) return pair!;
        return HighCard.FromHand(hand);
    }
}
