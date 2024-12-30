using System.Numerics;
using Poker.NET.Engine;

namespace Poker.NET.LookupGeneration.Tests.Helpers;

public static class CardBinaryHelper
{
    public static int GetCardCount(this Cards cards)
        => BitOperations.PopCount((ulong)cards);

    public static Dictionary<Cards, int> ToSuitAndRankDictionary(this Cards cards)
        => HandsGenerator.GetAllSuits()
            .Concat(HandsGenerator.GetAllRanks())
            .ToDictionary(
                suitOrRank => suitOrRank,
                suitOrRank => (cards & suitOrRank).GetCardCount()
            );

    public static bool AreTheSameSuit(this Cards cards)
        => (cards & Cards.Clubs) == cards
        || (cards & Cards.Diamonds) == cards
        || (cards & Cards.Hearts) == cards
        || (cards & Cards.Spades) == cards;

    public static bool AreTheSameRank(this Cards cards)
        => (cards & Cards.Twos) == cards
        || (cards & Cards.Threes) == cards
        || (cards & Cards.Fours) == cards
        || (cards & Cards.Fives) == cards
        || (cards & Cards.Sixes) == cards
        || (cards & Cards.Sevens) == cards
        || (cards & Cards.Eights) == cards
        || (cards & Cards.Nines) == cards
        || (cards & Cards.Tens) == cards
        || (cards & Cards.Jacks) == cards
        || (cards & Cards.Queens) == cards
        || (cards & Cards.Kings) == cards
        || (cards & Cards.Aces) == cards;
    
    public static bool ContainsRanks(this Cards cards, params Ranks[] ranks)
    {
        foreach (Ranks rank in ranks)
        {
            // Convert the rank enum value to its corresponding Cards value
            Cards cardsRank = rank switch
            {
                Ranks.Twos => Cards.Twos,
                Ranks.Threes => Cards.Threes,
                Ranks.Fours => Cards.Fours,
                Ranks.Fives => Cards.Fives,
                Ranks.Sixes => Cards.Sixes,
                Ranks.Sevens => Cards.Sevens,
                Ranks.Eights => Cards.Eights,
                Ranks.Nines => Cards.Nines,
                Ranks.Tens => Cards.Tens,
                Ranks.Jacks => Cards.Jacks,
                Ranks.Queens => Cards.Queens,
                Ranks.Kings => Cards.Kings,
                Ranks.Aces => Cards.Aces,
                _ => throw new ArgumentException($"Invalid rank: {rank}")
            };

            if ((cards & cardsRank) == Cards.None) return false;
        }
        return true;
    }
}