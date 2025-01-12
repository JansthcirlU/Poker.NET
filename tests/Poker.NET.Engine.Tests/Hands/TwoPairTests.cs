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
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs,
        Rank.Four,
        Rank.Two,
        Rank.Ten)]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.FourOfHearts,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.SixOfHearts | Cards.TenOfClubs,
        Rank.Six,
        Rank.Four,
        Rank.Ten)]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.ThreeOfClubs,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.SixOfHearts | Cards.TenOfClubs,
        Rank.Six,
        Rank.Two,
        Rank.Ten)]
    [InlineData(
        Cards.JackOfClubs | Cards.AceOfSpades,
        Cards.FourOfHearts | Cards.FourOfClubs | Cards.SixOfSpades | Cards.SixOfHearts | Cards.TenOfClubs,
        Rank.Six,
        Rank.Four,
        Rank.AceHigh)]
    public void TwoPair_WhenConstructedWithValidCards_ShouldSucceed(
        Cards holeCards,
        Cards communityCards,
        Rank expectedHighestPairRank,
        Rank expectedLowestPairRank,
        Rank expectedKickerRank)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = TwoPair.TryGetFromHand(hand, out TwoPair? tp);

        // Assert
        Assert.True(success);
        Assert.NotNull(tp);

        TwoPair twoPair = (TwoPair)tp!;
        Assert.Equal(expectedHighestPairRank, twoPair.HighestPairRank);
        Assert.Equal(expectedLowestPairRank, twoPair.LowestPairRank);
        Assert.Equal(expectedKickerRank, twoPair.KickerRank);
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

    [Theory]
    [InlineData(
        Cards.AceOfSpades | Cards.AceOfHearts,
        Cards.KingOfClubs | Cards.KingOfDiamonds,
        Cards.TwoOfClubs | Cards.TwoOfHearts | Cards.FourOfClubs | Cards.FiveOfClubs | Cards.JackOfHearts)]
    public override void CompareTo_WhenFirstBeatsSecond_ShouldBeGreaterThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        TwoPair firstTwoPair = TwoPair.FromHand(firstHand);
        TwoPair secondTwoPair = TwoPair.FromHand(secondHand);

        // Act
        int comparison = firstTwoPair.CompareTo(secondTwoPair);

        // Assert
        Assert.True(comparison > 0);
    }

    [Theory]
    [InlineData(
        Cards.KingOfClubs | Cards.KingOfDiamonds,
        Cards.AceOfSpades | Cards.AceOfHearts,
        Cards.TwoOfClubs | Cards.TwoOfHearts | Cards.FourOfClubs | Cards.FiveOfClubs | Cards.JackOfHearts)]
    public override void CompareTo_WhenFirstLosesToSecond_ShouldBeLessThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        TwoPair firstTwoPair = TwoPair.FromHand(firstHand);
        TwoPair secondTwoPair = TwoPair.FromHand(secondHand);

        // Act
        int comparison = firstTwoPair.CompareTo(secondTwoPair);

        // Assert
        Assert.True(comparison < 0);
    }

    [Theory]
    [InlineData(
        Cards.FourOfClubs | Cards.FiveOfClubs,
        Cards.FourOfHearts | Cards.ThreeOfHearts,
        Cards.TwoOfClubs | Cards.TwoOfDiamonds | Cards.KingOfClubs | Cards.KingOfDiamonds | Cards.JackOfHearts)]
    public override void CompareTo_WhenFirstDrawsWithSecond_ShouldBeEqualToZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        TwoPair firstTwoPair = TwoPair.FromHand(firstHand);
        TwoPair secondTwoPair = TwoPair.FromHand(secondHand);

        // Act
        int comparison = firstTwoPair.CompareTo(secondTwoPair);

        // Assert
        Assert.Equal(0, comparison);
    }
}
