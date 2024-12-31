using Poker.NET.Engine;
using Poker.NET.LookupGeneration.Tests.Helpers;

namespace Poker.NET.LookupGeneration.Tests;

public class GeneratorTests
{
    public static TheoryData<Cards> AllPairs => [.. HandsGenerator.GetAllPairs()];
    public static TheoryData<Cards> AllTwoPairs => [.. HandsGenerator.GetAllTwoPairs()];
    public static TheoryData<Cards> AllThreeOfAKind => [.. HandsGenerator.GetAllThreeOfAKind()];
    public static TheoryData<Cards> AllStraights => [.. HandsGenerator.GetAllStraights()];
    public static TheoryData<Cards> AllFlushes => [.. HandsGenerator.GetAllFlushes()];
    public static TheoryData<Cards> AllFullHouse => [.. HandsGenerator.GetAllFullHouse()];
    public static TheoryData<Cards> AllFourOfAKind => [.. HandsGenerator.GetAllFourOfAKind()];
    public static TheoryData<Cards> AllStraightFlushes => [.. HandsGenerator.GetAllStraightFlushes()];

    [Fact]
    public void SanityCheck()
    {
        // Arrange
        List<Cards> allSevenCardHands = HandsGenerator.GetAllSevenCardHands().ToList();

        // Assert
        int fiftyTwoChooseSeven = Combinatorics.SevenCardHandsCount;
        Assert.Equal(fiftyTwoChooseSeven, allSevenCardHands.Count);
    }

    [Fact]
    public void AllHandsSanityCheck()
    {
        // Arrange
        List<HoldemHand> firstNHands = HandsGenerator.GetAllHands()
            .Take(100)
            .ToList();
        
        Assert.NotEmpty(firstNHands);
    }

