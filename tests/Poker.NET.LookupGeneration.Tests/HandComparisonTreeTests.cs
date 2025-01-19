using Poker.NET.Engine;
using Poker.NET.Engine.Evaluators.Naive;
using Poker.NET.LookupGeneration.Data;

namespace Poker.NET.LookupGeneration.Tests;

public class HandComparisonTreeTests
{
    [Fact]
    public void Add_EmptyTree_AddsFirstHand()
    {
        // Arrange
        HandComparisonTree tree = [];
        HoldemHand hand = new(
            Cards.TwoOfDiamonds | Cards.ThreeOfHearts,
            Cards.FourOfDiamonds | Cards.FiveOfHearts | Cards.TenOfClubs | Cards.JackOfDiamonds | Cards.KingOfClubs);

        // Act
        tree.Add(hand);

        // Assert
        Assert.Single(tree);
        Assert.Equal(hand, tree.First());
    }

    [Fact]
    public void Add_DifferentStrengthHands_MaintainsOrder()
    {
        // Arrange
        HandComparisonTree tree = [];
        
        HoldemHand strongHand = new(
            Cards.TwoOfDiamonds | Cards.TwoOfHearts,
            Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs);
        HoldemHand mediumHand = new(
            Cards.TwoOfDiamonds | Cards.FourOfHearts,
            Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs);
        HoldemHand weakHand = new(
            Cards.TwoOfDiamonds | Cards.ThreeOfHearts,
            Cards.FourOfDiamonds | Cards.FiveOfHearts | Cards.TenOfClubs | Cards.JackOfDiamonds | Cards.KingOfClubs);

        // Act - add in mixed order to verify sorting
        tree.Add(mediumHand);
        tree.Add(weakHand);
        tree.Add(strongHand);

        // Assert
        List<HoldemHand> hands = [.. tree];
        Assert.Equal(3, hands.Count);
        
        NaiveHandEvaluator evaluator = new();
        Assert.True(evaluator.Compare(hands[0], hands[1]) <= 0);
        Assert.True(evaluator.Compare(hands[1], hands[2]) <= 0);
    }

    [Fact]
    public void Add_SameStrengthHands_GroupsInSameBin()
    {
        // Arrange
        HandComparisonTree tree = [];
        
        // TODO: Create two different hands of equal strength
        // For example, two different pairs of aces with different kickers
        HoldemHand firstHand = new(
            Cards.FourOfClubs | Cards.FourOfDiamonds,
            Cards.TwoOfClubs | Cards.SixOfHearts | Cards.EightOfHearts | Cards.TenOfClubs | Cards.AceOfSpades);
        HoldemHand secondHand = new(
            Cards.FourOfHearts | Cards.FourOfSpades,
            Cards.TwoOfClubs | Cards.SixOfHearts | Cards.EightOfHearts | Cards.TenOfClubs | Cards.AceOfSpades);

        // Act
        tree.Add(firstHand);
        tree.Add(secondHand);

        // Assert
        Assert.Equal(2, tree.Count());
        NaiveHandEvaluator evaluator = new();
        Assert.Equal(0, evaluator.Compare(firstHand, secondHand));
    }

    [Fact]
    public void FromExisting_ShouldEqualManualCreation()
    {
        // Arrange
        HoldemHand firstPair = new(
            Cards.FourOfClubs | Cards.FourOfDiamonds,
            Cards.TwoOfClubs | Cards.SixOfHearts | Cards.EightOfHearts | Cards.TenOfClubs | Cards.AceOfSpades);
        HoldemHand secondPair = new(
            Cards.FourOfHearts | Cards.FourOfSpades,
            Cards.TwoOfClubs | Cards.SixOfHearts | Cards.EightOfHearts | Cards.TenOfClubs | Cards.AceOfSpades);
        HoldemHand firstThreeOfAKind = new(
            Cards.TwoOfDiamonds | Cards.TwoOfHearts,
            Cards.TwoOfClubs | Cards.FourOfClubs | Cards.SixOfSpades | Cards.EightOfHearts | Cards.TenOfClubs);
        LinkedList<HoldemHand> pairBin = [];
        pairBin.AddLast(firstPair);
        pairBin.AddLast(secondPair);
        LinkedList<HoldemHand> threeOfAKindBin = [];
        threeOfAKindBin.AddLast(firstThreeOfAKind);
        List<LinkedList<HoldemHand>> existing = [pairBin, threeOfAKindBin];

        // Act
        HandComparisonTree fromExisting = HandComparisonTree.FromExisting(existing);
        HandComparisonTree tree = [];
        tree.Add(firstThreeOfAKind);
        tree.Add(firstPair);
        tree.Add(secondPair);

        // Assert
        Assert.Equal(tree.Count(), fromExisting.Count());
        foreach ((HoldemHand treeHand, HoldemHand fromExistingHand) in tree.Zip(fromExisting))
        {
            Assert.Equal(treeHand, fromExistingHand);
        }
    }
}