using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabt.Core.Entities;
using Talabt.Core.Repositories;

namespace Talabat.API.Controllers
{
    public class BasketController : APIBaseController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }
        #region Get All Recreate
        [HttpGet]
        public async Task <ActionResult<CustomerBasket>> GetCustomerBasket(string BasketId)
        {
            var Result = await _basketRepository.GetBasketAsync(BasketId);
            return Result is null? new CustomerBasket(BasketId) : Result;
        }
        #endregion
        #region Update Or Create
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket Basket)
        {
            var CreatedOrUpdate = await _basketRepository.UpdateBasketAsync(Basket);
            if (CreatedOrUpdate is null) return BadRequest();
            return Ok(CreatedOrUpdate);
        }
        #endregion
        #region Delete
        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepository.DeleteBasketAsync(BasketId);
        }
        #endregion
    }
}