    [Fact]
    public void AllRanksSanityCheck()
    {
        // Arrange
        List<Cards> allRanks = HandsGenerator.GetAllRanks().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllRanksCount, allRanks.Count);
    }

    [Fact]
    public void AllSuitsSanityCheck()
    {
        // Arrange
        List<Cards> allRanks = HandsGenerator.GetAllSuits().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllSuitsCount, allRanks.Count);
    }

    [Fact]
    public void AllPairsSanityCheck()
    {
        // Arrange
        List<Cards> allPairs = HandsGenerator.GetAllPairs().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllPairsCount, allPairs.Count);
    }

    [Theory]
    [MemberData(nameof(AllPairs))]
    public void IndividualPairSanityCheck(Cards pair)
    {
        // Assert
        Assert.Equal(2, pair.GetCardCount());
        Assert.True(pair.AreTheSameRank());
    }

    [Fact]
    public void AllTwoPairsSanityCheck()
    {
        // Arrange
        List<Cards> allTwoPairs = HandsGenerator.GetAllTwoPairs().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllTwoPairsCount, allTwoPairs.Count);
    }

    [Theory]
    [MemberData(nameof(AllTwoPairs))]
    public void IndividualTwoPairSanityCheck(Cards twoPair)
    {
        // Arrange
        Dictionary<Cards, int> counts = twoPair.ToSuitAndRankDictionary();
        int differentRanks = 0;

        // Act
        foreach (Cards rank in HandsGenerator.GetAllRanks())
        {
            if (counts[rank] == 2) differentRanks++;
        }

        // Assert
        Assert.Equal(2, differentRanks);
        Assert.Equal(4, twoPair.GetCardCount());
    }

    [Fact]
    public void AllThreeOfAKindSanityCheck()
    {
        // Arrange
        List<Cards> allThreeOfAKind = HandsGenerator.GetAllThreeOfAKind().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllThreeOfAKindCount, allThreeOfAKind.Count);
    }

    [Theory]
    [MemberData(nameof(AllThreeOfAKind))]
    public void IndividualThreeOfAKindSanityCheck(Cards threeOfAKind)
    {
        // Assert
        Assert.Equal(3, threeOfAKind.GetCardCount());
        Assert.True(threeOfAKind.AreTheSameRank());
    }

    [Fact]
    public void AllStraightsSanityCheck()
    {
        // Arrange
        List<Cards> allStraights = HandsGenerator.GetAllStraights().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllStraightsCount, allStraights.Count);
    }

    [Theory]
    [MemberData(nameof(AllStraights))]
    public void IndividualStraightSanityCheck(Cards straight)
    {
        // Assert
        Assert.Equal(5, straight.GetCardCount());
        Assert.False(straight.AreTheSameSuit()); // Must not be straight flush

        bool isFiveHigh = straight.ContainsRanks(Ranks.Fives, Ranks.Fours, Ranks.Threes, Ranks.Twos, Ranks.Aces);
        bool isSixHigh = straight.ContainsRanks(Ranks.Sixes, Ranks.Fives, Ranks.Fours, Ranks.Threes, Ranks.Twos);
        bool isSevenHigh = straight.ContainsRanks(Ranks.Sevens, Ranks.Sixes, Ranks.Fives, Ranks.Fours, Ranks.Threes);
        bool isEightHigh = straight.ContainsRanks(Ranks.Eights, Ranks.Sevens, Ranks.Sixes, Ranks.Fives, Ranks.Fours);
        bool isNineHigh = straight.ContainsRanks(Ranks.Nines, Ranks.Eights, Ranks.Sevens, Ranks.Sixes, Ranks.Fives);
        bool isTenHigh = straight.ContainsRanks(Ranks.Tens, Ranks.Nines, Ranks.Eights, Ranks.Sevens, Ranks.Sixes);
        bool isJackHigh = straight.ContainsRanks(Ranks.Jacks, Ranks.Tens, Ranks.Nines, Ranks.Eights, Ranks.Sevens);
        bool isQueenHigh = straight.ContainsRanks(Ranks.Queens, Ranks.Jacks, Ranks.Tens, Ranks.Nines, Ranks.Eights);
        bool isKingHigh = straight.ContainsRanks(Ranks.Kings, Ranks.Queens, Ranks.Jacks, Ranks.Tens, Ranks.Nines);
        bool isAceHigh = straight.ContainsRanks(Ranks.Aces, Ranks.Kings, Ranks.Queens, Ranks.Jacks, Ranks.Tens);
        Assert.True(
            isFiveHigh ||
            isSixHigh ||
            isSevenHigh ||
            isEightHigh ||
            isNineHigh ||
            isTenHigh ||
            isJackHigh ||
            isQueenHigh ||
            isKingHigh ||
            isAceHigh);
    }

    [Fact]
    public void AllFlushesSanityCheck()
    {
        // Arrange
        List<Cards> allFlushes = HandsGenerator.GetAllFlushes().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllFlushesCount, allFlushes.Count);
    }

    [Theory]
    [MemberData(nameof(AllFlushes))]
    public void IndividualFlushSanityCheck(Cards flush)
    {
        // Assert
        Assert.Equal(5, flush.GetCardCount());
        Assert.True(flush.AreTheSameSuit());
    }

    [Fact]
    public void AllFullHouseSanityCheck()
    {
        // Arrange
        List<Cards> allFullHouse = HandsGenerator.GetAllFullHouse().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllFullHouseCount, allFullHouse.Count);
    }

    [Theory]
    [MemberData(nameof(AllFullHouse))]
    public void IndividualFullHouseSanityCheck(Cards fullHouse)
    {
        // Arrange
        Dictionary<Cards, int> counts = fullHouse.ToSuitAndRankDictionary();
        int pairs = 0;
        int triples = 0;

        // Act
        foreach (Cards rank in HandsGenerator.GetAllRanks())
        {
            if (counts[rank] == 2) pairs++;
            if (counts[rank] == 3) triples++;
        }

        // Assert
        Assert.Equal(5, fullHouse.GetCardCount());
        Assert.Equal(1, pairs);
        Assert.Equal(1, triples);
    }

    [Fact]
    public void AllFourOfAKindSanityCheck()
    {
        // Arrange
        List<Cards> allFourOfAKind = HandsGenerator.GetAllFourOfAKind().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllFourOfAKindCount, allFourOfAKind.Count);
    }

    [Theory]
    [MemberData(nameof(AllFourOfAKind))]
    public void IndividualFourOfAKindSanityCheck(Cards fourOfAKind)
    {
        // Assert
        Assert.Equal(4, fourOfAKind.GetCardCount());
        Assert.True(fourOfAKind.AreTheSameRank());
    }

    [Fact]
    public void AllStraightFlushesSanityCheck()
    {
        // Arrange
        List<Cards> allStraightFlushes = HandsGenerator.GetAllStraightFlushes().ToList();

        // Assert
        Assert.Equal(Combinatorics.AllStraightFlushesCount, allStraightFlushes.Count);
    }

    [Theory]
    [MemberData(nameof(AllStraightFlushes))]
    public void IndividualStraightFlushSanityCheck(Cards straightFlush)
    {
        // Assert
        Assert.Equal(5, straightFlush.GetCardCount());
        Assert.True(straightFlush.AreTheSameSuit());

        bool isFiveHigh = straightFlush.ContainsRanks(Ranks.Fives, Ranks.Fours, Ranks.Threes, Ranks.Twos, Ranks.Aces);
        bool isSixHigh = straightFlush.ContainsRanks(Ranks.Sixes, Ranks.Fives, Ranks.Fours, Ranks.Threes, Ranks.Twos);
        bool isSevenHigh = straightFlush.ContainsRanks(Ranks.Sevens, Ranks.Sixes, Ranks.Fives, Ranks.Fours, Ranks.Threes);
        bool isEightHigh = straightFlush.ContainsRanks(Ranks.Eights, Ranks.Sevens, Ranks.Sixes, Ranks.Fives, Ranks.Fours);
        bool isNineHigh = straightFlush.ContainsRanks(Ranks.Nines, Ranks.Eights, Ranks.Sevens, Ranks.Sixes, Ranks.Fives);
        bool isTenHigh = straightFlush.ContainsRanks(Ranks.Tens, Ranks.Nines, Ranks.Eights, Ranks.Sevens, Ranks.Sixes);
        bool isJackHigh = straightFlush.ContainsRanks(Ranks.Jacks, Ranks.Tens, Ranks.Nines, Ranks.Eights, Ranks.Sevens);
        bool isQueenHigh = straightFlush.ContainsRanks(Ranks.Queens, Ranks.Jacks, Ranks.Tens, Ranks.Nines, Ranks.Eights);
        bool isKingHigh = straightFlush.ContainsRanks(Ranks.Kings, Ranks.Queens, Ranks.Jacks, Ranks.Tens, Ranks.Nines);
        bool isAceHigh = straightFlush.ContainsRanks(Ranks.Aces, Ranks.Kings, Ranks.Queens, Ranks.Jacks, Ranks.Tens);
        Assert.True(
            isFiveHigh ||
            isSixHigh ||
            isSevenHigh ||
            isEightHigh ||
            isNineHigh ||
            isTenHigh ||
            isJackHigh ||
            isQueenHigh ||
            isKingHigh ||
            isAceHigh);
    }
}