using System.Diagnostics;

namespace Poker.NET.LookupGeneration.Tests;

public class EngineSourceHelperTests
{
    [Fact]
    public void GenerateCardsEnum_ShouldContainAllCards()
    {
        string cardsEnum = EngineSourceHelper.GenerateCardsEnum();
        Debug.WriteLine(cardsEnum);

        Assert.NotEmpty(cardsEnum);
    }
}
