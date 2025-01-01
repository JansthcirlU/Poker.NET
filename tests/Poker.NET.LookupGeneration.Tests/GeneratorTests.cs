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
        int allSevenCardHandsCount = HandsGenerator.GetAllSevenCardHands().Count();

        // Assert
        Assert.Equal(Combinatorics.SevenCardHandsCount, allSevenCardHandsCount);
    }

    [Fact]
    public void AllHandsSanityCheck()
    {
        // Arrange
        ulong allHands = HandsGenerator.GetAllHands()
            .Aggregate(0ul, (total, _) => total + 1); // Count() can only go up to int.MaxValue, which is not enough to hold this number
        
        // Assert
        Assert.Equal(Combinatorics.AllHandsCount, allHands);
    }

    [Fact]
    public void AllRanksSanityCheck()
    {
        // Arrange
        int allRanksCount = HandsGenerator.GetAllRanks()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllRanksCount, allRanksCount);
    }

    [Fact]
    public void AllSuitsSanityCheck()
    {
        // Arrange
        int allRanksCount = HandsGenerator.GetAllSuits()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllSuitsCount, allRanksCount);
    }

    [Fact]
    public void AllPairsSanityCheck()
    {
        // Arrange
        int allPairsCount = HandsGenerator.GetAllPairs()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllPairsCount, allPairsCount);
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
        int allTwoPairsCount = HandsGenerator.GetAllTwoPairs()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllTwoPairsCount, allTwoPairsCount);
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
        int allThreeOfAKindCount = HandsGenerator.GetAllThreeOfAKind()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllThreeOfAKindCount, allThreeOfAKindCount);
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
        int allStraightsCount = HandsGenerator.GetAllStraights()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllStraightsCount, allStraightsCount);
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
        int allFlushesCount = HandsGenerator.GetAllFlushes()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllFlushesCount, allFlushesCount);
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
        int allFullHouseCount = HandsGenerator.GetAllFullHouse()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllFullHouseCount, allFullHouseCount);
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
        int allFourOfAKindCount = HandsGenerator.GetAllFourOfAKind()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllFourOfAKindCount, allFourOfAKindCount);
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
        int allStraightFlushesCount = HandsGenerator.GetAllStraightFlushes()
            .Distinct()
            .Count();

        // Assert
        Assert.Equal(Combinatorics.AllStraightFlushesCount, allStraightFlushesCount);
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