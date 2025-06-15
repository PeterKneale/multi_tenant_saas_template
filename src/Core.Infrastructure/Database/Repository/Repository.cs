using Ardalis.Specification.EntityFrameworkCore;
using Core.Application.Contracts;

namespace Core.Infrastructure.Database.Repository;

public class Repository<T>(DatabaseContext db) : RepositoryBase<T>(db), IRepository<T>
    where T : class;