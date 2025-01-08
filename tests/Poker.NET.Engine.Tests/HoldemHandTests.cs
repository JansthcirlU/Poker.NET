using Poker.NET.Engine.Helpers;
using Poker.NET.LookupGeneration;

namespace Poker.NET.Engine.Tests;

public class HoldemHandTests
{
    public const Cards TwoCardHand = Cards.TwoOfClubs | Cards.TwoOfSpades;
    public const Cards FiveCardHand = Cards.TwoOfClubs | Cards.TwoOfSpades | Cards.FourOfHearts | Cards.FiveOfDiamonds | Cards.SevenOfClubs;

    private static readonly List<Cards> AllOneCardHands = HandsGenerator.GetAllKCardHands(1).Take(10).ToList();
    private static readonly List<Cards> AllTwoCardHands = HandsGenerator.GetAllKCardHands(2).Take(10).ToList();
    private static readonly List<Cards> AllThreeCardHands = HandsGenerator.GetAllKCardHands(3).Take(10).ToList();
    private static readonly List<Cards> AllFourCardHands = HandsGenerator.GetAllKCardHands(4).Take(10).ToList();
    private static readonly List<Cards> AllFiveCardHands = HandsGenerator.GetAllKCardHands(5).Take(10).ToList();
    private static readonly List<Cards> AllSixCardHands = HandsGenerator.GetAllKCardHands(6).Take(10).ToList();
    private static readonly List<Cards> AllSevenCardHands = HandsGenerator.GetAllKCardHands(7).ToList();
    private static readonly List<Cards> AllButTwoCardHands = AllOneCardHands
        .Concat(AllThreeCardHands)
        .Concat(AllFourCardHands)
        .Concat(AllFiveCardHands)
        .Concat(AllSixCardHands)
        .Concat(AllSevenCardHands.Take(10))
        .ToList();
    private static readonly List<Cards> AllButFiveCardHands = AllOneCardHands
        .Concat(AllTwoCardHands)
        .Concat(AllThreeCardHands)
        .Concat(AllFourCardHands)
        .Concat(AllSixCardHands)
        .Concat(AllSevenCardHands.Take(10))
        .ToList();
    private static readonly List<(Cards HoleCards, Cards CommunityCards)> CombinedTwoAndFiveCardHands = AllTwoCardHands
        .SelectMany(holeCards => AllFiveCardHands.Select(communityCards => (holeCards, communityCards)))
        .ToList();

    public static IEnumerable<object[]> AllButTwoCardHandsData
        => AllButTwoCardHands
            .Where(c => (c & FiveCardHand) == Cards.None)
            .Select(h => new object[] { h, FiveCardHand });
    public static IEnumerable<object[]> AllButFiveCardHandsData
        => AllButFiveCardHands
            .Where(c => (c & TwoCardHand) == Cards.None)
            .Select(h => new object[] { TwoCardHand, h });
    public static IEnumerable<object[]> GetInvalidHoleCardAndCommunityCards
        => CombinedTwoAndFiveCardHands
            .Where(hand => !AllSevenCardHands.Contains(hand.HoleCards | hand.CommunityCards))
            .Select(hand => new object[] { hand.HoleCards, hand.CommunityCards });
    public static IEnumerable<object[]> GetValidHoleCardAndCommunityCards
        => AllSevenCardHands
            .Take(10)
            .Select(cards => new
            {
                HoleCards = HandsGenerator.GetAllKCardHands(2).First(mask => (cards & mask) == mask),
                Cards = cards
            })
            .Select(x => new object[] { x.HoleCards, x.Cards & ~x.HoleCards });

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

    [Theory]
    [MemberData(nameof(AllButTwoCardHandsData))]
    public void HoldemHand_WhenConstructedWithInvalidHoleCards_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => new HoldemHand(holeCards, communityCards)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"There are not exactly two hole cards in {holeCards.ToCardString()}", ex.Message);
    }

    [Theory]
    [MemberData(nameof(AllButFiveCardHandsData))]
    public void HoldemHand_WhenConstructedWithInvalidCommunityCards_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => new HoldemHand(holeCards, communityCards)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"There are not exactly five community cards in {communityCards.ToCardString()}", ex.Message);
    }

    [Theory]
    [MemberData(nameof(GetInvalidHoleCardAndCommunityCards))]
    public void HoldemHand_WhenConstructedWithOverlappingHoleAndCommunityCards_ShouldThrow(Cards holeCards, Cards communityCards)
    {
        // Act
        ArgumentException? ex = Assert.Throws<ArgumentException>(
            () => new HoldemHand(holeCards, communityCards)
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith($"Hole cards and community cards must not overlap in {holeCards.ToCardString()} and {communityCards.ToCardString()}", ex.Message);
    }

    [Theory]
    [MemberData(nameof(GetValidHoleCardAndCommunityCards))]
    public void HoldemHand_WhenConstructedWithValidCards_ShouldSucceed(Cards holeCards, Cards communityCards)
    {
        // Act
        HoldemHand holdemHand = new(holeCards, communityCards);

        // Assert
        Assert.Equal(holeCards, holdemHand.HoleCards);
        Assert.Equal(communityCards, holdemHand.CommunityCards);
    }
}