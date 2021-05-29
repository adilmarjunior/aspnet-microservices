using System;
using System.Threading.Tasks;
using AutoMapper;
using Discount.gRPC.Entities;
using Discount.gRPC.Protos;
using Discount.gRPC.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace Discount.gRPC.Services
{
  public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
  {

    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DiscountService> _logger;

    public DiscountService(IDiscountRepository discountRepository,
                           ILogger<DiscountService> logger,
                           IMapper mapper)
    {
      this._discountRepository = discountRepository ?? throw new ArgumentNullException(nameof(discountRepository));
      this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
      this._logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
      var coupon = await _discountRepository.GetDiscount(request.ProductName);
      if (coupon == null)
      {
        throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
      }
      _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

      var couponModel = _mapper.Map<CouponModel>(coupon);
      return couponModel;
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
      var coupon = _mapper.Map<Coupon>(request.Coupon);

      await _discountRepository.Create(coupon);
      
      _logger.LogInformation("Discount is created for ProductName : {productName}", coupon.ProductName);

      var couponModel = _mapper.Map<CouponModel>(coupon);
      return couponModel;
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
      var coupon = _mapper.Map<Coupon>(request.Coupon);

      await _discountRepository.Update(coupon);
      
      _logger.LogInformation("Discount is successfully updated. ProductName : {productName}", coupon.ProductName);

      var couponModel = _mapper.Map<CouponModel>(coupon);
      return couponModel;
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
      var deleted = await _discountRepository.Delete(request.ProductName);
      
      _logger.LogInformation("Discount is successfully deleted. ProductName : {productName}", request.ProductName);

      var response = new DeleteDiscountResponse 
      {
        Success = deleted
      };

      return response;
    }
  }
}