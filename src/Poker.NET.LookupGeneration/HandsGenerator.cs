using System.Numerics;
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

    public static IEnumerable<HoldemHand> GetAllHands()
    {
        ulong holeSet = (1UL << 2) - 1;
        ulong maxValue = 1UL << 52;
        
        while (holeSet < maxValue)
        {
            ulong communitySet = (1UL << 5) - 1;
            
            while (communitySet < maxValue)
            {
                if ((communitySet & holeSet) == 0)
                {
                    yield return new((Cards)holeSet, (Cards)communitySet);
                }
                communitySet = Gosper(communitySet);
            }
            holeSet = Gosper(holeSet);
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
        List<Cards> suits = GetAllSuits().ToList();
        List<Cards> ranks = GetAllRanks().ToList();
        List<Cards> straightFlushes = GetAllStraightFlushes().ToList();
        
        // Generate ace-low straights (A-2-3-4-5)
        foreach (Cards ace in GetCombinations(Cards.Aces, 1))
        {
            foreach (Cards two in GetCombinations(Cards.Twos, 1))
            {
                foreach (Cards three in GetCombinations(Cards.Threes, 1))
                {
                    foreach (Cards four in GetCombinations(Cards.Fours, 1))
                    {
                        foreach (Cards five in GetCombinations(Cards.Fives, 1))
                        {
                            Cards straight = ace | two | three | four | five;
                            if (straightFlushes.Contains(straight)) continue;
                            
                            yield return straight;
                        }
                    }
                }
            }
        }

        // Generate remaining straights (2-3-4-5-6 through 10-J-Q-K-A)
        for (int i = 0; i < 9; i++)  // Start with Two, end with Ten
        {
            List<IEnumerable<Cards>> cards = Enumerable.Range(0, 5)
                .Select(j => GetCombinations(ranks[i + j], 1))
                .ToList();

            foreach (Cards c1 in cards[0])
            {
                foreach (Cards c2 in cards[1])
                {
                    foreach (Cards c3 in cards[2])
                    {
                        foreach (Cards c4 in cards[3])
                        {
                            foreach (Cards c5 in cards[4])
                            {
                                Cards straight = c1 | c2 | c3 | c4 | c5;
                                if (straightFlushes.Contains(straight)) continue;
                                
                                yield return straight;
                            }
                        }
                    }
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
        
        // For each suit...
        foreach (Cards suit in suits)
        {
            // Generate ace-low straight flush (A-2-3-4-5)
            Cards[] aceLowCards =
            [
                Cards.Aces & suit,      // Ace
                Cards.Twos & suit,      // Two
                Cards.Threes & suit,    // Three
                Cards.Fours & suit,     // Four
                Cards.Fives & suit      // Five
            ];
            yield return aceLowCards.Aggregate((a, b) => a | b);

            // Generate remaining straight flushes (2-3-4-5-6 through 10-J-Q-K-A)
            List<Cards> ranks = GetAllRanks().ToList();
            for (int i = 0; i < 9; i++)  // Start with Two, end with Ten
            {
                Cards straightFlush = Cards.None;
                for (int j = 0; j < 5; j++)
                {
                    Cards ranksInSuit = ranks[i + j] & suit;
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

    private static ulong Gosper(ulong x)
    {
        ulong y = x & ~x + 1;
        ulong c = x + y;
        return (((x ^ c) >> 2) / y) | c;
    }
}
