using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.DTOs;
using Talabat.API.Errors;
using Talabt.Core.Entities;
using Talabt.Core.Services;

namespace Talabat.API.Controllers
{
    [Authorize]
    public class PaymentController : APIBaseController
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentController(IPaymentService paymentService,IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }
        [ProducesResponseType(typeof(CustomerBasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost]
       public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var CustomerBasket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (CustomerBasket is null) return BadRequest(new ApiResponse(400, "There is A Problem With Your Basket"));
            var MappedBasket = _mapper.Map<CustomerBasket, CustomerBasketDto>(CustomerBasket);
            return Ok(MappedBasket);
        }

    }
}
