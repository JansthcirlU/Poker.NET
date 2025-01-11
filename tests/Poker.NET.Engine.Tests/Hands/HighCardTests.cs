using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Tests.Hands.Base;

namespace Poker.NET.Engine.Tests.Hands;

public class HighCardTests : HandsTests
{
    public static readonly IEnumerable<object[]> AllExceptHighCardData = [PairData, TwoPairData, ThreeOfAKindData, StraightData, FlushData, FullHouseData, FourOfAKindData, StraightFlushData];

    [Fact]
    public void HighCard_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new HighCard()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a HighCard.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfDiamonds | Cards.ThreeOfHearts,
        Cards.FourOfDiamonds | Cards.FiveOfHearts | Cards.TenOfClubs | Cards.JackOfDiamonds | Cards.KingOfClubs,
        Rank.King,
        Rank.Jack,
        Rank.Ten,
        Rank.Five,
        Rank.Four)]
    public void HighCard_WhenConstructedWithValidCards_ShouldSucceed(
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
        bool success = HighCard.TryGetFromHand(hand, out HighCard? hc);

        // Assert
        Assert.True(success);
        Assert.NotNull(hc);

        HighCard highCard = (HighCard)hc!;
        Assert.Equal(expectedHighestRank, highCard.HighestRank);
        Assert.Equal(expectedSecondRank, highCard.SecondRank);
        Assert.Equal(expectedThirdRank, highCard.ThirdRank);
        Assert.Equal(expectedFourthRank, highCard.FourthRank);
        Assert.Equal(expectedFifthRank, highCard.FifthRank);
    }

    [Theory]
    [MemberData(nameof(AllExceptHighCardData))]
    public void HighCard_WhenConstructedWithOtherHand_ShouldFail(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        bool success = HighCard.TryGetFromHand(hand, out HighCard? highCard);

        // Assert
        Assert.False(success);
        Assert.Null(highCard);
    }

    [Theory]
    [MemberData(nameof(AllExceptHighCardData))]
    public void HighCard_WhenConstructedWithOtherHand_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => HighCard.FromHand(hand)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"The hold'em hand {hand} does not contain a high card.", ex.Message);
    }
}