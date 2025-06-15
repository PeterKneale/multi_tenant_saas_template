namespace Core.Domain.Common.Specifications;

public abstract class GlobalSingleResultSpecification<T> : SingleResultSpecification<T> where T : BaseEntity
{
    protected GlobalSingleResultSpecification()
    {
        Query
            .IgnoreQueryFilters();
    }
}