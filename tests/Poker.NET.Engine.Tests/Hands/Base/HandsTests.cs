namespace Poker.NET.Engine.Tests.Hands.Base;

public abstract class HandsTests
{
    protected static readonly object[] HighCardData = [
        Cards.TwoOfDiamonds | Cards.ThreeOfHearts,
        Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.NineOfDiamonds | Cards.TenOfClubs
    ];
    protected static readonly object[] PairData = [
        Cards.TwoOfDiamonds | Cards.ThreeOfHearts,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs
    ];
    protected static readonly object[] TwoPairData = [
        Cards.TwoOfDiamonds | Cards.FourOfHearts,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs
    ];
    protected static readonly object[] ThreeOfAKindData = [
        Cards.TwoOfDiamonds | Cards.TwoOfSpades,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs
    ];
    protected static readonly object[] StraightData = [
        Cards.ThreeOfDiamonds | Cards.FiveOfHearts,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs
    ];
    protected static readonly object[] FlushData = [
        Cards.QueenOfClubs | Cards.AceOfClubs,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs
    ];
    protected static readonly object[] FullHouseData = [
        Cards.TwoOfDiamonds | Cards.FourOfHearts,
        Cards.TwoOfClubs | Cards.TwoOfSpades | Cards.FourOfClubs | Cards.EightOfHearts | Cards.TenOfClubs
    ];
    protected static readonly object[] FourOfAKindData = [
        Cards.TwoOfDiamonds | Cards.TwoOfClubs,
        Cards.TwoOfSpades | Cards.TwoOfHearts | Cards.FourOfClubs | Cards.EightOfHearts | Cards.TenOfClubs
    ];
    protected static readonly object[] StraightFlushData = [
        Cards.ThreeOfClubs | Cards.AceOfClubs,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.FiveOfClubs | Cards.EightOfHearts | Cards.TenOfSpades
    ];

    public abstract void CompareTo_WhenFirstBeatsSecond_ShouldBeGreaterThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards);
    public abstract void CompareTo_WhenFirstLosesToSecond_ShouldBeLessThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards);
    public abstract void CompareTo_WhenFirstDrawsWithSecond_ShouldBeEqualToZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards);
}
