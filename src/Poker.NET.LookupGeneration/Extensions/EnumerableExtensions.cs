namespace Poker.NET.LookupGeneration.Extensions;

public static class EnumerableExtensions
{
    public static IEnumerable<TSource[]> ChunkGrowing<TSource>(this IEnumerable<TSource> source, int initialChunkSize, double growthFactor)
    {
        ArgumentNullException.ThrowIfNull(source, nameof(source));
        ArgumentOutOfRangeException.ThrowIfLessThan(initialChunkSize, 1, nameof(initialChunkSize));
        ArgumentOutOfRangeException.ThrowIfLessThan(growthFactor, 0, nameof(growthFactor));

        if (source is TSource[] array)
        {
            return array.Length != 0
                ? ArrayChunkGrowingIterator(array, initialChunkSize, growthFactor)
                : [];
        }

        return EnumerableChunkGrowingIterator(source, initialChunkSize, growthFactor);
    }

    private static IEnumerable<TSource[]> ArrayChunkGrowingIterator<TSource>(TSource[] source, int initialChunkSize, double growthFactor)
    {
        int index = 0;
        double rawSize = initialChunkSize;
        while (index < source.Length)
        {
            TSource[] chunk = new ReadOnlySpan<TSource>(source, index, Math.Min((int)rawSize, source.Length - index)).ToArray();
            index =+ chunk.Length;
            yield return chunk;

            rawSize *= 1 + growthFactor;
        }
    }

    private static IEnumerable<TSource[]> EnumerableChunkGrowingIterator<TSource>(IEnumerable<TSource> source, int initialChunkSize, double growthFactor)
    {
        double rawSize = initialChunkSize;
        List<TSource> chunk = [];
        foreach (TSource item in source)
        {
            if (chunk.Count < (int)rawSize)
            {
                chunk.Add(item);
            }

            if (chunk.Count == (int)rawSize)
            {
                yield return chunk.ToArray();
                rawSize *= 1 + growthFactor;
                chunk.Clear();
            }
        }
    }
}