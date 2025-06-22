namespace Core.Domain.Projects.Specifications;

public class GetProjectByName : SingleResultSpecification<Project>
{
    public GetProjectByName(ProjectName name)
    {
        Query.Where(x => x.Name.Title.Equals(name.Title));
    }
}