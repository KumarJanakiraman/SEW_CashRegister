using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CashRegister.Model;
using CashRegister.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CashRegister.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountController> _logger;
        public DiscountController(IDiscountRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<DiscountController>();
        }


        [Route("discounts")]
        [HttpGet()]
        [ProducesResponseType(typeof(List<Item>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDiscounts()
        {
            var itemMaster = await _repository.GetDiscountsAsync();
            if (itemMaster == null)
            {
                return Ok(new List<Discount>());
            }

            return Ok(itemMaster);
        }

        [Route("{discountID:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(Discount), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetDiscount(long discountID)
        {
            try
            {
                var discount = await _repository.GetDiscountAsync(discountID);
                return Ok(discount);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [Route("add")]
        [HttpPost]
        [ProducesResponseType(typeof(Discount), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddDiscount([FromBody]Discount value)
        {
            var validation = value.Validate(); //validation for discount
            if (validation.Count() > 0)
                return Ok(validation);

            var _oldDiscount = await _repository.GetDiscountByItemEffDateAsync(value.ItemID,value.EffectiveDateFrom,value.EffectiveDateTo);
            if(_oldDiscount != null)
            {
                return Ok(new { Message = $"Discount already exists" }); 
            }
            var _discount = await _repository.AddDiscountAsync(value);
            return Ok(_discount);
        }

        [Route("update")]
        [HttpPost]
        [ProducesResponseType(typeof(Discount), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateItem([FromBody]Discount value)
        {
            var _discount = await _repository.GetDiscountAsync(value.DiscountID);
            if (_discount == null)
            {
                return NotFound(new { Message = $"Discount  {value.DiscountDescription.ToString()}  does'not exist" });
            }
           
            var discount = await _repository.UpdateDiscountAsync(_discount);
            return Ok(discount);
        }

    }
}