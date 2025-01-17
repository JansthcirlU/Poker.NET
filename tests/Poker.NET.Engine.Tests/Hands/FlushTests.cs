using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Tests.Hands.Base;

namespace Poker.NET.Engine.Tests.Hands;

public class FlushTests : HandsTests
{
    public static readonly IEnumerable<object[]> AllExceptFlushData = [HighCardData, PairData, TwoPairData, ThreeOfAKindData, StraightData, FullHouseData, FourOfAKindData, StraightFlushData];

    [Fact]
    public void Flush_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new Flush()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a Flush.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfClubs | Cards.FourOfClubs,
        Cards.SixOfClubs | Cards.TenOfClubs | Cards.JackOfClubs | Cards.TwoOfDiamonds | Cards.AceOfHearts,
        Rank.Jack,
        Rank.Ten,
        Rank.Six,
        Rank.Four,
        Rank.Two)]
    public void Flush_WhenConstructedWithValidCards_ShouldSucceed(
        Cards holeCards,
        Cards communityCards,
        Rank expectedHighestRank,
        Rank expectedSecondRank,
        Rank expectedThirdRank,
        Rank expectedFourthRank,
        Rank expectedFifthRank)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = Flush.TryGetFromHand(hand, out Flush? f);

        // Assert
        Assert.True(success);
        Assert.NotNull(f);

        Flush flush = (Flush)f!;
        Assert.Equal(expectedHighestRank, flush.HighestRank);
        Assert.Equal(expectedSecondRank, flush.SecondRank);
        Assert.Equal(expectedThirdRank, flush.ThirdRank);
        Assert.Equal(expectedFourthRank, flush.FourthRank);
        Assert.Equal(expectedFifthRank, flush.FifthRank);
    }

    [Theory]
    [MemberData(nameof(AllExceptFlushData))]
    public void Flush_WhenConstructedWithOtherHand_ShouldFail(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        bool success = Flush.TryGetFromHand(hand, out Flush? flush);

        // Assert
        Assert.False(success);
        Assert.Null(flush);
    }

    [Theory]
    [MemberData(nameof(AllExceptFlushData))]
    public void Flush_WhenConstructedWithOtherHand_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => Flush.FromHand(hand)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"The hold'em hand {hand} does not contain any flushes.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.QueenOfClubs | Cards.AceOfClubs,
        Cards.JackOfClubs | Cards.KingOfClubs,
        Cards.TwoOfClubs | Cards.ThreeOfClubs | Cards.FourOfClubs | Cards.ThreeOfHearts | Cards.SevenOfDiamonds)]
    public override void CompareTo_WhenFirstBeatsSecond_ShouldBeGreaterThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        Flush firstFlush = Flush.FromHand(firstHand);
        Flush secondFlush = Flush.FromHand(secondHand);

        // Act
        int comparison = firstFlush.CompareTo(secondFlush);

        // Assert
        Assert.True(comparison > 0);
    }

    [Theory]
    [InlineData(
        Cards.JackOfClubs | Cards.KingOfClubs,
        Cards.QueenOfClubs | Cards.AceOfClubs,
        Cards.TwoOfClubs | Cards.ThreeOfClubs | Cards.FourOfClubs | Cards.ThreeOfHearts | Cards.SevenOfDiamonds)]
    public override void CompareTo_WhenFirstLosesToSecond_ShouldBeLessThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        Flush firstFlush = Flush.FromHand(firstHand);
        Flush secondFlush = Flush.FromHand(secondHand);

        // Act
        int comparison = firstFlush.CompareTo(secondFlush);

        // Assert
        Assert.True(comparison < 0);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfHearts | Cards.FiveOfDiamonds,
        Cards.AceOfSpades | Cards.KingOfHearts,
        Cards.TwoOfClubs | Cards.ThreeOfClubs | Cards.FourOfClubs | Cards.QueenOfClubs | Cards.AceOfClubs)]
    public override void CompareTo_WhenFirstDrawsWithSecond_ShouldBeEqualToZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        Flush firstFlush = Flush.FromHand(firstHand);
        Flush secondFlush = Flush.FromHand(secondHand);

        // Act
        int comparison = firstFlush.CompareTo(secondFlush);

        // Assert
        Assert.Equal(0, comparison);
    }
}
