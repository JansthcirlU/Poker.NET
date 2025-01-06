using Poker.NET.Engine.Hands;

namespace Poker.NET.Engine.Tests.Hands;

public class PairTests
{
    [Fact]
    public void Pair_WhenConstructedWithDefaultConstructor_ShouldThrow()
    {
        // Act
        InvalidOperationException? ex = Assert.Throws<InvalidOperationException>(
            () => new Pair()
        );

        // Assert
        Assert.NotNull(ex);
        Assert.StartsWith("You must not use the default constructor to create a Pair.", ex.Message);
    }

    // Use HandsGenerator.GetAllPairs() to test happy path

    // Use HandsGenerator and exclude all pairs to test exception path
}

// Similar test cases for other hand types