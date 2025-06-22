namespace Core.IntegrationTests.UseCases.Organisations;

public class MultiTenantTests(ServiceFixture service, ITestOutputHelper output)
    : MultipleOrganisationTest(service, output)
{
    [Fact]
    public async Task Attempting_to_change_name_to_a_name_that_already_exists_fails()
    {
        // arrange
        var organisation = await Query(new GetOrganisation.Query(), Org1Admin);
        var title = organisation.Title;

        // act
        var act = async () =>
            await Command(new UpdateOrganisationName.Command(title), Org2Admin);

        // assert
        await act.Should()
            .ThrowAsync<BusinessRuleBrokenException>()
            .WithMessage($"The organisation name '{title}' is in use by another organisation");
    }
}