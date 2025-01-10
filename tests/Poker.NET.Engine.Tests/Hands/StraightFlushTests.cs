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
}
