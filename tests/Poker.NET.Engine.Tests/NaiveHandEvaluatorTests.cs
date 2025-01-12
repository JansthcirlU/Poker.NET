using Poker.NET.Engine.Evaluators.Naive;

namespace Poker.NET.Engine.Tests;

public class NaiveHandEvaluatorTests
{
    public static TheoryData<HandComparisonData> FirstPlayerWins => [.. GetFirstPlayerWinsData()];
    public static TheoryData<HandComparisonData> SecondPlayerWins => [.. GetSecondPlayerWinsData()];
    public static TheoryData<HandComparisonData> BothPlayersDraw => [.. GetBothPlayersDrawData()];

    private readonly NaiveHandEvaluator _naiveHandEvaluator;

    public NaiveHandEvaluatorTests()
    {
        _naiveHandEvaluator = new();
    }

    [Theory]
    [MemberData(nameof(FirstPlayerWins))]
    public void NaiveHandEvaluator_WhenFirstBeatsSecond_ShouldBeGreaterThanZero(HandComparisonData data)
    {
        // Arrange
        HoldemHand first = new(data.FirstHoleCards, data.CommunityCards);
        HoldemHand second = new(data.SecondHoleCards, data.CommunityCards);

        // Assert
        int comparison = _naiveHandEvaluator.Compare(first, second);
        Assert.True(comparison > 0);
    }

    [Theory]
    [MemberData(nameof(SecondPlayerWins))]
    public void NaiveHandEvaluator_WhenFirstLosesToSecond_ShouldBeLessThanZero(HandComparisonData data)
    {
        // Arrange
        HoldemHand first = new(data.FirstHoleCards, data.CommunityCards);
        HoldemHand second = new(data.SecondHoleCards, data.CommunityCards);

        // Assert
        int comparison = _naiveHandEvaluator.Compare(first, second);
        Assert.True(comparison < 0);
    }

    [Theory]
    [MemberData(nameof(BothPlayersDraw))]
    public void NaiveHandEvaluator_WhenFirstDrawsWithSecond_ShouldBeEqualToZero(HandComparisonData data)
    {
        // Arrange
        HoldemHand first = new(data.FirstHoleCards, data.CommunityCards);
        HoldemHand second = new(data.SecondHoleCards, data.CommunityCards);

        // Assert
        int comparison = _naiveHandEvaluator.Compare(first, second);
        Assert.Equal(0, comparison);
    }

    private static IEnumerable<HandComparisonData> GetFirstPlayerWinsData()
    {
        // A♥A♣ vs 3♣4 (7♦3♥2♣J♥K♦)
        yield return new(
            Cards.AceOfHearts | Cards.AceOfClubs,
            Cards.ThreeOfClubs | Cards.FourOfSpades,
            Cards.SevenOfDiamonds | Cards.ThreeOfHearts | Cards.TwoOfClubs | Cards.JackOfHearts | Cards.KingOfDiamonds);

        // 2♥7♥ vs 2♦5♥ (Q♥T♥9♥3♥K♣)
        yield return new(
            Cards.TwoOfHearts | Cards.SevenOfHearts,
            Cards.TwoOfDiamonds | Cards.FiveOfHearts,
            Cards.QueenOfHearts | Cards.TenOfHearts | Cards.NineOfHearts | Cards.ThreeOfHearts | Cards.KingOfClubs);

        // 5♥K♦ vs 9♠9♥ (5♣5♦K♥K♠9♣)
        yield return new(
            Cards.FiveOfHearts | Cards.KingOfDiamonds,
            Cards.NineOfSpades | Cards.NineOfHearts,
            Cards.FiveOfClubs | Cards.FiveOfDiamonds | Cards.KingOfHearts | Cards.KingOfSpades | Cards.NineOfClubs);

        // K♥3♥ vs A♥2♥ (Q♥J♥T♥9♥8♣)
        yield return new(
            Cards.KingOfHearts | Cards.ThreeOfHearts,
            Cards.AceOfHearts | Cards.TwoOfHearts,
            Cards.QueenOfHearts | Cards.JackOfHearts | Cards.TenOfHearts | Cards.NineOfHearts | Cards.EightOfClubs);

        // 6♥7♣ vs 5♦9♠ (8♥5♥9♥T♠J♣)
        yield return new(
            Cards.SixOfHearts | Cards.SevenOfClubs,
            Cards.FiveOfDiamonds | Cards.NineOfSpades,
            Cards.EightOfHearts | Cards.FiveOfHearts | Cards.NineOfHearts | Cards.TenOfSpades | Cards.JackOfClubs);
    }

    private static IEnumerable<HandComparisonData> GetSecondPlayerWinsData()
        => GetFirstPlayerWinsData()
            .Select(data => new HandComparisonData(data.SecondHoleCards, data.FirstHoleCards, data.CommunityCards));

    private static IEnumerable<HandComparisonData> GetBothPlayersDrawData()
    {
        // A♥K♣ vs A♦Q♠ (2♥3♣4♦5♠6♥)
        yield return new(
            Cards.AceOfHearts | Cards.KingOfClubs,
            Cards.AceOfDiamonds | Cards.QueenOfSpades,
            Cards.TwoOfHearts | Cards.ThreeOfClubs | Cards.FourOfDiamonds | Cards.FiveOfSpades | Cards.SixOfHearts);

        // A♥A♣ vs A♦A♠ (7♦3♥2♣J♥K♦)
        yield return new(
            Cards.AceOfHearts | Cards.AceOfClubs,
            Cards.AceOfDiamonds | Cards.AceOfSpades,
            Cards.SevenOfDiamonds | Cards.ThreeOfHearts | Cards.TwoOfClubs | Cards.JackOfHearts | Cards.KingOfDiamonds);

        // A♣K♣ vs 2♦3♦ (5♥6♥7♥8♥9♥)
        yield return new(
            Cards.AceOfClubs | Cards.KingOfClubs,
            Cards.TwoOfDiamonds | Cards.ThreeOfDiamonds,
            Cards.FiveOfHearts | Cards.SixOfHearts | Cards.SevenOfHearts | Cards.EightOfHearts | Cards.NineOfHearts);

        // 5♥K♦ vs 5♠K♣ (5♣5♦K♥K♠9♣)
        yield return new(
            Cards.FiveOfHearts | Cards.KingOfDiamonds,
            Cards.FiveOfSpades | Cards.KingOfClubs,
            Cards.FiveOfClubs | Cards.FiveOfDiamonds | Cards.KingOfHearts | Cards.KingOfSpades | Cards.NineOfClubs);
    }
}
