using System;
using System.Threading.Tasks;
using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
  public class BasketRepository : IBasketRepository
  {
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache cache)
    {
      _redisCache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<ShoppingCart> Get(string userName)
    {
      var basket = await _redisCache.GetStringAsync(userName);

      if(String.IsNullOrEmpty(basket)){
        return null;
      }

      return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart> Update(ShoppingCart basket)
    {
      await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));

      return await this.Get(basket.UserName);
    }

    public async Task Delete(string userName)
    {
      await _redisCache.RemoveAsync(userName);
    }
  }
}