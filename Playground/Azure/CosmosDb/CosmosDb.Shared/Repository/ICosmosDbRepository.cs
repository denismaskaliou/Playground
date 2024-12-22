using System.Linq.Expressions;
using CosmosDb.Shared.Entities;

namespace CosmosDb.Shared.Repository;

public interface ICosmosDbRepository<T> where T : BaseEntity
{
    Task<IList<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> filter);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
}