using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Tests.Hands.Base;

namespace Poker.NET.Engine.Tests.Hands;

public class PairTests : HandsTests
{
    public static readonly IEnumerable<object[]> AllExceptPairData = [HighCardData, TwoPairData, ThreeOfAKindData, StraightData, FlushData, FullHouseData, FourOfAKindData, StraightFlushData];

    [Fact]
    public void Pair_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new Pair()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a Pair.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.ThreeOfHearts,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs,
        Rank.Two,
        Rank.Ten,
        Rank.Eight,
        Rank.Six)] // Makes a pair of twos
    [InlineData(
        Cards.FourOfClubs | Cards.FiveOfDiamonds,
        Cards.TwoOfClubs | Cards.FourOfSpades | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs,
        Rank.Four,
        Rank.Ten,
        Rank.Eight,
        Rank.Six)] // Makes a pair of fours
    [InlineData(
        Cards.SixOfHearts | Cards.SevenOfSpades,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs,
        Rank.Six,
        Rank.Ten,
        Rank.Eight,
        Rank.Seven)] // Makes a pair of sixes
    [InlineData(
        Cards.EightOfClubs | Cards.NineOfDiamonds,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs,
        Rank.Eight,
        Rank.Ten,
        Rank.Nine,
        Rank.Six)] // Makes a pair of eights
    [InlineData(
        Cards.TenOfHearts | Cards.JackOfSpades,
        Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs,
        Rank.Ten,
        Rank.Jack,
        Rank.Eight,
        Rank.Six)] // Makes a pair of tens
    public void Pair_WhenConstructedWithValidCards_ShouldSucceed(
        Cards holeCards,
        Cards communityCards,
        Rank expectedPairRank,
        Rank expectedHighestKickerRank,
        Rank expectedMiddleKickerRank,
        Rank expectedLowestKickerRank)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = Pair.TryGetFromHand(hand, out Pair? p);

        // Assert
        Assert.True(success);
        Assert.NotNull(p);

        Pair pair = (Pair)p!;
        Assert.Equal(expectedPairRank, pair.PairRank);
        Assert.Equal(expectedHighestKickerRank, pair.HighestKickerRank);
        Assert.Equal(expectedMiddleKickerRank, pair.MiddleKickerRank);
        Assert.Equal(expectedLowestKickerRank, pair.LowestKickerRank);
    }

    [Theory]
    [MemberData(nameof(AllExceptPairData))]
    public void Pair_WhenConstructedWithOtherHand_ShouldFail(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        bool success = Pair.TryGetFromHand(hand, out Pair? pair);

        // Assert
        Assert.False(success);
        Assert.Null(pair);
    }

    [Theory]
    [MemberData(nameof(AllExceptPairData))]
    public void Pair_WhenConstructedWithOtherHand_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => Pair.FromHand(hand)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"The hold'em hand {hand} does not contain exactly one pair.", ex.Message);
    }
}
