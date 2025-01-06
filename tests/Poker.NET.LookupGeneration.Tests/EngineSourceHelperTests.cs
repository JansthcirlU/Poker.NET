namespace Poker.NET.LookupGeneration.Tests;

public class EngineSourceHelperTests
{
    [Fact]
    public void GenerateCardsEnum_ShouldContainAllCards()
    {
        // Arrange
        string cardsEnum = EngineSourceHelper.GenerateCardsEnum();

        // Assert
        Assert.NotEmpty(cardsEnum);
    }

    [Fact]
    public void GenerateGetTwoPairRanks_ShouldContainAllTwoPairRanks()
    {
        // Arrange
        string getTwoPairRanks = EngineSourceHelper.GenerateGetTwoPairRanks();

        // Assert
        Assert.NotEmpty(getTwoPairRanks);
    }

    [Fact]
    public void GenerateStraightRanks_ShouldContainAllStraightRanks()
    {
        // Arrange
        string straightRanks = EngineSourceHelper.GenerateStraightRanks();

        // Assert
        Assert.NotEmpty(straightRanks);
    }

    [Fact]
    public void GenerateStraightFlushRanks_ShouldContainAllStraightFlushRanks()
    {
        // Arrange
        string straightFlushRanks = EngineSourceHelper.GenerateStraightFlushRanks();

        // Assert
        Assert.NotEmpty(straightFlushRanks);
    }

    [Fact]
    public void GenerateFlushRanks_ShouldContainAllFlushRanks()
    {
        // Arrange
        string flushRanks = EngineSourceHelper.GenerateFlushRanks();

        // Assert
        Assert.NotEmpty(flushRanks);
    }

    [Fact]
    public void GenerateFullHouseRanks_ShouldContainAllFullHouseRanks()
    {
        // Arrange
        string fullHouseRanks = EngineSourceHelper.GenerateFullHouseRanks();

        // Assert
        Assert.NotEmpty(fullHouseRanks);
    }
}
