namespace Core.IntegrationTests.UseCases.Organisations;

public class DeleteOrganisationTests(ServiceFixture service, ITestOutputHelper output)
    : MultipleOrganisationTest(service, output)
{
    [Fact]
    public async Task Organisation_can_be_deleted()
    {
        var organisation1 = Org1Admin.OrganisationId;
        var organisation2 = Org2Admin.OrganisationId;
        var adminId = Org1Admin.UserId;
        var memberId = Org1Member.UserId;

        // act
        await Command(new MoveUser.Command(adminId, organisation2));
        await Command(new MoveUser.Command(memberId, organisation2));
        await Command(new DeleteOrganisation.Command(organisation1));
    }

    [Fact]
    public async Task Cannot_delete_an_organisation_with_users()
    {
        var organisation1 = Org1Admin.OrganisationId;

        // act
        var act = async () => await Command(new DeleteOrganisation.Command(organisation1));

        // assert
        await act.Should().ThrowAsync<BusinessRuleBrokenException>()
            .WithMessage("Cannot delete an organisation with users");
    }
}