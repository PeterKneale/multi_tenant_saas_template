namespace Core.Domain.Projects.Contracts;

public interface IProjectNameCheck
{
    bool NameExists(ProjectName name);
}