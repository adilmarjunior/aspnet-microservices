using System.Threading.Tasks;

namespace Discount.gRPC.Repositories
{
  public interface IBaseRepository<TEntity>
  {
    Task<bool> Create(TEntity entity);
    Task<bool> Update(TEntity entity);
    Task<bool> Delete(string productName);
  }
}