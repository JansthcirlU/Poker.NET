namespace Poker.NET.Engine.Tests;

public class CardsTests
{
    [Fact]
    public void Deck_WhenConvertedToUlong_ShouldEqualBitSum()
    {
        // Arrange
        Cards deck = Cards.Deck;

        // Assert
        Assert.Equal((1ul << 52) - 1, (ulong)deck);
    }

    [Fact]
    public void CardsEnumEntries_WhenRepresentCard_ShouldBePowerOfTwo()
    {
        // Arrange
        List<Cards> singleCards = [.. Enum.GetValues<Cards>()];

        // Act
        List<Cards> powersOfTwoCards = singleCards
            .Where(cards => IsPowerOfTwo((ulong)cards))
            .ToList();

        // Assert
        Assert.Equal(52, powersOfTwoCards.Count);
    }

    private static bool IsPowerOfTwo(ulong number)
        => (number & (number - 1)) == 0; // e.g. 0b0010 & 0b0001 == 0b0000 but 0b1010 & 0b1001 == 0b1000
}