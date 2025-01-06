using System.Text;
using Poker.NET.Engine;
using Poker.NET.Engine.Helpers;

namespace Poker.NET.LookupGeneration;

public static class EngineSourceHelper
{
    private static readonly Dictionary<string, string> RankNamePlurals = new()
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
    
    public static Dictionary<Cards, int> ToSuitAndRankDictionary(this Cards cards)
        => HandsGenerator.GetAllSuits()
            .Concat(HandsGenerator.GetAllRanks())
            .ToDictionary(
                suitOrRank => suitOrRank,
                suitOrRank => (cards & suitOrRank).GetCardCount()
            );

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
                .AddCategory("Straight Flush", HandsGenerator.GetAllStraightFlushes())
            .AppendLine("}");
        return cardsEnumBuilder.ToString();
    }

    public static string GenerateGetTwoPairRanks()
    {
        List<Cards> allRanks = HandsGenerator.GetAllRanks().ToList();
        Dictionary<(Rank, Rank), IEnumerable<Cards>> allTwoPairs = HandsGenerator.GetAllTwoPairs()
            .Select(twoPair => new
            {
                Cards = twoPair,
                Ranks = twoPair
                    .ToSuitAndRankDictionary()
                    .Where(kvp => kvp.Value == 2 && allRanks.Contains(kvp.Key))
                    .Select(kvp => kvp.Key switch
                    {
                        Cards.Twos => Rank.Two,
                        Cards.Threes => Rank.Three,
                        Cards.Fours => Rank.Four,
                        Cards.Fives => Rank.Five,
                        Cards.Sixes => Rank.Six,
                        Cards.Sevens => Rank.Seven,
                        Cards.Eights => Rank.Eight,
                        Cards.Nines => Rank.Nine,
                        Cards.Tens => Rank.Ten,
                        Cards.Jacks => Rank.Jack,
                        Cards.Queens => Rank.Queen,
                        Cards.Kings => Rank.King,
                        Cards.Aces => Rank.AceHigh,
                        _ => Rank.None
                    })
                    .OrderByDescending(k => k)
                    .ToList()
            })
            .GroupBy(twoPair => (twoPair.Ranks[0], twoPair.Ranks[1]))
            .ToDictionary(group => group.Key, group => group.Select(g => g.Cards));

        StringBuilder methodBuilder = new();
        methodBuilder
            .AppendLine("public static (Rank HighestPairRank, Rank LowestPairRank) GetTwoPairRanks(Cards twoPair)")
            .AppendLine("    => twoPair switch")
            .AppendLine("    {");
        foreach (((Rank lowPairRank, Rank highPairRank), IEnumerable<Cards> twoPairs) in allTwoPairs)
        {
            string ranks = $"(Rank.{lowPairRank}, Rank.{highPairRank})";
            string cards = string.Join(" or ", twoPairs.Select(twoPair => $"Cards.{twoPair}"));
            methodBuilder.AppendLine($"        {cards} => {ranks},");
        }
        methodBuilder
            .AppendLine("        _ => throw new ArgumentException($\"{twoPair.ToCardString()} does not contain exactly two pairs.\", nameof(twoPair))")
            .AppendLine("    };");
        return methodBuilder.ToString();
    }

    public static string GenerateStraightRanks()
    {
        Dictionary<Rank, List<Cards>> straightsByRank = [];
        foreach (Cards straight in HandsGenerator.GetAllStraights())
        {
            List<Rank> ranks = straight
                .GetIndividualCards()
                .Select(c => c.GetRank())
                .OrderByDescending(r => r)
                .ToList();
            Rank highest = ranks.Contains(Rank.AceHigh) && ranks.Contains(Rank.Five)
                ? Rank.Five
                : ranks.First();
            
            if (!straightsByRank.ContainsKey(highest)) straightsByRank[highest] = [];
            straightsByRank[highest].Add(straight);
        }

        StringBuilder methodBuilder = new();
        methodBuilder
            .AppendLine("public static Rank GetStraightRank(Cards straight)")
            .AppendLine("    => straight switch")
            .AppendLine("    {");
        foreach ((Rank highestRank, IEnumerable<Cards> straights) in straightsByRank)
        {
            string rank = $"Rank.{highestRank}";
            string cards = string.Join(" or ", straights.Select(straight => $"Cards.{straight}"));
            methodBuilder.AppendLine($"        {cards} => {rank},");
        }
        methodBuilder
            .AppendLine("        _ => throw new ArgumentException($\"{straight.ToCardString()} is not a straight.\", nameof(straight))")
            .AppendLine("    };");
        return methodBuilder.ToString();
    }

    public static string GenerateStraightFlushRanks()
    {
        Dictionary<Rank, List<Cards>> straightsByRank = [];
        foreach (Cards straight in HandsGenerator.GetAllStraightFlushes())
        {
            List<Rank> ranks = straight
                .GetIndividualCards()
                .Select(c => c.GetRank())
                .OrderByDescending(r => r)
                .ToList();
            Rank highest = ranks.Contains(Rank.AceHigh) && ranks.Contains(Rank.Five)
                ? Rank.Five
                : ranks.First();
            
            if (!straightsByRank.ContainsKey(highest)) straightsByRank[highest] = [];
            straightsByRank[highest].Add(straight);
        }

        StringBuilder methodBuilder = new();
        methodBuilder
            .AppendLine("public static Rank GetStraightFlushRank(Cards straightFlush)")
            .AppendLine("    => straightFlush switch")
            .AppendLine("    {");
        foreach ((Rank highestRank, IEnumerable<Cards> straights) in straightsByRank)
        {
            string rank = $"Rank.{highestRank}";
            string cards = string.Join(" or ", straights.Select(straight => $"Cards.{straight}"));
            methodBuilder.AppendLine($"        {cards} => {rank},");
        }
        methodBuilder
            .AppendLine("        _ => throw new ArgumentException($\"{straightFlush.ToCardString()} is not a straight flush.\", nameof(straightFlush))")
            .AppendLine("    };");
        return methodBuilder.ToString();
    }

    public static string GenerateFlushRanks()
    {
        Dictionary<(Rank, Rank, Rank, Rank, Rank), IEnumerable<Cards>> allFlushes = HandsGenerator.GetAllFlushes()
            .Select(flush => new
            {
                Cards = flush,
                Ranks = flush
                    .GetIndividualCards()
                    .Select(c => c.GetRank())
                    .OrderByDescending(r => r)
                    .ToList()
            })
            .GroupBy(flush => (flush.Ranks[0], flush.Ranks[1], flush.Ranks[2], flush.Ranks[3], flush.Ranks[4]))
            .ToDictionary(group => group.Key, group => group.Select(g => g.Cards));

        StringBuilder methodBuilder = new();
        methodBuilder
            .AppendLine("public static (Rank Highest, Rank Second, Rank Third, Rank Fourth, Rank Fifth) GetFlushRanks(Cards flush)")
            .AppendLine("    => flush switch")
            .AppendLine("    {");
        foreach (((Rank highest, Rank second, Rank third, Rank fourth, Rank fifth), IEnumerable<Cards> flushes) in allFlushes)
        {
            string ranks = $"(Rank.{highest}, Rank.{second}, Rank.{third}, Rank.{fourth}, Rank.{fifth})";
            string cards = string.Join(" or ", flushes.Select(flush => $"Cards.{flush}"));
            methodBuilder.AppendLine($"        {cards} => {ranks},");
        }
        methodBuilder
            .AppendLine("        _ => throw new ArgumentException($\"{flush.ToCardString()} is not a flush.\", nameof(flush))")
            .AppendLine("    };");
        return methodBuilder.ToString();
    }

    public static string GenerateFullHouseRanks()
    {
        List<Cards> allRanks = HandsGenerator.GetAllRanks().ToList();
        Dictionary<(Rank, Rank), IEnumerable<Cards>> allFullHouses = HandsGenerator.GetAllFullHouse()
            .Select(fullHouse => new
            {
                Cards = fullHouse,
                Ranks = fullHouse
                    .ToSuitAndRankDictionary()
                    .Where(kvp => allRanks.Contains(kvp.Key))
                    .ToDictionary(
                        kvp => kvp.Key switch
                        {
                            Cards.Twos => Rank.Two,
                            Cards.Threes => Rank.Three,
                            Cards.Fours => Rank.Four,
                            Cards.Fives => Rank.Five,
                            Cards.Sixes => Rank.Six,
                            Cards.Sevens => Rank.Seven,
                            Cards.Eights => Rank.Eight,
                            Cards.Nines => Rank.Nine,
                            Cards.Tens => Rank.Ten,
                            Cards.Jacks => Rank.Jack,
                            Cards.Queens => Rank.Queen,
                            Cards.Kings => Rank.King,
                            Cards.Aces => Rank.AceHigh,
                            _ => Rank.None
                        },
                        kvp => kvp.Value
                    )
            })
            .GroupBy(x => (x.Ranks.Single(kvp => kvp.Value == 3).Key, x.Ranks.Single(kvp => kvp.Value == 2).Key))
            .ToDictionary(group => group.Key, group => group.Select(g => g.Cards));
        
        StringBuilder methodBuilder = new();
        methodBuilder
            .AppendLine("public static (Rank ThreeOfAKindRank, Rank PairRank) GetFullHouseRanks(Cards fullHouse)")
            .AppendLine("    => fullHouse switch")
            .AppendLine("    {");
        foreach (((Rank threeOfAKindRank, Rank pairRank), IEnumerable<Cards> fullHouses) in allFullHouses)
        {
            string ranks = $"(Rank.{threeOfAKindRank}, Rank.{pairRank})";
            string cards = string.Join(" or ", fullHouses.Select(fullHouse => $"Cards.{fullHouse}"));
            methodBuilder.AppendLine($"        {cards} => {ranks},");
        }
        methodBuilder
            .AppendLine("        _ => throw new ArgumentException($\"{fullHouse.ToCardString()} is not a full house.\", nameof(fullHouse))")
            .AppendLine("    };");
        return methodBuilder.ToString();
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
        cardsEnumBuilder.AppendLine("\n    // Ranks (four of a kind)");
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
