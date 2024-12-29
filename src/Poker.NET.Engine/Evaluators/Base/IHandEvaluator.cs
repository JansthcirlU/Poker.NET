namespace Poker.NET.Engine.Evaluators.Base;

public interface IHandEvaluator
{
    int Compare(HoldemHand first, HoldemHand second);
}