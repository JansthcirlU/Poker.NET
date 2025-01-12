using Poker.NET.Engine;

namespace Poker.NET.LookupGeneration.Tests;

public class BinaryHandHelperTests
{
    public static readonly IEnumerable<object[]> Data = [
        [
            Cards.TwoOfClubs | Cards.FourOfClubs,
            Cards.ThreeOfClubs | Cards.FiveOfClubs | Cards.SixOfClubs | Cards.EightOfClubs | Cards.NineOfClubs,
            12345
        ],
        [
            Cards.AceOfSpades | Cards.KingOfSpades,
            Cards.ThreeOfClubs | Cards.FiveOfClubs | Cards.SixOfDiamonds | Cards.EightOfHearts | Cards.QueenOfHearts,
            uint.MaxValue
        ],
    ];
    public static readonly TheoryData<byte[]> Bytes = [
        [0x00, 0x00, 0x40, 0x00, 0x2C, 0x00, 0x00, 0x30, 0x39],
        [0x01, 0x4B, 0x46, 0x4F, 0xC4, 0xFF, 0xFF, 0xFF, 0xFF]
    ];

    [Theory]
    [MemberData(nameof(Data))]
    public void PackFollowedByUnpack_ShouldContainOriginalData(
        Cards holeCards,
        Cards communityCards,
        uint score)
    {
        // Arrange
        HoldemHand hand = new(holeCards, communityCards);

        // Act
        byte[] packed = BinaryHandHelper.Pack(hand, score);
        (HoldemHand unpackedHand, uint unpackedScore) = BinaryHandHelper.Unpack(packed);

        // Assert
        Assert.Equal(score, unpackedScore);
        Assert.Equal(hand.HoleCards, unpackedHand.HoleCards);
        Assert.Equal(hand.CommunityCards, unpackedHand.CommunityCards);
    }

    [Theory]
    [MemberData(nameof(Bytes))]
    public void UnpackFollowedByPack_ShouldContainOriginalData(byte[] bytes)
    {
        // Act
        (HoldemHand hand, uint score) = BinaryHandHelper.Unpack(bytes);
        byte[] packed = BinaryHandHelper.Pack(hand, score);

        // Assert
        Assert.Equal(bytes.Length, packed.Length);
        for (int i = 0; i < bytes.Length; i++)
        {
            Assert.Equal(bytes[i], packed[i]);
        }
    }
}