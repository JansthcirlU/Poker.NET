using Poker.NET.Engine.Evaluators.Naive;

namespace Poker.NET.Engine.Tests;

public class NaiveHandEvaluatorTests
{
    private readonly NaiveHandEvaluator _naiveHandEvaluator;

    public NaiveHandEvaluatorTests()
    {
        _naiveHandEvaluator = new();
    }

    [Theory]
    [InlineData(
        Cards.AceOfHearts | Cards.AceOfClubs, // A♥A♣
        Cards.SevenOfDiamonds | Cards.ThreeOfHearts | Cards.TwoOfClubs | Cards.JackOfHearts | Cards.KingOfDiamonds, // 7♦3♥2♣J♥K♦
        Cards.ThreeOfClubs | Cards.FourOfSpades, // 3♣4♠
        Cards.SevenOfDiamonds | Cards.ThreeOfHearts | Cards.TwoOfClubs | Cards.JackOfHearts | Cards.KingOfDiamonds, // 7♦3♥2♣J♥K♦
        1)] // A♥A♣ beats 3♣3♥
    public void NaiveHandEvaluatorCompare(Cards firstHoleCards, Cards firstCommunityCards, Cards secondHoleCards, Cards secondCommunityCards, int comparison)
    {
        // Arrange
        HoldemHand first = new(firstHoleCards, firstCommunityCards);
        HoldemHand second = new(secondHoleCards, secondCommunityCards);

        // Assert
        Assert.Equal(comparison, _naiveHandEvaluator.Compare(first, second));
    }

    [Theory]
    [InlineData(
        Cards.TwoOfClubs | Cards.ThreeOfClubs,
        Cards.FourOfDiamonds | Cards.FiveOfHearts | Cards.AceOfSpades | Cards.QueenOfHearts | Cards.SixOfSpades,
        HandScore.Straight)]
    public void NaiveHandEvaluatorSamples(Cards holeCards, Cards communityCards, HandScore expected)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        HandScore score = NaiveHandEvaluator.GetHighestScore(hand);

        // Assert
        Assert.Equal(expected, score);
    }
}
