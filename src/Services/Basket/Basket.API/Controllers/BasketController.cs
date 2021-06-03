using Basket.API.Entities;
using Basket.API.Repositories;
using Basket.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
  [ApiController]
  [Route("api/v1/[controller]")]
  public class BasketController : ControllerBase
  {
    private readonly IBasketRepository _basketRepository;
    private readonly IDiscountGrpcService _discountGrIDiscountGrpcService;

    public BasketController(IBasketRepository repository, 
                            IDiscountGrpcService discountGrIDiscountGrpcService)
    {
      _basketRepository = repository ?? throw new ArgumentNullException(nameof(repository));
      _discountGrIDiscountGrpcService = discountGrIDiscountGrpcService ?? throw new ArgumentNullException(nameof(discountGrIDiscountGrpcService));
    }

    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
      var basket = await _basketRepository.Get(userName);
      return Ok(basket ?? new ShoppingCart(userName));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
      // TODO : Communicate with Discount.Grpc 
      // and Calculate latest prices of products into shopping cart.

      foreach (var item in basket.Items)
      {
        var coupon = await _discountGrIDiscountGrpcService.GetDiscount(item.ProductName);
        item.Price -= coupon.Amount;
      }

      return Ok(await _basketRepository.Update(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
      await _basketRepository.Delete(userName);
      return Ok();
    }
  }
}