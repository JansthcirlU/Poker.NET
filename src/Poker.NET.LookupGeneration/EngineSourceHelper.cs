using System.Text;
using Poker.NET.Engine;

namespace Poker.NET.LookupGeneration;

public static class EngineSourceHelper
{
    private static Dictionary<string, string> RankNamePlurals
        => new()
        {
            { "Two", "Twos" },
            { "Three", "Threes" },
            { "Four", "Fours" },
            { "Five", "Fives" },
            { "Six", "Sixes" },
            { "Seven", "Sevens" },
            { "Eight", "Eights" },
            { "Nine", "Nines" },
            { "Ten", "Tens" },
            { "Jack", "Jacks" },
            { "Queen", "Queens" },
            { "King", "Kings" },
            { "Ace", "Aces" },
        };

    public static string GenerateCardsEnum()
    {
        StringBuilder cardsEnumBuilder = new();
        cardsEnumBuilder
            .AppendLine("[Flags]")
            .AppendLine("public enum Cards : ulong")
            .AppendLine("{")
            .AppendLine("    None,")
                .AddSingleCards()
                .AddRanks()
                .AddSuits()
                .AddDeck()
                .AddCategory("Pair", HandsGenerator.GetAllPairs())
                .AddCategory("Two Pair", HandsGenerator.GetAllTwoPairs())
                .AddCategory("Three Of A Kind", HandsGenerator.GetAllThreeOfAKind())
                .AddCategory("Straight", HandsGenerator.GetAllStraights())
                .AddCategory("Flush", HandsGenerator.GetAllFlushes())
                .AddCategory("Full House", HandsGenerator.GetAllFullHouse())
                .AddCategory("Four Of A Kind", HandsGenerator.GetAllFourOfAKind())
                .AddCategory("Straight Flush", HandsGenerator.GetAllStraightFlushes())
            .AppendLine("}");
        return cardsEnumBuilder.ToString();
    }

    private static StringBuilder AddSingleCards(this StringBuilder cardsEnumBuilder)
    {
        int shifts = 0;
        foreach (IGrouping<string, string> suitGroup in GetCardNames().GroupBy(n => n.Split("Of").Last()))
        {
            cardsEnumBuilder.Append("\n    // ").AppendLine(suitGroup.Key);
            foreach (string card in suitGroup)
            {
                cardsEnumBuilder.Append("    ").Append(card).Append(" = 1ul << ").Append(shifts++).AppendLine(",");
            }
        }
        return cardsEnumBuilder;
    }

    private static StringBuilder AddRanks(this StringBuilder cardsEnumBuilder)
    {
        cardsEnumBuilder.AppendLine("\n    // Ranks");
        foreach (IGrouping<string, string> cardsByRank in GetCardNames().GroupBy(n => n.Split("Of").First()))
        {
            cardsEnumBuilder
                .Append("    ")
                .Append(RankNamePlurals[cardsByRank.Key])
                .Append(" = ")
                .Append(string.Join(" | ", cardsByRank))
                .AppendLine(",");
        }
        return cardsEnumBuilder;
    }

    private static StringBuilder AddSuits(this StringBuilder cardsEnumBuilder)
    {
        cardsEnumBuilder.AppendLine("\n    // Suits");
        foreach (IGrouping<string, string> cardsBySuit in GetCardNames().GroupBy(n => n.Split("Of").Last()))
        {
            cardsEnumBuilder
                .Append("    ")
                .Append(cardsBySuit.Key)
                .Append(" = ")
                .Append(string.Join(" | ", cardsBySuit))
                .AppendLine(",");
        }
        return cardsEnumBuilder;
    }

    private static StringBuilder AddDeck(this StringBuilder cardsEnumBuilder)
        => cardsEnumBuilder
            .AppendLine("\n    // Deck")
            .Append("    Deck = ")
            .Append(string.Join(" | ", HandsGenerator.GetAllSuits()))
            .AppendLine(",");

    private static StringBuilder AddCategory(this StringBuilder cardsEnumBuilder, string categoryName, IEnumerable<Cards> categoryCards)
    {
        int i = 1;
        string capitalizedCategoryName = categoryName.ToCapitalCamelCase();
        cardsEnumBuilder.Append("\n    // ").AppendLine(categoryName);
        foreach (Cards cards in categoryCards)
        {
            cardsEnumBuilder
                .Append("    ")
                .Append(capitalizedCategoryName)
                .Append(i++)
                .Append(" = ")
                .Append(cards.ToUnionString())
                .AppendLine(",");
        }
        return cardsEnumBuilder;
    }

    private static IEnumerable<string> GetRankNames()
    {
        yield return "Two";
        yield return "Three";
        yield return "Four";
        yield return "Five";
        yield return "Six";
        yield return "Seven";
        yield return "Eight";
        yield return "Nine";
        yield return "Ten";
        yield return "Jack";
        yield return "Queen";
        yield return "King";
        yield return "Ace";
    }

    private static IEnumerable<string> GetSuitNames()
    {
        yield return "Clubs";
        yield return "Diamonds";
        yield return "Hearts";
        yield return "Spades";
    }

    private static IEnumerable<string> GetCardNames()
    {
        foreach (string suit in GetSuitNames())
        {
            foreach (string rank in GetRankNames())
            {
                yield return $"{rank}Of{suit}";
            }
        }
    }

    private static string ToUnionString(this Cards cards)
        => cards.ToString().Replace(", ", " | ");

    private static string ToCapitalCamelCase(this string input)
    {
        if (string.IsNullOrEmpty(input)) return input;

        StringBuilder stringBuilder = new(input.Length);
        ReadOnlySpan<char> span = input.AsSpan();
        bool capitalizeNext = true;

        foreach (char c in span)
        {
            if (char.IsWhiteSpace(c) || !char.IsLetterOrDigit(c))
            {
                capitalizeNext = true;
            }
            else
            {
                if (capitalizeNext)
                {
                    stringBuilder.Append(char.ToUpperInvariant(c));
                    capitalizeNext = false;
                }
                else
                {
                    stringBuilder.Append(char.ToLowerInvariant(c));
                }
            }
        }
        return stringBuilder.ToString();
    }
}
