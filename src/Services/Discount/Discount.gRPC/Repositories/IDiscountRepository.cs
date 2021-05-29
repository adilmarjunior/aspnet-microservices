using Discount.gRPC.Entities;
using System.Threading.Tasks;

namespace Discount.gRPC.Repositories
{
  public interface IDiscountRepository : IBaseRepository<Coupon>
  {
    Task<Coupon> GetDiscount(string productName);
  }
}