using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
  public interface IBaseRepository<TEntity>
  {
    Task<IEnumerable<TEntity>> GetAll();
    Task<TEntity> GetById(string id);
    Task Create(TEntity entity);
    Task<bool> Update(TEntity entity);
    Task<bool> Delete(string id);
  }
}