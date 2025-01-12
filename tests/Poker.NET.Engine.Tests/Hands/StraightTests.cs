using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Tests.Hands.Base;

namespace Poker.NET.Engine.Tests.Hands;

public class StraightTests : HandsTests
{
    public static readonly IEnumerable<object[]> AllExceptStraightData = [HighCardData, PairData, TwoPairData, ThreeOfAKindData, FlushData, FullHouseData, FourOfAKindData, StraightFlushData];

    [Fact]
    public void Straight_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new Straight()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a Straight.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.ThreeOfHearts,
        Cards.FourOfDiamonds | Cards.FiveOfClubs | Cards.SixOfSpades | Cards.JackOfDiamonds | Cards.QueenOfSpades,
        Rank.Six)]
    public void Straight_WhenConstructedWithValidCards_ShouldSucceed(
        Cards holeCards,
        Cards communityCards,
        Rank expectedHighestRank)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = Straight.TryGetFromHand(hand, out Straight? s);

        // Assert
        Assert.True(success);
        Assert.NotNull(s);

        Straight straight = (Straight)s!;
        Assert.Equal(expectedHighestRank, straight.HighestRank);
    }

    [Theory]
    [MemberData(nameof(AllExceptStraightData))]
    public void Straight_WhenConstructedWithOtherHand_ShouldFail(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        bool success = Straight.TryGetFromHand(hand, out Straight? straight);

        // Assert
        Assert.False(success);
        Assert.Null(straight);
    }

    [Theory]
    [MemberData(nameof(AllExceptStraightData))]
    public void Straight_WhenConstructedWithOtherHand_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => Straight.FromHand(hand)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"The hold'em hand {hand} does not contain any straights.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.SixOfSpades | Cards.SevenOfDiamonds,
        Cards.AceOfDiamonds | Cards.TwoOfDiamonds,
        Cards.ThreeOfDiamonds | Cards.FourOfHearts | Cards.FiveOfDiamonds | Cards.QueenOfClubs | Cards.KingOfHearts)]
    public override void CompareTo_WhenFirstBeatsSecond_ShouldBeGreaterThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        Straight firstStraight = Straight.FromHand(firstHand);
        Straight secondStraight = Straight.FromHand(secondHand);

        // Act
        int comparison = firstStraight.CompareTo(secondStraight);

        // Assert
        Assert.True(comparison > 0);
    }

    [Theory]
    [InlineData(
        Cards.AceOfDiamonds | Cards.TwoOfDiamonds,
        Cards.SixOfSpades | Cards.SevenOfDiamonds,
        Cards.ThreeOfDiamonds | Cards.FourOfHearts | Cards.FiveOfDiamonds | Cards.QueenOfClubs | Cards.KingOfHearts)]
    public override void CompareTo_WhenFirstLosesToSecond_ShouldBeLessThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        Straight firstStraight = Straight.FromHand(firstHand);
        Straight secondStraight = Straight.FromHand(secondHand);

        // Act
        int comparison = firstStraight.CompareTo(secondStraight);

        // Assert
        Assert.True(comparison < 0);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfClubs | Cards.JackOfHearts,
        Cards.KingOfSpades | Cards.QueenOfSpades,
        Cards.TenOfHearts | Cards.JackOfDiamonds | Cards.QueenOfDiamonds | Cards.KingOfClubs | Cards.AceOfSpades)]
    public override void CompareTo_WhenFirstDrawsWithSecond_ShouldBeEqualToZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        Straight firstStraight = Straight.FromHand(firstHand);
        Straight secondStraight = Straight.FromHand(secondHand);

        // Act
        int comparison = firstStraight.CompareTo(secondStraight);

        // Assert
        Assert.Equal(0, comparison);
    }
}
