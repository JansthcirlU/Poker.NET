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
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs)]
    [InlineData(
        Cards.AceOfSpades | Cards.JackOfDiamonds,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.FourOfDiamonds | Cards.FourOfHearts | Cards.TenOfClubs)]
    public void ThreeOfAKind_WhenConstructedWithValidCards_ShouldSucceed(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = ThreeOfAKind.TryGetFromHand(hand, out ThreeOfAKind? threeOfAKind);

        // Assert
        Assert.True(success);
        Assert.NotNull(threeOfAKind);
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
        Assert.StartsWith($"The hold'em hand {hand} does not contain a three of a kind.", ex.Message);
    }
}