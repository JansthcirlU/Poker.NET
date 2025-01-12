namespace Poker.NET.Engine.Helpers;

public static class CardStringHelper
{
    public static string ToCardString(this Cards cards)
        => string.Join(
            string.Empty,
            cards
                .GetIndividualCards()
                .OrderBy(c => c.GetRank())
                .Select(ToIndividualCardString));

    private static string ToIndividualCardString(this Cards card)
    {
        CardHelper.ThrowIfNotSingle(card);

        return $"{card.GetRankSymbol()}{card.GetSuitSymbol()}";
    }

    private static char GetRankSymbol(this Cards card)
        => card.GetRank() switch
        {
            Rank.Two => '2',
            Rank.Three => '3',
            Rank.Four => '4',
            Rank.Five => '5',
            Rank.Six => '6',
            Rank.Seven => '7',
            Rank.Eight => '8',
            Rank.Nine => '9',
            Rank.Ten => 'T',
            Rank.Jack => 'J',
            Rank.Queen => 'Q',
            Rank.King => 'K',
            Rank.AceLow or Rank.AceHigh => 'A',
            _ => throw new ArgumentException($"The card {card} has no rank."),
        };

    private static char GetSuitSymbol(this Cards card)
        => card.GetSuit() switch
        {
            Suit.Clubs => '♣',
            Suit.Diamonds => '♦',
            Suit.Hearts => '♥',
            Suit.Spades => '♠',
            _ => throw new ArgumentException($"The card {card} has no suit.")
        };
}