using Poker.NET.Engine.Helpers;

namespace Poker.NET.Engine;

public readonly record struct HoldemHand
{
    public Cards HoleCards { get; }
    public Cards CommunityCards { get; }

    public HoldemHand()
    {
        throw new InvalidOperationException("You must not use the default constructor to create a HoldemHand.");
    }
    public HoldemHand(Cards holeCards, Cards communityCards)
    {
        if ((holeCards & communityCards) != Cards.None) throw new ArgumentException($"Hole cards and community cards must not overlap in {holeCards.ToCardString()} and {communityCards.ToCardString()}", nameof(communityCards));
        if (holeCards.GetCardCount() != 2) throw new ArgumentException($"There are not exactly two hole cards in {holeCards.ToCardString()}", nameof(holeCards));
        if (communityCards.GetCardCount() != 5) throw new ArgumentException($"There are not exactly five community cards in {communityCards.ToCardString()}", nameof(communityCards));

        HoleCards = holeCards;
        CommunityCards = communityCards;
    }

    public override string ToString()
        => (HoleCards | CommunityCards).ToCardString();
}
