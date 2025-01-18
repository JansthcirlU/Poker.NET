using System.Collections.Frozen;
using Poker.NET.Engine;

namespace Poker.NET.LookupGeneration;

public static class BinaryHandHelper
{
    const int BitsPerByte = 8;
    const int HoleCardBits = 11;
    const int CommunityCardBits = 22;
    const ulong ByteMask            = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_1111_1111;
    const ulong HoleCardMask        = 0b_0000_0000_0000_0000_0000_0000_0000_0001_1111_1111_1100_0000_0000_0000_0000_0000;
    const ulong CommunityCardMask   = 0b_0000_0000_0000_0000_0000_0000_0000_0000_0000_0000_0011_1111_1111_1111_1111_1111;

    private static readonly FrozenDictionary<Cards, ushort> HoleCardIndices
        = HandsGenerator.GetAllKCardHands(2)
            .Select((cards, index) => (cards, index))
            .ToFrozenDictionary(x => x.cards, x => (ushort)x.index);
    private static readonly FrozenDictionary<ushort, Cards> HoleCardsIndicesReversed
        = HoleCardIndices.ToFrozenDictionary(kvp => kvp.Value, x => x.Key);
    private static readonly FrozenDictionary<Cards, uint> CommunityCardIndices
        = HandsGenerator.GetAllKCardHands(5)
            .Select((cards, index) => (cards, index))
            .ToFrozenDictionary(x => x.cards, x => (uint)x.index);
    private static readonly FrozenDictionary<uint, Cards> CommunityCardsIndicesReversed
        = CommunityCardIndices.ToFrozenDictionary(kvp => kvp.Value, x => x.Key);

    public static byte[] Pack(HoldemHand hand, uint score)
    {
        int scoreBytes = sizeof(uint);
        int scoreBits = scoreBytes * BitsPerByte;
        int dataBits = HoleCardBits + CommunityCardBits + scoreBits;
        int paddingBits = 8 - (dataBits % 8);
        int sizeBytes = (paddingBits + dataBits) / 8;
        byte[] bytes = new byte[sizeBytes];
        
        // Encode the data
        ushort holeCardIndex = HoleCardIndices[hand.HoleCards];
        uint communityCardIndex = CommunityCardIndices[hand.CommunityCards];
        ulong combinedIndices = ((ulong)holeCardIndex << CommunityCardBits) | communityCardIndex;
        
        // Pack the score into the last four bytes
        bytes[^1] = (byte)(score & ByteMask);
        bytes[^2] = (byte)((score >> BitsPerByte) & ByteMask);
        bytes[^3] = (byte)((score >> (2 * BitsPerByte)) & ByteMask);
        bytes[^4] = (byte)((score >> (3 * BitsPerByte)) & ByteMask);

        // Pack the combined hole/community card indices into the remaining five bytes
        bytes[^5] = (byte)(combinedIndices & ByteMask);
        bytes[^6] = (byte)((combinedIndices >> BitsPerByte) & ByteMask);
        bytes[^7] = (byte)((combinedIndices >> (2 * BitsPerByte)) & ByteMask);
        bytes[^8] = (byte)((combinedIndices >> (3 * BitsPerByte)) & ByteMask);
        bytes[^9] = (byte)((combinedIndices >> (4 * BitsPerByte)) & ByteMask);
        return bytes;
    }

    public static (HoldemHand Hand, uint Score) Unpack(byte[] bytes)
    {
        // Decode score bytes
        ulong firstScoreByte = bytes[^1];
        ulong secondScoreByte = bytes[^2];
        ulong thirdScoreByte = bytes[^3];
        ulong fourthScoreByte = bytes[^4];
        ulong score = 
            firstScoreByte | 
            (secondScoreByte << BitsPerByte) | 
            (thirdScoreByte << (2 * BitsPerByte)) | 
            (fourthScoreByte << (3 * BitsPerByte));

        // Decode community index bytes
        ulong firstCommunityByte = bytes[^5];
        ulong secondCommunityByte = bytes[^6];
        ulong thirdCommunityByte = bytes[^7];
        ulong fourthCommunityByte = bytes[^8];
        ulong fifthCommunityByte = bytes[^9];
        ulong combinedIndex = 
            firstCommunityByte |
            (secondCommunityByte << BitsPerByte) |
            (thirdCommunityByte << (2 * BitsPerByte)) |
            (fourthCommunityByte << (3 * BitsPerByte)) |
            (fifthCommunityByte << (4 * BitsPerByte));

        // Mask out the community card and hole card indices
        uint communityCardIndex = (uint)(combinedIndex & CommunityCardMask);
        ushort holeCardIndex = (ushort)((combinedIndex & HoleCardMask) >> CommunityCardBits);

        // Construct hand
        Cards holeCards = HoleCardsIndicesReversed[holeCardIndex];
        Cards communityCards = CommunityCardsIndicesReversed[communityCardIndex];
        HoldemHand hand = new(holeCards, communityCards);
        return (hand, (uint)score);
    }
}