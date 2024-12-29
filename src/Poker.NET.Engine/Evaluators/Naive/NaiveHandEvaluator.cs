using Poker.NET.Engine.Evaluators.Base;

namespace Poker.NET.Engine.Evaluators.Naive;

public class NaiveHandEvaluator : IHandEvaluator
{
    public int Compare(HoldemHand first, HoldemHand second)
    {
        HandScore firstScore = GetHandScore(first);
        HandScore secondScore = GetHandScore(second);

        if (firstScore == secondScore) return BreakTie(first, second, firstScore);
        return firstScore.CompareTo(secondScore);
    }

    private static int BreakTie(HoldemHand first, HoldemHand second, HandScore score)
    {
        throw new NotImplementedException();
    }

    private static HandScore GetHandScore(HoldemHand hand)
    {
        throw new NotImplementedException();
    }
}
