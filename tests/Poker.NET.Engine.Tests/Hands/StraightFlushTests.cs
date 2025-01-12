using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Tests.Hands.Base;

namespace Poker.NET.Engine.Tests.Hands;

public class StraightFlushTests : HandsTests
{
    public static readonly IEnumerable<object[]> AllExceptStraightFlushData = [HighCardData, PairData, TwoPairData, ThreeOfAKindData, StraightData, FlushData, FullHouseData, FourOfAKindData];

    [Fact]
    public void StraightFlush_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new StraightFlush()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a StraightFlush.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.AceOfSpades | Cards.KingOfSpades,
        Cards.QueenOfSpades | Cards.JackOfSpades | Cards.TenOfSpades | Cards.ThreeOfDiamonds | Cards.TwoOfHearts,
        Rank.AceHigh)]
    [InlineData(
        Cards.TwoOfHearts | Cards.FourOfHearts,
        Cards.ThreeOfHearts | Cards.FourOfDiamonds | Cards.FourOfClubs | Cards.FiveOfHearts | Cards.AceOfHearts,
        Rank.Five)]
    public void StraightFlush_WhenConstructedWithValidCards_ShouldSucceed(
        Cards holeCards,
        Cards communityCards,
        Rank expectedHighestRank)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = StraightFlush.TryGetFromHand(hand, out StraightFlush? sf);

        // Assert
        Assert.True(success);
        Assert.NotNull(sf);

        StraightFlush straightFlush = (StraightFlush)sf!;
        Assert.Equal(expectedHighestRank, straightFlush.HighestRank);
    }

    [Theory]
    [MemberData(nameof(AllExceptStraightFlushData))]
    public void StraightFlush_WhenConstructedWithOtherHand_ShouldFail(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        bool success = StraightFlush.TryGetFromHand(hand, out StraightFlush? straightFlush);

        // Assert
        Assert.False(success);
        Assert.Null(straightFlush);
    }

    [Theory]
    [MemberData(nameof(AllExceptStraightFlushData))]
    public void StraightFlush_WhenConstructedWithOtherHand_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => StraightFlush.FromHand(hand)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"The hold'em hand {hand} does not contain a straight flush.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.SixOfDiamonds | Cards.SevenOfDiamonds,
        Cards.AceOfDiamonds | Cards.TwoOfDiamonds,
        Cards.ThreeOfDiamonds | Cards.FourOfDiamonds | Cards.FiveOfDiamonds | Cards.QueenOfClubs | Cards.KingOfHearts)]
    public override void CompareTo_WhenFirstBeatsSecond_ShouldBeGreaterThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        StraightFlush firstStraightFlush = StraightFlush.FromHand(firstHand);
        StraightFlush secondStraightFlush = StraightFlush.FromHand(secondHand);

        // Act
        int comparison = firstStraightFlush.CompareTo(secondStraightFlush);

        // Assert
        Assert.True(comparison > 0);
    }

    [Theory]
    [InlineData(
        Cards.AceOfDiamonds | Cards.TwoOfDiamonds,
        Cards.SixOfDiamonds | Cards.SevenOfDiamonds,
        Cards.ThreeOfDiamonds | Cards.FourOfDiamonds | Cards.FiveOfDiamonds | Cards.QueenOfClubs | Cards.KingOfHearts)]
    public override void CompareTo_WhenFirstLosesToSecond_ShouldBeLessThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        StraightFlush firstStraightFlush = StraightFlush.FromHand(firstHand);
        StraightFlush secondStraightFlush = StraightFlush.FromHand(secondHand);

        // Act
        int comparison = firstStraightFlush.CompareTo(secondStraightFlush);

        // Assert
        Assert.True(comparison < 0);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfClubs | Cards.KingOfSpades,
        Cards.AceOfSpades | Cards.QueenOfSpades,
        Cards.TenOfDiamonds | Cards.JackOfDiamonds | Cards.QueenOfDiamonds | Cards.KingOfDiamonds | Cards.AceOfDiamonds)]
    public override void CompareTo_WhenFirstDrawsWithSecond_ShouldBeEqualToZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        StraightFlush firstStraightFlush = StraightFlush.FromHand(firstHand);
        StraightFlush secondStraightFlush = StraightFlush.FromHand(secondHand);

        // Act
        int comparison = firstStraightFlush.CompareTo(secondStraightFlush);

        // Assert
        Assert.Equal(0, comparison);
    }
}
