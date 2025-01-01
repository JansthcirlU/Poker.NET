using System.Diagnostics;

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
}
