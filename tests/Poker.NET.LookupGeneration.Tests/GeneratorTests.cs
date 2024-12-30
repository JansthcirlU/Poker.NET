using Poker.NET.Engine;

namespace Poker.NET.LookupGeneration.Tests;

public class GeneratorTests
{
    [Fact]
    public void SanityCheck()
    {
        // Arrange
        List<Cards> allSevenCardHands = HandsGenerator.GetAllSevenCardHands().ToList();

        // Assert
        int fiftyTwoChooseSeven = 133_784_560;
        Assert.Equal(fiftyTwoChooseSeven, allSevenCardHands.Count);
    }

    [Fact]
    public void AllRanksSanityCheck()
    {
        // Arrange
        List<Cards> allRanks = HandsGenerator.GetAllRanks().ToList();

        // Assert
        Assert.Equal(13, allRanks.Count);
    }

    [Fact]
    public void AllSuitsSanityCheck()
    {
        // Arrange
        List<Cards> allRanks = HandsGenerator.GetAllSuits().ToList();

        // Assert
        Assert.Equal(4, allRanks.Count);
    }

    [Fact]
    public void AllPairsSanityCheck()
    {
        // Arrange
        List<Cards> allPairs = HandsGenerator.GetAllPairs().ToList();

        // Assert
        Assert.Equal(78, allPairs.Count);
    }

    [Fact]
    public void AllTwoPairsSanityCheck()
    {
        // Arrange
        List<Cards> allTwoPairs = HandsGenerator.GetAllTwoPairs().ToList();

        // Assert
        Assert.Equal(2808, allTwoPairs.Count);
    }

    [Fact]
    public void AllThreeOfAKindSanityCheck()
    {
        // Arrange
        List<Cards> allThreeOfAKind = HandsGenerator.GetAllThreeOfAKind().ToList();

        // Assert
        Assert.Equal(52, allThreeOfAKind.Count);
    }

    [Fact]
    public void AllStraightsSanityCheck()
    {
        // Arrange
        List<Cards> allStraights = HandsGenerator.GetAllStraights().ToList();

        // Assert
        Assert.Equal(10_200, allStraights.Count);
    }

    [Fact]
    public void AllFlushesSanityCheck()
    {
        // Arrange
        List<Cards> allFlushes = HandsGenerator.GetAllFlushes().ToList();

        // Assert
        Assert.Equal(5108, allFlushes.Count);
    }

    [Fact]
    public void AllFullHouseSanityCheck()
    {
        // Arrange
        List<Cards> allFullHouse = HandsGenerator.GetAllFullHouse().ToList();

        // Assert
        Assert.Equal(3744, allFullHouse.Count);
    }

    [Fact]
    public void AllFourOfAKindSanityCheck()
    {
        // Arrange
        List<Cards> allFourOfAKind = HandsGenerator.GetAllFourOfAKind().ToList();

        // Assert
        Assert.Equal(13, allFourOfAKind.Count);
    }

    [Fact]
    public void AllStraightFlushesSanityCheck()
    {
        // Arrange
        List<Cards> allStraightFlushes = HandsGenerator.GetAllStraightFlushes().ToList();

        // Assert
        Assert.Equal(40, allStraightFlushes.Count);
    }
}