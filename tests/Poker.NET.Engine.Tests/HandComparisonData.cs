using Xunit.Abstractions;

namespace Poker.NET.Engine.Tests;

public struct HandComparisonData : IXunitSerializable
{
    public Cards FirstHoleCards { get; private set; }
    public Cards SecondHoleCards { get; private set; }
    public Cards CommunityCards { get; private set; }

    public HandComparisonData(Cards firstHoleCards, Cards secondHoleCards, Cards communityCards)
    {
        FirstHoleCards = firstHoleCards;
        SecondHoleCards = secondHoleCards;
        CommunityCards = communityCards;
    }

    public void Deserialize(IXunitSerializationInfo info)
    {
        FirstHoleCards = info.GetValue<Cards>(nameof(FirstHoleCards));
        SecondHoleCards = info.GetValue<Cards>(nameof(SecondHoleCards));
        CommunityCards = info.GetValue<Cards>(nameof(CommunityCards));
    }

    public readonly void Serialize(IXunitSerializationInfo info)
    {
        info.AddValue(nameof(FirstHoleCards), FirstHoleCards);
        info.AddValue(nameof(SecondHoleCards), SecondHoleCards);
        info.AddValue(nameof(CommunityCards), CommunityCards);
    }
}