using System.Linq.Expressions;
using Board.Common.Models;

namespace Board.Common.Interfaces;

public interface IGenericRepository<T>  where T : BaseEntity
{
    Task<T> GetAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<T> GetAsync(Expression<Func<T, bool>> filter);
    Task CreateAsync(T entity);
    Task UpdateAsync(T entity);
    Task RemoveAsync(Guid id);
    Task RemoveAsync(T entity);
}