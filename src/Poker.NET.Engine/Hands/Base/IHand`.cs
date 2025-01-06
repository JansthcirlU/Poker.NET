namespace Poker.NET.Engine.Hands.Base;

public interface IHand<THand> : IHand, IComparable<THand>
    where THand : struct, IHand<THand>
{
    static abstract THand FromHand(HoldemHand hand);
    static abstract bool TryGetFromHand(HoldemHand hand, out THand? result);
}