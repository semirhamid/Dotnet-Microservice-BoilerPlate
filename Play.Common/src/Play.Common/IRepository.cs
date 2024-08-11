using System.Linq.Expressions;
using Play.Common.Entities;

namespace Play.Catalog.Service.Repository
{
  public interface IRepository<T> where T : IEntity
  {
    Task<IReadOnlyCollection<T>> GetAllAsync();
    Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
    Task<T> GetAsync(Guid id);
    Task<T> GetAsync(Expression<Func<T, bool>> filter);
    Task CreateAsync(T item);
    Task UpdateAsync(T item);
    Task DeleteAsync(Guid id);
  }
}