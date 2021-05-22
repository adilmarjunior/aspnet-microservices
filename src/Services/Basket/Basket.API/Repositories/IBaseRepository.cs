using System.Threading.Tasks;

namespace Basket.API.Repositories
{
  public interface IBaseRepository<TEntity>
  {
    Task<TEntity> Get(string key);
    Task<TEntity> Update(TEntity entity);
    Task Delete(string key);
  }
}