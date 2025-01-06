using System.Numerics;

namespace Poker.NET.Engine.Helpers;

public static class CardHelper
{
    public static readonly Dictionary<Cards, Rank> Ranks = new()
    {
        { Cards.TwoOfClubs, Rank.Two },
        { Cards.ThreeOfClubs, Rank.Three },
        { Cards.FourOfClubs, Rank.Four },
        { Cards.FiveOfClubs, Rank.Five },
        { Cards.SixOfClubs, Rank.Six },
        { Cards.SevenOfClubs, Rank.Seven },
        { Cards.EightOfClubs, Rank.Eight },
        { Cards.NineOfClubs, Rank.Nine },
        { Cards.TenOfClubs, Rank.Ten },
        { Cards.JackOfClubs, Rank.Jack },
        { Cards.QueenOfClubs, Rank.Queen },
        { Cards.KingOfClubs, Rank.King },
        { Cards.AceOfClubs, Rank.AceHigh },
        { Cards.TwoOfDiamonds, Rank.Two },
        { Cards.ThreeOfDiamonds, Rank.Three },
        { Cards.FourOfDiamonds, Rank.Four },
        { Cards.FiveOfDiamonds, Rank.Five },
        { Cards.SixOfDiamonds, Rank.Six },
        { Cards.SevenOfDiamonds, Rank.Seven },
        { Cards.EightOfDiamonds, Rank.Eight },
        { Cards.NineOfDiamonds, Rank.Nine },
        { Cards.TenOfDiamonds, Rank.Ten },
        { Cards.JackOfDiamonds, Rank.Jack },
        { Cards.QueenOfDiamonds, Rank.Queen },
        { Cards.KingOfDiamonds, Rank.King },
        { Cards.AceOfDiamonds, Rank.AceHigh },
        { Cards.TwoOfHearts, Rank.Two },
        { Cards.ThreeOfHearts, Rank.Three },
        { Cards.FourOfHearts, Rank.Four },
        { Cards.FiveOfHearts, Rank.Five },
        { Cards.SixOfHearts, Rank.Six },
        { Cards.SevenOfHearts, Rank.Seven },
        { Cards.EightOfHearts, Rank.Eight },
        { Cards.NineOfHearts, Rank.Nine },
        { Cards.TenOfHearts, Rank.Ten },
        { Cards.JackOfHearts, Rank.Jack },
        { Cards.QueenOfHearts, Rank.Queen },
        { Cards.KingOfHearts, Rank.King },
        { Cards.AceOfHearts, Rank.AceHigh },
        { Cards.TwoOfSpades, Rank.Two },
        { Cards.ThreeOfSpades, Rank.Three },
        { Cards.FourOfSpades, Rank.Four },
        { Cards.FiveOfSpades, Rank.Five },
        { Cards.SixOfSpades, Rank.Six },
        { Cards.SevenOfSpades, Rank.Seven },
        { Cards.EightOfSpades, Rank.Eight },
        { Cards.NineOfSpades, Rank.Nine },
        { Cards.TenOfSpades, Rank.Ten },
        { Cards.JackOfSpades, Rank.Jack },
        { Cards.QueenOfSpades, Rank.Queen },
        { Cards.KingOfSpades, Rank.King },
        { Cards.AceOfSpades, Rank.AceHigh },
    };
    public static readonly Dictionary<Cards, Suit> Suits = new()
    {
        { Cards.TwoOfClubs, Suit.Clubs },
        { Cards.ThreeOfClubs, Suit.Clubs },
        { Cards.FourOfClubs, Suit.Clubs },
        { Cards.FiveOfClubs, Suit.Clubs },
        { Cards.SixOfClubs, Suit.Clubs },
        { Cards.SevenOfClubs, Suit.Clubs },
        { Cards.EightOfClubs, Suit.Clubs },
        { Cards.NineOfClubs, Suit.Clubs },
        { Cards.TenOfClubs, Suit.Clubs },
        { Cards.JackOfClubs, Suit.Clubs },
        { Cards.QueenOfClubs, Suit.Clubs },
        { Cards.KingOfClubs, Suit.Clubs },
        { Cards.AceOfClubs, Suit.Clubs },
        { Cards.TwoOfDiamonds, Suit.Diamonds },
        { Cards.ThreeOfDiamonds, Suit.Diamonds },
        { Cards.FourOfDiamonds, Suit.Diamonds },
        { Cards.FiveOfDiamonds, Suit.Diamonds },
        { Cards.SixOfDiamonds, Suit.Diamonds },
        { Cards.SevenOfDiamonds, Suit.Diamonds },
        { Cards.EightOfDiamonds, Suit.Diamonds },
        { Cards.NineOfDiamonds, Suit.Diamonds },
        { Cards.TenOfDiamonds, Suit.Diamonds },
        { Cards.JackOfDiamonds, Suit.Diamonds },
        { Cards.QueenOfDiamonds, Suit.Diamonds },
        { Cards.KingOfDiamonds, Suit.Diamonds },
        { Cards.AceOfDiamonds, Suit.Diamonds },
        { Cards.TwoOfHearts, Suit.Hearts },
        { Cards.ThreeOfHearts, Suit.Hearts },
        { Cards.FourOfHearts, Suit.Hearts },
        { Cards.FiveOfHearts, Suit.Hearts },
        { Cards.SixOfHearts, Suit.Hearts },
        { Cards.SevenOfHearts, Suit.Hearts },
        { Cards.EightOfHearts, Suit.Hearts },
        { Cards.NineOfHearts, Suit.Hearts },
        { Cards.TenOfHearts, Suit.Hearts },
        { Cards.JackOfHearts, Suit.Hearts },
        { Cards.QueenOfHearts, Suit.Hearts },
        { Cards.KingOfHearts, Suit.Hearts },
        { Cards.AceOfHearts, Suit.Hearts },
        { Cards.TwoOfSpades, Suit.Spades },
        { Cards.ThreeOfSpades, Suit.Spades },
        { Cards.FourOfSpades, Suit.Spades },
        { Cards.FiveOfSpades, Suit.Spades },
        { Cards.SixOfSpades, Suit.Spades },
        { Cards.SevenOfSpades, Suit.Spades },
        { Cards.EightOfSpades, Suit.Spades },
        { Cards.NineOfSpades, Suit.Spades },
        { Cards.TenOfSpades, Suit.Spades },
        { Cards.JackOfSpades, Suit.Spades },
        { Cards.QueenOfSpades, Suit.Spades },
        { Cards.KingOfSpades, Suit.Spades },
        { Cards.AceOfSpades, Suit.Spades },
    };

    public static int GetCardCount(this Cards cards)
        => BitOperations.PopCount((ulong)cards);

    public static IEnumerable<Cards> GetIndividualCards(this Cards cards)
        => Enumerable.Range(0, 52)
            .Where(i => ((ulong)cards & (1ul << i)) != 0)
            .Select(i => (Cards)(1ul << i));

    public static Rank GetRank(this Cards card)
    {
        ThrowIfNotSingle(card);

        return Ranks[card];
    }

    public static Suit GetSuit(this Cards card)
    {
        ThrowIfNotSingle(card);

        return Suits[card];
    }
    
    public static void ThrowIfNotSingle(Cards card)
    {
        if (card.GetCardCount() != 1) throw new ArgumentException($"Cards value of {card} does not contain exactly one card.", nameof(card));
    }
}