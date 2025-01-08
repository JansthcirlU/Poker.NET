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
        Cards.SixOfClubs | Cards.TenOfClubs | Cards.JackOfClubs | Cards.TwoOfDiamonds | Cards.AceOfHearts)]
    public void Flush_WhenConstructedWithValidCards_ShouldSucceed(Cards holeCards, Cards communityCards)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);
        
        // Act
        bool success = Flush.TryGetFromHand(hand, out Flush? flush);

        // Assert
        Assert.True(success);
        Assert.NotNull(flush);
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
}
