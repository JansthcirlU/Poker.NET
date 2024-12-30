using Poker.NET.Engine;

namespace Poker.NET.LookupGeneration;

public static class HandsGenerator
{
    public static IEnumerable<Cards> GetAllSevenCardHands()
    {
        ulong hand = (1ul << 7) - 1;
        ulong max = hand << 52 - 7;

        while (hand <= max)
        {
            yield return (Cards)hand;

            // Gosper's Hack to find the next number with n bits set
            ulong c = hand & ~hand + 1;
            ulong r = hand + c;
            hand = ((r ^ hand) >> 2) / c | r;
        }
    }

    public static IEnumerable<Cards> GetAllRanks()
    {
        yield return Cards.Twos;
        yield return Cards.Threes;
        yield return Cards.Fours;
        yield return Cards.Fives;
        yield return Cards.Sixes;
        yield return Cards.Sevens;
        yield return Cards.Eights;
        yield return Cards.Nines;
        yield return Cards.Tens;
        yield return Cards.Jacks;
        yield return Cards.Queens;
        yield return Cards.Kings;
        yield return Cards.Aces;
    }

    public static IEnumerable<Cards> GetAllSuits()
    {
        yield return Cards.Clubs;
        yield return Cards.Diamonds;
        yield return Cards.Hearts;
        yield return Cards.Spades;
    }

    public static IEnumerable<Cards> GetAllPairs()
    {
        foreach (Cards rank in GetAllRanks())
        {
            foreach (Cards pair in GetCombinations(rank, 2))
            {
                yield return pair;
            }
        }
    }

    public static IEnumerable<Cards> GetAllTwoPairs()
    {
        List<IGrouping<Cards, Cards>> pairsByRank = GetAllPairs()
            .GroupBy(pair => GetAllRanks().First(rank => (pair & rank) == pair))
            .ToList();

        for (int i = 0; i < pairsByRank.Count; i++)
        {
            for (int j = i + 1; j < pairsByRank.Count; j++)
            {
                foreach (Cards firstPair in pairsByRank[i])
                {
                    foreach (Cards secondPair in pairsByRank[j])
                    {
                        yield return firstPair | secondPair;
                    }
                }
            }
        }
    }

    public static IEnumerable<Cards> GetAllThreeOfAKind()
    {
        foreach (Cards rank in GetAllRanks())
        {
            foreach (Cards triple in GetCombinations(rank, 3))
            {
                yield return triple;
            }
        }
    }

    public static IEnumerable<Cards> GetAllStraights()
    {
        List<Cards> ranks = GetAllRanks().ToList();
        List<Cards> straightFlushes = GetAllStraightFlushes().ToList();
        
        // For each possible starting rank (goes up to Ten for Ten-to-Ace straight)
        for (int i = 0; i < 10; i++)
        {
            // Get all possible combinations of one card from each rank in sequence
            List<Cards> rankSequence = Enumerable.Range(0, 5)
                .Select(j => ranks[(i + j) % 13])
                .ToList();

            // Get all combinations of one card from each rank
            List<IEnumerable<Cards>> combinations = rankSequence
                .Select(rank => GetCombinations(rank, 1))
                .ToList();

            // Generate all possible combinations using these ranks
            IEnumerable<Cards> result = combinations[0]
                .SelectMany(c1 => combinations[1]
                    .SelectMany(c2 => combinations[2]
                        .SelectMany(c3 => combinations[3]
                            .SelectMany(c4 => combinations[4]
                                .Select(c5 => c1 | c2 | c3 | c4 | c5)))));

            // Exclude straight flushes
            foreach (Cards straight in result)
            {
                if (!straightFlushes.Contains(straight))
                {
                    yield return straight;
                }
            }
        }
    }
    
    public static IEnumerable<Cards> GetAllFlushes()
    {
        List<Cards> suits = GetAllSuits().ToList();
        List<Cards> straightFlushes = GetAllStraightFlushes().ToList();
        
        foreach (Cards suit in suits)
        {
            // Get all 5-card combinations in this suit
            IEnumerable<Cards> flushCombinations = GetCombinations(suit, 5);
            
            // Exclude straight flushes
            foreach (Cards flush in flushCombinations)
            {
                if (!straightFlushes.Contains(flush))
                {
                    yield return flush;
                }
            }
        }
    }

    public static IEnumerable<Cards> GetAllFullHouse()
    {
        List<Cards> ranks = GetAllRanks().ToList();

        // For each possible three of a kind rank...
        for (int i = 0; i < ranks.Count; i++)
        {
            IEnumerable<Cards> threeOfKindCombos = GetCombinations(ranks[i], 3);

            // For each possible pair rank (excluding the three of a kind rank)...
            for (int j = 0; j < ranks.Count; j++)
            {
                if (j == i) continue; // Skip same rank

                IEnumerable<Cards> pairCombos = GetCombinations(ranks[j], 2);

                // Combine each three of a kind with each pair
                foreach (Cards three in threeOfKindCombos)
                {
                    foreach (Cards pair in pairCombos)
                    {
                        yield return three | pair;
                    }
                }
            }
        }
    }

    public static IEnumerable<Cards> GetAllFourOfAKind()
        => GetAllRanks();

    public static IEnumerable<Cards> GetAllStraightFlushes()
    {
        List<Cards> suits = GetAllSuits().ToList();
        List<Cards> ranks = GetAllRanks().ToList();
        
        // For each suit...
        foreach (Cards suit in suits)
        {
            // For each starting rank (goes up to Ten for Ten-to-Ace straight)
            for (int i = 0; i < 10; i++)
            {
                // Build a 5-card sequence
                Cards straightFlush = Cards.None;
                for (int j = 0; j < 5; j++)
                {
                    // Get the cards of this rank in this suit
                    Cards ranksInSuit = ranks[(i + j) % 13] & suit;
                    straightFlush |= ranksInSuit;
                }
                yield return straightFlush;
            }
        }
    }

    private static IEnumerable<Cards> GetCombinations(Cards source, int count)
    {
        List<Cards> cards = Enumerable.Range(0, 52)
            .Where(i => ((ulong)source & (1ul << i)) != 0)
            .Select(i => (Cards)(1ul << i))
            .ToList();

        return GetCombinationsRecursive(cards, count)
            .Select(combo => combo.Aggregate((a, b) => a | b));
    }

    private static IEnumerable<List<Cards>> GetCombinationsRecursive(List<Cards> cards, int count)
    {
        if (count == 0) return [[]];
        if (cards.Count < count) return [];

        Cards first = cards[0];
        List<Cards> rest = cards.Skip(1).ToList();

        return GetCombinationsRecursive(rest, count - 1)
            .Select<List<Cards>, List<Cards>>(combo => [first, .. combo])
            .Concat(GetCombinationsRecursive(rest, count));
    }
}
