namespace Poker.NET.Engine.Tests;

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
}