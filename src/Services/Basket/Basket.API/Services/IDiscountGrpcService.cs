using System;
using System.Threading.Tasks;
using Discount.gRPC.Protos;

namespace Basket.API.Services
{
  public interface IDiscountGrpcService
  {
    Task<CouponModel> GetDiscount(string productName);
  }
}