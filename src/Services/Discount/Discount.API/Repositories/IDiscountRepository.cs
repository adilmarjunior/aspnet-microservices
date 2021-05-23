using Discount.API.Entities;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
  public interface IDiscountRepository : IBaseRepository<Coupon>
  {
    Task<Coupon> GetDiscount(string productName);
  }
}