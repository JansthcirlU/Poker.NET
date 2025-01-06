using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine.Tests;

public class HoldemHandTests
{
    [Fact]
    public void HoldemHand_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new HoldemHand()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a HoldemHand.", ex.Message);
    }

    // TODO: Import Poker.NET.LookupGeneration and use HandsGenerator theory data for the following tests

    // Write tests for all single-card, three-card, ..., seven-card combinations (not two-card)
    // to check if hole card assignment throws exception
    [Theory]
    public void HoldemHand_WhenConstructedWithInvalidHoleCards_ShouldThrow(
        Cards holeCards,
        Cards communityCards)
    {
        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => new HoldemHand(holeCards, communityCards)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"There are not exactly two hole cards in {holeCards.ToCardString()}", ex.Message);
    }

    // Write tests for all single-card, two-card, ..., seven-card combinations (not five-card)
    // to check if community card assignment throws exception
    [Theory]
    public void HoldemHand_WhenConstructedWithInvalidCommunityCards_ShouldThrow(
        Cards holeCards,
        Cards communityCards)
    {
        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => new HoldemHand(holeCards, communityCards)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"There are not exactly five community cards in {communityCards.ToCardString()}", ex.Message);
    }

    // Write tests for all two-card and five-card combinations
    // to check if HoldemHand construction succeeds
    [Theory]
    public void HoldemHand_WhenConstructedWithValidCards_ShouldSucceed(
        Cards holeCards,
        Cards communityCards)
    {
        // Act
        HoldemHand hand = new(holeCards, communityCards);

        // Assert
        Assert.Equal(holeCards, hand.HoleCards);
        Assert.Equal(communityCards, hand.CommunityCards);
    }
}