using BraunauMobil.VeloBasar.Data;

namespace BraunauMobil.VeloBasar.Tests.Data.SellerExtensionsTests;

public class DefaultOrder
{
    [Fact]
    public void ShouldOrderByFirstAndLastName()
    {
        // Arrange
        IQueryable<SellerEntity> countries = new List<SellerEntity>()
        {
            new () { FirstName = "C", LastName = "X" },
            new () { FirstName = "B", LastName = "Y" },
            new () { FirstName = "A", LastName = "Z" },
            new () { FirstName = "A", LastName = "Z" },
            new () { FirstName = "B", LastName = "X" },
            new () { FirstName = "C", LastName = "Y" }
        }.AsQueryable();

        // Act
        SellerEntity[] result = countries.DefaultOrder().ToArray();

        // Assert
        result.Should().BeInAscendingOrder(new TestComparer());
    }

    private class TestComparer
        : IComparer<SellerEntity>
    {
        public int Compare(SellerEntity? x, SellerEntity? y)
        {
            ArgumentNullException.ThrowIfNull(x);
            ArgumentNullException.ThrowIfNull(y);

            int firstNameResult = x.FirstName.CompareTo(y.FirstName);
            if (firstNameResult == 0)
            {
                return x.LastName.CompareTo(y.LastName);
            }
            return firstNameResult;
        }
    }
}
