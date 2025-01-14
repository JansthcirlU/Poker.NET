using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Tests.Hands.Base;

namespace Poker.NET.Engine.Tests.Hands;

public class FourOfAKindTests : HandsTests
{
    public static readonly IEnumerable<object[]> AllExceptFourOfAKindData = [HighCardData, PairData, TwoPairData, ThreeOfAKindData, StraightData, FlushData, FullHouseData, StraightFlushData];

    [Fact]
    public void FourOfAKind_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new FourOfAKind()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a FourOfAKind.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.TwoOfHearts,
        Cards.TwoOfClubs | Cards.TwoOfSpades | Cards.FourOfClubs | Cards.EightOfHearts | Cards.TenOfClubs,
        Rank.Two,
        Rank.Ten)]
    public void FourOfAKind_WhenConstructedWithValidCards_ShouldSucceed(
        Cards holeCards,
        Cards communityCards,
        Rank expectedFourOfAKindRank,
        Rank expectedKickerRank)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = FourOfAKind.TryGetFromHand(hand, out FourOfAKind? f);

        // Assert
        Assert.True(success);
        Assert.NotNull(f);

        FourOfAKind fourOfAKind = (FourOfAKind)f!;
        Assert.Equal(expectedFourOfAKindRank, fourOfAKind.FourOfAKindRank);
        Assert.Equal(expectedKickerRank, fourOfAKind.KickerRank);
    }

    [Theory]
    [MemberData(nameof(AllExceptFourOfAKindData))]
    public void FourOfAKind_WhenConstructedWithOtherHand_ShouldFail(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        bool success = FourOfAKind.TryGetFromHand(hand, out FourOfAKind? fourOfAKind);

        // Assert
        Assert.False(success);
        Assert.Null(fourOfAKind);
    }

    [Theory]
    [MemberData(nameof(AllExceptFourOfAKindData))]
    public void FourOfAKind_WhenConstructedWithOtherHand_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => FourOfAKind.FromHand(hand)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"The hold'em hand {hand} does not contain a four of a kind.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.FourOfClubs | Cards.FourOfDiamonds,
        Cards.TwoOfClubs | Cards.TwoOfDiamonds,
        Cards.FourOfHearts | Cards.FourOfSpades | Cards.TwoOfHearts | Cards.TwoOfSpades | Cards.AceOfSpades)]
    public override void CompareTo_WhenFirstBeatsSecond_ShouldBeGreaterThanZero(
        Cards firstHoleCards,
        Cards secondHoleCards,
        Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        FourOfAKind firstFourOfAKind = FourOfAKind.FromHand(firstHand);
        FourOfAKind secondFourOfAKind = FourOfAKind.FromHand(secondHand);

        // Act
        int comparison = firstFourOfAKind.CompareTo(secondFourOfAKind);

        // Assert
        Assert.True(comparison > 0);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfClubs | Cards.TwoOfDiamonds,
        Cards.FourOfClubs | Cards.FourOfDiamonds,
        Cards.FourOfHearts | Cards.FourOfSpades | Cards.TwoOfHearts | Cards.TwoOfSpades | Cards.AceOfSpades)]
    public override void CompareTo_WhenFirstLosesToSecond_ShouldBeLessThanZero(
        Cards firstHoleCards,
        Cards secondHoleCards,
        Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        FourOfAKind firstFourOfAKind = FourOfAKind.FromHand(firstHand);
        FourOfAKind secondFourOfAKind = FourOfAKind.FromHand(secondHand);

        // Act
        int comparison = firstFourOfAKind.CompareTo(secondFourOfAKind);

        // Assert
        Assert.True(comparison < 0);
    }

    [Theory]
    [InlineData(
        Cards.KingOfClubs | Cards.AceOfClubs,
        Cards.SevenOfDiamonds | Cards.KingOfHearts,
        Cards.TwoOfHearts | Cards.TwoOfSpades | Cards.TwoOfClubs | Cards.TwoOfDiamonds | Cards.AceOfSpades)]
    public override void CompareTo_WhenFirstDrawsWithSecond_ShouldBeEqualToZero(
        Cards firstHoleCards,
        Cards secondHoleCards,
        Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        FourOfAKind firstFourOfAKind = FourOfAKind.FromHand(firstHand);
        FourOfAKind secondFourOfAKind = FourOfAKind.FromHand(secondHand);

        // Act
        int comparison = firstFourOfAKind.CompareTo(secondFourOfAKind);

        // Assert
        Assert.Equal(0, comparison);
    }
}
