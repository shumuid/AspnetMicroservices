using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Basket.API.Repositories;
using Basket.API.Entities;
using System.Net;
using Basket.API.GrpcServices;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly ILogger<BasketController> _logger;
        private readonly DiscountGrpcService _discountGrpcService;

        public BasketController(IBasketRepository repository, ILogger<BasketController> logger, DiscountGrpcService discountGrpcService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _discountGrpcService = discountGrpcService ?? throw new ArgumentNullException(nameof(discountGrpcService));
        }

        [HttpGet("{userName}", Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);

            return Ok(basket ?? new (userName));
        }

        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            // TODO: Communicate with Discount.Grpc and calculate latest prices of products into shopping cart
            foreach(var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= (decimal)coupon.Amount;
            }

            return Ok(await _repository.UpdateBasket(basket));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> Delete(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
