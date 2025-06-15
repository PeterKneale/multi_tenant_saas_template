namespace Core.Application.Contracts;

public interface IRepository<T> : IRepositoryBase<T> where T : class;