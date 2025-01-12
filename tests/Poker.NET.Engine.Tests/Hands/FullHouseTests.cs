using Poker.NET.Engine.Hands;
using Poker.NET.Engine.Tests.Hands.Base;

namespace Poker.NET.Engine.Tests.Hands;

public class FullHouseTests : HandsTests
{
    public static readonly IEnumerable<object[]> AllExceptFullHouseData = [HighCardData, PairData, TwoPairData, ThreeOfAKindData, StraightData, FlushData, FourOfAKindData, StraightFlushData];

    [Fact]
    public void FullHouse_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new FullHouse()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a FullHouse.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.TenOfClubs | Cards.TenOfDiamonds,
        Cards.FourOfDiamonds | Cards.FourOfHearts | Cards.FourOfClubs | Cards.TwoOfDiamonds | Cards.TwoOfHearts,
        Rank.Four,
        Rank.Ten)]
    public void FullHouse_WhenConstructedWithValidCards_ShouldSucceed(
        Cards holeCards,
        Cards communityCards,
        Rank expectedThreeOfAKindRank,
        Rank expectedPairRank)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = FullHouse.TryGetFromHand(hand, out FullHouse? fh);

        // Assert
        Assert.True(success);
        Assert.NotNull(fh);

        FullHouse fullHouse = (FullHouse)fh!;
        Assert.Equal(expectedThreeOfAKindRank, fullHouse.ThreeOfAKindRank);
        Assert.Equal(expectedPairRank, fullHouse.PairRank);
    }

    [Theory]
    [MemberData(nameof(AllExceptFullHouseData))]
    public void FullHouse_WhenConstructedWithOtherHand_ShouldFail(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        bool success = FullHouse.TryGetFromHand(hand, out FullHouse? fullHouse);

        // Assert
        Assert.False(success);
        Assert.Null(fullHouse);
    }

    [Theory]
    [MemberData(nameof(AllExceptFullHouseData))]
    public void FullHouse_WhenConstructedWithOtherHand_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => FullHouse.FromHand(hand)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"The hold'em hand {hand} does not contain any full houses.", ex.Message);
    }

    [Theory]
    [InlineData(
        Cards.ThreeOfClubs | Cards.ThreeOfDiamonds,
        Cards.TwoOfHearts | Cards.ThreeOfHearts,
        Cards.ThreeOfSpades | Cards.TwoOfSpades | Cards.TwoOfClubs | Cards.KingOfHearts | Cards.AceOfSpades)]
    public override void CompareTo_WhenFirstBeatsSecond_ShouldBeGreaterThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        FullHouse firstFullHouse = FullHouse.FromHand(firstHand);
        FullHouse secondFullHouse = FullHouse.FromHand(secondHand);

        // Act
        int comparison = firstFullHouse.CompareTo(secondFullHouse);

        // Assert
        Assert.True(comparison > 0);
    }

    [Theory]
    [InlineData(
        Cards.TwoOfHearts | Cards.ThreeOfHearts,
        Cards.ThreeOfClubs | Cards.ThreeOfDiamonds,
        Cards.ThreeOfSpades | Cards.TwoOfSpades | Cards.TwoOfClubs | Cards.KingOfHearts | Cards.AceOfSpades)]
    public override void CompareTo_WhenFirstLosesToSecond_ShouldBeLessThanZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        FullHouse firstFullHouse = FullHouse.FromHand(firstHand);
        FullHouse secondFullHouse = FullHouse.FromHand(secondHand);

        // Act
        int comparison = firstFullHouse.CompareTo(secondFullHouse);

        // Assert
        Assert.True(comparison < 0);
    }

    [Theory]
    [InlineData(
        Cards.AceOfClubs | Cards.KingOfDiamonds,
        Cards.AceOfDiamonds | Cards.KingOfClubs,
        Cards.TwoOfClubs | Cards.TwoOfDiamonds | Cards.TwoOfHearts | Cards.ThreeOfHearts | Cards.ThreeOfDiamonds)]
    public override void CompareTo_WhenFirstDrawsWithSecond_ShouldBeEqualToZero(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        // Arrange
        HoldemHand firstHand = new(firstHoleCards, communityCards);
        HoldemHand secondHand = new(secondHoleCards, communityCards);
        FullHouse firstFullHouse = FullHouse.FromHand(firstHand);
        FullHouse secondFullHouse = FullHouse.FromHand(secondHand);

        // Act
        int comparison = firstFullHouse.CompareTo(secondFullHouse);

        // Assert
        Assert.Equal(0, comparison);
    }
}
