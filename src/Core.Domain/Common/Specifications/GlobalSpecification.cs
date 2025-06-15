namespace Core.Domain.Common.Specifications;

public class GlobalSpecification<T> : Specification<T> where T : BaseEntity
{
    protected GlobalSpecification()
    {
        Query
            .IgnoreQueryFilters();
    }
}