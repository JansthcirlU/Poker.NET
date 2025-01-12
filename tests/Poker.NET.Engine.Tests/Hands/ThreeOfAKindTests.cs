using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Tests.Hands.Base;

namespace Poker.NET.Engine.Tests.Hands;

public class ThreeOfAKindTests : HandsTests
{
    public static readonly IEnumerable<object[]> AllExceptThreeOfAKindData = [HighCardData, PairData, TwoPairData, StraightData, FlushData, FullHouseData, FourOfAKindData, StraightFlushData];

    [Fact]
    public void ThreeOfAKind_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new ThreeOfAKind()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a ThreeOfAKind.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.TwoOfHearts,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs,
        Rank.Two,
        Rank.Ten,
        Rank.Eight)]
    [InlineData(
        Cards.AceOfSpades | Cards.JackOfDiamonds,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.FourOfDiamonds | Cards.FourOfHearts | Cards.TenOfClubs,
        Rank.Four,
        Rank.AceHigh,
        Rank.Jack)]
    public void ThreeOfAKind_WhenConstructedWithValidCards_ShouldSucceed(
        Cards holeCards,
        Cards communityCards,
        Rank expectedThreeOfAKindRank,
        Rank expectedHighestKickerRank,
        Rank expectedLowestKickerRank)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = ThreeOfAKind.TryGetFromHand(hand, out ThreeOfAKind? t);

        // Assert
        Assert.True(success);
        Assert.NotNull(t);

        ThreeOfAKind threeOfAKind = (ThreeOfAKind)t!;
        Assert.Equal(expectedThreeOfAKindRank, threeOfAKind.ThreeOfAKindRank);
        Assert.Equal(expectedHighestKickerRank, threeOfAKind.HighestKickerRank);
        Assert.Equal(expectedLowestKickerRank, threeOfAKind.LowestKickerRank);
    }

    [Theory]
    [MemberData(nameof(AllExceptThreeOfAKindData))]
    public void ThreeOfAKind_WhenConstructedWithOtherHand_ShouldFail(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        bool success = ThreeOfAKind.TryGetFromHand(hand, out ThreeOfAKind? threeOfAKind);

        // Assert
        Assert.False(success);
        Assert.Null(threeOfAKind);
    }

    [Theory]
    [MemberData(nameof(AllExceptThreeOfAKindData))]
    public void ThreeOfAKind_WhenConstructedWithOtherHand_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => ThreeOfAKind.FromHand(hand)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"The hold'em hand {hand} does not contain exactly one three of a kind.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.FourOfClubs | Cards.TenOfClubs,
        Cards.FourOfDiamonds | Cards.NineOfHearts,
        Cards.FourOfSpades | Cards.FourOfHearts | Cards.FiveOfClubs | Cards.SixOfDiamonds | Cards.EightOfSpades)]
    public override void CompareTo_WhenFirstBeatsSecond_ShouldBeGreaterThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        ThreeOfAKind firstThreeOfAKind = ThreeOfAKind.FromHand(firstHand);
        ThreeOfAKind secondThreeOfAKind = ThreeOfAKind.FromHand(secondHand);

        // Act
        int comparison = firstThreeOfAKind.CompareTo(secondThreeOfAKind);

        // Assert
        Assert.True(comparison > 0);
    }

    [Theory]
    [InlineData(
        Cards.FourOfDiamonds | Cards.NineOfHearts,
        Cards.FourOfClubs | Cards.TenOfClubs,
        Cards.FourOfSpades | Cards.FourOfHearts | Cards.FiveOfClubs | Cards.SixOfDiamonds | Cards.EightOfSpades)]
    public override void CompareTo_WhenFirstLosesToSecond_ShouldBeLessThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        ThreeOfAKind firstThreeOfAKind = ThreeOfAKind.FromHand(firstHand);
        ThreeOfAKind secondThreeOfAKind = ThreeOfAKind.FromHand(secondHand);

        // Act
        int comparison = firstThreeOfAKind.CompareTo(secondThreeOfAKind);

        // Assert
        Assert.True(comparison < 0);
    }

    [Theory]
    [InlineData(
        Cards.FourOfClubs | Cards.TwoOfDiamonds,
        Cards.FourOfDiamonds | Cards.TwoOfHearts,
        Cards.FourOfSpades | Cards.FourOfHearts | Cards.FiveOfClubs | Cards.SixOfDiamonds | Cards.EightOfSpades)]
    public override void CompareTo_WhenFirstDrawsWithSecond_ShouldBeEqualToZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        ThreeOfAKind firstThreeOfAKind = ThreeOfAKind.FromHand(firstHand);
        ThreeOfAKind secondThreeOfAKind = ThreeOfAKind.FromHand(secondHand);

        // Act
        int comparison = firstThreeOfAKind.CompareTo(secondThreeOfAKind);

        // Assert
        Assert.Equal(0, comparison);
    }
}