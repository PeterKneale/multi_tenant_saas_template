using Core.Domain.Users;

namespace Core.Domain.UnitTests.Users;

public class NameTests
{
    [Fact]
    public void Names_are_equivalent()
    {
        var name1 = new Name("X", "x");
        var name2 = new Name("X", "x");
        Assert.Equal(name1, name2);
    }

    [Fact]
    public void Full_name_is_formatted()
    {
        // arrange
        var name = new Name("x", "y");

        // assert
        name.FullName.Should().Be("x y");
    }
}