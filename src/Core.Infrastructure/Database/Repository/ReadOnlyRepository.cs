using Ardalis.Specification.EntityFrameworkCore;
using Core.Application.Contracts;

namespace Core.Infrastructure.Database.Repository;

public class ReadOnlyRepository<T>(DatabaseContext db) : RepositoryBase<T>(db), IReadOnlyRepository<T>
    where T : class;