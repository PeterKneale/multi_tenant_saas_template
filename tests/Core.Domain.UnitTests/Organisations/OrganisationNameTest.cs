using Core.Domain.Organisations;

namespace Core.Domain.UnitTests.Organisations;

public class OrganisationNameTest
{
    [Fact]
    public void Names_are_equivalent()
    {
        var name1 = new OrganisationName("X");
        var name2 = new OrganisationName("X");
        Assert.Equal(name1, name2);
    }

    [Fact]
    public void Names_with_descriptions_are_equivalent()
    {
        var name1 = new OrganisationName("X", "x");
        var name2 = new OrganisationName("X", "x");
        Assert.Equal(name1, name2);
    }
}