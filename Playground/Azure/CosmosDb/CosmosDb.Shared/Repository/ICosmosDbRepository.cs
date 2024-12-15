using System.Linq.Expressions;

namespace CosmosDb.Shared.Repository;

public interface ICosmosDbRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task<IEnumerable<T>> FilterAsync(Expression<Func<T, bool>> filter);
    Task<T> CreateAsync(T entity);
    Task UpdateAsync(string id, T entity);
    Task DeleteAsync(string id);
}