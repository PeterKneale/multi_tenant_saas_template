namespace Core.Application.Contracts;

public interface IReadOnlyRepository<T> : IReadRepositoryBase<T> where T : class;