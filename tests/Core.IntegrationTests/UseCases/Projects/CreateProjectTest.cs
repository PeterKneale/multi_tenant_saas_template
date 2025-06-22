using Core.Application.Features;
using Core.Application.Projects;
using JetBrains.Annotations;

namespace Core.IntegrationTests.UseCases.Projects;

[TestSubject(typeof(CreateProject))]
public class CreateProjectTest(ServiceFixture service, ITestOutputHelper output) : SingleOrganisationTest(service, output)
{

    [Fact]
    public async Task Projects_can_be_have_features_added()
    {
        // arrange
        var projectId = Guid.NewGuid();
        var projectName = Fake.Company.CompanyName();
        var featureId = Guid.NewGuid();
        var featureName = Fake.Lorem.Word();
   
        // act
        var createProject = new CreateProject.Command(projectId, projectName);
        var createFeature = new CreateFeature.Command(projectId, featureId, featureName);

        await ExecuteInAdminContext(createProject);
        await ExecuteInAdminContext(createFeature);

        // assert
    }
}