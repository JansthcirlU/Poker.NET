namespace Poker.NET.Engine;

[Flags]
public enum Cards : ulong
{
    None,
    
    // Clubs
    TwoOfClubs = 1UL << 0,
    ThreeOfClubs = 1UL << 1,
    FourOfClubs = 1UL << 2,
    FiveOfClubs = 1UL << 3,
    SixOfClubs = 1UL << 4,
    SevenOfClubs = 1UL << 5,
    EightOfClubs = 1UL << 6,
    NineOfClubs = 1UL << 7,
    TenOfClubs = 1UL << 8,
    JackOfClubs = 1UL << 9,
    QueenOfClubs = 1UL << 10,
    KingOfClubs = 1UL << 11,
    AceOfClubs = 1UL << 12,

    // Diamonds
    TwoOfDiamonds = 1UL << 13,
    ThreeOfDiamonds = 1UL << 14,
    FourOfDiamonds = 1UL << 15,
    FiveOfDiamonds = 1UL << 16,
    SixOfDiamonds = 1UL << 17,
    SevenOfDiamonds = 1UL << 18,
    EightOfDiamonds = 1UL << 19,
    NineOfDiamonds = 1UL << 20,
    TenOfDiamonds = 1UL << 21,
    JackOfDiamonds = 1UL << 22,
    QueenOfDiamonds = 1UL << 23,
    KingOfDiamonds = 1UL << 24,
    AceOfDiamonds = 1UL << 25,

    // Hearts
    TwoOfHearts = 1UL << 26,
    ThreeOfHearts = 1UL << 27,
    FourOfHearts = 1UL << 28,
    FiveOfHearts = 1UL << 29,
    SixOfHearts = 1UL << 30,
    SevenOfHearts = 1UL << 31,
    EightOfHearts = 1UL << 32,
    NineOfHearts = 1UL << 33,
    TenOfHearts = 1UL << 34,
    JackOfHearts = 1UL << 35,
    QueenOfHearts = 1UL << 36,
    KingOfHearts = 1UL << 37,
    AceOfHearts = 1UL << 38,

    // Spades
    TwoOfSpades = 1UL << 39,
    ThreeOfSpades = 1UL << 40,
    FourOfSpades = 1UL << 41,
    FiveOfSpades = 1UL << 42,
    SixOfSpades = 1UL << 43,
    SevenOfSpades = 1UL << 44,
    EightOfSpades = 1UL << 45,
    NineOfSpades = 1UL << 46,
    TenOfSpades = 1UL << 47,
    JackOfSpades = 1UL << 48,
    QueenOfSpades = 1UL << 49,
    KingOfSpades = 1UL << 50,
    AceOfSpades = 1UL << 51,

    // Combined ranks
    Twos = TwoOfClubs | TwoOfDiamonds | TwoOfHearts | TwoOfSpades,
    Threes = ThreeOfClubs | ThreeOfDiamonds | ThreeOfHearts | ThreeOfSpades,
    Fours = FourOfClubs | FourOfDiamonds | FourOfHearts | FourOfSpades,
    Fives = FiveOfClubs | FiveOfDiamonds | FiveOfHearts | FiveOfSpades,
    Sixes = SixOfClubs | SixOfDiamonds | SixOfHearts | SixOfSpades,
    Sevens = SevenOfClubs | SevenOfDiamonds | SevenOfHearts | SevenOfSpades,
    Eights = EightOfClubs | EightOfDiamonds | EightOfHearts | EightOfSpades,
    Nines = NineOfClubs | NineOfDiamonds | NineOfHearts | NineOfSpades,
    Tens = TenOfClubs | TenOfDiamonds | TenOfHearts | TenOfSpades,
    Jacks = JackOfClubs | JackOfDiamonds | JackOfHearts | JackOfSpades,
    Queens = QueenOfClubs | QueenOfDiamonds | QueenOfHearts | QueenOfSpades,
    Kings = KingOfClubs | KingOfDiamonds | KingOfHearts | KingOfSpades,
    Aces = AceOfClubs | AceOfDiamonds | AceOfHearts | AceOfSpades,

    // Combined suits
    Clubs = TwoOfClubs | ThreeOfClubs | FourOfClubs | FiveOfClubs | SixOfClubs |
            SevenOfClubs | EightOfClubs | NineOfClubs | TenOfClubs | JackOfClubs |
            QueenOfClubs | KingOfClubs | AceOfClubs,

    Diamonds = TwoOfDiamonds | ThreeOfDiamonds | FourOfDiamonds | FiveOfDiamonds |
               SixOfDiamonds | SevenOfDiamonds | EightOfDiamonds | NineOfDiamonds |
               TenOfDiamonds | JackOfDiamonds | QueenOfDiamonds | KingOfDiamonds |
               AceOfDiamonds,

    Hearts = TwoOfHearts | ThreeOfHearts | FourOfHearts | FiveOfHearts | SixOfHearts |
             SevenOfHearts | EightOfHearts | NineOfHearts | TenOfHearts | JackOfHearts |
             QueenOfHearts | KingOfHearts | AceOfHearts,

    Spades = TwoOfSpades | ThreeOfSpades | FourOfSpades | FiveOfSpades | SixOfSpades |
             SevenOfSpades | EightOfSpades | NineOfSpades | TenOfSpades | JackOfSpades |
             QueenOfSpades | KingOfSpades | AceOfSpades,

    Deck = Clubs | Diamonds | Hearts | Spades
}
