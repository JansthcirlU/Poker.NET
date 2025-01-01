using System.Runtime.CompilerServices;
using Poker.NET.Engine.Evaluators.Base;
using Poker.NET.Engine.Helpers;

[assembly: InternalsVisibleTo("Poker.NET.Engine.Tests")]
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

    internal static int BreakTie(HoldemHand first, HoldemHand second, HandScore score)
    {
        throw new NotImplementedException();
    }

    internal static HandScore GetHandScore(HoldemHand hand)
    {
        throw new NotImplementedException();
    }

    internal static HandScore GetHighestScore(HoldemHand hand)
    {
        Cards hole = hand.HoleCards;
        Cards combined = hand.HoleCards | hand.CommunityCards;
        IEnumerable<HandScore> matchingKeys = HandScoreHelper.ScoresPerHand
            .Where(kvp => (hole & kvp.Key) != Cards.None) // At least one of the hole cards must contribute towards the score
            .Where(kvp => (combined & kvp.Key) == kvp.Key) // The full scorable hand must be included in the player's hand
            .Select(kvp => kvp.Value)
            .OrderByDescending(s => s);
        return matchingKeys.Any()
            ? matchingKeys.First()
            : HandScore.HighCard;
    }
}
