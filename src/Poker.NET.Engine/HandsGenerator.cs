namespace Poker.NET.Engine;

public static class HandsGenerator
{
    public static IEnumerable<Cards> GetAllSevenCardHands()
    {
        ulong hand = (1ul << 7) - 1;
        ulong max = hand << (52 - 7);

        while (hand <= max)
        {
            yield return (Cards)hand;

            // Gosper's Hack to find the next number with n bits set
            ulong c = hand & (~hand + 1);
            ulong r = hand + c;
            hand = (((r ^hand) >> 2) / c) | r;
        }
    }
}
