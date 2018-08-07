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
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<OrderController>();
        }


      

        [Route("{orderID:int}")]
        [HttpGet]
        [ProducesResponseType(typeof(Order), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetOrderMaster(long OrderMasterID)
        {
            try
            {
                var OrderMaster = await _repository.GetOrderAsync(OrderMasterID);
                return Ok(OrderMaster);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


   

    }
}