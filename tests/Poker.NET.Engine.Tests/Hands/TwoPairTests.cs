using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Tests.Hands.Base;

namespace Poker.NET.Engine.Tests.Hands;

public class TwoPairTests : HandsTests
{
    public static readonly IEnumerable<object[]> AllExceptTwoPairData = [HighCardData, PairData, ThreeOfAKindData, StraightData, FlushData, FullHouseData, FourOfAKindData, StraightFlushData];

    [Fact]
    public void TwoPair_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new TwoPair()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a TwoPair.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.FourOfHearts,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs)]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.FourOfHearts,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.SixOfHearts | Cards.TenOfClubs)]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.ThreeOfClubs,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.SixOfHearts | Cards.TenOfClubs)]
    [InlineData(
        Cards.JackOfClubs | Cards.AceOfSpades,
        Cards.FourOfHearts | Cards.FourOfClubs | Cards.SixOfSpades | Cards.SixOfHearts | Cards.TenOfClubs)]
    public void TwoPair_WhenConstructedWithValidCards_ShouldSucceed(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = TwoPair.TryGetFromHand(hand, out TwoPair? twoPair);

        // Assert
        Assert.True(success);
        Assert.NotNull(twoPair);
    }

    [Theory]
    [MemberData(nameof(AllExceptTwoPairData))]
    public void TwoPair_WhenConstructedWithOtherHand_ShouldFail(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        bool success = TwoPair.TryGetFromHand(hand, out TwoPair? twoPair);

        // Assert
        Assert.False(success);
        Assert.Null(twoPair);
    }

    [Theory]
    [MemberData(nameof(AllExceptTwoPairData))]
    public void TwoPair_WhenConstructedWithOtherHand_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => TwoPair.FromHand(hand)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"The hold'em hand {hand} does not contain a two pair.", ex.Message);
    }
}
