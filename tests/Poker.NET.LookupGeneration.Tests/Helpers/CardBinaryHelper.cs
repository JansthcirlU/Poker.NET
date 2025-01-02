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
    
    public static bool ContainsRanks(this Cards cards, params Rank[] ranks)
    {
        foreach (Rank rank in ranks)
        {
            // Convert the rank enum value to its corresponding Cards value
            Cards cardsRank = rank switch
            {
                Rank.Twos => Cards.Twos,
                Rank.Threes => Cards.Threes,
                Rank.Fours => Cards.Fours,
                Rank.Fives => Cards.Fives,
                Rank.Sixes => Cards.Sixes,
                Rank.Sevens => Cards.Sevens,
                Rank.Eights => Cards.Eights,
                Rank.Nines => Cards.Nines,
                Rank.Tens => Cards.Tens,
                Rank.Jacks => Cards.Jacks,
                Rank.Queens => Cards.Queens,
                Rank.Kings => Cards.Kings,
                Rank.Aces => Cards.Aces,
                _ => throw new ArgumentException($"Invalid rank: {rank}")
            };

            if ((cards & cardsRank) == Cards.None) return false;
        }
        return true;
    }
}